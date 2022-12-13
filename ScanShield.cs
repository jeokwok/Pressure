using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SANHUA_MAIN
{
    public partial class ScanShield : Form
    {
        private string password = "123";
        public ScanShield()
        {
            InitializeComponent();
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            if (this.textBox_password.Text.Trim().Equals(this.password))
            {
                this.DialogResult = DialogResult.OK;
            }
            else 
            {
                this.DialogResult = DialogResult.Cancel;
            
            }
        }

        private void Btn_CANCEL_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
