using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace obfuscript
{
    public partial class ObfuscriptDialog : Form
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

        public enum Types
        {

            Close,
            KeepOpen

        }

        Types setType;

        public ObfuscriptDialog(string title, string description, Types type)
        {
            InitializeComponent();

            lbText.Text = title;
            lbDescription.Text = description;
            setType = type;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (setType == Types.Close)
            {

                Application.Exit();
            }
            else
            {
                this.Dispose();
            }
        }

        private void btnGitHub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/mixrifyAI/obfuscript");
        }
    }
}