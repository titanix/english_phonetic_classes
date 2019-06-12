using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        if(args.Length != 2)
        {
            Console.WriteLine("Error. Usage: <cmu_dict> <output>. Exiting.");
            Environment.Exit(1);
        }

        Program p = new Program();
        p.TransformFile(args[0], args[1]);
    }

    // from https://en.wikipedia.org/wiki/ARPABET
    Dictionary<string, string> Arpabet = 
        new Dictionary<string, string> {
            { "AA", "ɑ" },
            { "AE", "æ" },
            { "AH", "ʌ" },
            { "AO", "ɔ" },
            { "AW", "aʊ" },
            { "AX", "ə" },
            { "AY", "aɪ" },
            { "EH", "ɛ" },
            { "ER", "ɝ" },
            { "EY", "eɪ" },
            { "IH", "ɪ" },
            { "IX", "ɨ" },
            { "IY", "i" },
            { "OW", "oʊ" },
            { "OY", "ɔɪ" },
            { "UH", "ʊ" },
            { "UW", "u" },
            { "B", "b" },
            { "CH", "č" }, // tʃ 
            { "D", "d" },
            { "DH", "ð" },
            { "DX", "ɾ" }, // not in file
            { "EL", "l̩" }, // not in file
            { "EM", "m̩" }, // not in file
            { "EN", "n̩" }, // not in file
            { "F", "f" },
            { "G", "g" },
            { "HH", "h" },
            { "JH", "ǯ" }, // dʒ 
            { "K", "k" },
            { "L", "l" },
            { "M", "m" },
            { "N", "n" },
            { "NG", "ŋ" },
            { "P", "p" },
            { "Q", "ʔ" },
            { "R", "ɹ" },
            { "S", "s" },
            { "SH", "ʃ" },
            { "T", "t" },
            { "TH", "θ" },
            { "V", "v" },
            { "W", "w" },
            { "WH", "ʍ" },
            { "Y", "j" },
            { "Z", "z" },
            { "ZH", "ʒ" },
        };
    
    char[] charToSkip = new char[] { ';', '!', '"', '#', '%', '&', '\'', '(', ')',
        '+', ',', '-', '.', '/', '3', ':', '?' };

    public void TransformFile(string path, string outputPath)
    {
        if(!File.Exists(path))
        {
            throw new ArgumentException();
        }

        using(StreamReader sr = new StreamReader(path))
        using(StreamWriter output = new StreamWriter(outputPath))
        {
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if(charToSkip.Contains(line[0])) 
                    { continue; }
                
                if(line.Contains('('))
                    { continue; } // skip double pronunciation

                int firstSpace = line.IndexOf(' ');
                string word = line.Substring(0, firstSpace);
                string code = line.Substring(firstSpace);

                // delete accent information
                code = code.Replace("0", "");
                code = code.Replace("1", "");
                code = code.Replace("2", "");

                output.Write(word.ToLower());
                output.Write('/');

                string[] phonemes = code.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string apiCode = String.Join(" ", phonemes.Select(x => Arpabet[x]));
                output.WriteLine(apiCode);
            }
        }
    }
}