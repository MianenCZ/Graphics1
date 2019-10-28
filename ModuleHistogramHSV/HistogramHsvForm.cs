using Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mianen.Modules
{
  public partial class HistogramHsvForm : Form
  {
    /// <summary>
    /// Back-buffer. Resized to current client-size of the form.
    /// </summary>
    protected Bitmap backBuffer = null;

    /// <summary>
    /// Associated raster module (to be notified in case of form close).
    /// </summary>
    protected IRasterModule module;

    public HistogramHsvForm (IRasterModule hModule)
    {
      module = hModule;
      InitializeComponent();
      this.FormClosed += HistogramForm_FormClosed;
      this.Resize += HistogramForm_Resize;
      this.Paint += HistogramForm_Paint;
    }

    public void SetResult (Bitmap result)
    {
      backBuffer?.Dispose();
      backBuffer = result;
      Invalidate();
    }

    private void HistogramForm_FormClosed (object sender, FormClosedEventArgs e)
    {
      backBuffer?.Dispose();
      backBuffer = null;

      module?.OnGuiWindowClose();
    }

    private void HistogramForm_Paint (object sender, PaintEventArgs e)
    {
      if (backBuffer == null)
      {
        e.Graphics.Clear(Color.White);
      }
      else
      {
        e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);
      }
    }

    private void HistogramForm_Resize (object sender, System.EventArgs e)
    {
      if (backBuffer == null ||
          backBuffer.Width != ClientSize.Width ||
          backBuffer.Height != ClientSize.Height)
      {
        module.Update();
      }
    }
  }
}
