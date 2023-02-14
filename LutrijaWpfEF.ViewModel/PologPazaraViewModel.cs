using Caliburn.Micro;
using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
    public class PologPazaraViewModel : ObservableObject, INotifyPropertyChanged
    {
        private POLOG_PAZAR currentPologPazar;
        private PologPazarCollection pologPazarList;
        //Da bi je koristili, neophodno je da referenciramo biblioteku Presentation Framework
        //posrednik izmedju stvarne kolekcije i kontrole 
        private ListCollectionView pologPazarListView;

        //PologCollection ???

        private string filteringText;

        //Nema potrebe da kreiramo property, jer ce se ovo polje koristiti interno unutar ove klase
        private Mediator mediator;

        private ICommand deleteCommand;

        //SacuvajPologCommand
        //    IzmjeniPologCommand



        public DateTime CurrentDate = DateTime.Now;

        #region Property

        public POLOG_PAZAR CurrentPologPazar
        {
            get { return currentPologPazar; }
            set
            {
                if (currentPologPazar == value)
                {
                    return;
                }
                currentPologPazar = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPologPazar"));
            }
        }

        public PologPazarCollection PologPazarList
        {
            get { return pologPazarList; }
            set
            {
                if (pologPazarList == value)
                {
                    return;
                }
                pologPazarList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PologPazarList"));
            }
        }

        public ListCollectionView PologPazarListView
        {
            get { return pologPazarListView; }
            set
            {
                if (pologPazarListView == value)
                {
                    return;
                }
                pologPazarListView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PologPazarListView"));
            }
        }

        public String FilteringText
        {
            get { return filteringText; }
            set
            {
                if (filteringText == value)
                {
                    return;
                }
                filteringText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilteringText"));
            }
        }

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set
            {
                if (deleteCommand == value)
                {
                    return;
                }
                deleteCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommand"));
            }
        }

        void DeleteExecute(object obj)
        {

           // CurrentPologPazar.DeletePologPazar();
            //Kada se izvrsi brisanje instance, treba da se promjena vidi u samoj listi
            //Da ne bi ponovo citali listu, samo uklonimo iz liste
            PologPazarListView.Remove(CurrentPologPazar);
        }

        //Ukoliko je nesto selektovano, komanda se moze izvrsiti
        //Ukoliko je vrijednost null, nece biti u mogucnosti da izvrsi komandu tj. bit ce disable button
        bool CanDelete(object obj)
        {
            if (CurrentPologPazar == null) return false;

            return true;
        }
        #endregion

        private ApplicationViewModel _av;
        private DateTime _od = DateTime.Now;
        private komitenti_ime_matbr_zracun _odabraniKomitent;

        //private ObservableCollection<IGRE> sveIgre;
        //private IGRE igra;
        private List<IGRE> igreList;
        public ICommand KomitentiCommand { get; set; }

        public ICommand SacuvajPologCommand { get; set; }

        #region Constructor


        //Greska - System.ArgumentException: 'An item with the same key has already been added.'
        public PologPazaraViewModel(Mediator mediator)
        {
            this.mediator = mediator;

           


            //Komandu instanciramo
            DeleteCommand = new NewRelayCommand(DeleteExecute, CanDelete);

            //Ova metoda se aktivira svaki put kada dodje do promjene vrijednosti nekog propertija
            this.PropertyChanged += PologPazaraViewModel_PropertyChanged;

            PologPazarList = PologPazarCollection.DohvatiPolog();

            //Kolekciju dodijelimo pogledu - PologPazarListView
            //Ova klasa posjeduje svojstvo filter kojim je moguce def handler metodu, koja obavlja filtriranje
            PologPazarListView = new ListCollectionView(PologPazarList);
            //Postavljamo handler metodu. PologPazarFilter metoda koja obavlja logiku za filtriranje
            PologPazarListView.Filter = PologPazarFilter;

            CurrentPologPazar = new POLOG_PAZAR();

            mediator.Register("PologPromjena", PologPromjena);

            this.KomitentiCommand = new RelayCommand(Komitenti);
        }


        //public PologPazaraViewModel(ApplicationViewModel avm)
        //{

        //    //this.mediator = mediator;
        //    //mediator.Register("PologPromjena", PologPromjena);


        //    //Komandu instanciramo
        //    DeleteCommand = new NewRelayCommand(DeleteExecute, CanDelete);

        //    //Ova metoda se aktivira svaki put kada dodje do promjene vrijednosti nekog propertija
        //    this.PropertyChanged += PologPazaraViewModel_PropertyChanged;

        //    PologPazarList = PologPazarCollection.DohvatiPolog();

        //    //Kolekciju dodijelimo pogledu - PologPazarListView
        //    //Ova klasa posjeduje svojstvo filter kojim je moguce def handler metodu, koja obavlja filtriranje
        //    PologPazarListView = new ListCollectionView(PologPazarList);
        //    //Postavljamo handler metodu. PologPazarFilter metoda koja obavlja logiku za filtriranje
        //    PologPazarListView.Filter = PologPazarFilter;

        //    CurrentPologPazar = new POLOG_PAZAR();

        //    var IgreContext = new LutrijaEntities1();
        //    igreList = IgreContext.IGRE.ToList();

        //    _av = avm;
        //    _odabraniKomitent = new komitenti_ime_matbr_zracun();
        //    _odabraniKomitent.IME = "Odaberi Komitenta";

        //    //igra = new IGRE();
        //    //igra.NAZIV = "Odaberi naziv igre";

        //    this.KomitentiCommand = new RelayCommand(Komitenti);

        //    this.SacuvajPologCommand = new RelayCommand(SacuvajPologPazara);
        //}

        //Konstruktor za unos novog
        public PologPazaraViewModel()
        {
            PologPazarList = PologPazarCollection.DohvatiPolog();
            CurrentPologPazar = new POLOG_PAZAR();
        }
        //Konstruktor za Editovanje
        public PologPazaraViewModel(POLOG_PAZAR pOLOG_PAZAR)
        {
            CurrentPologPazar = pOLOG_PAZAR;

        }
        #endregion

        //Ovaj metod prihvata jedan parametar, a to je ustvari objekat koji se promjenio
        private void PologPromjena(object obj)
        {
            //Vrsimom castovanje objekt tipa u konkretan tip PologPazar
            POLOG_PAZAR pOLOG_PAZAR = (POLOG_PAZAR)obj;

            //Instanca koja je pristigla sa prozora za unos i azuriranje 
            //Utvrdjujemo da li instanca postoji u kolekciji. Za to koristimo ovu metodu IndexOf. Ona za provjeru jednakosti objekata koristi Equals metodu
            //koju smo implementirali 
            int index = PologPazarList.IndexOf(pOLOG_PAZAR);

            //Ako instanca postoji, odnosno ako PologPazar postoji u kolekciji, ona se izbacuje iz kolekcije metodom RemoveAt(), a na njeno mjesto se 
            //smjesta azurirana instanca metodom Insert()
            if (index != -1)
            {
                PologPazarList.RemoveAt(index);
                PologPazarList.Insert(index, pOLOG_PAZAR);
            }
            //Ukoliko instance nema u kolekciji, znaci da je izvrseno ubacivanje nove osobe, tako da se vrsi samo dodavanje PologPazar kolekciji metodom Add

            else
            {
                PologPazarList.Add(pOLOG_PAZAR);
            }


            //Sada je potrebno da izvrsim azuriranje same kolekcije objekata tipa Polog_Pazar 

            //Kako znati da li je doslo do promjene, odnosno da li je doslo do unosa novog ili promjene(to radimo utvrdjivanjem jednakosti objekata
            //metodom Equals)

        }


        private void PologPazaraViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Interesuje nas samo jedan property, a to je prop za filtering tekst
            //Ako je doslo do promjene propertija za FilteringText, osvjezimo kolikciju metodom Refresh();
            if (e.PropertyName.Equals("FilteringText"))
            {
                PologPazarListView.Refresh();
            }
        }

        //Filtriranje se obavlja na osnovu unijetog teksta za filtraciju
        private bool PologPazarFilter(object obj)
        {
            //Ako je vrijednost null filteriranja nece biti i zato ce autom biti vracena vrijednost true
            if (FilteringText == null) return true;
            //isto vazi i ako je tekst za filtraciju prazan string
            if (FilteringText.Equals("")) return true;

            //Ukoliko postoji neki unijeti tekst u polju za filtraciju
            POLOG_PAZAR polog_pazar = obj as POLOG_PAZAR;
            //Uzimamo op_broj koji smo unijeli u filtering tekst. Ako vrati true stavka ce se naci u pogledu
            //Dovoljno je jedan broj da zapocne filtraciju
            return (polog_pazar.OP_BROJ_PROD.StartsWith(FilteringText));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        
        private void Komitenti()
        {
            if (_av.OdabraniVM == this)
            {

               // _av.OdabraniVM = new OdaberiKomitentaViewModel(_av);

            }
        }
        private void SacuvajPologPazara()
        {
           // currentPologPazar.Save();
        }

        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SviKomitenti"); } }
    }
}
