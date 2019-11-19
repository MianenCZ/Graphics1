using MathSupport;
using System;
using System.Collections.Generic;
using System.Drawing;
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

  public static class ColorScience
  {
    public static double GetHue (Color Color)
    {
      Arith.ColorToHSV(Color, out double h, out double s, out double v);
      return h;
    }
    public static double GetSaturation (Color Color)
    {
      Arith.ColorToHSV(Color, out double h, out double s, out double v);
      return s;
    }
    public static double GetPower (Color Color)
    {
      Arith.ColorToHSV(Color, out double h, out double s, out double v);
      return s * v;
    }
    public static double GetValue (Color color)
    {
      int max = Math.Max(color.R, Math.Max(color.G, color.B));
      return max / 255.0;
    }

    public static Color[] GetAll (int Size)
    {

      if (Size < (1 << 24))
      {
        throw new ArgumentException($"Size is not enough to create all colors");
      }
      Color[] all = new Color[Size];
      int Counter = 0;

      int fullcount = Size/(1<<24);
      for (int ser = 0; ser < fullcount; ser++)
      {
        Parallel.For(0, 256, (r) =>
        {
          for (int g = 0; g < 256; g++)
          {
            for (int b = 0; b < 256; b++)
            {
              //Flat[x + WIDTH * (y + DEPTH * z)] = Original[x, y, z]
              all[(ser * (1 << 24)) + b + 256 * (g + 256 * r)] = Color.FromArgb(r, g, b);
            }
          }
        });
      }


      Counter += (1 << 24);

      while (true)
      {
        for (int hue = 0; hue < 360; hue++)
        {
          for (float sa = 0.9f; sa < 1.0f; sa += 0.0002f)
          {
            if (Counter >= Size)
            {
              return all;
            }

            Arith.HSVToRGB(hue, sa, 1, out double r, out double g, out double b);
            all[Counter] = Color.FromArgb((int)r, (int)g, (int)b);
            Counter++;
            Arith.HSVToRGB(hue, 1 - sa, 1, out r, out g, out b);
            all[Counter] = Color.FromArgb((int)r, (int)g, (int)b);
            Counter++;


          }
        }
      }


    }
  }
}
