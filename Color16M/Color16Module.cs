using MathSupport;
using Mianen.MathTool;
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

    public override string Tooltip => "[scale][,slow][,low-mem]\n[wid=<width>][hei=<height>]\nOn low-mem is used my own Merge sort instead preimplemented quicksort";

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
      this.param = "scale,low-mem";
      //this.param = "wid=4095,hei=4095,low-mem";
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
      if (this.Out is object)
      {
        this.Out.Dispose();
      }

      bool scale = false;
      bool fast = true;
      bool lowmem = false;

      int wid = 4096;
      int hei = 4096;

      // We are not using 'paramDirty', so the 'Param' string has to be parsed every time.
      Dictionary<string, string> p = Util.ParseKeyValueList(param);
      if (p.Count > 0)
      {
        scale = p.ContainsKey("scale");
        if (!scale)
        {
          if (Util.TryParse(p, "wid", ref wid))
            wid = Math.Max(1, wid);

          // hei=<int> [image height in pixels]
          if (Util.TryParse(p, "hei", ref hei))
            hei = Math.Max(1, wid);
        }

        // slow ... use Bitmap.SetPixel()
        fast = !p.ContainsKey("slow");

        lowmem = p.ContainsKey("low-mem");
      }
      Pixel[] inData = default;
      int inHeight;
      int inWidth;
      if (scale)
      {
        inData = GetScaled(out inHeight, out inWidth);
      }
      else
      {
        inData = GetSized(hei, wid);
        inHeight = hei;
        inWidth = wid;
      }
      DrawFromPixel(inData, inHeight, inWidth, fast, lowmem);


      long colors = Draw.ColorNumber(this.Out);
      message = colors == (1 << 24) ? "Colors: 16M, Ok" : $"Colors: {colors}, Fail";
    }

    /// <summary>
    /// Returns an optional output message.
    /// Can return null.
    /// </summary>
    /// <param name="slot">Slot number from 0 to OutputSlots-1.</param>
    public override string GetOutputMessage (
      int slot = 0) => message;


    /*
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

    }

    //*/

    private void DrawFromPixel (Pixel[] indata, int Height, int Width, bool fast, bool lowmem)
    {

      Pixel[] insort= default;
      Color[] colsort = default;
      if (fast && lowmem)
      {
        //insort = indata.AsParallel().WithDegreeOfParallelism(8).OrderBy(x => ColorScience.GetValue(x.Color)).ToArray();
        //colsort = GetAll(In.Width * Scale * In.Height * Scale).AsParallel().WithDegreeOfParallelism(8).OrderBy(x => ColorScience.GetValue(x)).ToArray();

        MultyThreadMergeSort<Pixel> sort = new MultyThreadMergeSort<Pixel>(indata, new PixelComparer(), 16);
        sort.Sort();
        colsort = GetAll(Width * Height);
        MultyThreadMergeSort<Color> colorMS = new MultyThreadMergeSort<Color>(colsort, new ColorComparer(), 16);
        colorMS.Sort();
        insort = indata;
      }
      else if (fast && !lowmem)
      {
        insort = indata.AsParallel().WithDegreeOfParallelism(16).OrderBy(x => ColorScience.GetValue(x.Color)).ToArray();
        colsort = GetAll(Width * Height).AsParallel().WithDegreeOfParallelism(16).OrderBy(x => ColorScience.GetValue(x)).ToArray();
      }
      else if (!fast && lowmem)
      {
        MultyThreadMergeSort<Pixel> sort = new MultyThreadMergeSort<Pixel>(indata, new PixelComparer(), 1);
        sort.Sort();
        colsort = GetAll(Width * Height);
        MultyThreadMergeSort<Color> colorMS = new MultyThreadMergeSort<Color>(colsort, new ColorComparer(), 1);
        colorMS.Sort();
        insort = indata;
      }
      else if (!fast && !lowmem)
      {
        insort = indata.OrderBy(x => ColorScience.GetValue(x.Color)).ToArray();
        colsort = GetAll(Width * Height).OrderBy(x => ColorScience.GetValue(x)).ToArray();
      }
      else
      {
        message = "I don't know how did you do that, but you are good crasher";
        return;
      }

      DirectBitmap bmp = new DirectBitmap(Width, Height);
      Parallel.For(0, insort.Length, (i) => { bmp.SetPixel(insort[i].X, insort[i].Y, colsort[i]); });

      this.Out = bmp.Bitmap;
      insort = null;
      colsort = null;
      indata = null;
      GC.Collect();

    }

    private Pixel[] GetScaled (out int Height, out int Width)
    {
      int size = In.Width * In.Height; //16*9
      int Scale = 1;
      if (size < (1 << 24))
      {
        double difq = (16777216.0)/size;
        double dif = Math.Sqrt(difq);
        Scale = (int)Math.Floor(dif) + 1;
      }
      Height = this.In.Height * Scale;
      Width = this.In.Width * Scale;

      Pixel[] indata = new Pixel[In.Width * Scale * In.Height * Scale];
      int index = 0;

      System.Drawing.Imaging.BitmapData inData =
            In.LockBits(
              new Rectangle(0, 0, In.Width, In.Height),
              System.Drawing.Imaging.ImageLockMode.ReadOnly,
              In.PixelFormat);

      IntPtr ptr = inData.Scan0;

      unsafe
      {
        byte* iptr;
        byte ri, gi, bi;
        int wi = In.Width;
        int hi = In.Height;

        for (int x = 0; x < wi; x++)
        {
          for (int y = 0; y < hi; y++)
          {
            iptr = (byte*)inData.Scan0 + ((y * inData.Width) + x) * 3;


            bi = iptr[0];
            gi = iptr[1];
            ri = iptr[2];

            for (int i = 0; i < Scale; i++)
            {
              for (int j = 0; j < Scale; j++)
              {

                indata[(x * hi + y) * Scale * Scale + i * Scale + j] = new Pixel
                {
                  X = x * Scale + i,
                  Y = y * Scale + j,
                  Color = Color.FromArgb(bi, gi, ri),
                };
              }
            }
          }
        }
      }
      this.In.UnlockBits(inData);
      inData = null;
      GC.Collect();

      return indata;
    }

    private Pixel[] GetSized (int Height, int Width)
    {
      Pixel[] indata = new Pixel[Height * Width];

      System.Drawing.Imaging.BitmapData inData =
            In.LockBits(
              new Rectangle(0, 0, In.Width, In.Height),
              System.Drawing.Imaging.ImageLockMode.ReadOnly,
              In.PixelFormat);

      unsafe
      {
        byte* iptr;
        byte ri, gi, bi;
        for (int x = 0; x < Width; x++)
        {
          for (int y = 0; y < Height; y++)
          {
            iptr = (byte*)inData.Scan0 + (((y % In.Height) * inData.Width) + (x % In.Width)) * 3;


            bi = iptr[0];
            gi = iptr[1];
            ri = iptr[2];

            indata[y * Width + x] = new Pixel
            {
              X = x,
              Y = y,
              Color = Color.FromArgb(bi, gi, ri),
            };
          }
        }
      }
      this.In.UnlockBits(inData);
      inData = null;
      GC.Collect();
      return indata;

    }

    private Color[] GetAll (int Size)
    {
      Color[] all = new Color[Size];
      int Counter = 0;

      bool stable = true;
      int fullcount = Size/(1<<24);
      for (int ser = 0; ser < fullcount; ser++)
      {
        Parallel.For(0, 256, (r) =>
        {
          for (int g = 0; g < 256; g++)
          {
            for (int b = 0; b < 256; b++)
            {
              if (UserBreak)
              {
                stable = false;
                return;
              }
              //Flat[x + WIDTH * (y + DEPTH * z)] = Original[x, y, z]
              all[(ser * (1 << 24)) + b + 256 * (g + 256 * r)] = Color.FromArgb(r, g, b);
            }
          }
        });
      }


      Counter += fullcount * (1 << 24);

      if (fullcount > 0)
      {
        bool run = true;
        while (run)
        {
          all[Counter] = Color.FromArgb(0,0,0);
          Counter++;

          if (Counter >= Size)
          {
            run = false;
            return all;
          }
        }
      }
      while (true)
      {
        for (int hue = 0; hue < 360; hue++)
        {
          for (float va = 0.6f; va < 1.0f; va += 0.0002f)
          {
            if (UserBreak)
            {
              return null;
            }

            Arith.HSVToRGB(hue, 1, va, out double r, out double g, out double b);
            all[Counter] = Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
            //Console.WriteLine(all[Counter]);
            Counter++;

            if (Counter >= Size)
            {
              return all;
            }
            Arith.HSVToRGB(hue, 1, 1 - va, out r, out g, out b);
            all[Counter] = Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
            //Console.WriteLine(all[Counter]);
            Counter++;
            if (Counter >= Size)
            {
              return all;
            }


          }
        }
      }


    }

  }
}
