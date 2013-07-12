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
        public List<Func<Control,bool>> predicates;

        public RecordPackage(Control currentControl, Form toAct, EventInfo eventI,List<Func<Control,bool>> pred)
        {
            this.toAct = (FormRecorder)toAct;
            this.eventI = eventI;
            this.current = currentControl;
            eventType = eventI.EventHandlerType;
            predicates = pred;
        }

        public void funcDelegate(Object sender, EventArgs args)
        {
            if (toAct.RecordPressed)
            {
                foreach (var func in predicates)
                {
                    if (!func(current))
                        return;
                }

                toAct.richTextBox1.AppendText(eventI.Name + " at " + toAct.getTicksTime() + " occured on " + ((Control)sender).GetType().Name + " " + ((Control)sender).Name + " !\n");
                toAct.addListReplayWithTime(new ReplayPackage(this, args, (Control)sender));
            }
        }


    }


    public class ReplayPackage
    {
        public RecordPackage package;
        public EventArgs arguments;
        public Control sender;
        public int time;

        public ReplayPackage(RecordPackage pack, EventArgs args, Control send)
        {
            package=pack;
            arguments=args;
            sender = send;
        }

        public void replay(object sender, EventArgs e)
        {
            replay_Action();
            ((Timer)sender).Stop();
        }

        private void replay_Action()
        {
            Type fi = this.package.toAct.GetType();
            //                sender---------
            MethodInfo mi = this.package.current.GetType().GetMethod("On" + this.package.eventI.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Control c = this.sender;

            object[] objs = new object[] { this.arguments };
            mi.Invoke(this.sender, objs);
            //this.package.toAct.Update();
            this.package.current.Update();
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
        
        static void Main()
        {
        }
    }
}
