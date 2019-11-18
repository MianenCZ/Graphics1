using Mianen.Utils;
using Modules;
using Raster;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Mianen.Modules
{
  class Color16Module : DefaultRasterModule
  {
    protected new string param;
    protected bool dirtyParam;
    protected string message;
    protected Bitmap In;
    protected Bitmap Out;

    public override string Author => "Jiri Mianen Pelc";

    public override string Name => "16M Colors";

    public override string Tooltip => "[wid=<width>][,hei=<height>][,slow][,ignore-input][,seed]\nOn";

    public override string Param
    {
      get => param;
      set
      {
        if (value != this.param)
        {
          this.param = value;
          this.dirtyParam = true;
          Update();
        }
      }
    }
    public override int InputSlots => 1;
    public override bool HasPixelUpdate => false;
    public override int OutputSlots => 1;
    public override bool GuiWindow => false;
    /// <summary>
    /// Output message (color check).
    /// </summary>
    

    public Color16Module ()
    {
      this.Out = null;
      this.In = null;
      this.param = "fast=true";
    }

    public override Bitmap GetOutput (int slot = 0)
    {
      return this.Out;
    }

    public override void SetInput (Bitmap inputImage, int slot = 0)
    {
      this.In = inputImage;
    }

    public override void Update ()
    {
      int wid = 4096;
      int hei = 4096;
      bool fast = true;
      bool ignoreInput = false;
      int seed = 42;

      // We are not using 'paramDirty', so the 'Param' string has to be parsed every time.
      Dictionary<string, string> p = Util.ParseKeyValueList(param);
      if (p.Count > 0)
      {
        // wid=<int> [image width in pixels]
        if (Util.TryParse(p, "wid", ref wid))
          wid = Math.Max(1, wid);

        // hei=<int> [image height in pixels]
        if (Util.TryParse(p, "hei", ref hei))
          hei = Math.Max(1, wid);

        // slow ... use Bitmap.SetPixel()
        fast = !p.ContainsKey("slow");

        // ignore-input ... ignore input image even if it is present
        ignoreInput = p.ContainsKey("ignore-input");

        // no-check ... disable color check at the end
        if (Util.TryParse(p, "seed", ref seed))
          seed = 42;
      }

      if (ignoreInput)
      {
        DrawNoImput(wid, hei, fast, seed);
      }
      else
      {
        DrawFromImput(fast, seed);
      }
    }

    /// <summary>
    /// Returns an optional output message.
    /// Can return null.
    /// </summary>
    /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
    public override string GetOutputMessage (
      int slot = 0) => message;
  
    private void DrawNoImput (int wid, int hei, bool fast, int seed)
    {
      if (wid * hei < (1 << 24))
      {
        message = "ERROR: Wrong size";
      }


      Color[] all = GetAll(wid*hei);

      if (UserBreak || all is null)
      {
        message = "Stop during color generation";
        return;
      }

      Random rnd = new Random(seed);
      var data = all.OrderBy(x => rnd.NextDouble()).ToArray();
      DirectBitmap btm = new DirectBitmap(wid, hei);
      int OutConter = 0;
      for (int x = 0; x < wid; x++)
      {
        for (int y = 0; y < hei; y++)
        {
          if (UserBreak)
          {
            message = "Stop during randomization";
            Out = btm.Bitmap;
            return;
          }
          btm.SetPixel(x, y, data[OutConter]);
          OutConter++;
        }
      }
      Out = btm.Bitmap;
      return;
    }

    private void DrawFromImput (bool fast, int seed)
    {
      int size = In.Width * In.Height; //16*9
      int Squeare = 5;

      double scaleq = (16777216)/size;
      double scale = Math.Sqrt(scaleq);
      int Size = (int)Math.Floor(scale) + 1;

      int tarsize = (In.Width * Size) * (In.Height* Size);


      DirectBitmap bmp = new DirectBitmap(In.Width * Size * Squeare, In.Height* Size * Squeare);
      Color[] all = GetAll(In.Width * Size * In.Height* Size);
      Random rnd = new Random(seed);
      all = all.OrderBy(x => rnd.NextDouble()).ToArray();



      int Counter = 0;
      for (int x = 0; x < In.Width; x++)
      {
        for (int y = 0; y < In.Height; y++)
        {
          //Square true every original pixel
          Color c = In.GetPixel(x, y);
          for (int i = 0; i < Size; i++)
          {
            for (int j = 0; j < Size; j++)
            {              
              for (int _x = 0; _x < Squeare; _x++)
              {
                for (int _y = 0; _y < Squeare; _y++)
                {
                  if (UserBreak)
                  {
                    this.Out = bmp.Bitmap;
                    return;
                  }


                  bmp.SetPixel((x * Size + i)* Squeare + _x, (y * Size + j)* Squeare + _y, c);
                }
              }
              bmp.SetPixel((x * Size + i) * Squeare + rnd.Next(0, Squeare), (y * Size + j) * Squeare + rnd.Next(0, Squeare), all[Counter]);
              Counter++;
            }
          }
        }
      }

      this.Out = bmp.Bitmap;

      long colors = Draw.ColorNumber(this.Out);
      message = colors == (1 << 24) ? "Colors: 16M, Ok" : $"Colors: {colors}, Fail";
    }

    private Color[] GetAll (int Size)
    {
      Color[] all = new Color[Size];
      int Counter = 0;
      while (true)
      {
        for (int r = 0; r < 256; r++)
        {
          for (int g = 0; g < 256; g++)
          {
            for (int b = 0; b < 256; b++)
            {
              if (UserBreak)
              {
                return null;
              }

              if (Counter >= Size)
              {
                return all;
              }
              all[Counter] = Color.FromArgb(r, g, b);
              Counter++;
            }
          }
        }
      }
    }
  }
}
