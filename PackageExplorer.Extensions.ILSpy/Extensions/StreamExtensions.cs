// ReSharper disable CheckNamespace
// Note: Extensions have been put in the namespace of the extended Type to ease discoverability.
namespace System.IO
// ReSharper restore CheckNamespace
{
    internal static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            var streamLength = Convert.ToInt32(stream.Length);
            byte[] data = new byte[streamLength + 1];

            //convert to to a byte array
            stream.Read(data, 0, streamLength);
            stream.Close();

            return data;
        }
    }
}
