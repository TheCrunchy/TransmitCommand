using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;
using Torch.API;

namespace TransmitCommand
{
    public class TransmitPlugin : TorchPluginBase
    {
        public static Logger Log = LogManager.GetCurrentClassLogger();
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            Log.Info("OOOOOOOOOH RADAR");


        }
    }
}
