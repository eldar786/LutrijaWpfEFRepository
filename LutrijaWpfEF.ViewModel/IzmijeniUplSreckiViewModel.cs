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
    public class IzmijeniUplSreckiDinoViewModel : ObservableObject
    {
        EOP_SIN _odabranaUplSrecki;
        ApplicationViewModel _avm;
        komitenti_ime_matbr_zracun _odabraniKomitent;
        private IGRE igra;
        private List<IGRE> igreList;
        //private List<IGRE>
        private int _odabranaSerijaSrecke;
        private string _odabranaOPC_SIF;


        private bool _omogucenoDugme = true;
        public ICommand KomitentiCommand { get; set; }
        public ICommand SpasiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }

        public ICommand OpstinaCommand { get; set; }

        public IzmijeniUplSreckiDinoViewModel(ApplicationViewModel avm, EOP_SIN uplS)
        {
            if (uplS.DATUM == DateTime.MinValue)
            {
                uplS.DATUM = DateTime.Now;
            }
            _avm = avm;
            _odabranaUplSrecki = uplS;

            //Iz kolone SRECKA izvukli smo zadnji broj i ubacili u polje Serija
            string serijaSreckeString = Convert.ToString(_odabranaUplSrecki.SRECKA);
            int serijaSrecke = Int32.Parse(serijaSreckeString.Substring(serijaSreckeString.Length - 1));
            _odabranaSerijaSrecke = serijaSrecke;


            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.SpasiCommand = new RelayCommand(Spasi);
            this.OdustaniCommand = new RelayCommand(Odustani);
            this.OpstinaCommand = new RelayCommand(Opstina);

            var IgreContext = new LutrijaEntities1();
            igreList = IgreContext.IGRE.Where(i => i.SIF_BAZ_IGR == 4).ToList();
        }


        private void Komitenti()
        {
            if (_avm.OdabraniVM == this)
            {
                _avm.OdabraniVM = new OdaberiKomitentaViewModel(this, _odabranaUplSrecki);
            }
        }

        private void Opstina()
        {
            if (_avm.OdabraniVM == this)
            {
                //OpcineViewModel opcineViewModel = new OpcineViewModel(_avm, _odabranaUplSrecki);
                //_avm.OdabraniVM = opcineViewModel;

            }
        }

        private void Spasi()
        {
            if (_odabranaUplSrecki != null)
            {
                UplSreckiRepository pr = new UplSreckiRepository(_odabranaUplSrecki, _odabraniKomitent, igra, _odabranaSerijaSrecke);
                pr.DodajUplS();
            }
            DinoUplSreckiViewModel ru = new DinoUplSreckiViewModel(_avm);
            _avm.OdabraniVM = ru;
        }

        private void Odustani()
        {
            DinoUplSreckiViewModel ru = new DinoUplSreckiViewModel(_avm);
            _avm.OdabraniVM = ru;
        }

        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }
        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SveIgre"); } }

        public IGRE Igra { get => igra; set { igra = value; OnPropertyChanged("Igra"); } }

        public EOP_SIN OdabranaUplSrecki { get => _odabranaUplSrecki; set { _odabranaUplSrecki = value; OnPropertyChanged("OdabranaUplSrecki"); } }

        public bool OmogucenoDugme { get => _omogucenoDugme; set { _omogucenoDugme = value; OnPropertyChanged("OmogucenoDugme"); } }
        public int OdabranaSerijaSrecke { get => _odabranaSerijaSrecke; set { _odabranaSerijaSrecke = value; OnPropertyChanged("OdabranaSerijaSrecke"); } }
    }
}
