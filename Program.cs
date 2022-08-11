using System;
using System.Text;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

class MainClass
{
    public static void Main(string[] args)
    {
        using (ZipInputStream s = new ZipInputStream(File.OpenRead(args[0])))
        {

            ZipEntry theEntry;
            int size = 2048;
            byte[] data = new byte[2048];

            while ((theEntry = s.GetNextEntry()) != null)
            {
                if (theEntry.IsFile)
                {
                    Console.Write("Show contents (y/n) ?");
                    if (Console.ReadLine() == "y")
                    {
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                Console.Write(new ASCIIEncoding().GetString(data, 0, size));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        while (true) ;
    }
}