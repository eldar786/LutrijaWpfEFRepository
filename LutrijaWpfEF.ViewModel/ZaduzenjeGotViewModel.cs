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
    public class ZaduzenjeGotViewModel : ObservableObject
    {

        private komitenti_ime_matbr_zracun _odabraniKomitent;

        private List<ZADUZENJE_GOTOVINE> _zaduzenjeGotList;
        private ObservableCollection<ZADUZENJE_GOTOVINE> _svoZadGot;
        private ZADUZENJE_GOTOVINE odabranoZadGotovine;
        private ApplicationViewModel _avm;
        private GlavniViewModel _gvm;
       
        private string _pretraga;
      

        //private ObservableCollection<IGRE> sveIgre;
        //private IGRE igra;
        private List<IGRE> igreList;
        public ICommand KomitentiCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DodajCommand { get; set; }
        public ICommand IzmijeniCommand { get; set; }


        public ZaduzenjeGotViewModel(GlavniViewModel gvm)
        {
            _gvm = gvm;
            igreList = _gvm.AVM.Gr.Igre;
            _odabraniKomitent = new komitenti_ime_matbr_zracun();
            _odabraniKomitent.IME = "Odaberi Komitenta";

            SvoZadGot = new ObservableCollection<ZADUZENJE_GOTOVINE>();
            _zaduzenjeGotList = _gvm.AVM.Gr.Zaduzenja; 

            foreach (ZADUZENJE_GOTOVINE item in _zaduzenjeGotList)
            {
                SvoZadGot.Add(item);
            }
            Sortiraj();
            //igra = new IGRE();
            //igra.NAZIV = "Odaberi naziv igre";

           // this.KomitentiCommand = new RelayCommand(Komitenti);
            this.DeleteCommand = new RelayCommand(Delete);
            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);

        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        private void Sortiraj()
        {
            ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_svoZadGot);
            komitentiView.SortDescriptions.Clear();
            komitentiView.SortDescriptions.Add(new SortDescription("DATUM", ListSortDirection.Descending));
        }
        public void TraziZaduzenje(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {
                SvoZadGot = new ObservableCollection<ZADUZENJE_GOTOVINE>(from i in _svoZadGot
                                                                  where i.ODOBRITI_BLAGAJNIKA.ToString().IndexOf(_pretraga) >= 0 || i.ZADUZITI_BLAGAJNIKA.ToString().ToUpper().IndexOf(_pretraga) >= 0
                                                                        select i);
            }
            else
            {
                SvoZadGot.Clear();

                if (_zaduzenjeGotList != null)
                {

                    foreach (ZADUZENJE_GOTOVINE zad in _zaduzenjeGotList)
                    {
                        SvoZadGot.Add(zad);
                    }
                }
            }
            Sortiraj();
        }

        public void OnpropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //private void Komitenti()
        //{

        //    if (_gvm.OdabraniVM == this)
        //    {
        //        _gvm.OdabraniVM = new OdaberiKomitentaViewModel(_gvm, 3);
        //    }
        //}

        private void Dodaj()
        {
            //System.NullReferenceException: 'Object reference not set to an instance of an object.'
            //_avm was null.
            if (_gvm.OdabraniVM == this)
            {

                odabranoZadGotovine = new ZADUZENJE_GOTOVINE();
                odabranoZadGotovine.DATUM = DateTime.Now;

                _gvm.OdabraniVM = new IzmijeniZadGotovineViewModel(this, odabranoZadGotovine);
            }
        }

        private void Izmijeni()
        {
            if (_gvm.OdabraniVM == this)
            {
                _gvm.OdabraniVM = new IzmijeniZadGotovineViewModel(this, odabranoZadGotovine);
            }
        }

        private void Delete()
        {
            var context = new LutrijaEntities1();

            foreach (var item in context.ZADUZENJE_GOTOVINE.Where(z => z.ID == OdabranoZadGotovine.ID))
            {
                context.ZADUZENJE_GOTOVINE.Remove(item);
                SvoZadGot.Remove(OdabranoZadGotovine);
            }
            context.SaveChanges();
        }


        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SviKomitenti"); } }
        public ObservableCollection<ZADUZENJE_GOTOVINE> SvoZadGot { get => _svoZadGot; set { _svoZadGot = value; OnPropertyChanged("SvoZadGot"); } }
        public ZADUZENJE_GOTOVINE OdabranoZadGotovine { get => odabranoZadGotovine; set { odabranoZadGotovine = value; OnPropertyChanged("OdabranoZadGotovine"); } }

        public GlavniViewModel GVM { get => _gvm; set { _gvm = value; OnPropertyChanged("GVM"); } }

        public string Pretraga { get => _pretraga; set { _pretraga = value; TraziZaduzenje(_pretraga); } }
    }
}

