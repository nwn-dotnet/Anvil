using NWN.Plugins;
using SimpleInjector;

namespace NWN.Services
{
  public class InstallerInfo
  {
    public Container CoreContainer;
    public Container ServiceContainer;
    public ServiceManager ServiceManager;
    public ITypeLoader TypeLoader;
  }
}