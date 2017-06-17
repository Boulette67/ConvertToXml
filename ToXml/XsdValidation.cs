using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace ToXml
{
    internal class XsdValidation
    {

        public static void ValidationXSD(string SchemaName, string FileName)
        {
            Program.WriteColorLine($"{DateTime.Now.ToLongTimeString()} - Validation du document \"{FileName}\" avec le schema \"{SchemaName}\".");

            FileInfo file;
            if (!(file = new FileInfo(SchemaName)).Exists)
                throw new FileNotFoundException($"Le schema {SchemaName} n'existe pas.");

            if (!(file = new FileInfo(FileName)).Exists)
                throw new FileNotFoundException($"Le fichier {FileName} n'existe pas.");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(null, SchemaName);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationEventHandler);

            XmlReader reader = XmlReader.Create(FileName, settings);
            while (reader.Read()) { }

            Program.WriteColorLine($"{DateTime.Now.ToLongTimeString()} - La validation terminée.");
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    Program.WriteColorLine($"{DateTime.Now.ToLongTimeString()} - Erreur: {args.Message}" , ConsoleColor.DarkRed);
                    break;
                case XmlSeverityType.Warning:
                    Program.WriteColorLine($"{DateTime.Now.ToLongTimeString()} - Avertissement: {args.Message}", ConsoleColor.DarkYellow);
                    break;
            }
        }

    }
}
