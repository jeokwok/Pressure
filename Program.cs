using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SANHUA_MAIN
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

         /*   Form_login login = new Form_login();
            login.ShowDialog();
            if (login.DialogResult  == DialogResult.OK)
            {
                Application.Run(new Form_main());
            }*/
            Application.Run(new Form_login());
        }
    }
}
