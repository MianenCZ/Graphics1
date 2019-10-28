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

  public static class RangeMath
  {
    /// <summary>
    /// Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <returns>Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included</returns>
    public static bool IsInRange (float Value, float Lower, float Upper)
    {
      return ((Value >= Lower) && (Value <= Upper));
    }

    /// <summary>
    /// Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <returns>Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included</returns>
    public static bool IsInRange (double Value, double Lower, double Upper)
    {
      return ((Value >= Lower) && (Value <= Upper));
    }

    /// <summary>
    /// Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <returns>Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included</returns>
    public static bool IsInRange (int Value, int Lower, int Upper)
    {
      return ((Value >= Lower) && (Value <= Upper));
    }

    /// <summary>
    /// Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <returns>Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included</returns>
    public static bool IsInRange (decimal Value, decimal Lower, decimal Upper)
    {
      return ((Value >= Lower) && (Value <= Upper));
    }

    /// <summary>
    /// Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <returns>Returns true if <paramref name="Value"/> is between <paramref name="Lower"/> and <paramref name="Upper"/> included</returns>
    public static bool IsInRange (long Value, long Lower, long Upper)
    {
      return ((Value >= Lower) && (Value <= Upper));
    }

    /// <summary>
    /// Set <paramref name="Value"/> to <paramref name="Default"/> if is not inside range <paramref name="Lower"/> and <paramref name="Upper"/>
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <param name="Default"><paramref name="Value"/> is set to <paramref name="Default"/> when not fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    public static void InRangeOrDefault (ref float Value, long Lower, long Upper, long Default)
    {
      if (!IsInRange(Value, Lower, Upper))
        Value = Default;
    }

    /// <summary>
    /// Set <paramref name="Value"/> to <paramref name="Default"/> if is not inside range <paramref name="Lower"/> and <paramref name="Upper"/>
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <param name="Default"><paramref name="Value"/> is set to <paramref name="Default"/> when not fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    public static void InRangeOrDefault (ref double Value, double Lower, double Upper, double Default)
    {
      if (!IsInRange(Value, Lower, Upper))
        Value = Default;
    }

    /// <summary>
    /// Set <paramref name="Value"/> to <paramref name="Default"/> if is not inside range <paramref name="Lower"/> and <paramref name="Upper"/>
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <param name="Default"><paramref name="Value"/> is set to <paramref name="Default"/> when not fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    public static void InRangeOrDefault (ref int Value, int Lower, int Upper, int Default)
    {
      if (!IsInRange(Value, Lower, Upper))
        Value = Default;
    }

    /// <summary>
    /// Set <paramref name="Value"/> to <paramref name="Default"/> if is not inside range <paramref name="Lower"/> and <paramref name="Upper"/>
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <param name="Default"><paramref name="Value"/> is set to <paramref name="Default"/> when not fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    public static void InRangeOrDefault (ref long Value, long Lower, long Upper, long Default)
    {
      if (!IsInRange(Value, Lower, Upper))
        Value = Default;
    }

    /// <summary>
    /// Set <paramref name="Value"/> to <paramref name="Default"/> if is not inside range <paramref name="Lower"/> and <paramref name="Upper"/>
    /// </summary>
    /// <param name="Value">Value tor fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    /// <param name="Lower">Lower barrier</param>
    /// <param name="Upper">Upper barrier</param>
    /// <param name="Default"><paramref name="Value"/> is set to <paramref name="Default"/> when not fit between <paramref name="Lower"/> and <paramref name="Upper"/></param>
    public static void InRangeOrDefault (ref decimal Value, decimal Lower, decimal Upper, decimal Default)
    {
      if (!IsInRange(Value, Lower, Upper))
        Value = Default;
    }
  }
}
