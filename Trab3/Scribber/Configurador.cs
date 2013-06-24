using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scribber;
using System.Reflection;
using System.Text.RegularExpressions;

using Configuration;

namespace netscribber
{

    class recorderreplayer
    {





    }
    class Program
    {
        static void Main()
        {
            var formPaint = new ScribberForm();
            var formRecord = new netscribber.FormRecorder();

            Configuration<Button> ic = new Configuration<Button>(formPaint);

            formRecord.Show();

            Application.Run(formPaint);
        }
    }
}



