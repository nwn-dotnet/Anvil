using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API
{
  [DebuggerDisplay("{" + nameof(Name) + "}")]
  public abstract partial class NwObject : IEquatable<NwObject>
  {
    private protected static EventService EventService { get; private set; }

    private protected static ResourceManager ResourceManager { get; private set; }

    [ServiceBinding(typeof(APIBindings))]
    [ServiceBindingOptions(BindingOrder.API)]
    internal sealed class APIBindings
    {
      public APIBindings(EventService eventService, ResourceManager resourceManager)
      {
        EventService = eventService;
        ResourceManager = resourceManager;
      }
    }

    internal const uint Invalid = NWScript.OBJECT_INVALID;

    [UsedImplicitly]
    internal readonly ICGameObject Object;

    protected readonly uint ObjectId;

    protected NwObject(ICGameObject gameObject)
    {
      Object = gameObject;
      ObjectId = gameObject.m_idSelf;
    }

    public static implicit operator uint(NwObject gameObject)
    {
      return gameObject == null ? Invalid : gameObject.ObjectId;
    }

    internal abstract CNWSScriptVarTable ScriptVarTable { get; }

    /// <summary>
    /// Gets the globally unique identifier for this object.
    /// </summary>
    public Guid UUID
    {
      get
      {
        if (this == Invalid)
        {
          return Guid.Empty;
        }

        // TODO - Better Handle UUID conflicts.
        string uid = NWScript.GetObjectUUID(this);
        if (string.IsNullOrEmpty(uid))
        {
          ForceRefreshUUID();
          uid = NWScript.GetObjectUUID(this);
        }

        return Guid.TryParse(uid, out Guid guid) ? guid : Guid.Empty;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this object has an assigned UUID.
    /// </summary>
    public bool HasUUID
    {
      get => PeekUUID() != null;
    }

    /// <summary>
    /// Gets the resource reference used to create this object.
    /// </summary>
    public string ResRef
    {
      get => NWScript.GetResRef(this);
    }

    /// <summary>
    /// Gets a value indicating whether this is a valid object.
    /// </summary>
    public bool IsValid
    {
      get => NWScript.GetIsObjectValid(this).ToBool();
    }

    /// <summary>
    /// Gets or sets the name of this object.
    /// </summary>
    public string Name
    {
      get => NWScript.GetName(this);
      set => NWScript.SetName(this, value);
    }

    /// <summary>
    /// Gets the original description for this object as defined in the toolset.
    /// </summary>
    public string OriginalDescription
    {
      get => NWScript.GetDescription(this, true.ToInt());
    }

    /// <summary>
    /// Gets or sets the description for this object.
    /// </summary>
    public string Description
    {
      get => NWScript.GetDescription(this);
      set => NWScript.SetDescription(this, value);
    }

    /// <summary>
    /// Gets or sets the tag for this object.
    /// </summary>
    public string Tag
    {
      get => NWScript.GetTag(this);
      set => NWScript.SetTag(this, value);
    }

    /// <summary>
    /// Gets all local variables assigned on this object.
    /// </summary>
    public IEnumerable<LocalVariable> LocalVariables
    {
      get
      {
        foreach ((CExoString key, CNWSScriptVar value) in ScriptVarTable.m_vars)
        {
          if (value.HasFloat())
          {
            yield return LocalVariable<float>.Create(this, key.ToString());
          }

          if (value.HasInt())
          {
            yield return LocalVariable<int>.Create(this, key.ToString());
          }

          if (value.HasLocation())
          {
            yield return LocalVariable<Location>.Create(this, key.ToString());
          }

          if (value.HasObject())
          {
            yield return LocalVariable<NwObject>.Create(this, key.ToString());
          }

          if (value.HasString())
          {
            yield return LocalVariable<string>.Create(this, key.ToString());
          }
        }
      }
    }

    /// <summary>
    /// Notifies then awaits for this object to become the current active object for the purpose of implicitly assigned values (e.g. effect creators).<br/>
    /// If the current active object is already this object, then the code runs immediately. Otherwise, it will be run with all other closures.<br/>
    /// This is the async equivalent of AssignCommand in NWScript.
    /// </summary>
    public async Task WaitForObjectContext()
    {
      if (NWScript.OBJECT_SELF == this)
      {
        return;
      }

      if (!IsValid)
      {
        throw new InvalidOperationException("Cannot wait for the context of an invalid object.");
      }

      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      NWScript.AssignCommand(this, () => { tcs.SetResult(true); });

      await tcs.Task;
    }

    /// <summary>
    /// Inserts the function call aCommand into the Action Queue, ensuring the calling object will perform actions in a particular order.
    /// </summary>
    public async Task AddActionToQueue(System.Action action)
    {
      await WaitForObjectContext();
      NWScript.ActionDoCommand(action);
    }

    /// <summary>
    ///  Clear all the object's actions.
    ///  * No return value, but if an error occurs, the log file will contain
    ///    "ClearAllActions failed.".
    ///  - clearCombatState: if true, this will immediately clear the combat state
    ///    on a creature, which will stop the combat music and allow them to rest,
    ///    engage in dialog, or other actions that they would normally have to wait for.
    /// </summary>
    public async Task ClearActionQueue(bool clearCombatState = false)
    {
      await WaitForObjectContext();
      NWScript.ClearAllActions(clearCombatState.ToInt());
    }

    /// <summary>
    /// Instructs this object to speak.
    /// </summary>
    /// <param name="message">The message the object should speak.</param>
    /// <param name="talkVolume">The channel/volume of this message.</param>
    /// <param name="queueAsAction">Whether the object should speak immediately (false), or be queued in the object's action queue (true).</param>
    public async Task SpeakString(string message, TalkVolume talkVolume = TalkVolume.Talk, bool queueAsAction = false)
    {
      await WaitForObjectContext();
      if (!queueAsAction)
      {
        NWScript.SpeakString(message, (int)talkVolume);
      }
      else
      {
        NWScript.ActionSpeakString(message, (int)talkVolume);
      }
    }

    /// <summary>
    /// Gets the specified local variable for this object.
    /// </summary>
    /// <param name="name">The variable name.</param>
    /// <typeparam name="T">The variable type.</typeparam>
    /// <returns>A LocalVariable instance for getting/setting the variable's value.</returns>
    public LocalVariable<T> GetLocalVariable<T>(string name)
    {
      return LocalVariable<T>.Create(this, name);
    }

    /// <summary>
    /// Attempts to get the UUID of this object, if assigned.
    /// </summary>
    /// <returns>The UUID if assigned, otherwise no value.</returns>
    public abstract Guid? PeekUUID();

    public void ForceRefreshUUID()
    {
      NWScript.ForceRefreshObjectUUID(this);
    }

    /// <summary>
    /// Clears any event subscriptions associated with this object.
    /// </summary>
    public void ClearEventSubscriptions()
    {
      EventService.ClearObjectSubscriptions(this);
    }

    /// <summary>
    /// Gets the script assigned to run for the specified object event.
    /// </summary>
    /// <param name="eventType">The event type to query.</param>
    /// <returns>The script that has been assigned to the event, otherwise an <see cref="string.Empty"/> string.</returns>
    public string GetEventScript(EventScriptType eventType)
    {
      return NWScript.GetEventScript(this, (int)eventType);
    }

    /// <summary>
    /// Sets the script to be run on the specified object event.
    /// </summary>
    /// <param name="eventType">The event to be assigned.</param>
    /// <param name="script">The new script to assign to this event.</param>
    /// <exception cref="InvalidOperationException">Thrown if this event is locked as a service has subscribed to this event. See <see cref="IsEventLocked"/> to determine if an event script can be changed.</exception>
    public void SetEventScript(EventScriptType eventType, string script)
    {
      if (IsEventLocked(eventType))
      {
        throw new InvalidOperationException("The specified event has already been subscribed by an event handler and cannot be modified.");
      }

      if (script.IsValidScriptName())
      {
        NWScript.SetEventScript(this, (int)eventType, script);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the event script can be modified for the specified event.
    /// </summary>
    /// <param name="eventType">The event type to query.</param>
    /// <returns>True if the event is locked and the script cannot be modified, otherwise false.</returns>
    public bool IsEventLocked(EventScriptType eventType)
    {
      string current = GetEventScript(eventType);
      return current is ScriptConstants.GameEventScriptName or ScriptConstants.NWNXEventScriptName;
    }

    /// <summary>
    /// The ID of this object as a string. Can be used in <see cref="StringExtensions.ParseObject"/> while the object is alive.<br/>
    /// This cannot be used across server restarts. See <see cref="UUID"/> for a persistent unique identifier.
    /// </summary>
    public override string ToString()
    {
      return ObjectId.ToString("x");
    }

    public bool Equals(NwObject other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return ObjectId == other.ObjectId;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      return Equals((NwObject)obj);
    }

    public override int GetHashCode()
    {
      return (int)ObjectId;
    }

    public static bool operator ==(NwObject left, NwObject right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(NwObject left, NwObject right)
    {
      return !Equals(left, right);
    }
  }
}
