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
    public class RecordPackage
    {
        
        public EventInfo eventI;
        public Control current;
        public FormRecorder toAct;
        public Type eventType;


        public RecordPackage(Control currentControl,Form toAct, EventInfo eventI)
        {
            this.toAct = (FormRecorder)toAct;
            this.eventI = eventI;
            this.current = currentControl;
            eventType = eventI.EventHandlerType;

        }

        public void funcDelegate(Object sender, EventArgs args)
        {
            Console.WriteLine("Estou a fazer a funcao funcDelegate!!");
            if (toAct.RecordPressed)
                toAct.richTextBox1.AppendText("XXX\n");

            //MethodInfo writerFunc = GetType().GetMethod("richTextBox1_TextChanged", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Delegate d = Delegate.CreateDelegate( , writerFunc);

        }
        
    }

    public class RecorderReplayer
    {
        private static String TEXTBOX_NAME = "richTextBox1";
        static FormRecorder formRecorder;
        static Configuration<Control> config;

        public RecorderReplayer(Configuration<Control> configuration, FormRecorder form)
        {
            config = configuration;
            formRecorder = form;
        }

        public void Start()
        {
            formRecorder.EnableRecord(config);
        }
    }
    
    public class RecorderService
    {
        
        [STAThread]
        static void Main()
        {
        }
    }
}
