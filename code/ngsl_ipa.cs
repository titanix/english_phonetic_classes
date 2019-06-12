using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    // input: ngsl.txt, cmu_ipa.txt
    // output: ngsl_ipa.txt
    public static void Main(string[] args)
    {
        if(args.Length != 3)
        {
            Console.WriteLine("Error: missing argument. Usage <ngsl_file> <cmu_ipa> <output_file>.");
            Environment.Exit(1);
        }

        Program p = new Program();
        p.LoadNgsl(args[0]);
        p.Extract(args[1], args[2]);
    }

    HashSet<string> ngsl = new HashSet<string>();

    public void LoadNgsl(string path)
    {
        using(StreamReader sr = new StreamReader(path))
        {
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                ngsl.Add(line);
            }
        }
    }

    public void Extract(string input, string output)
    {
        using(StreamReader inFile = new StreamReader(input))
        using(StreamWriter outFile = new StreamWriter(output))
        {
            while(!inFile.EndOfStream)
            {
                string line = inFile.ReadLine();

                string[] parts = line.Split('/');
                string word = parts[0];

                if(ngsl.Contains(word))
                {
                    outFile.WriteLine(line);
                }
            }
        }
    }
}