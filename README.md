# NWM
NWM is a C# library that attempts to wrap NWScript with C# niceties and contexts, instead of a collection of functions.

# Getting Started

## Bootstrap
To initialize the managed system you can do this:

```csharp
namespace NWN
{
  public static class Internal
  {
    public static int Bootstrap(IntPtr arg, int argLength) => NWM.Main.Bootstrap(arg, argLength);
  }
}
```

The class path (`NWN.Internal`) should match the `ENTRYPOINT` environmental variable as defined in the [NWNX:EE config](https://nwnxee.github.io/unified/group__dotnet.html#dotnet)

# Services
TODO
