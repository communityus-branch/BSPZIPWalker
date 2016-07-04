﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectoryWalker
{
    class Program
    {
        static List<string> output;

        static int Main(string[] args)
        {
            string currentDir = string.Empty;
            string outputFile = string.Empty;

            if (args.Length > 0)
                currentDir = args[0];
            //else
            //    currentDir = Environment.CurrentDirectory;

            if (args.Length > 1)
                outputFile = args[1];
            //else
            //    outputFile = "DirectoryWalker.out.txt";

            if (currentDir == string.Empty || outputFile == string.Empty)
            {
                Console.WriteLine("Need more parameters:\npara[0] = Current Directory\npara[1] = Output File");
                return -1;
            }

            if (!Directory.Exists(currentDir))
            {
                Console.WriteLine("Directory doesn't exist.");
                return -1;
            }

            // Ensure we've got the fully qualified path
            currentDir = new DirectoryInfo(currentDir).FullName;

            output = new List<string>();
            if (ProcessDirectory(currentDir, currentDir) != 0)
                return -1;

            // Write the final output
            File.WriteAllLines(outputFile, output);

            return 0;

        }

        static int ProcessDirectory(string root, string currentDir)
        {
            try
            {
                var files = Directory.GetFiles(currentDir);
                var directories = Directory.GetDirectories(currentDir);

                foreach (var file in files)
                {
                    string fullName = new FileInfo(file).FullName;
                    string relativeName = fullName.Replace(root + @"\", string.Empty);

                    output.Add(relativeName);
                    output.Add(fullName);
                }

                foreach (var directory in directories)
                    if (ProcessDirectory(root, new DirectoryInfo(directory).FullName) != 0)
                        return -1;
            }
            catch (Exception ex)
            {
                File.AppendAllText("DirectoryWalker.error.txt", ex.ToString() + Environment.NewLine);
                Console.WriteLine("Exception in DirectoryWalker:ProcessDirectory" + Environment.NewLine + ex.ToString());

                return -1;
            }

            return 0;
        }
    }
}
