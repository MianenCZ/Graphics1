using Mianen.MathTool;
using Mianen.Utils;
using Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _117raster.Mianen.Scary_Generator
{
  class ScaryGeneratorModule : DefaultRasterModule
  {
    public override string Author => "Jiri Mianen Pelc";

    public override string Name => "Obscure input generator";

    public override int InputSlots => 0;
    public override int OutputSlots => 1;
    private Bitmap Out;

    public override Bitmap GetOutput (int slot = 0)
    {
      return Out;
    }

    public override string GetOutputMessage (int slot = 0)
    {
      return base.GetOutputMessage(slot);
    }

    public override void Update ()
    {
      GenerateOneLine();
    }

    public void GenerateOneLine ()
    {
      DirectBitmap bmp = new DirectBitmap((1 << 24), 1);
      Color[] data = ColorScience.GetAll((1 << 24));
      int l = data.Length;
      Parallel.For(0,l, (i)=> { bmp.SetPixel(i, 0, data[i]); });
      this.Out = bmp.Bitmap;

    }
  }
}
