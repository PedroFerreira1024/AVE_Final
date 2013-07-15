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
        private readonly int TIMER_INTERVAL = 1000/25;
        private int tickCount = 0,replayIndex;

        public Timer t = new Timer();
        public bool RecordPressed = false;

        private List<ReplayPackage> replayList = new List<ReplayPackage>();
        private ReplayPackage[] toReplayList;
        
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
                replayList.Clear();
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
            tickCount = 0;
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

            ++tickCount;
            if (replayIndex >= toReplayList.Length)
            {
                timer_Stop((Timer)sender);
                replayIndex = 0;
                return;
            }

            ReplayPackage rep = toReplayList[replayIndex];
            while (replayIndex < toReplayList.Length && rep.time == tickCount)
            {
                rep = toReplayList[replayIndex];
                rep.replay_Action();
                ++replayIndex;
            }
        }

        private void Replay_Click(object sender, EventArgs e)
        {
            t = new Timer();
            timer_Start(t,timerReplay_Func);
            toReplayList = replayList.ToArray();
        }



        private void Clear_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        
    }
}
