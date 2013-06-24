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
            return null;
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




    /*public class BasicConfiguration<T> : IConfiguration<T> where T : Control//classe <configuration>
    {

        public List<T> _formControls;

        public List<T> filteredControls;

        public Dictionary<Type,List<object>> composedConfiguration;


        public BasicConfiguration(Form f)
        {
            _formControls = new List<T>();
            foreach (Control c in f.Controls)
                _formControls.Add((T)c);
        }

        //public BasicConfiguration(IEnumerator enumerator)
        //{
        //    this._formControls = enumerator;
        //}

        ConfigurationItem<T> IConfiguration<T>.For<T>()
        {

            List<T> listControlsT = new List<T>(0);
            Type type = typeof(T);

            foreach (T c in _formControls)
            {
                if (c.GetType() == type)
                    listControlsT.Add((T)c);

                //listControlsT.AddRange(getControlsFromControl<T>(c, listControlsT));
            }



            return null;//new ConfigurationItem<T>(this);
        }

        protected List<T> getControlsFromControl<T>(T control, List<T> list) where T : Control
        {

            if (control == null) return new List<T>();

            Type type = control.GetType();

            foreach (T t in control.Controls)
            {
                if (t.GetType().IsSubclassOf(type))
                    list.Add(t);

                list.AddRange(getControlsFromControl(t, list));
            }

            return list;
        }

        public void CustomConfiguration() //colocar a configuraçao desejada
        {
            For<Button>();
                    //.WithName(".*").When(".*");
        }

    }


    /// =============================================================================================
    /// Ate aqui já esta a seguir a minha ideia(Pedro)  ||  O when terá de retornar um Iconfiguration
    /// =============================================================================================

    public class ConfigurationItem<T> where T: Control//<configurationItem>:
    {
        private IConfiguration<T> configuration;
        public Boolean f;

        public ConfigurationItem(IConfiguration<T> config)
        {
            configuration = config;
        }

        //public ControlRestriction<T> WithName(params String[] controlset)
        //{

        //    configuration.


        //    ArrayList listControlsWithName = new ArrayList();

        //    while (listControlsT.MoveNext())
        //    {
        //        T t = (T)listControlsT.Current;

        //        for (int i = 0; i < controlset.Length; i++)
        //        {
        //            if (!Regex.IsMatch(t.Name, controlset[i]))
        //            {
        //                f = false;
        //                break;
        //            }
        //        }

        //        if (f)
        //            listControlsWithName.Add(t); //se um dos listControlsT nao estiver contido em controlset[i] nao adiciono a lista final de controls
        //        f = true;
        //    }

        //    return new ControlRestriction<T>(listControlsWithName);
        //}
    }

    public class ControlRestriction<T> where T : Control//classe <configuration>         
    {
        //private IEnumerator listControlsWithName;

        //public ControlRestriction(ArrayList list)
        //{
        //    listControlsWithName = list.GetEnumerator();
        //}

        //public BasicConfiguration<T> When(params  String[] eventSet)
        //{
        //    ArrayList listControlsT = new ArrayList();
        //    bool flag = true;

        //    while (listControlsWithName.MoveNext()) //por cada control
        //    {
        //        T t = (T)listControlsWithName.Current;

        //        Type type = t.GetType();//saco o seu tipo

        //        IEnumerable<EventInfo> list = type.GetRuntimeEvents();//saco os seus eventos

        //        foreach (EventInfo item in list)    //corro os eventos desse control
        //        {
        //            for (int i = 0; i < eventSet.Length; i++)//corro o array de eventSet recebido por parametro
        //            {
        //                if (!Regex.IsMatch(item.Name, eventSet[i]))
        //                {
        //                    flag = false;
        //                    break;
        //                }
        //            }
        //            if (!flag) break; //se um dos eventos nao estiver contido em eventSet[i] nao adiciono a lista final de controls
        //        }
        //        if (flag)

        //            listControlsT.Add(t);

        //        flag = true;
        //    }

        //    return new BasicConfiguration(listControlsT.GetEnumerator());
        //}

        //public ControlRestriction<T> And(Func<T, bool> predicate)
        //{
        //    ArrayList listControlsPredicate = new ArrayList();
        //    //ir a todos os contols e filtrar por predicate

        //    while (listControlsWithName.MoveNext())
        //    {
        //        T t = (T)listControlsWithName.Current;
        //        Type type = t.GetType();

        //        if (predicate(t))       //corrigir
        //            listControlsPredicate.Add(listControlsWithName.Current);
        //    }

        //    this.listControlsWithName = listControlsPredicate.GetEnumerator();

        //    return this;
        //}

        //public ControlRestriction<T> WithText(String text)
        //{
        //    ArrayList listControlsPredicate = new ArrayList();
        //    while (listControlsWithName.MoveNext())
        //    {
        //        T t = (T)listControlsWithName.Current;

        //        if (t.Text.Equals(text))
        //            listControlsPredicate.Add(t);
        //    }

        //    this.listControlsWithName = listControlsPredicate.GetEnumerator();
        //    return this;
        //}
    }*/
}
