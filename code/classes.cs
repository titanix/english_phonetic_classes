using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    // input: cmu_ipa.txt, classes.txt
    // output: cmu_classes.txt
    public static void Main(string[] args)
    {
        if(args.Length != 3)
        {
            Console.WriteLine("Error: missing argument. Usage <cmu_ipa_file> <class_mapping_file> <output_file>.");
            Environment.Exit(1);
        }

        Program p = new Program();
        p.LoadClassMapping(args[1]);
        p.TransformFile(args[0]);
        p.WriteResult(args[2]);
    }

    Dictionary<string, string> classIndex = new Dictionary<string, string>();

    public void LoadClassMapping(string path)
    {
        using(StreamReader sr = new StreamReader(path))
        {
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if(String.IsNullOrEmpty(line)) continue;
                if(line[0] == '#') continue;

                string[] parts = line.Split(' ');

                string pClass = parts[0];

                for (int i = 1; i < parts.Length; i++)
                {
                    classIndex.Add(parts[i], pClass);
                }
            }
        }
    }

    Dictionary<string, List<string>> classResults = new Dictionary<string, List<string>>();

    public void TransformFile(string input)
    {
        using(StreamReader inFile = new StreamReader(input))
        {
            while(!inFile.EndOfStream)
            {
                string line = inFile.ReadLine();

                string[] parts = line.Split('/');
                string word = parts[0];
                string ipa = String.Join("", parts[1].Split(' '));
                string classified = "";

                foreach (char c in ipa)
                {
                    if(classIndex.ContainsKey(c.ToString()))
                    {
                        classified += classIndex[c.ToString()];
                    }
                    else
                    {
                        classified += "_";
                    }
                }

                if(!classResults.ContainsKey(classified))
                {
                    List<string> list = new List<string>();
                    list.Add(word);
                    classResults.Add(classified, list);
                }
                else
                {
                    classResults[classified].Add(word);
                }
            }
        }
    }

    public void WriteResult(string output)
    {
        using(StreamWriter outFile = new StreamWriter(output))
        {
            foreach (KeyValuePair<string, List<string>> kv in classResults)
            {
                outFile.Write(kv.Key);
                outFile.Write("->");
                outFile.WriteLine(String.Join(' ', kv.Value));
            }
        }
    }
}