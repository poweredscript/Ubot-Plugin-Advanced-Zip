using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ICSharpCode.SharpZipLib.Zip;
using UBotPlugin;

namespace Advanced_Zip
{
    class XUnZipFilesX : IUBotCommand
    {
        private readonly List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();

        public XUnZipFilesX()
        {
            _parameters.Add(new UBotParameterDefinition("Zip File Path", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("UnZip To Path", UBotType.String));
        }
        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            var filesIn = parameters["Zip File Path"];
            var unZipPath = parameters["UnZip To Path"].Trim();
            
            try
            {
                UnZipFiles(filesIn, unZipPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void UnZipFiles(string zipPathAndFile, string outputFolder)
        {
            var s = new ZipInputStream(File.OpenRead(zipPathAndFile));
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                var directoryName = outputFolder;
                var fileName = Path.GetFileName(theEntry.Name);
                // create directory 
                if (directoryName != "")
                {
                    Directory.CreateDirectory(directoryName);
                }
                if (fileName != String.Empty)
                {
                    
                    var fullPath = string.Format("{0}\\{1}", directoryName, theEntry.Name);
                    fullPath = fullPath.Replace("\\ ", "\\");
                    var fullDirPath = Path.GetDirectoryName(fullPath);
                    if (fullDirPath != null && !Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                    var streamWriter = File.Create(fullPath);
                    var data = new byte[2048];
                    while (true)
                    {
                        var size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                    
                }
            }
            s.Close();
        }
    

        public string Category
        {
            get
            {
                return "Zip Commands";
            }
        }

        public string CommandName
        {
            get
            {
                return "unzip file";
            }
        }

        public bool IsContainer
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get
            {
                return _parameters;
            }
        }

        public UBotVersion UBotVersion
        {
            get
            {
                return UBotVersion.Standard;
            }
        }    
    }
}
