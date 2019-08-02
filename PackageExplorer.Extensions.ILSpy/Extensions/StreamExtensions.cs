using System.IO;

namespace PackageExplorer.Extensions.ILSpy
{
    internal static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            using var newStream = new MemoryStream();

            var buffer = new byte[2048]; // 2KB
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                newStream.Write(buffer, 0, read);
            }
            var result = newStream.ToArray();
            return result;
        }
    }
}
