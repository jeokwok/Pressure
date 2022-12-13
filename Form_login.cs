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
    public partial class Form_login : Form
    {

        public Form_login()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string user = tb_user.Text.Trim();
            string staff = tb_staff.Text;
            string passeord = tb_password.Text.Trim();


            if (staff != null)
            {

                if (user == "管理员" & passeord == "123")
                {
                    
                    Form_main form_main = new Form_main();
                    this.Hide();
                    form_main.ShowDialog();
                    this.Dispose();  //打开新窗口，关闭登录窗口
                    
                   // form_main.Show();
                    
                }
                else 
                {
                    MessageBox.Show("请输入用户名,员工号和密码");
                }
            }
            else
            {    
                MessageBox.Show("员工号为空");
            }
           /* if (user != null & passeord != null & staff != null)
            {

                if (user == "管理员" & staff != " " & passeord == "123")
                {
                    Form_main form_main = new Form_main();
                   
                    form_main.Show();
                    

                }
                else 
                {
                    MessageBox.Show("请输入用户名,员工号和密码");

                }
            }
            else 
            {
                MessageBox.Show("请输入用户名,员工号和密码");
            
            }*/

            //输入相关用户信息后，点击记住登录，在点击登录会帮这次登录的用户信息保存
            KeyEventArgs key = new KeyEventArgs((Keys)13);
            if (radioButton_remember.Checked) {
                System.IO.File.WriteAllText("C:/Users/pc/source/repos/SANHUA_MAIN/SANHUA_MAIN/bin/Debug/user_info.text", user + "\r\n" + staff + "\r\n" + passeord + "\r\n");    
            }
        }

        private void Form_login_Load(object sender, EventArgs e)
        {
            //对用户初始化默认为管理员
            tb_user.Text = "管理员";
            //窗口初始化从用户信息文件中读取记住登录的用户信息
            string user_info = System.IO.File.ReadAllText("C:/Users/pc/source/repos/SANHUA_MAIN/SANHUA_MAIN/bin/Debug/user_info.text");
            string[] info = user_info.Split('\r');
            tb_user.Text = info[0];
            tb_staff.Text = info[1];
            tb_password.Text = info[2];
        }
        /// <summary>
        /// 登录按钮的回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_login_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btn_login.PerformClick();
                    break;
                default:
                    break;
            }
        }
    }
    
}
