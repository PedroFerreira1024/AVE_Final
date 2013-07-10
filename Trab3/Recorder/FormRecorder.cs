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
        public bool RecordPressed=false, ReplayPressed=false;
        public List<ReplayPackage> replaylist = new List<ReplayPackage>();

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
            }
            else
            {
                this.Recoder_Stop.Text = "Recorder";
                this.RecordPressed = false;
            }
        }

        private void Replay_Click(object sender, EventArgs e)
        {
            ReplayPressed = true;
            foreach (var r in replaylist)
            {
                Type fi = r.package.toAct.GetType();
                //                sender---------
                MethodInfo mi = r.package.current.GetType().GetMethod("On" + r.package.eventI.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                Control c = r.sender;

                object[] objs = new object[]{r.arguments};
                mi.Invoke(r.sender,objs);
                //
                //FALTA SABER PORQUE NAO AFECTA A FORM QUE FOI ANALIZADA
                //
            }
            ReplayPressed = false;

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
