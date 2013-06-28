using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;


namespace Configuration
{

    public class ControlConfigPackage
    {
        public List<EventInfo> _listEvent;
        public List<Func> _listfunc;

        ControlConfigPackage(List<EventInfo> list,List<Func> func)
        {
            _listEvent = list;
            _listfunc  = func;
        }
    }


    public class ConfigurationX
    {
        //Aglumerado de configurações de um derterminado tipo T
    }

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
        IConfigurationRestriction<T> And(Func<object> predicate);//decidir o Func
        IConfigurationRestriction<T> WithText(String name);
        IConfiguration<T> When<T>(params  String[] eventSet) where T : Control;
    }

    public class Configuration<T> : IConfiguration<T>, IConfigurationItem<T>, IConfigurationRestriction<T> where T : Control
    {
        private List<Control> _formControls;

       // public Dictionary<String,List<EventInfo>> controlEvents;//« alterar para Um tipo com duas listas ao invez desta lista de Eventinfo
          public Dictionary<String, ControlConfigPackage> controlEvents;
        
        public List<T> filteredControls;
        public List<EventInfo> eventsList;
        public List<Func<object>> predicateList;

        public Dictionary<Type,List<ConfigurationX>> composedConfiguration;

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
            }

            return (IConfigurationItem<T>)this;
        }

        IConfigurationRestriction<T> IConfigurationItem<T>.WhithName<T>(params String[] controlset)
        {
	        bool isName = false;
	        foreach(var c in filteredControls){
		        foreach(var str in controlset){
                    if (Regex.IsMatch(c.Name, str))
				        isName = true;
		        }
                if (!isName)
                    filteredControls.Remove(c);
                isName = false;
	        }
            
            //
            //Falta provavelmente adicionar alguma coisa ao dicionario do
            //
            try
            {

            }
            catch(ArgumentException)
            {

            }

            return (IConfigurationRestriction<T>)this;
        }

        IConfigurationRestriction<T> IConfigurationRestriction<T>.And(Func<object> predicate)
        {
            return null;
        }

        IConfigurationRestriction<T> IConfigurationRestriction<T>.WithText(String name)
        {
            bool isName = false;
            foreach (var c in filteredControls)
            {
                if(c.Text.Contains(name))
                    filteredControls.Remove(c);
            }
            return (IConfigurationRestriction<T>)this;
        }

        IConfiguration<T> IConfigurationRestriction<T>.When<T>(params  String[] eventSet) 
        {
            Type controlType = typeof(T);
            EventInfo ei;

            foreach (String eventName in eventSet)
            {
                ei = controlType.GetEvent(eventName);
                if (ei != null)
                    eventsList.Add(ei);
            }



            foreach(var control in _formControls)
            
            try
            {
                controlEvents.Add(control.Name,eventsList);
                //marcar no dicionario » controlEvents, pelo nome do controlo, o eventInfo
            }
            catch (ArgumentException)
            {
                controlEvents[control.Name].AddRange(eventsList);
                //caso já já esteja algum evento marcado para o controlo, acrescentar a lista o novo evento ...
            }


            List<ConfigurationX> listToAdd = new List<ConfigurationX>();
            try
            {
                listToAdd.Add(this); // << Erro por nao conseguir converter uma coisa para ela própria
                //composedConfiguration.Add(controlType,listToAdd);
            }
            catch (ArgumentException)
            {
                ((Configuration<Control>)(composedConfiguration[controlType])).eventsList.Add(eventsList);
            }

            return (IConfiguration<T>)this;
        }

        public void CostumConfiguration()
        {
            ((IConfiguration<Button>)this).For<Button>();
        }

    }
}
