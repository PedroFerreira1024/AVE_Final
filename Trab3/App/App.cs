using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using netscribber;
using System.Windows.Forms;
using Configuration;
using Recorder;

namespace App
{
    class App
    {
      
        static void Main(string[] args)
        {


            var form = new FormRecorder();
            var config = new Configuration<Control>(form);
            config.CostumConfiguration();

            var recorderService = RecorderReplayer.Create(config, form);
            recorderService.Start();

            Application.Run(form);
        }
    }
}
