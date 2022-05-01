using System;
using System.Collections.Generic;
using Anvil.Services;

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

    internal NuiWindowToken(NwPlayer player, int token, string windowId = "")
    {
      Player = player;
      Token = token;
      WindowId = windowId;
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
    public string WindowId { get; }

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
        Player.NuiDestroy(Token);
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
  }
}
