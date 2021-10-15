using System;
using System.Text;

namespace FacebookInformationPoster
{
    public static class Base64Encode
    {
        public static string Decode(this string encoded)
        {
            byte[] data = Convert.FromBase64String(encoded);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
    }
}
