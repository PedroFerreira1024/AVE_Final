using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scribber;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace netscribber
{
    
    class RecorderReplayer
    {

        public static void Create(Configuration myConfig, Form formRecord)
        {
            //FAZER       
        }


        public static Thread Execute(Form form)
        {

            //REFAZER IDEIA

            Thread t = new Thread(() =>
            {

                Type typeForm = form.GetType();
                IEnumerator eventForm = typeForm.GetRuntimeEvents().GetEnumerator();

                //System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt");

                while (eventForm.MoveNext())
                {
                    EventInfo s2 = (EventInfo)eventForm.Current;
             ///       file.Write(s2.Name);
                }

             //   file.Close();
            });

            //   t.Start();
            return t;
        }


    }
    class Program
    {
        static void Main()
        {
            var formPaint = new ScribberForm();
            var formRecord = new FormRecorder();

            var csConfig = new Configuration(formPaint);
            csConfig.CustomConfiguration();

            // var recorderService = RecorderReplayer.Create(csConfig,formRecord );

           // RecorderReplayer.Execute(formPaint).Start();

        //    Application.Run(formPaint);
        }
    }
}




