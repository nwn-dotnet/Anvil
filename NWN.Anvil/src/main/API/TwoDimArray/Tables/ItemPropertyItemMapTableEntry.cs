using System.Collections.Generic;

namespace Anvil.API
{
  public sealed class ItemPropertyItemMapTableEntry : ITwoDimArrayEntry
  {
    /// <summary>
    /// Gets a dictionary mapping each base item type to a boolean indicating if the property is valid.
    /// </summary>
    public IReadOnlyDictionary<NwBaseItem, bool> ValidItems { get; private set; } = null!;

    /// <summary>
    /// Gets the StrRef of the item property.
    /// </summary>
    public StrRef? StrRef { get; private set; }

    /// <summary>
    /// Gets the label of the item property.
    /// </summary>
    public string? Label { get; private set; }

    public int RowIndex { get; init; }

    /// <summary>
    /// Gets if this item property is valid for the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the item is valid for this property, otherwise false.</returns>
    public bool IsItemPropertyValidForItem(NwItem item)
    {
      return IsItemPropertyValidForItem(item.BaseItem);
    }

    /// <summary>
    /// Gets if this item property is valid for the specified base item type.
    /// </summary>
    /// <param name="item">The item type to check.</param>
    /// <returns>True if the item is valid for this property, otherwise false.</returns>
    public bool IsItemPropertyValidForItem(NwBaseItem item)
    {
      ValidItems.TryGetValue(item, out bool retVal);
      return retVal;
    }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      StrRef = entry.GetStrRef("StringRef");
      Label = entry.GetString("Label");
      ValidItems = CalculateValidItemsForProperty(entry);
    }

    private IReadOnlyDictionary<NwBaseItem, bool> CalculateValidItemsForProperty(TwoDimArrayEntry entry)
    {
      bool[] isValid = new bool[entry.Columns.Length];
      for (int i = 0; i < entry.Columns.Length; i++)
      {
        string column = entry.Columns[i];
        if (column is not "StringRef" or "Label")
        {
          isValid[i] = entry.GetBool(i) == true;
        }
      }

      Dictionary<NwBaseItem, bool> retVal = new Dictionary<NwBaseItem, bool>();
      foreach (NwBaseItem item in NwRuleset.BaseItems)
      {
        if (item.ItemPropertyColumnId < isValid.Length)
        {
          retVal[item] = isValid[item.ItemPropertyColumnId];
        }
        else
        {
          retVal[item] = false;
        }
      }

      return retVal;
    }
  }
}
