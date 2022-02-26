namespace Anvil.Services
{
  internal interface ICoreService
  {
    void Init();
    void Load();
    void Unload();
    void Shutdown();
  }
}
