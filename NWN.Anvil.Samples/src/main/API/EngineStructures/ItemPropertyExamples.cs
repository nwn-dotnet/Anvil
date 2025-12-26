/*
 * Examples for creating, applying and removing item properties.
 */

using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(ItemPropertyExamples))]
  public class ItemPropertyExamples
  {
    // Most item properties can be declared as fields. This allows you to reuse effects, rather than creating them every time.
    private readonly ItemProperty hasteProperty = ItemProperty.Haste();

    public ItemPropertyExamples()
    {
      NwModule.Instance.OnItemEquip += TemporaryHasteExample;
      NwModule.Instance.OnClientEnter += RemoveItemPropertyExample;
    }

    /// <summary>
    /// Apply a haste effect for 5 rounds if someone equips an item with the "temp_haste" tag.
    /// </summary>
    private void TemporaryHasteExample(OnItemEquip eventData)
    {
      // Check if the tag matches the item that was equipped.
      if (eventData.Item.Tag == "temp_haste")
      {
        // Apply the haste item property.
        eventData.Item.AddItemProperty(hasteProperty, EffectDuration.Temporary, NwTimeSpan.FromRounds(5));
      }
    }

    /// <summary>
    /// Finds an item with the tag "sword_special", and removes any item property with the tag "special_temp".
    /// </summary>
    private void RemoveItemPropertyExample(ModuleEvents.OnClientEnter eventData)
    {
      // Find an item in the player's inventory with the tag "sword_special"
      NwItem? item = eventData.Player.ControlledCreature!.FindItemWithTag("sword_special");

      // If they don't have the item, early return.
      if (item == null)
      {
        return;
      }

      // Loop over the item's properties. Remove any that have a matching tag.
      foreach (ItemProperty property in item.ItemProperties)
      {
        if (property.Tag == "special_temp")
        {
          item.RemoveItemProperty(property);
        }
      }
    }
  }
}
