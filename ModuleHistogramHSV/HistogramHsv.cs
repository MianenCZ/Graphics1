using MathSupport;
using Mianen.MathTool;
using Mianen.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Utilities;

namespace Mianen.Utilities
{
  public static class ImageHistogramHsv
  {
    public const char ColorPaleteCommnad = 'C';
    public const char HistogramCommnad = 'H';
    public const char ValuesCommnad = 'V';
    public const char PowersCommnad = 'P';


    private static ImageHistogramHsvMode mode = ImageHistogramHsvMode.Standart;
    public static void DrawHistogram (Bitmap graph)
    {
      switch (mode)
      {
        case ImageHistogramHsvMode.Standart:
          ImageHistogramHsvExist.DrawHistogram(graph);
          break;
        case ImageHistogramHsvMode.Distribution:
          ImageHistogramHsvOcc.DrawHistogram(graph);
          break;
        case ImageHistogramHsvMode.Vals:
          ImageHistogramHsvOcc.DrawHistogram(graph);
          break;
        case ImageHistogramHsvMode.Power:
          ImageHistogramHsvOcc.DrawHistogram(graph);
          break;
        case ImageHistogramHsvMode.None:
          break;
      }
    }

    public static void ComputeHistogram (Bitmap input, string param)
    {
      param = param.Trim();

      bool ColorPalete = (param[0] == ColorPaleteCommnad);   //ColorPalete
      bool Histogram = (param[0] == HistogramCommnad);   //Histogram
      bool Values = (param[0] == ValuesCommnad);   //Values
      bool Powers = (param[0] == PowersCommnad);   //Powers

      bool total = ColorPalete || Histogram || Values || Powers;

      bool onlyOne = BoolMath.OnlyOne(ColorPalete, Histogram, Values, Powers);

      if (!total)
      {
        MessageBox.Show("Missing starting command");
        mode = ImageHistogramHsvMode.None;
        return;
      }
      if (!onlyOne)
      {
        MessageBox.Show("To many commands");
        mode = ImageHistogramHsvMode.None;
        return;
      }

      if (ColorPalete)
      {
        mode = ImageHistogramHsvMode.Standart;
        param = param.Substring(param.IndexOf(ColorPaleteCommnad) + 1);
        Dictionary<string, string> rec = Util.ParseKeyValueList(param, ' ');


        int hue = ImageHistogramHsvExist.OptimalHue;
        int saturation = ImageHistogramHsvExist.OptimalSaturation;
        Util.TryParse(rec, "Hue", ref hue);
        Util.TryParse(rec, "Sat", ref saturation);
        if (rec.ContainsKey("Colors") && rec["Colors"] == "Dynamic")
        {
          ImageHistogramHsvExist.DynamicColors = true;
        }
        else if (rec.ContainsKey("Colors") && rec["Colors"] == "Discrete")
        {
          ImageHistogramHsvExist.DynamicColors = false;
        }
        else
        {
          ImageHistogramHsvExist.DynamicColors = true;
        }
        ImageHistogramHsvExist.ComputeHistogram(input, hue, saturation);
      }
      else if (Histogram || Powers || Values)
      {
        if (Histogram)
        {
          mode = ImageHistogramHsvMode.Distribution;
          param = param.Substring(param.IndexOf(HistogramCommnad) + 1);
        }
        if (Values)
        {
          mode = ImageHistogramHsvMode.Vals;
          param = param.Substring(param.IndexOf(ValuesCommnad) + 1);
        }
        if (Powers)
        {
          mode = ImageHistogramHsvMode.Power;
          param = param.Substring(param.IndexOf(PowersCommnad) + 1);
        }

        Dictionary<string, string> rec = Util.ParseKeyValueList(param, ' ');

        int hue = ImageHistogramHsvOcc.OptimalHue;
        float zoom = ImageHistogramHsvOcc.OptimalZoom;
        Util.TryParse(rec, "Hue", ref hue);
        if (!Util.TryParse(rec, "Zoom", ref zoom) && rec.ContainsKey("Zoom") && (rec["Zoom"] == "A" || rec["Zoom"] == "Auto"))
        {
          ImageHistogramHsvOcc.AutoZoom = true;
        }
        else
        {
          ImageHistogramHsvOcc.AutoZoom = false;
        }
        ImageHistogramHsvOcc.Zoom = zoom;
        ImageHistogramHsvOcc.ComputeHistogram(input, mode, hue);

      }
    }
  }

  internal class ImageHistogramHsvExist
  {
    public const int MaxSaturation = 10000;
    public const int OptimalSaturation = 200;
    public const int MinSaturation = 1;

    public const int MaxHue = 5760;
    public const int OptimalHue = 360;
    public const int MinHue = 8;

    public static bool DynamicColors = true;

    /// <summary>
    /// Cached histogram data.
    /// </summary>
    protected static bool[,] histArray = null;

    // Histogram mode (0 .. red, 1 .. green, 2 .. blue, 3 .. gray)

    // Graph appearance (just an example of second visualization option
    // read from param string).
    protected static bool alt = false;

    /// <summary>
    /// Draws the current histogram to the given raster image.
    /// </summary>
    /// <param name="graph">Result image (already scaled to the desired size).</param>
    public static void DrawHistogram (Bitmap graph)
    {
      if (histArray == null)
      {
        return;
      }

      int centerX = graph.Width/2;
      int centerY = graph.Height/2;
      int radius = System.Math.Min(graph.Height/2, graph.Width/2) - 25;

      int hue = histArray.GetLength(0);
      int saturation = histArray.GetLength(1);

      DirectBitmap hsv = new DirectBitmap(graph.Width, graph.Height);

      Color foreground = Color.FromArgb(250,250,250);

      for (int x = 0; x < graph.Width; x++)
      {
        for (int y = 0; y < graph.Height; y++)
        {
          int dist = (int)Math.Pow((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY), 0.5);

          if (dist < radius)
          {

            var radian = Math.Atan2(y - centerY, x - centerX);
            int h_index = (int)(((radian * (180 / Math.PI) + 360) % 360) / 360 * hue);
            int s_index = (int)(dist / (double)radius * (saturation - 1));

            if (histArray[h_index, s_index])
            {
              if (DynamicColors)
              {
                double h = (radian * (180 / Math.PI) + 360) % 360;
                double s = (dist / (double)radius);
                hsv.SetPixel(x, y, Arith.HSVToColor(h, s, 1));
              }
              else
              {

                double h = ((2*h_index + 1) / 2.0) / (hue)*360;
                double s = ((s_index + s_index + 1) / 2.0) / (saturation - 1.0);
                hsv.SetPixel(x, y, Arith.HSVToColor(h, s, 1));
              }
            }
          }
          else if (dist < radius + 2)
          {
            hsv.SetPixel(x, y, Color.FromArgb(0, 0, 0));
          }
          else
          {
            hsv.SetPixel(x, y, foreground);
          }
        }
      }


      using (Graphics gfx = Graphics.FromImage(graph))
      {
        gfx.Clear(Color.White);
        gfx.DrawImage(hsv.Bitmap, 0, 0);
      }
    }


    /// <summary>
    /// Recomputes image histogram and draws the result in the given raster image.
    /// </summary>
    /// <param name="input">Input image.</param>
    /// <param name="param">Textual parameter.</param>
    public static void ComputeHistogram (Bitmap input, int hue = OptimalHue, int saturation = OptimalSaturation)
    {
      // 1. Histogram recomputation.
      histArray = new bool[hue, saturation];

      int width = input.Width;
      int height = input.Height;
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          Color col = input.GetPixel( x, y );
          double H, S, V;
          Arith.ColorToHSV(col, out H, out S, out V);

          int h = (int)Math.Round(H * (hue-1) / 360.0);
          int s = (int) Math.Round(S * (saturation-1));
          histArray[h, s] = true;

          //histArray[mode == 0 ? col.R : (mode == 1 ? col.G : (mode == 2 ? col.B : Y))]++;
        }
      }
    }
  }

  internal class ImageHistogramHsvOcc
  {
    public const int MaxHue = 5760;
    public const int OptimalHue = 360;
    public const int MinHue = 8;
    public const float OptimalZoom = 20f;

    public static float Zoom = OptimalZoom;
    public static bool AutoZoom = false;

    /// <summary>
    /// Cached histogram data.
    /// </summary>
    protected static double[] histArray = null;

    // Histogram mode (0 .. red, 1 .. green, 2 .. blue, 3 .. gray)
    protected static ImageHistogramHsvMode mode = ImageHistogramHsvMode.Standart;

    // Graph appearance (just an example of second visualization option
    // read from param string).
    protected static bool alt = false;

    /// <summary>
    /// Draws the current histogram to the given raster image.
    /// </summary>
    /// <param name="graph">Result image (already scaled to the desired size).</param>
    public static void DrawHistogram (Bitmap graph)
    {
      if (histArray == null)
        return;

      int centerX = graph.Width/2;
      int centerY = graph.Height/2;
      int radius = Math.Min(graph.Height/2, graph.Width/2) - 25;

      int hue = histArray.Length;

      DirectBitmap hsv = new DirectBitmap(graph.Width, graph.Height);

      //Color background = Color.FromArgb(255,255,255);
      Color foreground = Color.FromArgb(25,25,25);

      if (AutoZoom)
      {
        double Max = histArray.Max();
        Zoom = 1.0f / (float)(Max / radius);

      }

      for (int x = 0; x < graph.Width; x++)
      {
        for (int y = 0; y < graph.Height; y++)
        {
          int dist = (int)Math.Pow((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY), 0.5);

          if (dist < radius)
          {

            var radian = Math.Atan2(y - centerY, x - centerX);
            int h_index = (int)(((radian * (180 / Math.PI) + 360) % 360) / 360 * hue);

            double h = (radian * (180 / Math.PI) + 360) % 360;
            if (dist < (histArray[h_index] * radius * Zoom) && dist < radius)
              hsv.SetPixel(x, y, Arith.HSVToColor(h, 1, 1));
            else
              hsv.SetPixel(x, y, Arith.HSVToColor(h, 0.1f /** ((float)dist / radius)*/, 1));

          }
          else if (dist < radius + 2)
          {
            hsv.SetPixel(x, y, Color.FromArgb(0, 0, 0));
          }
          else
          {
            hsv.SetPixel(x, y, Color.FromArgb(250, 250, 250));
          }
        }
      }




      using (Graphics gfx = Graphics.FromImage(graph))
      {


        gfx.Clear(Color.White);
        gfx.DrawImage(hsv.Bitmap, 0, 0);

      }
    }


    /// <summary>
    /// Recomputes image histogram and draws the result in the given raster image.
    /// </summary>
    /// <param name="input">Input image.</param>
    /// <param name="param">Textual parameter.</param>
    public static void ComputeHistogram (Bitmap input, ImageHistogramHsvMode mode, int hue = OptimalHue)
    {
      // 1. Histogram recomputation.
      double[] occ = new double[hue];

      int width = input.Width;
      int height = input.Height;
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          Color col = input.GetPixel( x, y );
          double H, S, V;
          Arith.ColorToHSV(col, out H, out S, out V);

          int h = (int)Math.Round(H * (hue-1) / 360.0);

          if (mode == ImageHistogramHsvMode.Distribution)
          {
            occ[h]++;
          }
          else if (mode == ImageHistogramHsvMode.Vals)
          {
            occ[h] += V;
          }
          else if (mode == ImageHistogramHsvMode.Power)
          {
            occ[h] += V * S;
          }
          //histArray[mode == 0 ? col.R : (mode == 1 ? col.G : (mode == 2 ? col.B : Y))]++;
        }
      }

      double Sum = occ.Sum();

      histArray = new double[hue];
      for (int i = 0; i < hue; i++)
      {
        histArray[i] = occ[i] / Sum;
      }

    }
  }

  public enum ImageHistogramHsvMode
  {
    None = -1,
    Standart = 0,
    Distribution = 1,
    Vals = 2,
    Power = 3
  }

}
