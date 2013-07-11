using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Configuration;
using Recorder;



namespace netscribber
{
    public partial class FormRecorder : Form
    {
        public readonly int TIMER_INTERVAL = 1000/20;
        public int tickCount = 0,currentTick=0,lastTicks=0;

        public Timer t = new Timer();
        public bool RecordPressed=false, ReplayPressed=false;

        public List<ReplayPackage> replayList = new List<ReplayPackage>();
        
        private Dictionary<int, List<ReplayPackage>> replayTime = new Dictionary<int, List<ReplayPackage>>();
        private int[] keys;

        public FormRecorder()
        {
            InitializeComponent();
        }

        private void Recoder_Stop_Click(object sender, EventArgs e)
        {
            if (!RecordPressed)
            {
                this.Recoder_Stop.Text = "Stop";
                this.RecordPressed = true;
                
                timer_Start(timerRecord_Func);
            }
            else
            {
                this.Recoder_Stop.Text = "Recorder";
                this.RecordPressed = false;
                timer_Stop();
            }
        }

        private void timer_Start(EventHandler target)
        {
            t.Tick += target;
            t.Interval = TIMER_INTERVAL;
            t.Start();
        }

        private void timer_Stop()
        {
            t.Stop();
            keys = replayTime.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value).Keys.ToArray();
            currentTick=0;
        }

        private void timerRecord_Func(object sender, EventArgs e)
        {
            ++tickCount;
            List<ReplayPackage> aux = new List<ReplayPackage>();
            aux.AddRange(replayList);
            if (replayList.Count > 0)
            {
                replayTime.Add(tickCount,aux);
                replayList.Clear();
            }
        }

        //======================================================================================================================
        private void timerReplay_Func(object sender, EventArgs e)
        {
            ++tickCount;
        }

        private void Replay_Click(object sender, EventArgs e)
        {
            ReplayPressed = true;
            
            for (int i = 0; i < keys.Length; ++i)
            {
                System.Threading.Thread.Sleep((keys[i] - lastTicks) * TIMER_INTERVAL);
                replay_Moment(i);
            }
            
            ReplayPressed = false;
        }

        private void replay_Moment(int moment)
        {
            foreach (var r in replayTime[keys[moment]])
            {
                Type fi = r.package.toAct.GetType();
                //                sender---------
                MethodInfo mi = r.package.current.GetType().GetMethod("On" + r.package.eventI.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                Control c = r.sender;

                object[] objs = new object[] { r.arguments };
                mi.Invoke(r.sender, objs);
                Update();
            }
            lastTicks = keys[moment];
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        public void EnableRecord(Configuration<Control> config)
        {
            var dic = config.getComposedConfiguration();

            foreach (List<ConfigurationX<Control>> elem in dic.Values) //Percorre o dicionario do composedconfiguration
                foreach (ConfigurationX<Control> configX in elem)   // Percorre a lista ConfigurationX
                    foreach (Control control in configX.controlEventsAndPredicates.Keys) // percorre o dicionario do tipo configurationX
                        foreach (EventInfo eventElem in configX.controlEventsAndPredicates[control]._listEvent)
                        { //percorre a lista de eventos

                            RecordPackage pack = new RecordPackage(control, this , eventElem);

                            MethodInfo mInf = pack.GetType().GetMethod("funcDelegate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                            Delegate del = Delegate.CreateDelegate(eventElem.EventHandlerType, pack, mInf, false);

                            eventElem.AddEventHandler(control, del);
                        }
        }
    }
}
