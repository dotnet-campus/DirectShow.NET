/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using DirectShowLib;

using Microsoft.Win32;

namespace DirectShowLib.Sample
{
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuItem menuFile;
        private System.Windows.Forms.MenuItem menuFileOpen;
        private System.Windows.Forms.MenuItem menuFileClose;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuFileExit;
        private IContainer components;

        // DirectShow stuff
        private IFilterGraph2 graphBuilder = null; 
        private IMediaControl mediaControl = null; 
        private IBaseFilter vmr9 = null;
        private IVMRMixerBitmap9 mixerBitmap = null;
        private IVMRWindowlessControl9 windowlessCtrl = null;
        private bool handlersAdded = false;
        private SaveFileDialog saveFileDialog; // Needed to remove delegates

        // Menus stuff
        private MenuItem menuSnap;

        public MainForm()
        {
            InitializeComponent();

            // We paint the windows ourself
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            this.saveFileDialog.DefaultExt = ".bmp";
            this.saveFileDialog.AddExtension = true;
            this.saveFileDialog.CheckPathExists = true;
        }

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }

            // Clean-up DirectShow interfaces
            CloseInterfaces();

            base.Dispose( disposing );
        }

        #region Code généré par le Concepteur Windows Form
        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuFileOpen = new System.Windows.Forms.MenuItem();
            this.menuSnap = new System.Windows.Forms.MenuItem();
            this.menuFileClose = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuFileExit = new System.Windows.Forms.MenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileOpen,
            this.menuSnap,
            this.menuFileClose,
            this.menuItem1,
            this.menuFileExit});
            this.menuFile.Text = "File";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Index = 0;
            this.menuFileOpen.Text = "Open Clip...";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuSnap
            // 
            this.menuSnap.Index = 1;
            this.menuSnap.Text = "Snap";
            this.menuSnap.Click += new System.EventHandler(this.menuSnap_Click);
            // 
            // menuFileClose
            // 
            this.menuFileClose.Index = 2;
            this.menuFileClose.Text = "Close Clip";
            this.menuFileClose.Click += new System.EventHandler(this.menuFileClose_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuFileExit
            // 
            this.menuFileExit.Index = 4;
            this.menuFileExit.Text = "Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = resources.GetString("openFileDialog.Filter");
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "VMR Snapper";
            this.ResumeLayout(false);

        }
        #endregion

        private void CloseInterfaces()
        {
            if (mediaControl != null)
                mediaControl.Stop();

            if (handlersAdded)
                RemoveHandlers();

            if (vmr9 != null)
            {
                Marshal.ReleaseComObject(vmr9);
                vmr9 = null;
                windowlessCtrl = null;
                mixerBitmap = null;
            }

            if (graphBuilder != null)
            {
                Marshal.ReleaseComObject(graphBuilder);
                graphBuilder = null;
                mediaControl = null;
            }

        }

        private void BuildGraph(string fileName)
        {
            int hr = 0;

            try
            {
                graphBuilder = (IFilterGraph2) new FilterGraph();
                mediaControl = (IMediaControl) graphBuilder;

                vmr9 = (IBaseFilter) new VideoMixingRenderer9();

                ConfigureVMR9InWindowlessMode();

                hr = graphBuilder.AddFilter(vmr9, "Video Mixing Renderer 9");
                DsError.ThrowExceptionForHR(hr);

                hr = graphBuilder.RenderFile(fileName, null);
                DsError.ThrowExceptionForHR(hr);
            }
            catch(Exception e)
            {
                CloseInterfaces();
                MessageBox.Show("An error occured during the graph building : \r\n\r\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureVMR9InWindowlessMode()
        {
            int hr = 0;

            IVMRFilterConfig9 filterConfig = (IVMRFilterConfig9) vmr9;

            // Not really needed for VMR9 but don't forget calling it with VMR7
            hr = filterConfig.SetNumberOfStreams(1);
            DsError.ThrowExceptionForHR(hr);

            // Change VMR9 mode to Windowless
            hr = filterConfig.SetRenderingMode(VMR9Mode.Windowless);
            DsError.ThrowExceptionForHR(hr);

            windowlessCtrl = (IVMRWindowlessControl9) vmr9;

            // Set "Parent" window
            hr = windowlessCtrl.SetVideoClippingWindow(this.Handle);
            DsError.ThrowExceptionForHR(hr);

            // Set Aspect-Ratio
            hr = windowlessCtrl.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);
            DsError.ThrowExceptionForHR(hr);

            // Add delegates for Windowless operations
            AddHandlers();

            // Call the resize handler to configure the output size
            MainForm_ResizeMove(null, null);
        }

        private void AddHandlers()
        {
            // Add handlers for VMR purpose
            this.Paint += new PaintEventHandler(MainForm_Paint); // for WM_PAINT
            this.Resize += new EventHandler(MainForm_ResizeMove); // for WM_SIZE
            this.Move += new EventHandler(MainForm_ResizeMove); // for WM_MOVE
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged); // for WM_DISPLAYCHANGE
            handlersAdded = true;
        }

        private void RemoveHandlers()
        {
            // remove handlers when they are no more needed
            handlersAdded = false;
            this.Paint -= new PaintEventHandler(MainForm_Paint);
            this.Resize -= new EventHandler(MainForm_ResizeMove);
            this.Move -= new EventHandler(MainForm_ResizeMove);
            SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        private void RunGraph()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Run();
                DsError.ThrowExceptionForHR(hr);
            }
        }

        private void StopGraph()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Stop();
                DsError.ThrowExceptionForHR(hr);
            }
        }

        // Menus click handlers
        private void menuFileOpen_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CloseInterfaces();
                BuildGraph(openFileDialog.FileName);
                RunGraph();
            }
        }

        private void menuFileClose_Click(object sender, System.EventArgs e)
        {
            StopGraph();
            CloseInterfaces();
            this.Invalidate();
        }

        private void menuFileExit_Click(object sender, System.EventArgs e)
        {
            StopGraph();
            CloseInterfaces();
            this.Dispose();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (windowlessCtrl != null)
            {
                IntPtr hdc = e.Graphics.GetHdc();
                int hr = windowlessCtrl.RepaintVideo(this.Handle, hdc);
                e.Graphics.ReleaseHdc(hdc);
            }
        }

        private void MainForm_ResizeMove(object sender, EventArgs e)
        {
            if (windowlessCtrl != null)
            {
                int hr = windowlessCtrl.SetVideoPosition(null, DsRect.FromRectangle(this.ClientRectangle));
            }
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (windowlessCtrl != null)
            {
                int hr = windowlessCtrl.DisplayModeChanged();
            }
        }

        [STAThread]
        static void Main() 
        {
            using (MainForm form = new MainForm())
            {
                Application.Run(form);
            }
        }

        private void menuSnap_Click(object sender, EventArgs e)
        {
            if (windowlessCtrl != null)
            {
                IntPtr currentImage;
                windowlessCtrl.GetCurrentImage(out currentImage);
                BitmapInfoHeader structure = new BitmapInfoHeader();
                Marshal.PtrToStructure(currentImage, structure);

                Bitmap bmp = new Bitmap(structure.Width, structure.Height, (structure.BitCount / 8) * structure.Width, System.Drawing.Imaging.PixelFormat.Format32bppArgb, currentImage);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(saveFileDialog.FileName.ToString());
                }

                bmp.Dispose();
                Marshal.FreeCoTaskMem(currentImage);
            }

        }

    }
}
