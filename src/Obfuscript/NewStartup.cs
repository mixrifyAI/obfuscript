using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace obfuscript
{
    public partial class NewStartup : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        public NewStartup()
        {
            InitializeComponent();

            // For Testing
            //obfuscript.Properties.Settings.Default.FirstTime = true;
            //obfuscript.Properties.Settings.Default.KeyToRemember = "";
            //obfuscript.Properties.Settings.Default.Save();
        }

        private void LicenseKey_Tick(object sender, EventArgs e)
        {
            LicenseKey.Stop();

            pnKey.BringToFront();
            pnKey.Show();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbKey.Text == "FREETIER" || tbKey.Text == "FREETIER".ToLower())
            {

                obfuscript.Properties.Settings.Default.KeyToRemember = "FREETIER";
                obfuscript.Properties.Settings.Default.Save();

                lbError.ForeColor = Color.FromArgb(187, 134, 252);
                lbError.Text = "Valid License key, redirecting.";
                lbError.Left = (this.ClientSize.Width - lbError.Size.Width) / 2;
                lbError.Visible = true;

                btnLogin.Enabled = false;
                tbKey.Enabled = false;

                this.Hide();

                MainFormNew Main = new MainFormNew();
                Main.Show();

            }
            else
            {

                lbError.ForeColor = Color.Firebrick;
                lbError.Text = "Invalid License Key";
                lbError.Left = (this.ClientSize.Width - lbError.Size.Width) / 2;
                lbError.Visible = true;

            }
        }

        private void NewStartup_Load(object sender, EventArgs e)
        {


        }
    }
}