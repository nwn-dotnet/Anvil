using System;
using System.Globalization;
using System.IO;
using System.Text;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ItemAppearance
  {
    private readonly CNWSItem item;

    internal ItemAppearance(CNWSItem item)
    {
      this.item = item;
    }

    /// <summary>
    /// Clears any per-part color overrides set for the specified part slot.
    /// </summary>
    /// <param name="modelSlot">The model portion of the slot to clear.</param>
    /// <param name="colorSlot">The color portion of the slot to clear.</param>
    public void ClearArmorPieceColor(ItemAppearanceArmorModel modelSlot, ItemAppearanceArmorColor colorSlot)
    {
      SetArmorPieceColor(modelSlot, colorSlot, 255);
    }

    /// <summary>
    /// Copies this item appearance to another item.
    /// <param name="otherItem">The item to copy this appearance to.</param>
    /// </summary>
    public void CopyTo(NwItem otherItem)
    {
      ItemAppearance otherAppearance = otherItem.Appearance;

      for (int idx = 0; idx < 6; idx++)
      {
        otherAppearance.item.m_nLayeredTextureColors[idx] = item.m_nLayeredTextureColors[idx];
      }

      for (int idx = 0; idx < 3; idx++)
      {
        otherAppearance.item.m_nModelPart[idx] = item.m_nModelPart[idx];
      }

      for (int idx = 0; idx < 19; idx++)
      {
        otherAppearance.item.m_nArmorModelPart[idx] = item.m_nArmorModelPart[idx];
      }

      for (byte texture = 0; texture < 6; texture++)
      {
        for (byte part = 0; part < 19; part++)
        {
          otherAppearance.item.SetLayeredTextureColorPerPart(texture, part, item.GetLayeredTextureColorPerPart(texture, part));
        }
      }
    }

    /// <summary>
    /// Updates this item appearance using the value retrieved through <see cref="Serialize"/>.
    /// </summary>
    /// <param name="serialized">The serialized item appearance.</param>
    /// <exception cref="ArgumentException">Thrown if an invalid serialized string is specified.</exception>
    public void Deserialize(string serialized)
    {
      if (serialized == null || serialized.Length != 2 * 142 && serialized.Length != 2 * 28)
      {
        throw new ArgumentException("invalid string length, must be 284", serialized);
      }

      using StringReader stringReader = new StringReader(serialized);
      Span<char> buffer = stackalloc char[2];

      for (int idx = 0; idx < 6; idx++)
      {
        stringReader.ReadBlock(buffer);
        item.m_nLayeredTextureColors[idx] = byte.Parse(buffer, NumberStyles.AllowHexSpecifier);
      }

      for (int idx = 0; idx < 3; idx++)
      {
        stringReader.ReadBlock(buffer);
        item.m_nModelPart[idx] = byte.Parse(buffer, NumberStyles.AllowHexSpecifier);
      }

      for (int idx = 0; idx < 19; idx++)
      {
        stringReader.ReadBlock(buffer);
        item.m_nArmorModelPart[idx] = byte.Parse(buffer, NumberStyles.AllowHexSpecifier);
      }

      if (serialized.Length == 2 * 142)
      {
        for (byte texture = 0; texture < 6; texture++)
        {
          for (byte part = 0; part < 19; part++)
          {
            stringReader.ReadBlock(buffer);
            item.SetLayeredTextureColorPerPart(texture, part, byte.Parse(buffer, NumberStyles.AllowHexSpecifier));
          }
        }
      }
    }

    /// <summary>
    /// Gets the armor color of this item.
    /// </summary>
    /// <param name="slot">The armor color slot index to query.</param>
    public byte GetArmorColor(ItemAppearanceArmorColor slot)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 119)
      {
        //1.69 colors
        if (index <= 5)
        {
          return item.m_nLayeredTextureColors[index];
        }

        //per-part coloring
        byte part = (byte)((index - 6) / 6);
        byte texture = (byte)(index - 6 - part * 6);
        return item.GetLayeredTextureColorPerPart(texture, part);
      }

      return 0;
    }

    /// <summary>
    /// Gets the armor model of this item.
    /// </summary>
    /// <param name="slot">The armor model slot index to query.</param>
    public byte GetArmorModel(ItemAppearanceArmorModel slot)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 18)
      {
        return item.m_nArmorModelPart[index];
      }

      return 0;
    }

    /// <summary>
    /// Gets the armor color for a piece of this item.
    /// </summary>
    /// <param name="modelSlot">The model portion of the slot to query.</param>
    /// <param name="colorSlot">The color portion of the slot to query.</param>
    public byte GetArmorPieceColor(ItemAppearanceArmorModel modelSlot, ItemAppearanceArmorColor colorSlot)
    {
      const int numColors = NWScript.ITEM_APPR_ARMOR_NUM_COLORS;
      int index = numColors + (int)modelSlot * numColors + (int)colorSlot;
      return GetArmorColor((ItemAppearanceArmorColor)index);
    }

    /// <summary>
    /// Gets the base model of this item.
    /// </summary>
    public byte GetSimpleModel()
    {
      return item.m_nModelPart[0];
    }

    /// <summary>
    /// Gets the weapon color of this item.
    /// </summary>
    /// <param name="slot">The weapon color index to query.</param>
    public byte GetWeaponColor(ItemAppearanceWeaponColor slot)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 5)
      {
        return item.m_nLayeredTextureColors[index];
      }

      return 0;
    }

    /// <summary>
    /// Gets the weapon model of this item.
    /// </summary>
    /// <param name="slot">The weapon model index to query.</param>
    public byte GetWeaponModel(ItemAppearanceWeaponModel slot)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 2)
      {
        return item.m_nModelPart[index];
      }

      return 0;
    }

    /// <summary>
    /// Gets a string containing the entire appearance for this item.
    /// </summary>
    /// <returns>A string representing the item's appearance.</returns>
    public string Serialize()
    {
      // Based on the serialization method used in NWNX to ensure cross-compatibility: https://github.com/nwnxee/unified/blob/master/Plugins/Item/Item.cpp#L120-L154
      StringBuilder stringBuilder = new StringBuilder();

      for (int idx = 0; idx < 6; idx++)
      {
        stringBuilder.Append(item.m_nLayeredTextureColors[idx].ToString("X2"));
      }

      for (int idx = 0; idx < 3; idx++)
      {
        stringBuilder.Append(item.m_nModelPart[idx].ToString("X2"));
      }

      for (int idx = 0; idx < 19; idx++)
      {
        stringBuilder.Append(item.m_nArmorModelPart[idx].ToString("X2"));
      }

      for (byte texture = 0; texture < 6; texture++)
      {
        for (byte part = 0; part < 19; part++)
        {
          stringBuilder.Append(item.GetLayeredTextureColorPerPart(texture, part).ToString("X2"));
        }
      }

      return stringBuilder.ToString();
    }

    /// <summary>
    /// Sets the armor color of this item.
    /// </summary>
    /// <param name="slot">The armor color slot index to be assigned.</param>
    /// <param name="value">The new color to assign.</param>
    public void SetArmorColor(ItemAppearanceArmorColor slot, byte value)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 119)
      {
        //1.69 colors
        if (index <= 5)
        {
          item.m_nLayeredTextureColors[index] = value;
        }

        //per-part coloring
        else
        {
          byte part = (byte)((index - 6) / 6);
          byte texture = (byte)(index - 6 - part * 6);
          item.SetLayeredTextureColorPerPart(texture, part, value);
        }
      }
    }

    /// <summary>
    /// Sets the armor model of this item.
    /// </summary>
    /// <param name="slot">The armor model slot index to be assigned.</param>
    /// <param name="value">The new model to assign.</param>
    public void SetArmorModel(ItemAppearanceArmorModel slot, byte value)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 18)
      {
        item.m_nArmorModelPart[index] = value;
        item.m_nArmorValue = item.ComputeArmorClass();
      }
    }

    /// <summary>
    /// Sets the armor color for a piece of this item.
    /// </summary>
    /// <param name="modelSlot">The model portion of the slot to assign.</param>
    /// <param name="colorSlot">The color portion of the slot to assign.</param>
    /// <param name="value">The new color to assign.</param>
    public void SetArmorPieceColor(ItemAppearanceArmorModel modelSlot, ItemAppearanceArmorColor colorSlot, byte value)
    {
      const int numColors = NWScript.ITEM_APPR_ARMOR_NUM_COLORS;
      int index = numColors + (int)modelSlot * numColors + (int)colorSlot;
      SetArmorColor((ItemAppearanceArmorColor)index, value);
    }

    /// <summary>
    /// Sets the base model of this item.
    /// </summary>
    public void SetSimpleModel(byte value)
    {
      if (value > 0)
      {
        item.m_nModelPart[0] = value;
      }
    }

    /// <summary>
    /// Sets the weapon color of this item.
    /// </summary>
    /// <param name="slot">The weapon color index to be assigned.</param>
    /// <param name="value">The new color to assign.</param>
    public void SetWeaponColor(ItemAppearanceWeaponColor slot, byte value)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 5)
      {
        item.m_nLayeredTextureColors[index] = value;
      }
    }

    /// <summary>
    /// Sets the weapon model of this item.
    /// </summary>
    /// <param name="slot">The weapon model index to be assigned.</param>
    /// <param name="value">The new model to assign.</param>
    public void SetWeaponModel(ItemAppearanceWeaponModel slot, byte value)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 2)
      {
        item.m_nModelPart[index] = value;
      }
    }
  }
}
