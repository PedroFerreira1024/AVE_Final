using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Scribber  {
	using System;
	using System.Collections;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Drawing.Drawing2D;

	public class ScribberForm : Form  {
        Stroke CurrentStroke = null;
        private Button red;
        private Button green;
        private Button blue;
        private Button yellow;
		ArrayList Strokes = new ArrayList ();
        private Button black;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
	    private Color currentColor;
	    private int currentSize;

		public ScribberForm ()
		{
		    currentColor = Color.Black;
		    currentSize = 5;
		    InitializeComponent();
            blue.Click +=  colors_Click;
            green.Click += colors_Click;
            yellow.Click += colors_Click;
            black.Click += colors_Click;
		    button1.Click += size_Click;
            button2.Click += size_Click;
            button3.Click += size_Click;
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			// Draw all currently recorded strokes
		    
			foreach (Stroke stroke in Strokes)
				stroke.Draw (e.Graphics);
		}

		protected override void OnMouseDown (MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) 
			{
				// Create a new Stroke and assign it to CurrentStroke
				CurrentStroke = new Stroke (e.X, e.Y, currentColor, currentSize);
			}
		    base.OnMouseDown(e);
		}

		protected override void OnMouseMove (MouseEventArgs e)
		{
           
			if ((e.Button & MouseButtons.Left) != 0 && CurrentStroke != null) 
			{
				// Add a new segment to the current stroke
				CurrentStroke.Add (e.X, e.Y);
				Graphics g = Graphics.FromHwnd (Handle);
				CurrentStroke.DrawLastSegment (g);
				g.Dispose ();
			}
            base.OnMouseMove(e);
		}

		protected override void OnMouseUp (MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && CurrentStroke != null) 
			{
				// Complete the current stroke
				if (CurrentStroke.Count > 1)
					Strokes.Add (CurrentStroke);
				CurrentStroke = null;
			}
            base.OnMouseUp(e);
		}

		protected override void OnKeyDown (KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) 
			{
				// Delete all strokes and repaint
				Strokes.Clear ();
				Invalidate ();
			}
		    base.OnKeyDown(e);
		}
        /*
		static void Main ()
		{
			Application.Run (new ScribberForm ());
		}
        */
		private void InitializeComponent()
		{
            this.red = new System.Windows.Forms.Button();
            this.green = new System.Windows.Forms.Button();
            this.blue = new System.Windows.Forms.Button();
            this.yellow = new System.Windows.Forms.Button();
            this.black = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // red
            // 
            this.red.BackColor = System.Drawing.Color.Red;
            this.red.Location = new System.Drawing.Point(3, 581);
            this.red.Name = "red";
            this.red.Size = new System.Drawing.Size(75, 50);
            this.red.TabIndex = 1;
            this.red.UseVisualStyleBackColor = false;
            this.red.Click += new System.EventHandler(this.colors_Click);
            // 
            // green
            // 
            this.green.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.green.Location = new System.Drawing.Point(84, 580);
            this.green.Name = "green";
            this.green.Size = new System.Drawing.Size(75, 50);
            this.green.TabIndex = 2;
            this.green.UseVisualStyleBackColor = false;
            // 
            // blue
            // 
            this.blue.BackColor = System.Drawing.Color.Blue;
            this.blue.Location = new System.Drawing.Point(165, 581);
            this.blue.Name = "blue";
            this.blue.Size = new System.Drawing.Size(75, 50);
            this.blue.TabIndex = 3;
            this.blue.UseVisualStyleBackColor = false;
            // 
            // yellow
            // 
            this.yellow.BackColor = System.Drawing.Color.Yellow;
            this.yellow.Location = new System.Drawing.Point(246, 581);
            this.yellow.Name = "yellow";
            this.yellow.Size = new System.Drawing.Size(75, 50);
            this.yellow.TabIndex = 4;
            this.yellow.UseVisualStyleBackColor = false;
            // 
            // black
            // 
            this.black.BackColor = System.Drawing.Color.Black;
            this.black.Location = new System.Drawing.Point(327, 580);
            this.black.Name = "black";
            this.black.Size = new System.Drawing.Size(75, 50);
            this.black.TabIndex = 5;
            this.black.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(671, 600);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 10);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(728, 599);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(51, 12);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(802, 598);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 15);
            this.button3.TabIndex = 8;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(894, 593);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(60, 20);
            this.button4.TabIndex = 9;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.size_Click);
            // 
            // ScribberForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(991, 634);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.black);
            this.Controls.Add(this.yellow);
            this.Controls.Add(this.blue);
            this.Controls.Add(this.green);
            this.Controls.Add(this.red);
            this.KeyPreview = true;
            this.Name = "ScribberForm";
            this.Text = "ScribberForm";
            this.Load += new System.EventHandler(this.ScribberForm_Load);
            this.ResumeLayout(false);

		}

		private void ScribberForm_Load(object sender, System.EventArgs e)
		{
		
		}

        private void colors_Click(object sender, EventArgs e)
        {
            currentColor = ((Button) sender).BackColor;
        }

        private void size_Click(object sender, EventArgs e)
        {
            currentSize = ((Button) sender).Height;
        }
	}

	class Stroke
	{
		ArrayList Points = new ArrayList ();
	    private Color color;
	    private int size;

		public int Count
		{
			get { return Points.Count; }
		}

		public Stroke (int x, int y, Color color, int size)
		{
			Points.Add (new Point (x, y));
		    this.color = color;
		    this.size = size;
		}

		public void Add (int x, int y)
		{
			Points.Add (new Point (x, y));
		}

		public void Draw (Graphics g)
		{
			Pen pen = new Pen (color, size);
			pen.EndCap = LineCap.Round;
			for (int i=0; i<Points.Count - 1; i++)
				g.DrawLine (pen, (Point) Points[i], (Point) Points[i + 1]);
			pen.Dispose ();
		}

		public void DrawLastSegment (Graphics g)
		{
			Point p1 = (Point) Points[Points.Count - 2];
			Point p2 = (Point) Points[Points.Count - 1];
			Pen pen = new Pen (color, size);
			pen.EndCap = LineCap.Round;
			g.DrawLine (pen, p1, p2);
			pen.Dispose ();
		}

	}
}
