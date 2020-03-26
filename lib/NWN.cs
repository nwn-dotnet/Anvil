using System;
using System.Runtime.InteropServices;

namespace NWN
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Vector
    {
        public float x;
        public float y;
        public float z;

        public Vector(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public partial class Effect
    {
        public IntPtr Handle;
        internal Effect(IntPtr handle) { Handle = handle; }
        ~Effect() { Internal.NativeFunctions.FreeEffect(Handle); }
    }

    public partial class Event
    {
        public IntPtr Handle;
        internal Event(IntPtr handle) { Handle = handle; }
        ~Event() { Internal.NativeFunctions.FreeEvent(Handle); }
    }

    public partial class Location
    {
        public IntPtr Handle;
        internal Location(IntPtr handle) { Handle = handle; }
        ~Location() { Internal.NativeFunctions.FreeLocation(Handle); }
    }

    public partial class Talent
    {
        public IntPtr Handle;
        internal Talent(IntPtr handle) { Handle = handle; }
        ~Talent() { Internal.NativeFunctions.FreeTalent(Handle); }
    }

    public partial class ItemProperty
    {
        public IntPtr Handle;
        internal ItemProperty(IntPtr handle) { Handle = handle; }
        ~ItemProperty() { Internal.NativeFunctions.FreeItemProperty(Handle); }
    }

    public delegate void ActionDelegate();
}
