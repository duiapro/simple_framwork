namespace System
{
    public static class GuidExtension
    {
        /// <summary>
        /// GUID转换成唯一数字
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static long ToLongID(this Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
