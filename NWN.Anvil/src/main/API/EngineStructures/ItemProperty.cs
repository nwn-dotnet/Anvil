using System;
using System.Linq;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  //! ## Examples
  //! @include ItemPropertyExamples.cs

  /// <summary>
  /// Represents an item property effect engine structure.
  /// </summary>
  public sealed partial class ItemProperty : EffectBase
  {
    internal ItemProperty(CGameEffect effect, bool memoryOwn) : base(effect, memoryOwn) {}

    /// <summary>
    /// Gets the cost table used by this item property.
    /// </summary>
    public TwoDimArray<ItemPropertyCostTableEntry>? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(Effect.GetInteger(2))?.Table;

    /// <summary>
    /// Gets or sets the cost table entry that is set for this item property.<br/>
    /// </summary>
    public ItemPropertyCostTableEntry? CostTableValue
    {
      get
      {
        int tableIndex = Effect.GetInteger(3);
        TwoDimArray<ItemPropertyCostTableEntry>? table = CostTable;
        if (tableIndex >= 0 && table != null && tableIndex < table.Count)
        {
          return table[tableIndex];
        }

        return null;
      }
      set => Effect.SetInteger(3, value?.RowIndex ?? -1);
    }

    /// <summary>
    /// Gets or sets whether this item property is a permanent or temporary effect.
    /// </summary>
    public EffectDuration DurationType
    {
      get => (EffectDuration)Effect.m_nSubType;
      set => Effect.m_nSubType = (ushort)value;
    }

    /// <summary>
    /// Gets the #1 param table used by this item property.
    /// </summary>
    public TwoDimArray<ItemPropertyParamTableEntry>? Param1Table => NwGameTables.ItemPropertyParamTables.ElementAtOrDefault(Effect.GetInteger(4))?.Table;

    /// <summary>
    /// Gets or sets the #1 param table entry that is set for this item property.<br/>
    /// </summary>
    public ItemPropertyParamTableEntry? Param1TableValue
    {
      get
      {
        int tableIndex = Effect.GetInteger(5);
        TwoDimArray<ItemPropertyParamTableEntry>? table = Param1Table;

        if (tableIndex >= 0 && table != null && tableIndex < table.Count)
        {
          return table[tableIndex];
        }

        return null;
      }
      set => Effect.SetInteger(5, value?.RowIndex ?? -1);
    }

    /// <summary>
    /// Gets the base item property used to create this item property.
    /// </summary>
    public ItemPropertyTableEntry Property => NwGameTables.ItemPropertyTable[Effect.GetInteger(0)];

    /// <summary>
    /// Gets the remaining duration until the item property expires (if this item property is temporary). Otherwise, returns <see cref="TimeSpan.Zero"/>.
    /// </summary>
    public TimeSpan RemainingDuration => TimeSpan.FromSeconds(NWScript.GetItemPropertyDurationRemaining(this));

    /// <summary>
    /// Gets the sub type table used by this item property.
    /// </summary>
    public TwoDimArray<ItemPropertySubTypeTableEntry>? SubTypeTable => Property.SubTypeTable;

    /// <summary>
    /// Gets or sets the sub type that is set on this item property.
    /// </summary>
    public ItemPropertySubTypeTableEntry? SubType
    {
      get => Property.SubTypeTable?.ElementAtOrDefault(Effect.GetInteger(1));
      set => Effect.SetInteger(1, value?.RowIndex ?? -1);
    }

    /// <summary>
    /// Gets or sets the tag for this item property.
    /// </summary>
    public string? Tag
    {
      get => Effect.GetString(0).ToString();
      set => Effect.SetString(0, value.ToExoString());
    }

    /// <summary>
    /// Gets the total duration of the item property effect (if this item property is temporary). Otherwise, returns <see cref="TimeSpan.Zero"/>.
    /// </summary>
    public TimeSpan TotalDuration => TimeSpan.FromSeconds(NWScript.GetItemPropertyDuration(this));

    public bool Usable
    {
      get => Effect.GetInteger(8).ToBool();
      set => Effect.SetInteger(8, value.ToInt());
    }

    public int UsesPerDay
    {
      get => Effect.GetInteger(6);
      set => Effect.SetInteger(6, value);
    }

    /// <summary>
    /// Gets a value indicating whether this item property is valid.
    /// </summary>
    public bool Valid => NWScript.GetIsItemPropertyValid(this).ToBool();

    protected override int StructureId => NWScript.ENGINE_STRUCTURE_ITEMPROPERTY;

    public static implicit operator ItemProperty?(IntPtr intPtr)
    {
      return intPtr != IntPtr.Zero ? new ItemProperty(CGameEffect.FromPointer(intPtr), true) : null;
    }
  }
}
