using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if (buttonPress)
            {
                this.Recoder_Stop.Text = "Stop";
                this.buttonPress = false;
            }
            else
            {
                this.Recoder_Stop.Text = "Recorder";
                this.buttonPress = true;
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

        private void ListEvents_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
