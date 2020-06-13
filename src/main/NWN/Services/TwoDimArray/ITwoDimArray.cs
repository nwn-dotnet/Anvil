namespace NWN.Services
{
  public interface ITwoDimArray
  {
    void DeserializeRow(int rowIndex, TwoDimEntry twoDimEntry);
  }
}