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
        public List<Func<Control,bool>> _listfunc;

        public ControlConfigPackage() {
            _listEvent = new List<EventInfo>();
            _listfunc = new List<Func<Control,bool>>();
        }

        public ControlConfigPackage(List<EventInfo> list,List<Func<Control,bool>> func)
        {
            _listEvent = list;
            _listfunc  = func;
        }
    }

    public class ConfigurationX<T> where T:Control
    {
        public Dictionary<Control, ControlConfigPackage> controlEventsAndPredicates;
        public List<T> controls;

        public ConfigurationX(Dictionary<Control, ControlConfigPackage> dictionary, List<T> list)
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
        IConfigurationRestriction<T> And(Func<Control,bool> predicate);//decidir o Func
        IConfigurationRestriction<T> WithText(String name);
        IConfiguration<T> When(params  String[] eventSet);
    }

    public class Configuration<T> : IConfiguration<T>, IConfigurationItem<T>, IConfigurationRestriction<T> where T : Control
    {
        private List<Control> _formControls;

        public Dictionary<Control, ControlConfigPackage> controlEvents;
        
        private List<Control> filteredControls;
        private List<EventInfo> eventsList;
        private List<Func<Control,bool>> predicateList;

        public Dictionary<Type,List<ConfigurationX<Control>>> composedConfiguration;

        public Configuration(Form f)
        {
            filteredControls = new List<Control>();
            eventsList = new List<EventInfo>();
            predicateList = new List<Func<Control, bool>>();
            composedConfiguration = new Dictionary<Type, List<ConfigurationX<Control>>>();
            controlEvents = new Dictionary<Control, ControlConfigPackage>();

            _formControls = new List<Control>();
            foreach (Control c in f.Controls)
            {
                _formControls.Add(c);
                _formControls.AddRange(getControlsFromControl<Control>(c, new List<Control>()));
            }
        }

        public Configuration(List<Control> controls, List<Control> filtered)
        {
            _formControls = controls;
            filteredControls = filtered;

            eventsList = new List<EventInfo>();
            predicateList = new List<Func<Control, bool>>();
            composedConfiguration = new Dictionary<Type, List<ConfigurationX<Control>>>();
            controlEvents = new Dictionary<Control, ControlConfigPackage>();
        }

        public Dictionary<Type, List<ConfigurationX<Control>>> getComposedConfiguration() {
            return composedConfiguration;
        }

        private List<Control> getControlsFromControl<T>(T control, List<Control> list)where T : Control
        {

            if (control == null) return new List<Control>();

            Type type = control.GetType();

            foreach (Control t in control.Controls)
            {
                if (t.GetType() == type)
                    list.Add(t);

                list.AddRange(getControlsFromControl(t, list));
            }

            return list;
        }

        IConfigurationItem<T> For<T>() where T : Control 
        {
            return((IConfiguration<T>)new Configuration<T>(_formControls,filteredControls)).For<T>();
        }

        IConfigurationItem<T> IConfiguration<T>.For<T>()
        {
            Type type = typeof(T);
               
            foreach (Control c in _formControls)
            {
                if (c.GetType() == type)
                    filteredControls.Add(c);
            }
            return (IConfigurationItem<T>)new Configuration<T>(_formControls, filteredControls);
        }

        public IConfigurationRestriction<T> WithName(params string[] controlset)
        {
            List<Control> listr = new List<Control>();
            foreach (Control c in filteredControls)
            {
                foreach (String str in controlset)
                {
                    if (Regex.IsMatch(c.Name, str))
                        listr.Add(c);
                }
            }
            filteredControls = listr;

            //
            //Falta provavelmente adicionar alguma coisa ao dicionario do
            //

            foreach (Control control in filteredControls)
            {
                try
                {
                    controlEvents.Add(control, new ControlConfigPackage());
                }
                catch (ArgumentException) { }
            }
            return (IConfigurationRestriction<T>)this;
        }

        IConfigurationRestriction<T> IConfigurationRestriction<T>.And(Func<Control,bool> predicate)
        {
            predicateList.Add(predicate);
            return (IConfigurationRestriction<T>)this;
        }

        IConfigurationRestriction<T> IConfigurationRestriction<T>.WithText(String name)
        {
            List<Control> temp = new List<Control>();
            foreach (var c in filteredControls)
            {
                if(c.Text.Contains(name))
                    temp.Add(c);
            }
            filteredControls = temp;

            return (IConfigurationRestriction<T>)this;

        }


        public IConfiguration<T> When(params string[] eventSet)
        {
            Type controlType = typeof(T);

            foreach (String eventName in eventSet)
            {
                EventInfo [] eventos = controlType.GetEvents();

                foreach (EventInfo e in eventos) {
                    if(Regex.IsMatch(e.Name,eventName))
                        eventsList.Add(e);
                }  
            }

            foreach (var control in filteredControls)
            {
                try
                {
                    controlEvents.Add(control, new ControlConfigPackage(eventsList, predicateList));
                    //marcar no dicionario » controlEvents, pelo nome do controlo, o eventInfo
                }
                catch (ArgumentException)
                {
                    controlEvents[control]._listEvent.AddRange(eventsList);
                    controlEvents[control]._listfunc.AddRange(predicateList);
                    //caso já já esteja algum evento marcado para o controlo, acrescentar a lista o novo evento ...
                }


            }

            

            List<ConfigurationX<Control>> listToAdd = new List<ConfigurationX<Control>>();
            listToAdd.Add(new ConfigurationX<Control>(controlEvents, filteredControls));///////////////////////////////////////////////////////////////////////////////
            try
            {
                composedConfiguration[controlType].AddRange(listToAdd);
            }
            catch (KeyNotFoundException)
            {
                composedConfiguration.Add(controlType, listToAdd);
            }

            filteredControls.Clear();
            eventsList.Clear();
            predicateList.Clear();

            return (IConfiguration<T>)this;
        }
        
        public void CostumConfiguration()
        {
            //this2 tem de ser do tipo Configuration<X> em que X é o tipo do ultimo For da Configuracao
            Configuration<Form> this2 = (Configuration<Form>)
                For<Form>().WithName(".*").When(".*");

            this.composedConfiguration = this2.composedConfiguration;
            this.controlEvents = this2.controlEvents;
        }

        
    }
}
