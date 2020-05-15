using SimpleInjector;

namespace NWM.Core
{
  public interface IBindingInstaller
  {
    void ConfigureBindings(Container container);
  }
}