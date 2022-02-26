namespace Anvil.Services
{
  internal interface ICoreService
  {
    void Init();
    void Load();
    void Shutdown();
    void Unload();
  }
}
