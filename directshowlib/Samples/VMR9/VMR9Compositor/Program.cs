using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DirectShowLib.Sample
{
  static class Program
  {
    /// <summary>
    /// Point d'entrée principal de l'application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      using (MainForm form = new MainForm())
      {
        Application.Run(form);
      }
    }
  }
}