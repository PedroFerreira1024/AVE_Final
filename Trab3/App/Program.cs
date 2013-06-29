using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using netscribber;
using System.Windows.Forms;

namespace App
{
    class Program
    {
      
        static void Main(string[] args)
        {
            var form = new FormRecorder();

            Application.Run(form);
        }
    }
}
