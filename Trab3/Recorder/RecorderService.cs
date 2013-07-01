using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using netscribber;
using Configuration;
using System.Reflection;

namespace Recorder
{
    public delegate void delegateEvent(Object sender, EventArgs args);

    public class RecordPackage
    {
        public Configuration<Control> config;
        public FormRecorder form;
        public delegateEvent del;

        public RecordPackage(Configuration<Control> config, FormRecorder form, delegateEvent del)
        {
            this.config = config;
            this.form = form;
            this.del = del;
        }

        public void Start()
        {
            var dic = config.getComposedConfiguration();

            foreach (List<ConfigurationX<Control>> elem in dic.Values) //Percorre o dicionario do composedconfiguration
                foreach (ConfigurationX<Control> configX in elem)   // Percorre a lista ConfigurationX
                    foreach (var configXDic in configX.controlEventsAndPredicates.Values) // percorre o dicionario do tipo configurationX
                        foreach (var eventElem in configXDic._listEvent) //percorre a lista de eventos
                            eventElem.AddEventHandler(null, del);
        }
    }

    public class RecorderReplayer
    {

        static FormRecorder formRecorder;

        public static void funcDelegate(Object sender, EventArgs args)
        {
            Control[] f = formRecorder.Controls.Find("ListEvents", true);
            Control c = (Control)sender;
            f[0].Text += (args.GetType().Name + " at " + TimeZone.CurrentTimeZone + " on " + c.Name + " " + sender.GetType().Name + "\n");
            Console.WriteLine(args.GetType().Name + " at " + TimeZone.CurrentTimeZone + " on " + c.Name + " " + sender.GetType().Name + "\n");
        }

        public static RecordPackage Create(Configuration<Control> config, FormRecorder form)
        {
            formRecorder = form;
            return new RecordPackage(config, form, funcDelegate);
        }


    }
    
    static class RecorderService
    {
        
        [STAThread]
        static void Main()
        {
        }
    }
}
