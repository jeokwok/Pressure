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
    public partial class Form_Product_Info : Form
    {
        public delegate void SendProductInfo(string msg);
        public event SendProductInfo send_product_info;
        public Form_Product_Info()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //发送修改好的产品信息到主窗口
            send_product_info(textBox_product_info.Text);
            DialogResult result = MessageBox.Show("修改完成","提示",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
