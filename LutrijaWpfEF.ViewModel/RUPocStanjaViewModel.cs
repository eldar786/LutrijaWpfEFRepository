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


        public class RUPocStanjaViewModel : ObservableObject
        {
            private List<POC_STANJA> _pretragaPocStanja;
        private ObservableCollection<POC_STANJA> _svaPocStanja;
            private POC_STANJA _odabranoStanje;
            private ApplicationViewModel _avm;
            private string _pretraga;
            public ICommand DodajCommand { get; set; }
            public ICommand IzmijeniCommand { get; set; }


            public RUPocStanjaViewModel(ApplicationViewModel avm)
            {

                _avm = avm;
                SvaPocStanja = new ObservableCollection<POC_STANJA>();
                _pretragaPocStanja = NapuniStanja();

                foreach (POC_STANJA p in _pretragaPocStanja)
                {
                    SvaPocStanja.Add(p);
                }

                Sortiraj();

                this.DodajCommand = new RelayCommand(Dodaj);
                this.IzmijeniCommand = new RelayCommand(Izmijeni);
            }

            private void Sortiraj()
            {
                ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_svaPocStanja);
                komitentiView.SortDescriptions.Clear();
                komitentiView.SortDescriptions.Add(new SortDescription("DATUM", ListSortDirection.Descending));
            }
            public void TraziStanje(string _pretraga)
            {
                if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
                {
                    SvaPocStanja = new ObservableCollection<POC_STANJA>(from i in _svaPocStanja
                                                                            where i.OP_BROJ.ToString().IndexOf(_pretraga) >= 0
                                                                            select i);
                }
                else
                {
                SvaPocStanja.Clear();

                    if (_pretragaPocStanja != null)
                    {

                        foreach (POC_STANJA stanje in _pretragaPocStanja)
                        {
                            SvaPocStanja.Add(stanje);
                        }
                    }
                }
                Sortiraj();
            }
            private void Dodaj()
            {

                if (_avm.OdabraniVM == this)
                {

                    _odabranoStanje = new POC_STANJA();

                    _avm.OdabraniVM = new IzmijeniPocStanjeViewModel(_avm, _odabranoStanje);

                }
            }

            private void Izmijeni()
            {

                if (_avm.OdabraniVM == this)
                {
                    _avm.OdabraniVM = new IzmijeniPocStanjeViewModel(_avm, _odabranoStanje);

                }
            }

            private List<POC_STANJA> NapuniStanja()
            {
                List<POC_STANJA> st = new List<POC_STANJA>();
                using (var context = new LutrijaEntities1())
                {
                    st = context.POC_STANJA.ToList();
                }
                return st;
            }

            public ObservableCollection<POC_STANJA> SvaPocStanja{ get => _svaPocStanja; set { _svaPocStanja = value; OnPropertyChanged("SvaPocStanja"); } }
            public POC_STANJA OdabraniPazar { get => _odabranoStanje; set { _odabranoStanje = value; OnPropertyChanged("OdabraniPazar"); } }

            public string Pretraga { get => _pretraga; set { _pretraga = value; TraziStanje(_pretraga); } }
        }
    }

