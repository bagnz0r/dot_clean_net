using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dot_clean
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\ndot_clean for .NET v1.0\n(c) 2013 by bagnz0r <http://github.com/bagnz0r>n\n");

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: dot_clean <path>\n\n");
            }

            string path = args[0];

            Console.WriteLine("Looking for junk files... (Be patient)\n");
            string[] dotUnderscore = Directory.GetFiles(path, "._*", SearchOption.AllDirectories);
            string[] dsStore = Directory.GetFiles(path, ".DS_Store", SearchOption.AllDirectories);

            Console.WriteLine("Found " + (dotUnderscore.Length + dsStore.Length).ToString() + " junk files!\n\n");

            try
            {
                foreach (string dotFile in dotUnderscore)
                {
                    if (Directory.Exists(dotFile))
                    {
                        Directory.Delete(dotFile);
                    }
                    else
                    {
                        File.Delete(dotFile);
                    }

                    Console.WriteLine("Removed '" + dotFile + "'...\n");
                }

                foreach (string dsFile in dsStore)
                {
                    if (Directory.Exists(dsFile))
                    {
                        Directory.Delete(dsFile);
                    }
                    else
                    {
                        File.Delete(dsFile);
                    }

                    Console.WriteLine("Removed '" + dsFile + "'...\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The program has failed to run!");
                File.WriteAllText("error.log", ex.Message + ": " + ex.StackTrace);
            }

        }
    }
}
