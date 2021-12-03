using Anvil.Internal;

namespace Anvil.API
{
  public sealed class JournalEntry
  {
    public uint CalendarDay { get; set; } = LowLevel.ServerExoApp.GetWorldTimer().GetWorldTimeCalendarDay();
    public string Name { get; set; }

    public uint Priority { get; set; }

    public bool QuestCompleted { get; set; }

    public bool QuestDisplayed { get; set; }

    public string QuestTag { get; set; }

    public uint State { get; set; }

    public string Text { get; set; }

    public uint TimeOfDay { get; set; } = LowLevel.ServerExoApp.GetWorldTimer().GetWorldTimeTimeOfDay();

    public bool Updated { get; set; }
  }
}
