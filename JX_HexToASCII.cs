using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SANHUA_MAIN
{

    //十六进制转换成ascii 
    class JX_HexToASCII
    {
        public JX_HexToASCII() { }
        public string hex_to_ascii(string hex_str_in) 
        {
            string result = "";
            switch (int.Parse(hex_str_in))
            {
                case 30:
                    result = "0";
                    break;
                case 31:
                    result = "1";
                    break;
                case 32:
                    result = "2";
                    break;
                case 33:
                    result = "3";
                    break;
                case 34:
                    result = "4";
                    break;
                case 35:
                    result = "5";
                    break;
                case 36:
                    result = "6";
                    break;
                case 37:
                    result = "7";
                    break;
                case 38:
                    result = "8";
                    break;
                case 39:
                    result = "9";
                    break;
                case 0x3a:
                    result = ":";
                    break;

                case 41:
                    result = "A";
                    break;
                case 42:
                    result = "B";
                    break;
                case 43:
                    result = "C";
                    break;
                case 44:
                    result = "D";
                    break;
                case 45:
                    result = "E";
                    break;
                case 46:
                    result = "F";
                    break;
                case 47:
                    result = "G";
                    break;
                case 48:
                    result = "H";
                    break;
                case 49:
                    result = "I";
                    break;
                default:
                    break;
            }

            return result;
        }
        public static string HexStringToASCLL(string hexstring) 
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + "";

            }
            string[] ss = lin.Trim().Split(new char[] {' '});
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt16(ss[i]);
                c[i] = Convert.ToChar(a);
            }
            string b = new string(c);
            return b;
        }

        private static byte[] HexStringToBinary(string hexstring) {
            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i],16);
            }
            return buff;
        }
    }
}
