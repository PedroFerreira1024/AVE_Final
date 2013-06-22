using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Configuration
{

    public abstract class IConfiguration
    {
        
        public abstract ConfigurationItem<T> For<T>() where T : Control;
        public abstract IConfiguration addPredicate();

        protected List<T> getControlsFromControl<T>(T control,List<T> list) where T : Control
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
    }

    public class BasicConfiguration<T> : IConfiguration where T : Control//classe <configuration>
    {
        private ICollection _fromControls;
        

        public BasicConfiguration(Form f)
        {
            _fromControls = f.Controls;
        }

        public BasicConfiguration(IEnumerator enumerator)
        {
            this._fromControls = enumerator;
        }
        
        public override ConfigurationItem<T> For<T>()
        {

            List<T> listControlsT = new List<T>(0);
            Type type = typeof(T);

            foreach (T c in _fromControls)
            {
                if (c.GetType().IsSubclassOf(type))
                    listControlsT.Add(c);

                listControlsT.AddRange(getControlsFromControl<T>(c, listControlsT));
            }

            return new ConfigurationItem<T>(listControlsT);
        }


        public void CustomConfiguration() //colocar a configuraçao desejada
        {
            For<Button>()
                    .WithName(".*").When(".*");
        }
    }


    public class ComplexConfiguration : IConfiguration
    {

        private Dictionary<Type, List<IConfiguration>> filteredControls;


        public ComplexConfiguration(IConfiguration config)
        {

        }

        public ConfigurationItem<T> For<T>()
        {

            return null;
        }
    }



    /// =============================================================================================
    /// Ate aqui já esta a seguir a minha ideia(Pedro)  ||  O when terá de retornar um Iconfiguration
    /// =============================================================================================

    public class ConfigurationItem<T> where T : Control  //<configurationItem>:
    {
        private List<T> listControlsT;
        public Boolean f;

        public ConfigurationItem(List<T> list)
        {
            listControlsT = list;
        }

        public ControlRestriction<T> WithName(params String[] controlset)
        {

            ArrayList listControlsWithName = new ArrayList();

            while (listControlsT.MoveNext())
            {
                T t = (T)listControlsT.Current;

                for (int i = 0; i < controlset.Length; i++)
                {
                    if (!Regex.IsMatch(t.Name, controlset[i]))
                    {
                        f = false;
                        break;
                    }
                }

                if (f)
                    listControlsWithName.Add(t); //se um dos listControlsT nao estiver contido em controlset[i] nao adiciono a lista final de controls
                f = true;
            }

            return new ControlRestriction<T>(listControlsWithName);
        }
    }

    public class ControlRestriction<T> where T : Control//classe <configuration>         
    {
        private IEnumerator listControlsWithName;

        public ControlRestriction(ArrayList list)
        {
            listControlsWithName = list.GetEnumerator();
        }

        public BasicConfiguration<T> When(params  String[] eventSet)
        {
            ArrayList listControlsT = new ArrayList();
            bool flag = true;

            while (listControlsWithName.MoveNext()) //por cada control
            {
                T t = (T)listControlsWithName.Current;

                Type type = t.GetType();//saco o seu tipo

                IEnumerable<EventInfo> list = type.GetRuntimeEvents();//saco os seus eventos

                foreach (EventInfo item in list)    //corro os eventos desse control
                {
                    for (int i = 0; i < eventSet.Length; i++)//corro o array de eventSet recebido por parametro
                    {
                        if (!Regex.IsMatch(item.Name, eventSet[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag) break; //se um dos eventos nao estiver contido em eventSet[i] nao adiciono a lista final de controls
                }
                if (flag)

                    listControlsT.Add(t);

                flag = true;
            }

            return new BasicConfiguration(listControlsT.GetEnumerator());
        }

        public ControlRestriction<T> And(Func<T, bool> predicate)
        {
            ArrayList listControlsPredicate = new ArrayList();
            //ir a todos os contols e filtrar por predicate

            while (listControlsWithName.MoveNext())
            {
                T t = (T)listControlsWithName.Current;
                Type type = t.GetType();

                if (predicate(t))       //corrigir
                    listControlsPredicate.Add(listControlsWithName.Current);
            }

            this.listControlsWithName = listControlsPredicate.GetEnumerator();

            return this;
        }

        public ControlRestriction<T> WithText(String text)
        {
            ArrayList listControlsPredicate = new ArrayList();
            while (listControlsWithName.MoveNext())
            {
                T t = (T)listControlsWithName.Current;

                if (t.Text.Equals(text))
                    listControlsPredicate.Add(t);
            }

            this.listControlsWithName = listControlsPredicate.GetEnumerator();
            return this;
        }
    }
}
