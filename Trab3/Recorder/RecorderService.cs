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

                toAct.listBox.Items.Add(eventI.Name + " at " + toAct.getTicksTime() + " occured on " + ((Control)sender).GetType().Name + " " + ((Control)sender).Name + " !");
                toAct.addListReplayWithTime(new ReplayPackage(this, args, (Control)sender));
            }
        }
    }


    public class ReplayPackage
    {
        public RecordPackage package;
        public EventArgs arguments;
        public Control sender;
        public Type formType;
        public MethodInfo method;
        public int time;

        public ReplayPackage(RecordPackage pack, EventArgs args, Control send)
        {
            package=pack;
            arguments=args;
            sender = send;
            formType = package.toAct.GetType();
            method = package.current.GetType().GetMethod("On" + this.package.eventI.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        public void replay_Action()
        {            
            method.Invoke(this.sender, new object[] { this.arguments });
            
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

        public void EnableConfig()
        {
            var dic = config.getComposedConfiguration();

            foreach (List<ConfigurationX> elem in dic.Values) //Percorre o dicionario do composedconfiguration
                foreach (ConfigurationX configX in elem)   // Percorre a lista ConfigurationX
                    foreach (Control control in configX.controlEventsAndPredicates.Keys)
                    { // percorre o dicionario do tipo configurationX
                        foreach (EventInfo eventElem in configX.controlEventsAndPredicates[control]._listEvent)
                        { //percorre a lista de eventos

                            List<Func<Control, bool>> list = configX.controlEventsAndPredicates[control]._listfunc;

                            RecordPackage pack = new RecordPackage(control, formRecorder, eventElem, list);

                            MethodInfo mInf = pack.GetType().GetMethod("funcDelegate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                            Delegate del = Delegate.CreateDelegate(eventElem.EventHandlerType, pack, mInf, false);

                            eventElem.AddEventHandler(control, del);
                        }
                    }
        }

        static void Main()
        {
        }
    }
}
