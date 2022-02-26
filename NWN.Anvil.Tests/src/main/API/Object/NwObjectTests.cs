using System.Collections.Generic;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Tests.Resources;
using NUnit.Framework;
using NWN.Core;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwObjectTests
  {
    private readonly List<NwGameObject> createdTestObjects = new List<NwGameObject>();

    [Test(Description = "Tests if assigning an action/closure using WaitForObjectContext correctly updates the script context.")]
    [Timeout(2000)]
    public async Task WaitForObjectContextEntersCorrectContext()
    {
      NwModule module = NwModule.Instance;

      await module.WaitForObjectContext();

      Assert.That(NWScript.OBJECT_SELF.ToNwObject(), Is.EqualTo(module));

      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation);
      createdTestObjects.Add(creature);

      await creature.WaitForObjectContext();

      Assert.That(NWScript.OBJECT_SELF.ToNwObject(), Is.EqualTo(creature));
    }

    [Test(Description = "Tests if adding an action correctly queues an action on the game object.")]
    [Timeout(2000)]
    public async Task QueueCreatureActionIsQueued()
    {
      NwCreature creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation);
      createdTestObjects.Add(creature);

      bool actionExecuted = false;
      await creature.AddActionToQueue(() =>
      {
        actionExecuted = true;
      });

      await NwTask.WaitUntil(() => actionExecuted);
      Assert.That(actionExecuted, Is.EqualTo(true));
    }

    [TearDown]
    public void CleanupTestObjects()
    {
      foreach (NwGameObject testObject in createdTestObjects)
      {
        testObject.PlotFlag = false;
        testObject.Destroy();
      }

      createdTestObjects.Clear();
    }
  }
}
