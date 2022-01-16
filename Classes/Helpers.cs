using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Xml;



namespace CloudUpdater.Classes
{
    class Helpers
    {
        public Dictionary<string, string> parameters = new Dictionary<string, string>(); 
        public Helpers()
        {
            this.truncateFile("cloudUpdater.log");
            this.initParameters();
        }

        public void initParameters()
        {

            string xmlFile = File.ReadAllText("Params.xml");
            XmlReader rdr = XmlReader.Create(new StringReader(xmlFile));

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            while (rdr.Read())
            {
                if (rdr.NodeType == XmlNodeType.Element)
                {
                    if (rdr.LocalName != "root" && rdr.LocalName != "parameters")
                        this.parameters.Add(rdr.LocalName, rdr.ReadInnerXml());
                }
            }
            this.writeLog("Parameters are initialized from file");
        }

        public void truncateFile(string filePath)
        {
            File.Delete(@filePath);
        }

        public void writeLog(string log)
        {
            try
            {
                using (var w = new StreamWriter("cloudUpdater.log", true))
                {
                    w.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ": " + log);

                }
                Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ": " + log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

        }
    }
}
