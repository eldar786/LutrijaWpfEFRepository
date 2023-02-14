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
    public class IzmijeniIsplSreckiViewModel : ObservableObject
    {
        ISPLATA _odabranaIsplataS;
        ApplicationViewModel _avm;
        komitenti_ime_matbr_zracun _odabraniKomitent;
        private bool _omogucenoDugme;
        private IGRE igra;
        private List<IGRE> igreList;


        public ICommand KomitentiCommand { get; set; }
        public ICommand SpasiCommand { get; set; }

        public ICommand OdustaniCommand { get; set; }

        private string _opis;

        //private string _tipDokumenta;
        private decimal? _iznos;
        private string _opBroj;

        public IzmijeniIsplSreckiViewModel(ApplicationViewModel avm, ISPLATA isplata)
        {
            if (isplata.LIS_VRISPL == DateTime.MinValue && isplata.LIS_VRISPL == null)
            {
                isplata.LIS_VRISPL = DateTime.Now;
            }
            _avm = avm;
            _odabranaIsplataS = isplata;

            _omogucenoDugme = false;
            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.SpasiCommand = new RelayCommand(Spasi);
            this.OdustaniCommand = new RelayCommand(Odustani);

            var IgreContext = new LutrijaEntities1();
            igreList = IgreContext.IGRE.Where(i => i.SIF_BAZ_IGR == 4).ToList();
        }

        private void Komitenti()
        {
            if (_avm.OdabraniVM == this)
            {
                _avm.OdabraniVM = new OdaberiKomitentaViewModel(this, _odabranaIsplataS);
            }
        }

        private void Spasi()
        {
            if (_odabranaIsplataS != null && _odabraniKomitent != null)
            {
                IsplSreckiRepository pr = new IsplSreckiRepository(_odabranaIsplataS, _odabraniKomitent, igra);
                pr.DodajIsplO();
            }
            DinoIsplSreckiViewModel isp = new DinoIsplSreckiViewModel(_avm);
            _avm.OdabraniVM = isp;
        }

        private void Odustani()
        {
            DinoIsplSreckiViewModel dip = new DinoIsplSreckiViewModel(_avm);
            _avm.OdabraniVM = dip;
        }


        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }
        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SveIgre"); } }

        public IGRE Igra { get => igra; set { igra = value; OnPropertyChanged("Igra"); } }

        public ISPLATA OdabranaIsplataS { get => _odabranaIsplataS; set { _odabranaIsplataS = value; OnPropertyChanged("OdabranaIsplataS"); } }
    }
}
