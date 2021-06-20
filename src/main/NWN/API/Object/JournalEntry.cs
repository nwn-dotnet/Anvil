using Anvil.Internal;

namespace NWN.API
{
  public sealed class JournalEntry
  {
    public string Name { get; set; }

    public string Text { get; set; }

    public string QuestTag { get; set; }

    public uint State { get; set; }

    public uint Priority { get; set; }

    public bool QuestCompleted { get; set; }

    public bool QuestDisplayed { get; set; }

    public bool Updated { get; set; }

    public uint CalendarDay { get; set; } = LowLevel.ServerExoApp.GetWorldTimer().GetWorldTimeCalendarDay();

    public uint TimeOfDay { get; set; } = LowLevel.ServerExoApp.GetWorldTimer().GetWorldTimeTimeOfDay();
  }
}
