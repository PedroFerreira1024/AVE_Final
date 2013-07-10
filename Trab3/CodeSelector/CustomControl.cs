using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Control;

namespace CodeSelector
{
    class CustomControl
    {
        public event System.Windows.Forms.Control Control;
        protected override void CustomConfiguration(){ 
            For<Control>().WithName(".*").When(".*"); 
        }

        public Control For(this)
        {
        }

    }
}
