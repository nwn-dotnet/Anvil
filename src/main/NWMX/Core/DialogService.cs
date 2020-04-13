using NWM.Core;
using NWNX;

namespace NWMX.Core
{
  [Service]
  public class DialogService
  {
    public string CurrentNodeText
    {
      get => DialogPlugin.GetCurrentNodeText();
      set => DialogPlugin.SetCurrentNodeText(value);
    }
  }
}