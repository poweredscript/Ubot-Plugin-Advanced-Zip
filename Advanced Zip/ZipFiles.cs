using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using UBotPlugin;

namespace Advanced_Zip
{
    class XZipFilesX : IUBotCommand
    {
        private readonly List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();

        public XZipFilesX()
        {
            _parameters.Add(new UBotParameterDefinition("File Path/Lists", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Zip Save Path", UBotType.String));
        }
        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            var filesIn1 = parameters["File Path/Lists"];
            var zipSavePath = parameters["Zip Save Path"].Trim();
            try
            {
                var filesIn2 = Regex.Split(filesIn1, "\\n");
                using (var s = new ZipOutputStream(File.Create(@zipSavePath)) { UseZip64 = UseZip64.Dynamic })
                {
                   
                    s.SetLevel(3); // 0 - store only to 9 - means best compression
                    foreach (var file in filesIn2)
                    {
                        var file2 = file.Trim();
                        if (file2 != "")
                        {
                            var fs = File.OpenRead(@file2);
                            var buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            var di = new DirectoryInfo(@file);
                            var entry = new ZipEntry(ZipEntry.CleanName(di.Name))
                            {
                                DateTime = DateTime.Now,
                                ZipFileIndex = 1,
                                Size = fs.Length
                            };
                            fs.Close();
                            s.PutNextEntry(entry);
                            s.Write(buffer, 0, buffer.Length);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch
            {
               // MessageBox.Show(ex.Message);
            }
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
                return "zip files";
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