using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

using ICSharpCode.SharpZipLib.Zip;

class MainClass
{
    public static void ByteToOutput(byte[] source)
    {
        for (int i = 0; i < source.Length; i++)
        {
            if ((source[i] >= 0x20) && (source[i] < 0x7f))
                Console.Write(Encoding.ASCII.GetChars(new byte[] { source[i] }));
            else
                Console.Write(".");
        }
    }
    public static void ByteToOutputLn(byte[] source)
    {
        ByteToOutput(source);
        Console.WriteLine();
    }
    public static void Main(string[] args)
    {
        string fname = null;
        if (args.Length == 0)
        {
            Console.Write("Input the source zip file you want to check.\n> ");
            fname = Console.ReadLine();
        }
        else
            fname = args[0];
        using (ZipInputStream s = new ZipInputStream(File.OpenRead(fname)))
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
                Console.WriteLine("Is a " + rr + " Show contents?\n[y]es [n]o [b]inary");
                string inp = Console.ReadLine().ToLower();
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
                if (inp == "b")
                {
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            List<byte> hexbyte = new List<byte>();
                            for (int j = 0; j < size; j++)
                            {
                                hexbyte.Add(data[j]);
                                if (j % 16 == 15)
                                {
                                    Console.Write(BitConverter.ToString(hexbyte.ToArray()) + "\t");
                                    ByteToOutputLn(hexbyte.ToArray());
                                    hexbyte.Clear();
                                }
                            }
                            Console.Write(BitConverter.ToString(hexbyte.ToArray()));
                            for (int k = hexbyte.ToArray().Length * 3; k <= 48; k++) Console.Write(' '); 
                            ByteToOutputLn(hexbyte.ToArray());
                            hexbyte.Clear();
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
