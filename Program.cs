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
        Console.WriteLine("Contents of the zip file:");
        using (ZipInputStream s = new ZipInputStream(File.OpenRead(fname)))
        {
            ZipEntry list_entry;
            while ((list_entry = s.GetNextEntry()) != null)
            {
                if (list_entry.IsFile)
                    Console.ForegroundColor = ConsoleColor.Blue;
                else
                    Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(list_entry.Name);
            }
        }
        Console.ResetColor();
        using (ZipInputStream s = new ZipInputStream(File.OpenRead(fname)))
        {

            ZipEntry theEntry;
            int size = 2048;
            byte[] data = new byte[2048];
            int i = 0;
            bool isallshow = false;
            bool isallbinary = false;

            while ((theEntry = s.GetNextEntry()) != null)
            {
                string rr = null;
                if (theEntry.IsFile)
                    rr = "File.\t";
                else
                    rr = "Directory.";
                Console.WriteLine();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine(rr + "\tName: " + theEntry.Name);
                string inp = "";
                if ((!isallshow) && (!isallbinary))
                {
                    Console.WriteLine("Show contents?\n[y]es [n]o [b]inary [a]ll All_b[i]nary");
                    inp = Console.ReadLine().ToLower();
                }
                else 
                {
                    inp = "";
                    Console.WriteLine();
                }
                if (inp == "a") isallshow = true;
                if (inp == "i") isallbinary = true;
                if ((inp == "y") || isallshow)
                {
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            Console.Write(new ASCIIEncoding().GetString(data, 0, size));
                        }
                        else
                            break;
                    }
                }
                if (inp == "b" || isallbinary)
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
                            if (hexbyte.Count != 0) for (int k = hexbyte.ToArray().Length * 3; k <= 48; k++) Console.Write(' ');
                            ByteToOutput(hexbyte.ToArray());
                            hexbyte.Clear();
                        }
                        else
                            break;
                    }
                }
                i++;
            }
        }
        //while (true) ;
    }
}
