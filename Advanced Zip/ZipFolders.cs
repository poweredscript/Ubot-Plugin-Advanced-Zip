using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using UBotPlugin;

namespace Advanced_Zip
{
    class XZipFoldersX : IUBotCommand
    {
        private readonly List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();

        public XZipFoldersX()
        {
            _parameters.Add(new UBotParameterDefinition("Folder Path/Lists", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Zip Save Path", UBotType.String));
        }
        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            var foldersIn1 = parameters["Folder Path/Lists"];
            var zipSavePath = parameters["Zip Save Path"].Trim();
            try
            {
                var foldersIn2 = Regex.Split(foldersIn1, "\\n");//MessageBox.Show(String.Format("1:{0} / 2:{1}", foldersIn2[0], foldersIn2[1]));
                using (var z = new ZipOutputStream(File.Create(@zipSavePath)))
                {
                 
                    z.UseZip64 = UseZip64.Dynamic;
                    z.SetLevel(3);
                    foreach (var folder in foldersIn2)
                    {
                        var folder2 = folder.Trim();
                        if (folder2 != "")
                            ZipFolder(@folder2, @folder2, z);
                    }
                    z.Finish();
                    z.Close();
                }
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private static void ZipFolder(string rootFolder, string currentFolder, ZipOutputStream zStream)
    {

        var subFolders = Directory.GetDirectories(currentFolder);
        foreach (var folder in subFolders)
            ZipFolder(rootFolder, folder, zStream);

        var relativePath = currentFolder.Substring(rootFolder.Length) + "/";

        if (relativePath.Length > 1)
        {
            var dirEntry = new ZipEntry(relativePath) {DateTime = DateTime.Now};
        }
            foreach (var file in Directory.GetFiles(currentFolder))
        {
            AddFileToZip(zStream, relativePath, file);
        }
    }
        private static void AddFileToZip(ZipOutputStream zStream, string relativePath, string file)
        {
            var buffer = new byte[4096];
            var fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty) + Path.GetFileName(file);
            var entry = new ZipEntry(fileRelativePath) {DateTime = DateTime.Now};
            zStream.PutNextEntry(entry);
            using (var fs = File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);

                } while (sourceBytes > 0);
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
                return "zip folders";
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
