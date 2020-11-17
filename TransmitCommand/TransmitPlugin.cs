using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;
using Torch.API;

namespace TransmitCommand
{
    public class TransmitPlugin : TorchPluginBase
    {
        public static StorageFile config;
        public static Logger Log = LogManager.GetCurrentClassLogger();
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            Log.Info("OOOOOOOOOH RADAR");

            CreatePath();
            LoadFile();
        }
        public static string path;
        public string CreatePath()
        {
            string fileName = "CrunchRadar";

            foreach (var c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c, '_');

            var folder = Path.Combine(StoragePath, fileName);
            Directory.CreateDirectory(folder);
           
            return folder;
        }
        public static void SaveConfigFile(StorageFile storage)
        {
            string fileName = "CrunchRadar";

            var folder = Path.Combine(path, fileName);
            Directory.CreateDirectory(path + fileName);
            FileUtils xml = new FileUtils();
            xml.WriteToJsonFile(path + "CrunchRadar.json", storage, false);

        }
        public static StorageFile LoadFile()
        {

            FileUtils xml = new FileUtils();
            StorageFile file = xml.ReadFromJsonFile<StorageFile>(path + "CrunchRadar.json");
            if (file == null)
            {
                TransmitPlugin.Log.Info("No file");
            }

            return file;
        
        }
    }
}
