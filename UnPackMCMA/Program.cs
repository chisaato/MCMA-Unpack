using System;
using System.IO;
using System.IO.Compression;

namespace UnPackMCMA
{
    class Program
    {
        private const string cta = "http://www.mcmyadmin.com/Downloads/MCMA2-Latest.zip";
        private const string ctb = "McMyAdmin.ebi";
        public static string ctc = "";
        private static bool ctd = false;
        private static bool cte = false;
        private static byte[] g = new byte[]
        {
            69,
            66,
            73,
            1
        };

        public static byte[] gzipDecompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        static void Main(string[] args)
        {
            // 下面是你要读取的文件位置
            byte[] array = File.ReadAllBytes("G:\\MCMA\\McMyAdmin.ebi"); 
            for (int i = 0; i < g.Length; i++)
            {
                if (array[i] != g[i])
                {
                    Console.WriteLine("Unrecognised File Header");
                }
            }
            MemoryStream memoryStream = new MemoryStream(array, false);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Seek(-12L, SeekOrigin.End);
            int num = binaryReader.ReadInt32() - 2;
            int num2 = binaryReader.ReadInt32();
            int num3 = binaryReader.ReadInt32();
            memoryStream.Seek((long)(-(long)(num + 20 - num3)), SeekOrigin.Current);
            int count = binaryReader.ReadInt32();
            int count2 = binaryReader.ReadInt32();
            memoryStream.Seek(0L, SeekOrigin.Begin);
            byte[] array2 = new byte[]
        {
            31,
            139
        };
            binaryReader.ReadBytes(4);
            byte[] signature = binaryReader.ReadBytes(count);
            byte[] keyBlob = binaryReader.ReadBytes(count2);
            memoryStream.Seek(8L, SeekOrigin.Current);
            byte[] array3 = binaryReader.ReadBytes(num);
            byte[] array4 = new byte[num + 2];
            array2.CopyTo(array4, 0);
            array3.CopyTo(array4, 2);
            byte[] array5 = new byte[num2];
            MemoryStream a_ = new MemoryStream(array4);

            // 解压GZIP数组,省去再次解压的步骤
            byte[] output = gzipDecompress(array4);

            // 在下面定义你要输出的文件位置
            FileStream fs = new FileStream("G:\\MCMA\\McMyAdmin-ebi-UnPack.exe", FileMode.Create); 
            //将byte数组写入文件中
            fs.Write(output, 0, output.Length);
            fs.Close();
        }
    }
}
