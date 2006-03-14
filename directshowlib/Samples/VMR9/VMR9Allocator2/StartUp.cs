/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Windows.Forms;

namespace DirectShowLib.Sample
{
  public class StartUp
  {
    [STAThread]
    static void Main() 
    {
      OpenFileDialog openDialog = new OpenFileDialog();
      if (openDialog.ShowDialog() == DialogResult.OK)
      {
        using (MainForm form = new MainForm())
        {
          form.Show();
          form.InitializeGraphics();
          form.InitVMR9(openDialog.FileName);
          form.Focus();

          Application.Idle += new EventHandler(form.OnApplicationIdle);
          Application.Run(form);
        }
      }
    }

  }
}
