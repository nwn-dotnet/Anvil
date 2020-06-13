using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWN.Services;

namespace NWN.API
{
  public class SpellItemMetadata : ITwoDimArray
  {
    private readonly List<Entry> entries = new List<Entry>();

    public Entry GetEntry(Spell spell)
    {
      int nSpell = (int) spell;

      foreach (Entry itemMeta in entries)
      {
        if (itemMeta.SpellId == nSpell)
        {
          return itemMeta;
        }
      }

      return null;
    }

    void ITwoDimArray.DeserializeRow(int rowIndex, TwoDimEntry twoDimEntry)
    {
      if (int.TryParse(twoDimEntry("IPRP_SpellIndex"), out int spellIndex))
      {
        bool noPotion = twoDimEntry("NoPotion").ParseIntBool();
        bool noWand = twoDimEntry("NoWand").ParseIntBool();
        bool noScroll = twoDimEntry("NoScroll").ParseIntBool();
        int level = twoDimEntry("Level").ParseInt();
        bool castOnItems = twoDimEntry("CastOnItems").ParseIntBool();

        entries.Add(new Entry(rowIndex, spellIndex, noPotion, noWand, noScroll, level, castOnItems));
      }
    }

    public class Entry
    {
      public readonly int SpellId;
      public readonly int ItemPropertySpellIndex;
      public readonly bool NoPotion;
      public readonly bool NoWand;
      public readonly bool NoScroll;
      public readonly int Level;
      public readonly bool CastOnItems;

      public Entry(int spellId, int itemPropertySpellIndex, bool noPotion, bool noWand, bool noScroll, int level, bool castOnItems)
      {
        SpellId = spellId;
        ItemPropertySpellIndex = itemPropertySpellIndex;
        NoPotion = noPotion;
        NoWand = noWand;
        NoScroll = noScroll;
        Level = level;
        CastOnItems = castOnItems;
      }
    }
  }
}