using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
 

public partial class CodeSelectorForm : Form
{
    public CodeSelectorForm()
    {
        InitializeComponent();
    }

    private void Calc_Load(object sender, EventArgs e)
    {
           
    }

    private void button_Click(object sender, EventArgs e)
    {
        Rectangle r = textBox1.RectangleToScreen(textBox1.ClientRectangle);
        Point p = new Point(r.X+20,r.Y+5);
        int maxIndex = textBox1.GetCharIndexFromPosition(p);
        if (textBox1.Text.Length == 26)
            textBox1.Text = textBox1.Text.Substring(1);
        textBox1.Text += ((Button)sender).Text;
    }

     
    private void button6_Click(object sender, EventArgs e)
    {
        textBox1.Text = "";
    }
    
}