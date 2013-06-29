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

    public class ReccordPackage<T> where T : Control
    {
        public Configuration<Control> config;
        public FormRecorder form;
        public delegateEvent del;

        public ReccordPackage(Configuration<Control> config, FormRecorder form, delegateEvent del)
        {
            this.config = config;
            this.form = form;
            this.del = del;
        }


    }

    public class RecorderReplayer<T> where T : Control
    {

        static FormRecorder formRecorder;

        public static void funcDelegate(Object sender, EventArgs args)
        {
            Control[] f = formRecorder.Controls.Find("ListEvents", true);
            Control c = (Control)sender;
            f[0].Text += (args.GetType().Name + " at " + TimeZone.CurrentTimeZone + " on " + c.Name + " " + sender.GetType().Name + "\n");
        }

        public static ReccordPackage<T> Create(Configuration<Control> config, FormRecorder form)
        {
            formRecorder = form;
            return new ReccordPackage<T>(config, form, funcDelegate);
        }


    }
    
    static class Program
    {
        

        [STAThread]
        static void Main()
        {
        }
    }
}
