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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace LutrijaWpfEF.ViewModel
{
    public class DinoUplOsnovnihViewModel : ObservableObject
    {
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private List<EOP_SIN> _pretragaUplO;
        private ObservableCollection<EOP_SIN> _sveUplateOsnovnih;
        private List<IGRE> _igre;
        private EOP_SIN _odabranaUplata;
        private ApplicationViewModel _avm;
        private GlavniViewModel _gvm;
        private OdaberiKomitentaViewModel _okvm;
        private string _op;
        private string _pretraga;


        public ICommand DodajCommand { get; set; }
        public ICommand IzmijeniCommand { get; set; }

        public DinoUplOsnovnihViewModel(OdaberiKomitentaViewModel okvm, string op)
        {
            _okvm = okvm;

            _op = op;

            _gvm = _okvm.GVM;
            _igre = _gvm.AVM.Gr.OsnovneIgre;
            SveUplateOsnovnih = new ObservableCollection<EOP_SIN>();
            _pretragaUplO = NapuniUplOsnovnihZaKomitenta(_op);

            foreach (EOP_SIN i in _pretragaUplO)
            {
                i.NazivIgre = (from ig in _igre
                               where ig.SIF_UPL == i.IGRA
                               select ig.NAZIV).FirstOrDefault();
                SveUplateOsnovnih.Add(i);
            }

            Sortiraj();

            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);
        }
        public DinoUplOsnovnihViewModel(ApplicationViewModel avm)
        {
            _avm = avm;
            SveUplateOsnovnih = new ObservableCollection<EOP_SIN>();
            _pretragaUplO = NapuniUplOsnovnih();

            foreach (EOP_SIN i in _pretragaUplO)
            {
                i.NazivIgre = (from ig in _igre
                               where ig.SIF_UPL == i.IGRA
                               select ig.NAZIV).FirstOrDefault();
                SveUplateOsnovnih.Add(i);
            }

            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);
        }

        private void Sortiraj()
        {
            ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_sveUplateOsnovnih);
            komitentiView.SortDescriptions.Clear();
            komitentiView.SortDescriptions.Add(new SortDescription("DATUM", ListSortDirection.Descending));
        }

        public void TraziUplO(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {

                SveUplateOsnovnih = new ObservableCollection<EOP_SIN>(from i in _sveUplateOsnovnih
                                                                      where i.OP_BROJ.ToString().IndexOf(_pretraga) >= 0
                                                                      select i);
            }
            else
            {
                SveUplateOsnovnih.Clear();

                if (_pretragaUplO != null)
                {

                    foreach (EOP_SIN uplata in _pretragaUplO)
                    {
                        SveUplateOsnovnih.Add(uplata);
                    }
                }
            }
            Sortiraj();
        }

        private void Dodaj()
        {

            if (_gvm.OdabraniVM == this)
            {

                _odabranaUplata = new EOP_SIN();
                _odabranaUplata.OP_BROJ = int.Parse(_op);

                _gvm.OdabraniVM = new IzmijeniUplOsnovnihViewModel(this, _odabranaUplata);

            }
        }

        private void Izmijeni()
        {

            if (_gvm.OdabraniVM == this)
            {
                if (_odabranaUplata != null)
                {
                    _gvm.OdabraniVM = new IzmijeniUplOsnovnihViewModel(this, _odabranaUplata);
                }
                else
                {
                    MessageBox.Show("Morate odabrati stavku za izmjenu.");
                }

            }
        }
        private List<EOP_SIN> NapuniUplOsnovnih()
        {
            List<EOP_SIN> upl = new List<EOP_SIN>();
            using (var context = new LutrijaEntities1())
            {
                _igre = context.IGRE.ToList();
                upl = context.EOP_SIN.Where(s => s.SRECKA == 0).ToList();
            }
            return upl;
        }

       
        private List<EOP_SIN> NapuniUplOsnovnihZaKomitenta(string op)
        {
            List<EOP_SIN> upl = new List<EOP_SIN>();
            int opb = int.Parse(op);
            using (var context = new LutrijaEntities1())
            {     
                upl = context.EOP_SIN.Where(s => s.SRECKA == 0 && s.OP_BROJ == opb).ToList();
            }
            return upl;
        }
    
    public ObservableCollection<EOP_SIN> SveUplateOsnovnih { get => _sveUplateOsnovnih; set { _sveUplateOsnovnih = value; OnPropertyChanged("SveUplateOsnovnih"); } }

        public EOP_SIN OdabranaUplata { get => _odabranaUplata; set { _odabranaUplata = value; OnPropertyChanged("OdabranaUplata"); } }

        public GlavniViewModel GVM { get => _gvm; set { _gvm = value; OnPropertyChanged("GVM"); } }

        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }

        public string Pretraga { get => _pretraga; set { _pretraga = value; TraziUplO(_pretraga); } }


    }
}