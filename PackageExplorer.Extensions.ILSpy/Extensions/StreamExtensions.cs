// ReSharper disable CheckNamespace
// Note: Extensions have been put in the namespace of the extended Type to ease discoverability.
namespace System.IO
// ReSharper restore CheckNamespace
{
    internal static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);

                return ms.ToArray();
            }
        }
    }
}
