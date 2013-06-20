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
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("\ndot_clean for .NET v1.0\n(c) 2013 by bagnz0r <http://github.com/bagnz0r>\n\n");

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: dot_clean <path>\n\n");
            }

            // Take path and verify that it exists.
            string path = args[0];
            if (Directory.Exists(path) != true)
            {
                Console.WriteLine("Path doesn't exist.");
                Environment.Exit(1);
            }

            // List all possible junk files here.
            Console.WriteLine("Looking for junk files... (Be patient)\n");
            List<string> junkFiles = FindJunkFiles(path);

            // Spam with some statistical bullshit.
            Console.WriteLine("Found " + (junkFiles.Count).ToString() + " junk files!\n\n");

            // Delete all 'em suckers.
            try
            {
                DeleteJunkFiles(junkFiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The program has failed to run!");
                File.WriteAllText("error.log", ex.Message + ": " + ex.StackTrace);
            }

        }

        /// <summary>
        /// Finds and returns all the junk files possible.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static List<string> FindJunkFiles(string path)
        {
            List<string> junkFiles = new List<string>();

            // Find all the possible junk files that occur in every directory (OS X specific).
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, ".com.apple*", SearchOption.AllDirectories));
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, "._*", SearchOption.AllDirectories));
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, ".DS_Store", SearchOption.AllDirectories));
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, ".__MACOSX", SearchOption.AllDirectories));

            // Find all the possible junk files that occur in every directory (Windows specific).
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, "Thumbs.db", SearchOption.AllDirectories));

            // Find all the possible junk folders that occur only in top path (OS X specific).
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, ".Spotlight-V100", SearchOption.TopDirectoryOnly));
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, ".TemporaryItems", SearchOption.TopDirectoryOnly));
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, ".Trashes", SearchOption.TopDirectoryOnly));
            AddAllItemsToList(junkFiles, Directory.GetFiles(path, ".fseventsd", SearchOption.AllDirectories));

            return junkFiles;
        }

        /// <summary>
        /// Deletes all the junk files and logs the progress.
        /// </summary>
        /// <param name="junkFiles"></param>
        private static void DeleteJunkFiles(List<string> junkFiles)
        {
            StreamWriter logWriter = new StreamWriter("dot_clean.log");

            foreach (string junkFile in junkFiles)
            {
                if (Directory.Exists(junkFile))
                {
                    Directory.Delete(junkFile);
                }
                else
                {
                    File.Delete(junkFile);
                }

                string removedMsg = "Removed '" + junkFile + "'...";
                Console.WriteLine(removedMsg + "\n");
                logWriter.WriteLine(removedMsg);
            }

            logWriter.Flush();
            logWriter.Dispose();
        }


        /// <summary>
        /// Loops over all the items in an array and adds them to referenced list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private static List<string> AddAllItemsToList(List<string> list, string[] items)
        {
            foreach (string item in items)
            {
                list.Add(item);
            }

            return list;
        }

    }
}
