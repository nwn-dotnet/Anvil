namespace Anvil.Services
{
  internal interface ICoreService
  {
    void Init();
    void Load();
    void Start();
    void Shutdown();
    void Unload();
  }
}
