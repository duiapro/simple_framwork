using System.Security.Cryptography;
using System.Text;

namespace Simple.Framework.Tools
{
    public static class EncryptionHelper
    {

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string SHA1(string parameter)
        {
            var buffer = Encoding.UTF8.GetBytes(parameter);
            var data = System.Security.Cryptography.SHA1.Create().ComputeHash(buffer);
            var sub = new StringBuilder();

            foreach (var t in data)
            {
                sub.Append(t.ToString("X2"));
            }

            return sub.ToString();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string SHA256(string parameter)
        {
            var buffer = Encoding.UTF8.GetBytes(parameter);
            var sha256 = new SHA256CryptoServiceProvider();
            var data = sha256.ComputeHash(buffer);

            var sub = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sub.Append(data[i].ToString("x2"));
            }
            return sub.ToString();
        }

        /// <summary>
        /// SHA256 转换为 Hex字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Sha256Hex(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha256.ComputeHash(bytes);
                return ByteArrayToHexString(hash);
            }
        }

        /// <summary>
        /// Md5Hex
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Md5Hex(string data)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] dataHash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in dataHash)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字节数组转换为Hex字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="toLowerCase"></param>
        /// <returns></returns>
        private static string ByteArrayToHexString(byte[] data, bool toLowerCase = true)
        {
            var hex = BitConverter.ToString(data).Replace("-", string.Empty);
            return toLowerCase ? hex.ToLower() : hex.ToUpper();
        }

    }
}
