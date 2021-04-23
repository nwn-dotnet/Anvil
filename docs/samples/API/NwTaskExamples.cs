/*
 * Example usages of all Async APIs.
 */

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NWN.API;
using NWN.Services;

[ServiceBinding(typeof(NwTaskExamples))]
public class NwTaskExamples
{
  public NwTaskExamples()
  {
    DoAsyncStuff();
  }

  private async void DoAsyncStuff()
  {
    // Do some heavy work on another thread using a standard task, then return to a safe script context.
    await Task.Run(() => Thread.Sleep(1000));
    await NwTask.SwitchToMainThread();

    // Wait for a frame, or a certain amount of frames to pass.
    await NwTask.NextFrame();
    await NwTask.DelayFrame(100);

    // Wait for 30 seconds to pass. (DelayCommand replacement)
    await NwTask.Delay(TimeSpan.FromSeconds(30));

    // Wait for a certain game period to pass.
    await NwTask.Delay(NwTimeSpan.FromRounds(2));
    await NwTask.Delay(NwTimeSpan.FromTurns(3));
    await NwTask.Delay(NwTimeSpan.FromHours(1));

    // Wait for an expression to evaluate to true
    await NwTask.WaitUntil(() => NwModule.Instance.Players.Count() > 5);

    // Wait for a value to change.
    await NwTask.WaitUntilValueChanged(() => NwModule.Instance.Players.Count());

    // Start some tasks.
    Task task1 = Task.Run(() => true); // Executed in the thread pool, you cannot use NWN APIs here.

    Task task2 = Task.Run(async () =>
    {
      // Executed in the thread pool, you cannot use NWN APIs here.
      await Task.Delay(TimeSpan.FromSeconds(5));
      return 20;
    });

    Task task3 = NwTask.Run(async () =>
    {
      // Executed in the server thread, you can use NWN APIs here.
      await NwTask.Delay(NwTimeSpan.FromRounds(5));
      NwModule.Instance.SendMessageToAllDMs("5 rounds elapsed!");
      return 20;
    });

    // ...wait for any of them to complete.
    await NwTask.WhenAny(task1, task2, task3);

    // ...wait for all of them to complete.
    await NwTask.WhenAll(task1, task2, task3);
  }
}
