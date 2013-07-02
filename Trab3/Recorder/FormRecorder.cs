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
        private bool buttonPress;
        public FormRecorder()
        {
            InitializeComponent();
        }

        private void Recoder_Stop_Click(object sender, EventArgs e)
        {
            if (!buttonPress)
            {
                this.Recoder_Stop.Text = "Stop";
                this.buttonPress = true;
            }
            else
            {
                this.Recoder_Stop.Text = "Recorder";
                this.buttonPress = false;
            }
        }

        private void Replay_Click(object sender, EventArgs e)
        {
            //reproduz a lista de eventos
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            //apaga a lista de eventos
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        public void Start(Configuration<Control> config)
        {
            var dic = config.getComposedConfiguration();

            foreach (List<ConfigurationX<Control>> elem in dic.Values) //Percorre o dicionario do composedconfiguration
                foreach (ConfigurationX<Control> configX in elem)   // Percorre a lista ConfigurationX
                    foreach (Control control in configX.controlEventsAndPredicates.Keys) // percorre o dicionario do tipo configurationX
                        foreach (EventInfo eventElem in configX.controlEventsAndPredicates[control]._listEvent)
                        { //percorre a lista de eventos

                            RecordPackage pack = new RecordPackage(control, this.richTextBox1, eventElem);

                            MethodInfo mInf = pack.GetType().GetMethod("funcDelegate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                            Delegate del = Delegate.CreateDelegate(eventElem.EventHandlerType, pack, mInf, false);

                            eventElem.AddEventHandler(control, del);
                        }
        }
    }
}
