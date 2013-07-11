using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using netscribber;
using System.Windows.Forms;
using Configuration;
using Recorder;
using Scribber;

namespace App
{
    class App
    {
      
        static void Main(string[] args)
        {
            var recorderForm = new FormRecorder();
            var toWatchForm = new CodeSelectorForm();
            var config = new Configuration<Control>(toWatchForm);
            config.CostumConfiguration();

            var recorderService = new RecorderReplayer(config,recorderForm);
            recorderService.Start();

            recorderForm.Show();
            Application.Run(toWatchForm);
        }
    }
}
