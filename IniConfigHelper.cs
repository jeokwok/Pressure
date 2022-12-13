using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace thinger.cn.Helper
{
    public class IniconfigHelper
    {
        #region API函数声明
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);
        #endregion


    }
    public static string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
    {
        if (File.Exists(iniFilePath))
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
            return temp.ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    public static bool WriteIniData(string Section, string Key, string Value, string iniFileData)
    {
        if (File.Exists(iniFileData))
        {
            return WritePrivateProfileString(Section, Key, Value, iniFileData) != 0;
        }
        return false;
    }
}
