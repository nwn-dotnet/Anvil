namespace NWM.Core
{
  public interface ITwoDimArray
  {
    void DeserializeRow(int rowIndex, TwoDimEntry twoDimEntry);
  }
}