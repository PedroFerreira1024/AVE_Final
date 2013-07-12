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
        private readonly int TIMER_INTERVAL = 1000/20;
        private readonly int MINUTE=60000, SECOND=1000;
        public int tickCount = 0;

        public Timer t = new Timer();
        public bool RecordPressed = false;

        private List<ReplayPackage> replayList = new List<ReplayPackage>();
        
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
                
                timer_Start(t,timerRecord_Func);
            }
            else
            {
                this.Recoder_Stop.Text = "Recorder";
                this.RecordPressed = false;
                timer_Stop(t);
            }
        }

        private void timer_Start(Timer timer, EventHandler target)
        {
            timer.Tick += target;
            timer.Interval = TIMER_INTERVAL;
            timer.Start();
        }

        public void addListReplayWithTime(ReplayPackage rep)
        {
            rep.time = tickCount;
            replayList.Add(rep);
        }

        private void timer_Stop(Timer timer)
        {
            timer.Stop();
            tickCount=0;
        }

        public String getTicksTime()
        {
            TimeSpan t = TimeSpan.FromMilliseconds(tickCount * TIMER_INTERVAL);
            return t.Minutes +"."+ t.Seconds +"."+ t.Milliseconds;
        }

        private void timerRecord_Func(object sender, EventArgs e)
        {
            ++tickCount;
        }

        private void timerReplay_Func(object sender, EventArgs e)
        {

        }

        private void Replay_Click(object sender, EventArgs e)
        {
            timer_Start(t, timerRecord_Func);
            
            foreach(var rep in replayList)
            {
                Timer auxTimer = new Timer();
                auxTimer.Interval = (rep.time - tickCount) * TIMER_INTERVAL;
                auxTimer.Tick+=rep.replay;
                auxTimer.Start();
            }

            timer_Stop(t);
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

            foreach (List<ConfigurationX> elem in dic.Values) //Percorre o dicionario do composedconfiguration
                foreach (ConfigurationX configX in elem)   // Percorre a lista ConfigurationX
                    foreach (Control control in configX.controlEventsAndPredicates.Keys)
                    { // percorre o dicionario do tipo configurationX
                        foreach (EventInfo eventElem in configX.controlEventsAndPredicates[control]._listEvent)
                        { //percorre a lista de eventos
                            
                            List<Func<Control,bool>> list = configX.controlEventsAndPredicates[control]._listfunc;
                            
                            RecordPackage pack = new RecordPackage(control,this,eventElem,list);

                            MethodInfo mInf = pack.GetType().GetMethod("funcDelegate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                            Delegate del = Delegate.CreateDelegate(eventElem.EventHandlerType, pack, mInf, false);

                            eventElem.AddEventHandler(control, del);
                        }    
                    }
        }
    }
}
