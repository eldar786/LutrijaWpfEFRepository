using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.ViewModel
{
    //Ovu mediator klasu pravimo, zato sto zelimo da ostvarimo komunikaciju izmedju dva prozora. Kako bih obavijestio glavni prozor ako dodje
    //do izmjene ili unosa nove stavke u sistem

    //Ova klasa je SINGLETON klasa, to je jos jedan softwareski sablon. Po njemu se moze napraviti samo jedna instanca nekog tipa.
    //Zbog toga je konstruktor ove klase privatni, na taj nacin klasicno instanciranje nece biti moguce i da bi se dobila instanca ove klase,
    // koristi se staticko polje instance i javni property sa istim nazivom(Instance)
    public class Mediator
    {
        static readonly Mediator instance = new Mediator();

        //Ima samo get metodu, zato sto je readonly. Na ovaj nacin svakome kome bude potrebna instanca ove klase, dobit ce istu identicnu instancu
        public static Mediator Instance
        {
            get
            {
                return instance;
            }
        }

        private Mediator()
        {

        }
        //Kolekcija parova kljuceva vrijednosti. Kljucevi su stringovi, a vrijednosti su delegati Action tipa.
        //Ova kolekcija nam treba da cuvamo razlicite poruke, odnosno akcije i svakoj od njih ce biti moguce pridruziti akciju, odnosno radnju.
        //Greska
        //System.ArgumentException: 'An item with the same key has already been added.'
        private static Dictionary<string, Action<object>> subsribers = new Dictionary<string, Action<object>>();

        //Koristi se za registrovanje akcije za odredjenu poruku. Npr neko je zainteresovan da se u odredjenom trenutku obavi neka logika koju je on definisao
        //zainteresovana strana prilaze string podatak i delegat tipa Action, koji sadrzi tu logiku 

        //System.ArgumentException: 'An item with the same key has already been added.'
        public void Register(string message, Action<object> action)
        {
            if (!subsribers.ContainsKey(message))
            {
                subsribers.Add(message, action);
            }
            return;
        }

        //Ova metoda se koristi od strane onoga koji zeli da obavijesti sve zainteresovane strane, odnosno pretplatnike da se nesto dogodilo
        //pri tome se prolazi kroz kolekciju pretplatnika u ovoj petlji foreach i kada se utvrdi jednakost ovih string vrijednosti, vrsi se 
        //pozivanje metode na koju ukazuje prosljedjeni delegat, odnosno metode koja je prosljedjena ovom gore metode Register.
        public void Notify(string message, Object param)
        {
            foreach (var item in subsribers)
            {
                if (item.Key.Equals(message))
                {
                    Action<object> method = (Action<object>)item.Value;
                    method.Invoke(param);
                }
            }
        }
    }
}
