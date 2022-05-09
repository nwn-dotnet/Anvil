using System;
using System.Collections.Generic;
using Anvil.Services;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Represents a NUI window instance for a certain player.
  /// </summary>
  public readonly partial struct NuiWindowToken : IEquatable<NuiWindowToken>
  {
    public static NuiWindowToken Invalid = new NuiWindowToken(null, -1);

    [Inject]
    private static NuiWindowEventService NuiWindowEventService { get; set; }

    internal NuiWindowToken(NwPlayer player, int token)
    {
      Player = player;
      Token = token;
    }

    /// <summary>
    /// The player associated with this token.
    /// </summary>
    public NwPlayer Player { get; }

    /// <summary>
    /// The window unique token for this player.
    /// </summary>
    public int Token { get; }

    /// <summary>
    /// The user assigned window id of this token.<br/>
    /// Creating a new window with the same window id will replace the existing window.
    /// </summary>
    public string WindowId => NWScript.NuiGetWindowId(Player.ControlledCreature, Token);

    public static bool operator ==(NuiWindowToken left, NuiWindowToken right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(NuiWindowToken left, NuiWindowToken right)
    {
      return !left.Equals(right);
    }

    /// <summary>
    /// Close this window instance.
    /// </summary>
    public void Close()
    {
      Dispose();
    }

    public void Dispose()
    {
      if (Player != null && Player.IsValid)
      {
        NWScript.NuiDestroy(Player.ControlledCreature, Token);
      }
    }

    public bool Equals(NuiWindowToken other)
    {
      return Player.Equals(other.Player) && Token == other.Token;
    }

    public override bool Equals(object obj)
    {
      return obj is NuiWindowToken other && Equals(other);
    }

    /// <summary>
    /// Gets the current value of the specified bind for this window instance.
    /// </summary>
    /// <param name="bind">The bind value to query.</param>
    /// <typeparam name="T">The value type of the bind.</typeparam>
    /// <returns>The current assigned value of the bind, otherwise the default value of T.</returns>
    public T GetBindValue<T>(NuiBind<T> bind)
    {
      return bind.GetBindValue(Player, Token);
    }

    /// <summary>
    /// Gets the current values of the specified bind for this window instance.
    /// </summary>
    /// <param name="bind">The bind value to query.</param>
    /// <typeparam name="T">The value type of the bind.</typeparam>
    /// <returns>The current assigned value of the bind, otherwise the default value of T.</returns>
    public List<T> GetBindValues<T>(NuiBind<T> bind)
    {
      return bind.GetBindValues(Player, Token);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Player, Token);
    }

    /// <summary>
    /// Get the userdata of this token.
    /// </summary>
    /// <typeparam name="T">A serializable class structure matching the data to fetch.</typeparam>
    /// <returns>The fetched data, or null if the window does not exist on the given player, or has no userdata set.</returns>
    public T GetUserData<T>()
    {
      return JsonUtility.FromJson<T>(NWScript.NuiGetUserData(Player.ControlledCreature, Token));
    }

    /// <summary>
    /// Sets the current value of the specified bind for this window instance.
    /// </summary>
    /// <param name="bind">The bind value to assign.</param>
    /// <param name="value">The new value.</param>
    /// <typeparam name="T">The value type of the bind.</typeparam>
    public void SetBindValue<T>(NuiBind<T> bind, T value)
    {
      bind.SetBindValue(Player, Token, value);
    }

    /// <summary>
    /// Sets the current values of the specified bind for this window instance.
    /// </summary>
    /// <param name="bind">The bind value to assign.</param>
    /// <param name="values">The new values.</param>
    /// <typeparam name="T">The value type of the bind.</typeparam>
    public void SetBindValues<T>(NuiBind<T> bind, IEnumerable<T> values)
    {
      bind.SetBindValues(Player, Token, values);
    }

    /// <summary>
    /// Marks the specified bind as watched/unwatched for this player.
    /// </summary>
    /// <param name="bind"></param>
    /// <param name="watch"></param>
    /// <typeparam name="T"></typeparam>
    public void SetBindWatch<T>(NuiBind<T> bind, bool watch)
    {
      bind.SetBindWatch(Player, Token, watch);
    }

    /// <summary>
    /// Applies the specified group layout for this player.
    /// </summary>
    /// <param name="group">The group to apply the layout.</param>
    /// <param name="newLayout">The new layout.</param>
    public void SetGroupLayout(NuiGroup group, NuiLayout newLayout)
    {
      group.SetLayout(Player, Token, newLayout);
    }

    /// <summary>
    /// Sets an arbitrary json value as userdata on this token.<br/>
    /// This userdata is not read or handled by the game engine and not sent to clients.<br/>
    /// This mechanism only exists as a convenience for the programmer to store data bound to a windows' lifecycle.<br/>
    /// Will do nothing if the window does not exist.
    /// </summary>
    /// <param name="userData">The data to store.</param>
    /// <typeparam name="T">The type of data to store. Must be serializable to JSON.</typeparam>
    public void SetUserData<T>(T userData)
    {
      NWScript.NuiSetUserData(Player.ControlledCreature, Token, JsonUtility.ToJsonStructure(userData));
    }
  }
}
