using Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mianen.ModuleHSV
{
  class ModuleHSV : DefaultRasterModule
  {
    public override string Author => "Jiri Mianen Pelc";

    public override string Name => "ModuleHSV";

    /// <summary>
    /// Tooltip for Param (text parameters).
    /// </summary>
    public override string Tooltip => "";

    /// <summary>
    /// Default HSV transform parameters (have to by in sync with the default module state).
    /// </summary>
    protected string param = "";

    /// <summary>
    /// True if 'param' has to be parsed in the next recompute() call.
    /// </summary>
    protected bool paramDirty = true;

    public override string Param
    {
      get => param;
      set
      {
        if (value != param)
        {
          param = value;
          paramDirty = true;
          Recompute();
        }
      }
    }

    /// <summary>
    /// Usually read-only, optionally writable (client is defining number of inputs).
    /// </summary>
    public override int InputSlots => 1;

    /// <summary>
    /// Usually read-only, optionally writable (client is defining number of outputs).
    /// </summary>
    public override int OutputSlots => 1;

    /// <summary>
    /// Input raster image.
    /// </summary>
    protected Bitmap inImage = null;

    /// <summary>
    /// Output raster image.
    /// </summary>
    protected Bitmap outImage = null;

    protected void Recompute ()
    {

    }
  }
}
