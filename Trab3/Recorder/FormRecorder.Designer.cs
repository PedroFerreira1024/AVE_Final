namespace netscribber
{
    partial class FormRecorder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Recoder_Stop = new System.Windows.Forms.Button();
            this.Replay = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Recoder_Stop
            // 
            this.Recoder_Stop.Location = new System.Drawing.Point(61, 417);
            this.Recoder_Stop.Name = "Recoder_Stop";
            this.Recoder_Stop.Size = new System.Drawing.Size(75, 23);
            this.Recoder_Stop.TabIndex = 0;
            this.Recoder_Stop.Text = "Recorder";
            this.Recoder_Stop.UseVisualStyleBackColor = true;
            this.Recoder_Stop.Click += new System.EventHandler(this.Recoder_Stop_Click);
            // 
            // Replay
            // 
            this.Replay.Location = new System.Drawing.Point(155, 417);
            this.Replay.Name = "Replay";
            this.Replay.Size = new System.Drawing.Size(75, 23);
            this.Replay.TabIndex = 1;
            this.Replay.Text = "Replay";
            this.Replay.UseVisualStyleBackColor = true;
            this.Replay.Click += new System.EventHandler(this.Replay_Click);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(255, 417);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(75, 23);
            this.Clear.TabIndex = 2;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(12, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(522, 381);
            this.listBox.TabIndex = 3;
            // 
            // FormRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 469);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.Replay);
            this.Controls.Add(this.Recoder_Stop);
            this.Name = "FormRecorder";
            this.Text = "FormRecorder";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Recoder_Stop;
        private System.Windows.Forms.Button Replay;
        private System.Windows.Forms.Button Clear;
        public System.Windows.Forms.ListBox listBox;
    }
}