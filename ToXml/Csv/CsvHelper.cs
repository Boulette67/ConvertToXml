using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToXml.Csv
{
    internal class CsvHelper : Helper
    {
        public void ConvertToXml(string FileName, string Schema = "", string Output = "")
        {

            FileInfo file;

            if (!(file = new FileInfo(FileName)).Exists)
                throw new FileNotFoundException($"Le fichier {FileName} n'existe pas.");

            name = file.Name.Replace(file.Extension, "");

            XElement xml = new XElement(name + "s");
            doc = new XDocument(xml);

            using (StreamReader sr = file.OpenText())
            {
                int compteur = 1;
                string[] firstLine = sr.ReadLine().Split(';');
                string l = "";
                while ((l = sr.ReadLine()) != null)
                {
                    compteur++;
                    string[] currLine = l.Split(';');

                    if (firstLine.Length != currLine.Length)
                        throw new Exception($"Ligne {compteur} : Le nombre de colonnes ne correspond pas à celui de l'entête.");

                    XElement item = new XElement(name);
                    xml.Add(item);
                    for (int i = 0; i < firstLine.Length; i++)
                    {
                        item.Add(new XElement(XName.Get(firstLine[i]), currLine[i]));
                    }
                }
            }

            base.ConvertToXml(FileName, Schema, Output);

        }
    }
}
