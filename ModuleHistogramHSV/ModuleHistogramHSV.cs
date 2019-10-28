using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Raster;
using Utilities;
using Mianen.Utilities;
using Modules;

namespace Mianen.Modules
{
  class ModuleHistogramHSV : DefaultRasterModule
  {
    public ModuleHistogramHSV ()
    {
    }

    public override string Author => "Jiri Mianen Pelc";

    public override string Name => "HSV histogram module";

    public override string Tooltip =>
      $"{ImageHistogramHsv.ColorPaleteCommnad} [Hue=({ImageHistogramHsvExist.MinHue}-{ImageHistogramHsvExist.MaxHue})] [Sat=({ImageHistogramHsvExist.MinSaturation}-{ImageHistogramHsvExist.MaxSaturation})] [Colors=(Connected | Discrete)] \n" +
      $"{ImageHistogramHsv.HistogramCommnad} [Hue=({ImageHistogramHsvOcc.MinHue}-{ImageHistogramHsvOcc.MaxHue})] [Zoom=(positive float | A | Auto)] \n" +
      $"{ImageHistogramHsv.ValuesCommnad} [Hue=({ImageHistogramHsvOcc.MinHue}-{ImageHistogramHsvOcc.MaxHue})] [Zoom=(positive float | A | Auto)] \n" +
      $"{ImageHistogramHsv.PowersCommnad} [Hue=({ImageHistogramHsvOcc.MinHue}-{ImageHistogramHsvOcc.MaxHue})] [Zoom=(positive float | A | Auto)]";

    //protected string param = $"S Hue={ImageHistogramHsvExist.OptimalHue} Sat={ImageHistogramHsvExist.OptimalSaturation}";
    //protected string param = $"V Hue={ImageHistogramHsvOcc.OptimalHue} Zoom=Auto";
    protected string param = $"C Hue={ImageHistogramHsvExist.OptimalHue} Sat={ImageHistogramHsvExist.OptimalSaturation} Colors=Discrete";

    public override string Param
    {
      get => param;
      set
      {
        if (value != param)
        {
          param = value;
          dirty = true;     // to recompute histogram table

          Recompute();
        }
      }
    }

    /// <summary>
    /// True if the histogram needs recomputation.
    /// </summary>
    protected bool dirty = true;

    /// <summary>
    /// Input raster image.
    /// </summary>
    protected Bitmap inImage = null;

    /// <summary>
    /// Active histogram view window.
    /// </summary>
    protected HistogramHsvForm hForm = null;

    /// <summary>
    /// Interior (visualization) of the hForm.
    /// </summary>
    protected Bitmap hImage = null;

    /// <summary>
    /// Returns true if there is an active GUI window associated with this module.
    /// Open/close GUI window using the setter.
    /// </summary>
    public override bool GuiWindow
    {
      get => base.GuiWindow;
      set
      {
        if (value)
        {
          if (hForm == null)
          {
            hForm = new HistogramHsvForm(this);
            hForm.Show();
          }

          Recompute();
        }
        else
        {
          hForm?.Hide();
          hForm = null;
        }
      }
    }

    public override int InputSlots => 1;

    public override bool HasPixelUpdate => true;

    public override int OutputSlots => 0;

    public override bool Equals (object obj)
    {
      return base.Equals(obj);
    }

    public override int GetHashCode ()
    {
      return base.GetHashCode();
    }

    protected void Recompute ()
    {
      if (hForm != null &&
          inImage != null)
      {
        hImage = new Bitmap(hForm.ClientSize.Width, hForm.ClientSize.Height, PixelFormat.Format24bppRgb);
        if (dirty)
        {
          ImageHistogramHsv.ComputeHistogram(inImage, Param);
          dirty = false;
        }
        ImageHistogramHsv.DrawHistogram(hImage);
        hForm.SetResult(hImage);
      }
    }


    public override void PixelUpdate (int x, int y)
    {
      Recompute();
    }

    public override void SetInput (Bitmap inputImage, int slot = 0)
    {
      dirty = inImage != inputImage;     // to recompute histogram table
      inImage = inputImage;

      Recompute();
    }

    public override void Update ()
    {
      Recompute();
    }

    public override void OnGuiWindowClose ()
    {
      hForm = null;
    }
  }
}
