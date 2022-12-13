using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SANHUA_MAIN
{
    public partial class Form_OperatingInstructions : Form
    {


        [DllImport("kernel32.dll")]
        public static extern int WinExec(string programPath, int operType);


/*        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            int nShowCmd);*/

        public enum ShowWindowCommands : int 
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_SHOW = 5  
       
        }
        public Form_OperatingInstructions()
        {
            InitializeComponent();
        }

        private void Form_OperatingInstructions_Load(object sender, EventArgs e)
        {
            //实现打开本地程序
            //var result = WinExec("F:\\WeChat\\WeChat.exe", (int)ShowWindowCommands.SW_SHOW);

            System.Diagnostics.Process.Start("C:\\Users\\Lenovo\\Desktop\\三花气密性使用说明书.doc");
            this.Close();
            /*OpenFileDialog open_word_dlg = new OpenFileDialog();
            open_word_dlg.Filter = "word文件|*.doc";
            //object fileName = 0;
            open_word_dlg.CheckFileExists = true;
            string Path = @"C:\\Users\\Lenovo\\Desktop\\OperatingInstructions.docx";
            if (open_word_dlg.ShowDialog() == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("C:\\Users\\Lenovo\\Desktop\\OperatingInstructions.docx");
                //让richTextBox 显示Word文档的内容22.10.15
                //richTextBox_Instructions
            }*/

            //OpenWord(Path);
            /*
                        OpenFileDialog open_word_dlg = new OpenFileDialog();
                        open_word_dlg.Filter = "word文件|*.docx";
                        object fileName = 0;
                        if (open_word_dlg.ShowDialog() == DialogResult.OK)
                        {
                            fileName = open_word_dlg.FileName;

                        }
                        string str = (string)fileName;
                        OpenWord(str);*/
        }
        //打开操作说明文档
        private void OpenWord(string str)
        {
           //实现读取使用说明书
        }
    }
}
