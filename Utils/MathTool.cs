using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mianen.MathTool
{
  public static class BoolMath
  {
    /// <summary>
    /// Return 
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static bool OnlyOne (params bool[] p)
    {
      int occ = 0;
      for (int i = 0; i < p.Length; i++)
      {
        occ += (p[i]) ? 1 : 0;
      }
      return occ == 1;
    }
  }
}
