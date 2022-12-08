using System;
using System.Text;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

class MainClass
{
    public static void Main(string[] args)
    {
        if (args.Length == 0) return;
        using (ZipInputStream s = new ZipInputStream(File.OpenRead(args[0])))
        {

            ZipEntry theEntry;
            int size = 2048;
            byte[] data = new byte[2048];
            int i = 0;
            bool isallshow = false;

            while ((theEntry = s.GetNextEntry()) != null)
            {
                string rr = null;
                if (theEntry.IsFile)
                    rr = "File.";
                else
                    rr = "Directory.";
                Console.WriteLine();
                Console.WriteLine("-----------------------------------");
                Console.Write("Is a " + rr + " Show contents?\n[y]es [n]o [a]ll");
                string inp = Console.ReadLine().ToLower();
                if ((inp == "a") && (!isallshow))
                    isallshow = true;
                if ((inp == "y") || isallshow)
                {
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            Console.Write(new ASCIIEncoding().GetString(data, 0, size));
                            Console.Write("\nExtract (y/n) ?");
                            if (Console.ReadLine().ToLower() == "y")
                            {
                                try
                                {
                                    File.WriteAllBytes(rr + "content." + Convert.ToString(i), data);
                                }
                                catch { }
                            }
                        }
                        else
                            break;
                    }
                }
                i++;
            }
        }
        while (true) ;
    }
}