using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Anvil.Internal;
using Anvil.Services;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Base class for all world entities (game objects) and containers (modules, areas).
  /// </summary>
  [DebuggerDisplay("{" + nameof(Name) + "}")]
  [ObjectFilter(ObjectTypes.All)]
  public abstract partial class NwObject(ICGameObject gameObject) : IEquatable<NwObject>
  {
    internal const uint Invalid = NWScript.OBJECT_INVALID;

    [Inject]
    private protected static EventService EventService { get; private set; } = null!;

    [Inject]
    private protected static Lazy<ObjectVisibilityService> ObjectVisibilityService { get; private set; } = null!;

    [Inject]
    private protected static ResourceManager ResourceManager { get; private set; } = null!;

    [Inject]
    private protected static VirtualMachine VirtualMachine { get; private set; } = null!;

    /// <summary>
    /// The ID of this object instance. Not persistent, changes after every spawn of the object.<br/>
    /// See <see cref="UUID"/> for a persistent unique ID for objects.
    /// </summary>
    public readonly uint ObjectId = gameObject.m_idSelf;

    internal ICGameObject Object
    {
      get
      {
        AssertObjectValid();
        return gameObject;
      }
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
    /// Gets a value indicating whether this object has an assigned UUID.
    /// </summary>
    public bool HasUUID => PeekUUID() != null;

    /// <summary>
    /// Gets a value indicating whether this is a valid object.
    /// </summary>
    public bool IsValid => LowLevel.ServerExoApp.GetGameObject(ObjectId) != null;

    /// <summary>
    /// Gets all local variables assigned on this object.
    /// </summary>
    public IEnumerable<ObjectVariable> LocalVariables
    {
      get
      {
        foreach ((CExoString key, CNWSScriptVar value) in ScriptVarTable.m_vars)
        {
          string keyValue = key.ToString()!;
          if (value.HasFloat())
          {
            yield return ObjectVariable.Create<LocalVariableFloat>(this, keyValue);
          }

          if (value.HasInt())
          {
            yield return ObjectVariable.Create<LocalVariableInt>(this, keyValue);
          }

          if (value.HasLocation())
          {
            yield return ObjectVariable.Create<LocalVariableLocation>(this, keyValue);
          }

          if (value.HasObject())
          {
            yield return ObjectVariable.Create<LocalVariableObject<NwObject>>(this, keyValue);
          }

          if (value.HasString())
          {
            yield return ObjectVariable.Create<LocalVariableString>(this, keyValue);
          }
        }
      }
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
    public string OriginalDescription => NWScript.GetDescription(this, true.ToInt());

    /// <summary>
    /// Gets the resource reference used to create this object.
    /// </summary>
    public string ResRef => NWScript.GetResRef(this);

    /// <summary>
    /// Gets or sets the tag for this object.
    /// </summary>
    public string Tag
    {
      get => NWScript.GetTag(this);
      set => NWScript.SetTag(this, value);
    }

    /// <summary>
    /// Gets the globally unique identifier for this object.
    /// </summary>
    /// <remarks>
    /// If the UUID conflicts with an existing object, a new one will be generated.<br/>
    /// Use <see cref="TryGetUUID"/> to control this behaviour.
    /// </remarks>
    public Guid UUID
    {
      get
      {
        if (!IsValid)
        {
          return Guid.Empty;
        }

        if (!TryGetUUID(out Guid uid))
        {
          ForceRefreshUUID();
          TryGetUUID(out uid);
        }

        return uid;
      }
    }

    internal abstract CNWSScriptVarTable ScriptVarTable { get; }

    public static bool operator ==(NwObject? left, NwObject? right)
    {
      return Equals(left, right);
    }

    public static implicit operator uint(NwObject? gameObject)
    {
      return gameObject == null ? Invalid : gameObject.ObjectId;
    }

    public static bool operator !=(NwObject? left, NwObject? right)
    {
      return !Equals(left, right);
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
    public void ClearActionQueue(bool clearCombatState = false)
    {
      NWScript.ClearAllActions(clearCombatState.ToInt(), this);
    }

    /// <summary>
    /// Clears any event subscriptions associated with this object.
    /// </summary>
    public void ClearEventSubscriptions()
    {
      EventService.ClearObjectSubscriptions(this);
    }

    public bool Equals(NwObject? other)
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

    public override bool Equals(object? obj)
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

    public void ForceRefreshUUID()
    {
      NWScript.ForceRefreshObjectUUID(this);
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

    public override int GetHashCode()
    {
      return unchecked((int)ObjectId);
    }

    /// <summary>
    /// Gets the specified object variable for this object.
    /// </summary>
    /// <param name="name">The variable name.</param>
    /// <typeparam name="T">The variable type.</typeparam>
    /// <returns>A LocalVariable instance for getting/setting the variable's value.</returns>
    public T GetObjectVariable<T>(string name) where T : ObjectVariable, new()
    {
      return ObjectVariable.Create<T>(this, name);
    }

    /// <summary>
    /// Gets a value indicating whether the event script can be modified for the specified event.
    /// </summary>
    /// <param name="eventType">The event type to query.</param>
    /// <returns>True if the event is locked and the script cannot be modified, otherwise false.</returns>
    public bool IsEventLocked(EventScriptType eventType)
    {
      return GetEventScript(eventType).IsReservedScriptName();
    }

    /// <summary>
    /// Attempts to get the UUID of this object, if assigned.
    /// </summary>
    /// <returns>The UUID if assigned, otherwise no value.</returns>
    public abstract Guid? PeekUUID();

    /// <summary>
    /// Sets the script to be run on the specified object event.
    /// </summary>
    /// <param name="eventType">The event to be assigned.</param>
    /// <param name="script">The new script to assign to this event.</param>
    /// <exception cref="InvalidOperationException">Thrown if setting the event script failed. This can be from an invalid event script type, or this event is locked as a service has subscribed to this event. See <see cref="IsEventLocked"/> to determine if an event script can be changed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified script name is invalid.</exception>
    public void SetEventScript(EventScriptType eventType, string? script)
    {
      if (IsEventLocked(eventType))
      {
        throw new InvalidOperationException("The specified event has already been subscribed by an event handler and cannot be modified.");
      }

      if (!script.IsValidScriptName(true))
      {
        throw new ArgumentOutOfRangeException(nameof(script), $"The specified script name '{script}' is invalid.");
      }

      if (!NWScript.SetEventScript(this, (int)eventType, script!).ToBool())
      {
        throw new InvalidOperationException("The event script failed to apply. Are you using the correct script type?");
      }
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
    /// The ID of this object as a string. Can be used in <see cref="StringExtensions.ParseObject"/> while the object is alive.<br/>
    /// This cannot be used across server restarts. See <see cref="UUID"/> for a persistent unique identifier.
    /// </summary>
    public override string ToString()
    {
      return ObjectId.ToString("x");
    }

    /// <summary>
    /// Attempts to get the UUID for this object, assigning a new ID if it does not already exist.<br/>
    /// </summary>
    /// <remarks>See <see cref="PeekUUID"/> to check if the object has an existing UUID, without creating a new one.<br/>
    /// This function will return false if the UUID is not globally unique, and conflicts with an existing object.
    /// </remarks>
    /// <param name="uid">The object's UUID.</param>
    /// <returns>True if the object has a valid unique identifier, otherwise false.</returns>
    public bool TryGetUUID(out Guid uid)
    {
      string uidString = NWScript.GetObjectUUID(this);
      if (!string.IsNullOrEmpty(uidString))
      {
        return Guid.TryParse(uidString, out uid);
      }

      uid = Guid.Empty;
      return false;
    }

    /// <summary>
    /// Serializes this game object to a json representation
    /// </summary>
    public Json SerializeToJson(bool saveObjectState)
    {
      return NWScript.ObjectToJson(this, saveObjectState.ToInt());
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

      AssertObjectValid();

      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      NWScript.AssignCommand(this, () => { tcs.SetResult(true); });

      await tcs.Task;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected void AssertObjectValid()
    {
      if (LowLevel.ServerExoApp.GetGameObject(ObjectId)?.Pointer != gameObject.Pointer)
      {
        throw new InvalidOperationException("Object is not valid.");
      }
    }
  }
}
