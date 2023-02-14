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
    public class IzmijeniPazarDinoViewModel : ObservableObject
    {
        POLOG_PAZAR _odabraniPazar;
        ApplicationViewModel _avm;
        GlavniViewModel _gVM;
        RUDinoPologPazaraViewModel _rpp;
        komitenti_ime_matbr_zracun _odabraniKomitent;

        private bool _omogucenoDugme = true;     
        public ICommand KomitentiCommand { get; set; }
        public ICommand SpasiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }

        public IzmijeniPazarDinoViewModel(RUDinoPologPazaraViewModel rpp, POLOG_PAZAR pazar)
        {
            //_avm = avm;
           
            _odabraniPazar = pazar;
            if (_odabraniPazar.ID == 0 )
            {
                _odabraniPazar.DATUM = DateTime.Now;
            }
            _rpp = rpp;
       
            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.SpasiCommand =  new RelayCommand(async () => await Spasi());
            this.OdustaniCommand = new RelayCommand(Odustani);
        }

      
        private void Komitenti()
        {
            if (_rpp.GVM.OdabraniVM == this)
            {
                _rpp.GVM.OdabraniVM = new OdaberiKomitentaViewModel(this, _odabraniPazar);
            }
           
        }

       

        private async Task Spasi()
        {
            if (_odabraniPazar != null)
            {
                PazarRepository pr = new PazarRepository(_odabraniPazar, _odabraniKomitent);
                pr.DodajPazar();
                await Task.Run(() => this.RPP.GVM.AVM.Gr.IzmijeniPocStanje(_odabraniPazar.OP_BROJ_PROD, _odabraniPazar.DATUM));
                this.RPP.GVM.AVM.Gr.NapuniPazar();
            }
            //RUDinoPologPazaraViewModel ru = new RUDinoPologPazaraViewModel(_avm);
            //_avm.OdabraniVM = ru;
        }

        private void Odustani()
        {
            _rpp.GVM.OdabraniVM = _rpp;
        }

        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }
        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }

        public POLOG_PAZAR OdabraniPazar { get => _odabraniPazar; set { _odabraniPazar = value; OnPropertyChanged("OdabraniPazar"); } }

        public RUDinoPologPazaraViewModel RPP { get => _rpp; set { _rpp = value; OnPropertyChanged("RPP"); } }
        public bool OmogucenoDugme { get => _omogucenoDugme; set { _omogucenoDugme = value; OnPropertyChanged("OmogucenoDugme"); } }
    }
}
