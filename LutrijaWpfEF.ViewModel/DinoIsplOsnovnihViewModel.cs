using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace LutrijaWpfEF.ViewModel
{
    public class DinoIsplOsnovnihViewModel : ObservableObject
    {
        private List<ISPLATA> _pretragaIsplO;
        private ObservableCollection<ISPLATA> _sveIsplateOsnovnih;
        private ISPLATA _odabranaIsplata;
        private ApplicationViewModel _avm;
        private GlavniViewModel _gvm;
        private OdaberiKomitentaViewModel _okvm;
        private string _pretraga;
        private List<IGRE> igreList;
        private string _op;

        public ICommand DodajCommand { get; set; }
        public ICommand IzmijeniCommand { get; set; }

        public DinoIsplOsnovnihViewModel(GlavniViewModel gvm)
        {
            var IgreContext = new LutrijaEntities1();
            igreList = IgreContext.IGRE.ToList();
            _gvm = gvm;
            SveIsplateOsnovnih = new ObservableCollection<ISPLATA>();
            _pretragaIsplO = NapuniIsplOsnovnih();

            foreach (ISPLATA i in _pretragaIsplO)
            {
                SveIsplateOsnovnih.Add(i);
            }

            Sortiraj();

            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);
        }

        public DinoIsplOsnovnihViewModel(OdaberiKomitentaViewModel okvm, string op)
        {
            _okvm = okvm;

             _op = op;
            
            _gvm = _okvm.GVM;
            igreList = _gvm.AVM.Gr.OsnovneIgre;
            SveIsplateOsnovnih = new ObservableCollection<ISPLATA>();
            _pretragaIsplO = NapuniIsplOsnovnihZaKomitenta(_op);

            foreach (ISPLATA i in _pretragaIsplO)
            {
                SveIsplateOsnovnih.Add(i);
            }

            Sortiraj();

            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);
        }

        private void Sortiraj()
        {
            ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_sveIsplateOsnovnih);
            komitentiView.SortDescriptions.Clear();
            komitentiView.SortDescriptions.Add(new SortDescription("LIS_VRISPL", ListSortDirection.Descending));
        }

        public void TraziIsplO(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {
                SveIsplateOsnovnih = new ObservableCollection<ISPLATA>(from i in _sveIsplateOsnovnih
                                                                       where i.LIS_ISPL.IndexOf(_pretraga) >= 0
                                                                       select i);
            }
            else
            {
                SveIsplateOsnovnih.Clear();

                if (_pretragaIsplO != null)
                {
                    foreach (ISPLATA isplata in _pretragaIsplO)
                    {
                        SveIsplateOsnovnih.Add(isplata);
                    }
                }
            }
            Sortiraj();
        }

        private void Dodaj()
        {

            if (_gvm.OdabraniVM == this)
            {

                _odabranaIsplata = new ISPLATA();
                _odabranaIsplata.LIS_ISPL = _op;
                _odabranaIsplata.LIS_VRISPL = DateTime.Now;

                _gvm.OdabraniVM = new IzmijeniIsplOsnovnihViewModel(this, _odabranaIsplata);

            }
        }

        private void Izmijeni()
        {

            if (_gvm.OdabraniVM == this)
            {
                _gvm.OdabraniVM = new IzmijeniIsplOsnovnihViewModel(this, _odabranaIsplata);

            }
        }
        private List<ISPLATA> NapuniIsplOsnovnih()
        {
            List<ISPLATA> ispl = new List<ISPLATA>();
            using (var context = new LutrijaEntities1())
            {
                ispl = context.ISPLATA.Where(s => s.LIS_SERIJA_SRE == 0 ).ToList();
            }
            return ispl;
        }

        private List<ISPLATA> NapuniIsplOsnovnihZaKomitenta(string op)
        {
            List<ISPLATA> ispl = new List<ISPLATA>();
            using (var context = new LutrijaEntities1())
            {
                ispl = context.ISPLATA.Where(s => s.LIS_SERIJA_SRE == 0 && s.LIS_ISPL == op).ToList();
            }
            return ispl;
        }

        public ObservableCollection<ISPLATA> SveIsplateOsnovnih { get => _sveIsplateOsnovnih; set { _sveIsplateOsnovnih = value; OnPropertyChanged("SveIsplateOsnovnih"); } }

        public ISPLATA OdabranaIsplata { get => _odabranaIsplata; set { _odabranaIsplata = value; OnPropertyChanged("OdabranaIsplata"); } }

        public string Pretraga { get => _pretraga; set { _pretraga = value; TraziIsplO(_pretraga); } }

        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SviKomitenti"); } }

        public GlavniViewModel GVM { get => _gvm; set { _gvm = value; OnPropertyChanged("GVM"); } }
    }
}
