using System.Collections.Generic;

namespace NWM.Core
{
  public interface I2da
  {
    void DeserializeRow(IEnumerable<string> columns);
  }
}