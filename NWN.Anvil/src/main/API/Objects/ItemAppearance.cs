using System;
using System.Globalization;
using System.IO;
using System.Text;
using Anvil.Services;
using NWN.Core;

namespace Anvil.API
{
  public sealed class ItemAppearance
  {
    [Inject]
    private static FeedbackService FeedbackService { get; set; } = null!;

    private readonly NwItem item;

    internal ItemAppearance(NwItem item)
    {
      this.item = item;
    }

    /// <summary>
    /// Creates a new item with the specified appearance changes.<br/>
    /// The existing item is destroyed and replaced with the new item.<br/>
    /// If the item was equipped, it is restored to the original equipment slot.
    /// </summary>
    /// <param name="changes">The appearance changes to apply.</param>
    /// <returns>The new item with the updated appearance.</returns>
    public NwItem ChangeAppearance(Action<ItemAppearance> changes)
    {
      NwGameObject? possessor = item.Possessor;
      EquipmentSlots slot = EquipmentSlots.None;
      Location location = possessor?.Location ?? item.Location!;

      NwCreature? creature = possessor as NwCreature;
      possessor.IsPlayerControlled(out NwPlayer? player);

      if (creature is not null)
      {
        slot = creature.GetSlotFromItem(item);
      }

      NwItem? clone = item.Clone(location);
      if (clone == null)
      {
        throw new InvalidOperationException("Failed to make item clone.");
      }

      NwModule.Instance.MoveObjectToLimbo(clone);
      changes.Invoke(clone.Appearance);

      if (player != null)
      {
        FeedbackService.AddCombatLogMessageFilter(CombatLogMessage.Feedback);
      }

      item.RemoveFromArea();
      item.PlotFlag = false;
      item.Destroy();

      if (creature is not null)
      {
        creature.AcquireItem(clone);
        if (slot != EquipmentSlots.None)
        {
          creature.RunEquip(clone, slot);
        }
      }
      else if (possessor is NwItem container)
      {
        container.AcquireItem(clone);
      }
      else if (possessor is NwStore store)
      {
        store.AcquireItem(clone);
      }
      else if (possessor is NwPlaceable placeable)
      {
        placeable.AcquireItem(clone);
      }
      else
      {
        clone.Location = location;
      }

      if (player != null)
      {
        FeedbackService.RemoveCombatMessageFilter(CombatLogMessage.Feedback);
      }

      return clone;
    }

    /// <summary>
    /// Clears any per-part color overrides set for the specified part slot.
    /// </summary>
    /// <param name="modelSlot">The model portion of the slot to clear.</param>
    /// <param name="colorSlot">The color portion of the slot to clear.</param>
    public void ClearArmorPieceColor(CreaturePart modelSlot, ItemAppearanceArmorColor colorSlot)
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
        otherAppearance.item.Item.m_nLayeredTextureColors[idx] = item.Item.m_nLayeredTextureColors[idx];
      }

      for (int idx = 0; idx < 3; idx++)
      {
        otherAppearance.item.Item.m_nModelPart[idx] = item.Item.m_nModelPart[idx];
      }

      for (int idx = 0; idx < 19; idx++)
      {
        otherAppearance.item.Item.m_nArmorModelPart[idx] = item.Item.m_nArmorModelPart[idx];
      }

      for (byte texture = 0; texture < 6; texture++)
      {
        for (byte part = 0; part < 19; part++)
        {
          otherAppearance.item.Item.SetLayeredTextureColorPerPart(texture, part, item.Item.GetLayeredTextureColorPerPart(texture, part));
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
      if (string.IsNullOrEmpty(serialized))
      {
        throw new ArgumentException("Serialized string must not be null or empty.");
      }

      // 3 Serialization formats determined by size:
      // 56 = No .35 extended model parts, no layered colors.
      // 284 = No .35 extended model parts
      // 328 = .35+ extended model parts

      using StringReader stringReader = new StringReader(serialized);
      Span<char> byteBuffer = stackalloc char[2];
      Span<char> ushortBuffer = stackalloc char[4];

      Span<char> modelBuffer = serialized.Length == 328 ? ushortBuffer : byteBuffer;

      for (int idx = 0; idx < 6; idx++)
      {
        stringReader.ReadBlock(byteBuffer);
        item.Item.m_nLayeredTextureColors[idx] = byte.Parse(byteBuffer, NumberStyles.AllowHexSpecifier);
      }

      for (int idx = 0; idx < 3; idx++)
      {
        stringReader.ReadBlock(modelBuffer);
        item.Item.m_nModelPart[idx] = ushort.Parse(modelBuffer, NumberStyles.AllowHexSpecifier);
      }

      for (int idx = 0; idx < 19; idx++)
      {
        stringReader.ReadBlock(modelBuffer);
        item.Item.m_nArmorModelPart[idx] = ushort.Parse(modelBuffer, NumberStyles.AllowHexSpecifier);
      }

      if (serialized.Length > 56)
      {
        for (byte texture = 0; texture < 6; texture++)
        {
          for (byte part = 0; part < 19; part++)
          {
            stringReader.ReadBlock(byteBuffer);
            item.Item.SetLayeredTextureColorPerPart(texture, part, byte.Parse(byteBuffer, NumberStyles.AllowHexSpecifier));
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
          return item.Item.m_nLayeredTextureColors[index];
        }

        //per-part coloring
        byte part = (byte)((index - 6) / 6);
        byte texture = (byte)(index - 6 - part * 6);
        return item.Item.GetLayeredTextureColorPerPart(texture, part);
      }

      return 0;
    }

    /// <summary>
    /// Gets the armor model of this item.
    /// </summary>
    /// <param name="slot">The armor model slot index to query.</param>
    public ushort GetArmorModel(CreaturePart slot)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 18)
      {
        return item.Item.m_nArmorModelPart[index];
      }

      return 0;
    }

    /// <summary>
    /// Gets the armor color for a piece of this item.
    /// </summary>
    /// <param name="modelSlot">The model portion of the slot to query.</param>
    /// <param name="colorSlot">The color portion of the slot to query.</param>
    public byte GetArmorPieceColor(CreaturePart modelSlot, ItemAppearanceArmorColor colorSlot)
    {
      const int numColors = NWScript.ITEM_APPR_ARMOR_NUM_COLORS;
      int index = numColors + (int)modelSlot * numColors + (int)colorSlot;
      return GetArmorColor((ItemAppearanceArmorColor)index);
    }

    /// <summary>
    /// Gets the base model of this item.
    /// </summary>
    public ushort GetSimpleModel()
    {
      return item.Item.m_nModelPart[0];
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
        return item.Item.m_nLayeredTextureColors[index];
      }

      return 0;
    }

    /// <summary>
    /// Gets the weapon model of this item.
    /// </summary>
    /// <param name="slot">The weapon model index to query.</param>
    public ushort GetWeaponModel(ItemAppearanceWeaponModel slot)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 2)
      {
        return item.Item.m_nModelPart[index];
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
        stringBuilder.Append(item.Item.m_nLayeredTextureColors[idx].ToString("X2"));
      }

      for (int idx = 0; idx < 3; idx++)
      {
        stringBuilder.Append(item.Item.m_nModelPart[idx].ToString("X4"));
      }

      for (int idx = 0; idx < 19; idx++)
      {
        stringBuilder.Append(item.Item.m_nArmorModelPart[idx].ToString("X4"));
      }

      for (byte texture = 0; texture < 6; texture++)
      {
        for (byte part = 0; part < 19; part++)
        {
          stringBuilder.Append(item.Item.GetLayeredTextureColorPerPart(texture, part).ToString("X2"));
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
          item.Item.m_nLayeredTextureColors[index] = value;
        }

        //per-part coloring
        else
        {
          byte part = (byte)((index - 6) / 6);
          byte texture = (byte)(index - 6 - part * 6);
          item.Item.SetLayeredTextureColorPerPart(texture, part, value);
        }
      }
    }

    /// <summary>
    /// Sets the armor model of this item.
    /// </summary>
    /// <param name="slot">The armor model slot index to be assigned.</param>
    /// <param name="value">The new model to assign.</param>
    public void SetArmorModel(CreaturePart slot, ushort value)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 18)
      {
        item.Item.m_nArmorModelPart[index] = value;
        item.Item.m_nArmorValue = item.Item.ComputeArmorClass();
      }
    }

    /// <summary>
    /// Sets the armor color for a piece of this item.
    /// </summary>
    /// <param name="modelSlot">The model portion of the slot to assign.</param>
    /// <param name="colorSlot">The color portion of the slot to assign.</param>
    /// <param name="value">The new color to assign.</param>
    public void SetArmorPieceColor(CreaturePart modelSlot, ItemAppearanceArmorColor colorSlot, byte value)
    {
      const int numColors = NWScript.ITEM_APPR_ARMOR_NUM_COLORS;
      int index = numColors + (int)modelSlot * numColors + (int)colorSlot;
      SetArmorColor((ItemAppearanceArmorColor)index, value);
    }

    /// <summary>
    /// Sets the base model of this item.
    /// </summary>
    public void SetSimpleModel(ushort value)
    {
      if (value > 0)
      {
        item.Item.m_nModelPart[0] = value;
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
        item.Item.m_nLayeredTextureColors[index] = value;
      }
    }

    /// <summary>
    /// Sets the weapon model of this item.
    /// </summary>
    /// <param name="slot">The weapon model index to be assigned.</param>
    /// <param name="value">The new model to assign.</param>
    public void SetWeaponModel(ItemAppearanceWeaponModel slot, ushort value)
    {
      int index = (int)slot;

      if (index >= 0 && index <= 2)
      {
        item.Item.m_nModelPart[index] = value;
      }
    }
  }
}
