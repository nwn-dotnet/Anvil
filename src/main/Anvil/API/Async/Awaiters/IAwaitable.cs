namespace Anvil.API
{
  public interface IAwaitable
  {
    IAwaiter GetAwaiter();
  }
}
