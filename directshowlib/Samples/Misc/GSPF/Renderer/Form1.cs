using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Runtime.InteropServices;
using DxPlayx;

namespace Renderer2
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnStart;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Button button1;

		DxPlay m_play = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
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
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnStart = new System.Windows.Forms.Button();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(112, 72);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 40);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(40, 24);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(248, 20);
            this.tbFileName.TabIndex = 3;
            this.tbFileName.Text = "c:\\1.avi";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(216, 80);
            this.button1.Name = "button1";
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 170);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			// If we have no class open
			if (m_play == null)
			{
				try
				{
					m_play = new DxPlay(tbFileName.Text);
					m_play.Completed +=new EventHandler(m_play_Completed);
                }
				catch(COMException ce)
				{
					MessageBox.Show("Failed create DxPlay: " + ce.Message, "Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				m_play.Stop();
			}
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (m_play != null)
			{
				m_play.Stop();
				m_play.Dispose();
				m_play = null;
			}
		}

		private void m_play_Completed(object sender, EventArgs e)
		{
			//DxPlay.CompletedArgs c = e as DxPlay.CompletedArgs;
			if (m_play != null)
			{
				m_play.Dispose();
				m_play = null;
			}
			btnStart.Text = "Start";
		}

        private void button1_Click(object sender, System.EventArgs e)
        {
            m_play.Start();
            btnStart.Text = "Stop";
        }
	}
}
