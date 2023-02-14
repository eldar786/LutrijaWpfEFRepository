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


    public class RUDinoPologPazaraViewModel : ObservableObject
    {
        private List<POLOG_PAZAR> _pretragaPazara;
        private ObservableCollection<POLOG_PAZAR> _sviPoloziPazara;
        private POLOG_PAZAR _odabraniPazar;
        private ApplicationViewModel _avm;
        private GlavniViewModel _gVM;
        private OdaberiKomitentaViewModel _okvm;
        private string _op;
        private string _pretraga;
        public ICommand DodajCommand { get; set; }
        public ICommand IzmijeniCommand { get; set; }

       
        public RUDinoPologPazaraViewModel(OdaberiKomitentaViewModel okvm, string op)
        {
            _okvm = okvm;
            _op = op;
            _gVM = _okvm.GVM;

            SviPoloziPazara = new ObservableCollection<POLOG_PAZAR>();
            _pretragaPazara = NapuniPazareZaKomitenta(_op);

            foreach (POLOG_PAZAR p in _pretragaPazara)
            {
                SviPoloziPazara.Add(p);
            }

            Sortiraj();

            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);
        }

        private void Sortiraj()
        {
            ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_sviPoloziPazara);
            komitentiView.SortDescriptions.Clear();
            komitentiView.SortDescriptions.Add(new SortDescription("DATUM", ListSortDirection.Descending));
        }
        public void TraziPazar(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {
                SviPoloziPazara = new ObservableCollection<POLOG_PAZAR>(from i in _sviPoloziPazara
                                                                                       where i.OP_BROJ_PROD.IndexOf(_pretraga) >= 0 || i.OPIS.ToUpper().IndexOf(_pretraga) >= 0
                                                                                    select i);
            }
            else
            {
                SviPoloziPazara.Clear();

                if (_pretragaPazara != null)
                {

                    foreach (POLOG_PAZAR pazar in _pretragaPazara)
                    {
                        SviPoloziPazara.Add(pazar);
                    }
                }
            }
            Sortiraj();
        }
        private void Dodaj()
        {

            if (_gVM.OdabraniVM == this)
            {
            
                _odabraniPazar = new POLOG_PAZAR();
                          
                _gVM.OdabraniVM = new IzmijeniPazarDinoViewModel(this, _odabraniPazar);

            }
        }
        private List<POLOG_PAZAR> NapuniPazareZaKomitenta(string op)
        {
            List<POLOG_PAZAR> upl = new List<POLOG_PAZAR>();
           
            using (var context = new LutrijaEntities1())
            {
                upl = context.POLOG_PAZAR.Where(s => s.OP_BROJ_PROD == op).ToList();
            }
            return upl;
        }
        private void Izmijeni()
        {

            if (_gVM.OdabraniVM == this)
            {
                _gVM.OdabraniVM = new IzmijeniPazarDinoViewModel(this, _odabraniPazar);

            }
        }

        //private List<POLOG_PAZAR> NapuniPazare()
        //{
        //    List<POLOG_PAZAR> paz = _gVM.AVM.Gr.NapuniPazar();
           
        //}

        public ObservableCollection<POLOG_PAZAR> SviPoloziPazara { get => _sviPoloziPazara; set { _sviPoloziPazara = value; OnPropertyChanged("SviPoloziPazara"); } }
        public POLOG_PAZAR OdabraniPazar { get => _odabraniPazar; set { _odabraniPazar = value; OnPropertyChanged("OdabraniPazar"); } }

        public GlavniViewModel GVM { get => _gVM; set { _gVM = value; OnPropertyChanged("GVM"); } }

        public string Pretraga { get => _pretraga; set { _pretraga = value; TraziPazar(_pretraga); } }
    }
}