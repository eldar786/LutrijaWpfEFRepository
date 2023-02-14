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
    public class IzmijeniIsplOsnovnihViewModel : ObservableObject
    {
        private ApplicationViewModel _av;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private List<IGRE> igreList;
        ISPLATA _odabranaIsplataO;
        ApplicationViewModel _avm;
        private bool _omogucenoDugme = true;
        private IGRE igra;
        private DinoIsplOsnovnihViewModel _diovm;
        public ICommand KomitentiCommand { get; set; }
        public ICommand SpasiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }

        public IzmijeniIsplOsnovnihViewModel(DinoIsplOsnovnihViewModel diovm, ISPLATA isplata)
        {
            _diovm = diovm;
            _odabranaIsplataO = isplata;
            igreList = diovm.SveIgre;
            if (_odabranaIsplataO.SIF_IGRE != null)
            {
                igra = (from IGRE i in igreList
                        where i.SIF_ISP == _odabranaIsplataO.SIF_IGRE
                        select i).First();
            }
            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.SpasiCommand = new RelayCommand(async () => await Spasi());
            this.OdustaniCommand = new RelayCommand(Odustani);

          
        }


        public IzmijeniIsplOsnovnihViewModel(ApplicationViewModel avm)
        {
            var IgreContext = new LutrijaEntities1();
            igreList = IgreContext.IGRE.ToList();
            _av = avm;
            _odabraniKomitent = new komitenti_ime_matbr_zracun();
            _odabraniKomitent.IME = "Odaberi Komitenta";

            this.KomitentiCommand = new RelayCommand(Komitenti);
        }

        private void Komitenti()
        {
            if (_diovm.GVM.OdabraniVM == this)
            {
                _diovm.GVM.OdabraniVM = new OdaberiKomitentaViewModel(this, _odabranaIsplataO);
            }
        }

        private async Task Spasi()
        {
            if (_odabranaIsplataO != null)
            {
                IsplOsnovnihRepository pr = new IsplOsnovnihRepository(_odabranaIsplataO, igra);
                pr.DodajIsplO();
            }
            //DinoIsplOsnovnihViewModel io = new DinoIsplOsnovnihViewModel(_);
            //_avm.OdabraniVM = io;

            await Task.Run(() => this._diovm.GVM.AVM.Gr.IzmijeniPocStanje(_odabranaIsplataO.LIS_ISPL, _odabranaIsplataO.LIS_VRISPL));
            this._diovm.GVM.AVM.Gr.NapuniIsplateOI();

        }

        private void Odustani()
        {
            //DinoIsplOsnovnihViewModel di = new DinoIsplOsnovnihViewModel(_diovm.GVM);
            //_diovm.GVM.OdabraniVM = di;

            _diovm.GVM.OdabraniVM = _diovm;
        }


        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }
        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SviKomitenti"); } }
        public IGRE Igra { get => igra; set { igra = value; OnPropertyChanged("Igra"); } }
        public ISPLATA OdabranaIsplata { get => _odabranaIsplataO; set { _odabranaIsplataO = value; OnPropertyChanged("OdabranaIsplata"); } }

        public DinoIsplOsnovnihViewModel DIOVM { get => _diovm; set { _diovm = value; OnPropertyChanged("DIOVM"); } }
        public bool OmogucenoDugme { get => _omogucenoDugme; set { _omogucenoDugme = value; OnPropertyChanged("OmogucenoDugme"); } }
    }
}
