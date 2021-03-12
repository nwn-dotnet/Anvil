using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  public class ItemAppearance
  {
    private readonly CNWSItem item;

    internal ItemAppearance(CNWSItem item)
    {
      this.item = item;
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
    /// Sets the base model of this item.
    /// </summary>
    public void SetSimpleModel(byte value)
    {
      if (value > 0)
      {
        byte[] modelParts = item.m_nModelPart;
        modelParts[0] = value;
        item.m_nModelPart = modelParts;
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

      if (value <= 255 && index >= 0 && index <= 5)
      {
        byte[] layeredTextureColors = item.m_nLayeredTextureColors;
        layeredTextureColors[index] = value;
        item.m_nLayeredTextureColors = layeredTextureColors;
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
        byte[] modelParts = item.m_nModelPart;
        modelParts[index] = value;
        item.m_nModelPart = modelParts;
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
        byte[] armorModelParts = item.m_nArmorModelPart;
        armorModelParts[index] = value;
        item.m_nArmorModelPart = armorModelParts;
        item.m_nArmorValue = item.ComputeArmorClass();
      }
    }

    /// <summary>
    /// Sets the armor color of this item.
    /// </summary>
    /// <param name="slot">The armor color slot index to be assigned.</param>
    /// <param name="value">The new color to assign.</param>
    public void SetArmorColor(ItemAppearanceArmorColor slot, byte value)
    {
      int index = (int)slot;

      if (value <= 255 && index >= 0 && index <= 119)
      {
        //1.69 colors
        if (index <= 5)
        {
          byte[] layeredTextureColors = item.m_nLayeredTextureColors;
          layeredTextureColors[index] = value;
          item.m_nLayeredTextureColors = layeredTextureColors;
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
    /// Clears any per-part color overrides set for the specified part slot.
    /// </summary>
    /// <param name="modelSlot">The model portion of the slot to clear.</param>
    /// <param name="colorSlot">The color portion of the slot to clear.</param>
    public void ClearArmorPieceColor(ItemAppearanceArmorModel modelSlot, ItemAppearanceArmorColor colorSlot)
      => SetArmorPieceColor(modelSlot, colorSlot, 255);
  }
}
