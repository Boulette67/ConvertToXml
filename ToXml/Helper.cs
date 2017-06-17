using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToXml
{
    internal class Helper
    {
        public string name;
        public XDocument doc;

        public void ConvertToXml(string FileName, string Schema = "", string Output = "")
        {

            // Validation du xml
            string outFileName = $"{name}.xml";
            if (!string.IsNullOrEmpty(Output))
            {
                DirectoryInfo dir = new DirectoryInfo(Output);
                if (!dir.Exists)
                    throw new DirectoryNotFoundException($"Le dossier {Output} n'est pas accessible.");
                outFileName = Path.Combine(Output, outFileName);
            }
            else
            {
                outFileName = $".\\{outFileName}";
            }

            doc.Save(outFileName);
            Program.WriteColorLine($"{ DateTime.Now.ToLongTimeString()} - La génération du fichier \"{outFileName}\" est terminée.");

            if (!string.IsNullOrEmpty(Schema))
                XsdValidation.ValidationXSD(Schema, outFileName);

        }
        
    }
}
