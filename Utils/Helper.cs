using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ChroniqueBot.Utils
{
    public class Helper
    {
        /*
         * 
         *   using (var saveFile = new SaveFileDialog()
            {
                Title = "Save settings file...",
                Filter = "XML files|*.xml",
                OverwritePrompt = true,
                InitialDirectory = Core.ProfilesDirectory,
            })
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    Helper.Save(saveFile.FileName, Core.Settings);
                }
            }
            BringToFront();*/


        public static string ProfilesDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.Combine(Directory.GetParent(Path.GetDirectoryName(path)).FullName, "ChroniqueUsers", "Profiles");
            }
        }
        public static void Save<T>(string file, T instance)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(Path.Combine(ProfilesDirectory, file)))
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true, IndentChars = "\t" }))
                {
                    serializer.Serialize(xmlWriter, instance, ns);
                }
            }
        }
        //Never used
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);  
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static T Load<T>(string file)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = XmlReader.Create(Path.Combine(ProfilesDirectory, file)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}

