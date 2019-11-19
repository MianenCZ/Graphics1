using MathSupport;
using Mianen.MathTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mianen.Utils
{
  public struct Pixel
  {
    public int X { get; set; }
    public int Y { get; set; }
    public Color Color { get; set; }
  }

  public class PixelComparer : IComparer<Pixel>
  {
    public int Compare (Pixel x, Pixel y)
    {
      return ColorScience.GetValue(x.Color).CompareTo(ColorScience.GetValue(y.Color));
    }
  }

  public class ColorComparer : IComparer<Color>
  {
    public int Compare (Color x, Color y)
    {
      return ColorScience.GetValue(x).CompareTo(ColorScience.GetValue(y));
    }
  }
}
