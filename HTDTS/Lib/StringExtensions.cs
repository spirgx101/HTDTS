using System;
using System.Text;

namespace HTDTS.Lib
{
    public static class StringExtensions
    {
        public static string Alignment(this string strTemp, int length)
        {
            byte[] byteStr = Encoding.Default.GetBytes(strTemp);
            int iNeed = length - byteStr.Length <= 0 ? 0 : length - byteStr.Length;

            iNeed = iNeed / Encoding.Default.GetBytes(" ").Length;

            byte[] newStr = new byte[length];
            Array.Copy( Encoding.Default.GetBytes(strTemp + string.Empty.PadRight(iNeed, ' ')), newStr, length);

            //return strTemp + string.Empty.PadRight(iNeed, ' ');
            return System.Text.Encoding.Default.GetString(newStr);
        }
    }
}
