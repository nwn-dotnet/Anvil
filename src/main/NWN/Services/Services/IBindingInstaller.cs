using SimpleInjector;

namespace NWN.Services
{
  public interface IBindingInstaller
  {
    void ConfigureBindings(Container container);
  }
}