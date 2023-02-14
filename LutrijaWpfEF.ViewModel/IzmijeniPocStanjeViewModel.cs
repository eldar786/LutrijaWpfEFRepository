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
    public class IzmijeniPocStanjeViewModel : ObservableObject
    {
        POC_STANJA _odabranoStanje;
        ApplicationViewModel _avm;
        komitenti_ime_matbr_zracun _odabraniKomitent;

        private bool _omogucenoDugme = true;
        public ICommand KomitentiCommand { get; set; }
        public ICommand SpasiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }

        public IzmijeniPocStanjeViewModel(ApplicationViewModel avm, POC_STANJA stanje)
        {
            _avm = avm;
            _odabranoStanje = stanje;


            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.SpasiCommand = new RelayCommand(Spasi);
            this.OdustaniCommand = new RelayCommand(Odustani);
        }


        private void Komitenti()
        {
            if (_avm.OdabraniVM == this)
            {
                _avm.OdabraniVM = new OdaberiKomitentaViewModel(this, _odabranoStanje);
            }

        }

        private void Spasi()
        {
            if (_odabranoStanje != null)
            {
                StanjeRepository pr = new StanjeRepository(_odabranoStanje, _odabraniKomitent);
                pr.DodajStanje();
                this.AVM.Gr.NapuniPocStanja();
            }
            RUPocStanjaViewModel ru = new RUPocStanjaViewModel(_avm);
            _avm.OdabraniVM = ru;
        }

        private void Odustani()
        {
            RUPocStanjaViewModel ru = new RUPocStanjaViewModel(_avm);
            _avm.OdabraniVM = ru;
        }

        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }
        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }

        public POC_STANJA OdabranoStanje { get => _odabranoStanje; set { _odabranoStanje = value; OnPropertyChanged("OdabranoStanje"); } }

        public bool OmogucenoDugme { get => _omogucenoDugme; set { _omogucenoDugme = value; OnPropertyChanged("OmogucenoDugme"); } }
    }
}
