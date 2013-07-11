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

        [STAThread]
        public void funcDelegate(Object sender, MouseEventArgs args)
        {
            Console.WriteLine("Estou a fazer a funcao funcDelegate!!");
            if (toAct.RecordPressed)
            {
                toAct.richTextBox1.AppendText(eventI.Name + " at " + "XX-XX-XX" + " occured on " + ((Control)sender).GetType().Name + " " + ((Control)sender).Name + " !\n");
                toAct.replayList.Add(new ReplayPackage(this, args, (Control)sender));
            }
            else
            {
                if(toAct.ReplayPressed)
                toAct.richTextBox1.AppendText(eventI.Name + " at " + "XX-XX-XX" + " occured on " + ((Control)sender).GetType().Name + " " + ((Control)sender).Name + " !\n");
            }
            
        }
        
    }


    public class ReplayPackage
    {
        public RecordPackage package;
        public MouseEventArgs arguments;
        public Control sender;

        public ReplayPackage(RecordPackage pack, MouseEventArgs args, Control send)
        {
            package=pack;
            arguments=args;
            sender = send;
        }
    }

    public class RecorderReplayer
    {
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
