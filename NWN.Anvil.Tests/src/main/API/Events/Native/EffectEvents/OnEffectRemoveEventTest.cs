using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Anvil.Tests.Resources;
using NUnit.Framework;

namespace Anvil.Tests.API.Events
{
  [TestFixture(Category = "API.Events.Native")]
  public sealed class OnEffectRemoveEventTest
  {
    private const string EffectTag = "test_tag";

    [Inject]
    private static EventService EventService { get; set; } = null!;

    private NwCreature? creature;

    private readonly List<EffectEventData> beforeEvents = [];
    private readonly List<EffectEventData> afterEvents = [];

    [SetUp]
    public void Setup()
    {
      creature = NwCreature.Create(StandardResRef.Creature.nw_bandit001, NwModule.Instance.StartingLocation);
    }

    [Test]
    public async Task EffectRemoveAreEventsCalled()
    {
      Assert.That(creature, Is.Not.Null);

      creature!.OnEffectRemove += OnEffectRemoveBefore;
      EventService.Subscribe<OnEffectRemove, OnEffectRemove.Factory>(creature, OnEffectRemoveAfter, EventCallbackType.After);

      Effect effect = Effect.Blindness();
      effect.Tag = EffectTag;

      creature.ApplyEffect(EffectDuration.Temporary, effect, TimeSpan.FromSeconds(2));

      await NwTask.Delay(TimeSpan.FromSeconds(3));

      Assert.Multiple(() =>
      {
        Assert.That(beforeEvents, Has.Count.EqualTo(1));
        Assert.That(afterEvents, Has.Count.EqualTo(1));
      });

      EffectEventData beforeEventData = beforeEvents.First();
      EffectEventData afterEventData = afterEvents.First();
      Assert.Multiple(() =>
      {
        Assert.That(beforeEventData, Is.Not.Null);
        Assert.That(beforeEventData.EffectType, Is.EqualTo(EffectType.Blindness));
        Assert.That(beforeEventData.Tag, Is.EqualTo(EffectTag));
        Assert.That(beforeEventData.DurationType, Is.EqualTo(EffectDuration.Temporary));

        Assert.That(afterEventData, Is.Not.Null);
        Assert.That(afterEventData.EffectType, Is.EqualTo(EffectType.Blindness));
        Assert.That(afterEventData.Tag, Is.EqualTo(EffectTag));
        Assert.That(afterEventData.DurationType, Is.EqualTo(EffectDuration.Temporary));
      });
    }

    private void OnEffectRemoveBefore(OnEffectRemove eventData)
    {
      Effect effect = eventData.Effect;
      if (effect.Tag != EffectTag)
      {
        return;
      }

      beforeEvents.Add(new EffectEventData
      {
        EffectType = effect.EffectType,
        Tag = effect.Tag,
        DurationType = effect.DurationType,
      });
    }

    private void OnEffectRemoveAfter(OnEffectRemove eventData)
    {
      Effect effect = eventData.Effect;
      if (effect.Tag != EffectTag)
      {
        return;
      }

      afterEvents.Add(new EffectEventData
      {
        EffectType = effect.EffectType,
        Tag = effect.Tag,
        DurationType = effect.DurationType,
      });
    }

    [TearDown]
    public void TearDown()
    {
      if (creature != null)
      {
        creature.ClearEventSubscriptions();
        creature.Destroy();
      }
    }

    private sealed class EffectEventData
    {
      public required EffectType EffectType { get; init; }
      public required string? Tag { get; init; }
      public required EffectDuration DurationType { get; init; }
    }
  }
}
