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
            Log.Info("Shitty radar enabled");

            SetupConfig();
            LoadFile();
        }
        public static string path;

        public static void SaveConfigFile(StorageFile storage)
        {
            string fileName = "CrunchRadar";

            var folder = Path.Combine(path, fileName);
            Directory.CreateDirectory(path + fileName);
            FileUtils xml = new FileUtils();
            xml.WriteToXmlFile(path + "CrunchRadar.xml", storage, false);

        }
        private void SetupConfig()
        {
            FileUtils utils = new FileUtils();
            path = StoragePath;
           
            if (File.Exists(StoragePath + "\\CrunchRadar.xml"))
            {
                config = utils.ReadFromXmlFile<StorageFile>(StoragePath + "\\CrunchRadar.xml");
                utils.WriteToXmlFile<StorageFile>(StoragePath + "\\CrunchRadar.xml", config, false);
            }
            else
            {
                config = new StorageFile();
                utils.WriteToXmlFile<StorageFile>(StoragePath + "\\CrunchRadar.xml", config, false);
            }
   
        }
        public static StorageFile LoadFile()
        {

            FileUtils xml = new FileUtils();
        
            StorageFile file = xml.ReadFromXmlFile<StorageFile>(path + "\\CrunchRadar.xml");
            if (file == null)
            {
                TransmitPlugin.Log.Info("No file");
            }

            return file;
        
        }
    }
}
