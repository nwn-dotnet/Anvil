using System;
using System.Collections.Generic;
using System.IO;
using Anvil.API;
using Anvil.Services;
using NLog;

namespace Anvil.Tests.Generators
{
  internal sealed class Palette
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const string StandardPaletteSuffix = "std";
    private readonly string standardPaletteResRef;

    [Inject]
    public ResourceManager ResourceManager { private get; init; }

    [Inject]
    public TlkTable TlkTable { private get; init; }

    private readonly List<PaletteEntry> blueprints = new List<PaletteEntry>();

    public Palette(string palettePrefix)
    {
      standardPaletteResRef = palettePrefix + StandardPaletteSuffix;
    }

    public List<PaletteEntry> GetBlueprints()
    {
      blueprints.Clear();
      TryLoadPalette(standardPaletteResRef, "Standard");
      return blueprints;
    }

    private void TryLoadPalette(string resRef, string rootPath)
    {
      using GffResource palette = ResourceManager.GetGenericFile(resRef, ResRefType.ITP);
      if (palette == null)
      {
        Log.Error("Failed to load palette {Palette}", resRef);
        return;
      }

      try
      {
        ProcessList(palette["MAIN"], rootPath);
      }
      catch (Exception e)
      {
        Log.Error(e, "Failed to parse palette file {Palette}", resRef);
      }
    }

    private void ProcessList(GffResourceField field, string path)
    {
      foreach (GffResourceField child in field.Values)
      {
        ProcessStruct(child, path);
      }
    }

    private void ProcessStruct(GffResourceField field, string path)
    {
      if (field.TryGetValue("RESREF", out GffResourceField resRefField))
      {
        string resRef = resRefField.Value<string>();
        string name = "Unknown";

        if (field.TryGetValue("NAME", out GffResourceField creatureNameField))
        {
          name = creatureNameField.Value<string>();
        }
        else if (field.TryGetValue("STRREF", out GffResourceField creatureNameStrRefField))
        {
          name = TlkTable.GetSimpleString(creatureNameStrRefField.Value<uint>());
        }

        blueprints.Add(new PaletteEntry
        {
          ResRef = resRef,
          Name = path + "/" + name,
        });
      }
      else
      {
        if (field.TryGetValue("STRREF", out GffResourceField groupStrRef))
        {
          path = Path.Combine(path, TlkTable.GetSimpleString(groupStrRef.Value<uint>()));
        }

        if (field.TryGetValue("LIST", out GffResourceField list))
        {
          ProcessList(list, path);
        }
      }
    }
  }
}
