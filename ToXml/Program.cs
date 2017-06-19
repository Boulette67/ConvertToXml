using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ToXml.Csv;

namespace ToXml
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Program.WriteColorLine($"=== {DateTime.Now.ToLongDateString()} ===");
            Program.WriteColorLine($"{DateTime.Now.ToLongTimeString()} - Début du traitement");

            string inputExtension = string.Empty;
            string name = string.Empty;
            string inputFile = string.Empty;
            string outputPath = string.Empty;
            string schema = string.Empty;
            FileInfo file;

            try
            {
                Dictionary<string, string> arguments = args
                    .Select(a => a.Split(new[] { '=' }, 2))
                    .GroupBy(a => a[0], a => a.Length == 2 ? a[1] : null)
                    .ToDictionary(g => g.Key, g => g.FirstOrDefault());

                if (arguments.Count == 0)
                    throw new ArgumentException("Pas d'arguments en paramètres.");

                KeyValuePair<string, string> first = arguments.First();
                if (arguments.Count == 1 && string.IsNullOrEmpty(first.Value))
                {
                    inputFile = first.Key;
                    if (!(file = new FileInfo(inputFile)).Exists)
                        throw new ArgumentException($"L'argument {inputFile} n'est pas reconnu comme un fichier valide.");
                }
                else
                {
                    // Gestions des paramètres
                    if (arguments.ContainsKey("-i"))
                    {
                        inputFile = arguments["-i"];
                        if (!(file = new FileInfo(inputFile)).Exists)
                            throw new FileNotFoundException($"L'argument {inputFile} n'est pas reconnu comme un fichier valide.");

                        name = file.Name.Replace(file.Extension, "");
                        inputExtension = file.Extension;
                    }

                    if (arguments.ContainsKey("-o"))
                    {
                        outputPath = arguments["-o"];
                        DirectoryInfo dir = new DirectoryInfo(outputPath);
                        if (!dir.Exists)
                            throw new DirectoryNotFoundException($"Le dossier {outputPath} n'est pas accessible.");
                    }

                    if (arguments.ContainsKey("-s"))
                    {
                        schema = arguments["-s"];
                        //if (!(file = new FileInfo(schema)).Exists)
                            //throw new FileNotFoundException($"L'argument {schema} n'est pas reconnu comme un fichier valide.");
                    }
                    
                }

                switch (inputExtension)
                {
                    case ".csv":
                        CsvHelper h = new CsvHelper();
                        h.ConvertToXml(inputFile, schema, outputPath);
                        break;
                    case ".xml":
                        XsdValidation.GenerateXSD(inputFile, schema);
                        break;
                    default:
                        throw new ArgumentException($"L'argument {inputFile} n'est pas reconnu comme un fichier valide.");
                        break;
                }
                return 1;
            }
            catch (Exception ex)
            {
                Program.WriteColorLine($"{DateTime.Now.ToLongTimeString()} - {ex}", ConsoleColor.DarkRed);

                return 0;
            }
            finally
            {
                Program.WriteColorLine($"{DateTime.Now.ToLongTimeString()} - Fin du traitement.");
                Console.ReadKey();
            }

        }

        public static void WriteColorLine(string message, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

    }
}
