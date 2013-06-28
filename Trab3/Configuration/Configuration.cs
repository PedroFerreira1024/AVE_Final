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
        public List<Func<object>> _listfunc;

        public ControlConfigPackage() { }

        public ControlConfigPackage(List<EventInfo> list,List<Func<object>> func)
        {
            _listEvent = list;
            _listfunc  = func;
        }
    }

    public class ConfigurationX<T> where T:Control
    {
        public Dictionary<String, ControlConfigPackage> controlEventsAndPredicates;
        public List<T> controls;

        public ConfigurationX(Dictionary<String, ControlConfigPackage> dictionary, List<T> list)
        {
            controlEventsAndPredicates = dictionary;
            controls = list;
        }
    }


    public interface IConfiguration<T>where T : Control
    {
        IConfigurationItem<K> For<K>() where K : Control;
        IConfiguration<T> When(params  String[] eventSet);
        IConfigurationRestriction<T> WithName(params String[] controlset);
    }

    public interface IConfigurationItem<T> where T : Control
    {
        IConfigurationRestriction<T> WithName(params String[] controlset);
    }

    public interface IConfigurationRestriction<T> where T : Control
    {
        IConfigurationRestriction<T> And(Func<object> predicate);//decidir o Func
        IConfigurationRestriction<T> WithText(String name);
        IConfiguration<T> When(params  String[] eventSet);
    }

    public class Configuration<T> : IConfiguration<T>, IConfigurationItem<T>, IConfigurationRestriction<T> where T : Control
    {
        private List<Control> _formControls;

        public Dictionary<String, ControlConfigPackage> controlEvents;
        
        public List<Control> filteredControls;
        public List<EventInfo> eventsList;
        public List<Func<object>> predicateList;

        public Dictionary<Type,List<ConfigurationX<Control>>> composedConfiguration;

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
            Type type = typeof(T);

            foreach (T c in _formControls)
            {
                if (c.GetType() == type)
                    filteredControls.Add(c);
            }
            return (IConfigurationItem<T>)this;
        }

        public IConfigurationRestriction<T> WithName(params string[] controlset)
        {
            bool isName = false;
            foreach (Control c in filteredControls)
            {
                foreach (String str in controlset)
                {
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

            foreach (Control control in filteredControls)
            {
                try
                {
                    controlEvents.Add(control.Name, new ControlConfigPackage());
                }
                catch (ArgumentException) { }
            }
            return (IConfigurationRestriction<T>)this;
        }

        IConfigurationRestriction<T> IConfigurationRestriction<T>.And(Func<object> predicate)
        {
            predicateList.Add(predicate);
            return (IConfigurationRestriction<T>)this;
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


        public IConfiguration<T> When(params string[] eventSet)
        {
            Type controlType = typeof(T);
            EventInfo ei;

            foreach (String eventName in eventSet)
            {
                ei = controlType.GetEvent(eventName);
                if (ei != null)
                    eventsList.Add(ei);
            }



            foreach (var control in _formControls)
            {

                try
                {
                    controlEvents.Add(control.Name, new ControlConfigPackage(eventsList, predicateList));
                    //marcar no dicionario » controlEvents, pelo nome do controlo, o eventInfo
                }
                catch (ArgumentException)
                {
                    controlEvents[control.Name]._listEvent.AddRange(eventsList);
                    controlEvents[control.Name]._listfunc.AddRange(predicateList);

                    //caso já já esteja algum evento marcado para o controlo, acrescentar a lista o novo evento ...
                }

            }

            List<ConfigurationX<Control>> listToAdd = new List<ConfigurationX<Control>>();
            listToAdd.Add(new ConfigurationX<Control>(controlEvents, filteredControls));///////////////////////////////////////////////////////////////////////////////
            try
            {
                composedConfiguration[controlType].Add(new ConfigurationX<Control>(controlEvents, filteredControls));
            }
            catch (KeyNotFoundException)
            {
                composedConfiguration.Add(controlType, listToAdd);
            }

            //limpar campos auxiliares

            return (IConfiguration<T>)this;
        }

        public void CostumConfiguration()
        {
            ((IConfiguration<Button>)this).For<Button>();
        }

        
    }
}
