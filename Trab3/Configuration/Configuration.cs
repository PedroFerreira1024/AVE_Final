using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace Configuration
{

    public interface IConfiguration<T>
    {
        IConfigurationItem<K> For<K>() where K : Control;
        //IConfiguration<T> When();
    }
    public interface IConfigurationItem<T>
    {
        IConfigurationRestriction<T> WhithName<T>(params String[] controlset) where T : Control;
    }
    public interface IConfigurationRestriction<T>
    {
        IConfiguration<T> When<T>(params  String[] eventSet) where T : Control;
    }

    public class Configuration<T> : IConfiguration<T>, IConfigurationItem<T>, IConfigurationRestriction<T> where T : Control
    {
        public List<Control> _formControls;

        public List<T> filteredControls;

        public Dictionary<Type,List<object>> composedConfiguration;


        public Configuration(Form f)
        {
            _formControls = new List<Control>();
            foreach (Control c in f.Controls)
                _formControls.AddRange(getControlsFromControl<Control>(c,new List<Control>()));
        }

        private List<T> getControlsFromControl<T>(T control, List<T> list)where T : Control
        {

            if (control == null) return new List<T>();

            Type type = control.GetType();

            foreach (T t in control.Controls)
            {
                if (t.GetType() == type)
                    list.Add(t);

                list.AddRange(getControlsFromControl(t, list));
            }

            return list;
        }

        IConfiguration<T> For<T>() where T : Control 
        {
            return (IConfiguration<T>)((IConfiguration<T>)this).For<T>();
        }

        IConfigurationItem<T> IConfiguration<T>.For<T>()
        {
            List<T> listControlsT = new List<T>(0);
            Type type = typeof(T);

            foreach (Control c in _formControls)
            {
                if (c.GetType() == type)
                    listControlsT.Add((T)c);

                listControlsT.AddRange(getControlsFromControl<T>((T)c, listControlsT));
           }

            return (IConfigurationItem<T>)this;
        }

        IConfigurationRestriction<T> IConfigurationItem<T>.WhithName<T>(params String[] controlset)
        {
	        bool isName = false;
	        foreach(var c in filteredControls){
		        foreach(var str in controlset){
			        if(c.Name.Equals(str))
				        isName = true;
		        }
                if (!isName)
                    filteredControls.Remove(c);
                isName = false;
	        }
            return (IConfigurationRestriction<T>)this;
        }

        IConfiguration<T> IConfigurationRestriction<T>.When<T>(params  String[] eventSet) 
        {
            return null;
        }

        public void CostumConfiguration()
        {
            ((IConfiguration<Button>)this).For<Button>();
        }

    }
}
