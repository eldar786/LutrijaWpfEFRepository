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
    public class IzmijeniUplOsnovnihViewModel : ObservableObject
    {
        EOP_SIN _odabranaUplataO;
        ApplicationViewModel _avm;
        komitenti_ime_matbr_zracun _odabraniKomitent;
        OPCINE _odabranaOpstina;
        private bool _omogucenoDugme;
        private IGRE igra;
        private List<IGRE> igreList;
        private DinoUplOsnovnihViewModel _duovm;

        public ICommand KomitentiCommand { get; set; }
        public ICommand SpasiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }
        public ICommand OpstinaCommand { get; set; }

        public IzmijeniUplOsnovnihViewModel(DinoUplOsnovnihViewModel duovm, EOP_SIN uplataO)
        {
            if (uplataO != null)
            { 
            if (uplataO.DATUM == DateTime.MinValue)
            {
                uplataO.DATUM = DateTime.Now;
            }
            _duovm = duovm;
            _odabranaUplataO = uplataO;
            igreList = duovm.GVM.AVM.Gr.OsnovneIgre;
           
                igra = (from IGRE i in igreList
                        where i.SIF_UPL == _odabranaUplataO.IGRA
                        select i).First();
           
            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.SpasiCommand = new RelayCommand(async () => await Spasi());
            this.OdustaniCommand = new RelayCommand(Odustani);
            this.OpstinaCommand = new RelayCommand(Opstina);
            }
           

        }
        public IzmijeniUplOsnovnihViewModel(ApplicationViewModel avm, EOP_SIN uplataO, OPCINE odabranaOpstina)
        {

            if (uplataO.DATUM == DateTime.MinValue)
            {
                uplataO.DATUM = DateTime.Now;
            }
            _avm = avm;
            _odabranaUplataO = uplataO;
            _odabranaUplataO.OPSTINA = odabranaOpstina.OPC_SIF;

            this.KomitentiCommand = new RelayCommand(Komitenti);
            //this.SpasiCommand = new RelayCommand(Spasi);
            this.OdustaniCommand = new RelayCommand(Odustani);
            this.OpstinaCommand = new RelayCommand(Opstina);

            var IgreContext = new LutrijaEntities1();
            igreList = IgreContext.IGRE.ToList();
        }

        private void Opstina()
        {
            int UplOsn1_UplSrc2 = 1;

            if (_duovm.GVM.OdabraniVM == this)
            {
                _duovm.GVM.OdabraniVM = new OpcineViewModel(this, _odabranaUplataO);
            }
        }

        private void Komitenti()
        {
            if (_duovm.GVM.OdabraniVM == this)
            {
                _duovm.GVM.OdabraniVM = new OdaberiKomitentaViewModel(this, _odabranaUplataO);
            }

        }

        private async Task Spasi()
        {
            UplRepository upO = new UplRepository(_odabranaUplataO, igra);
            upO.DodajUplO();


            await Task.Run(() => this._duovm.GVM.AVM.Gr.IzmijeniPocStanje(_odabranaUplataO.OP_BROJ.ToString().PadLeft(5, '0'), _odabranaUplataO.DATUM));
            this._duovm.GVM.AVM.Gr.NapuniUplateOI();

            //DinoUplOsnovnihViewModel io = new DinoUplOsnovnihViewModel(_duovm.GVM);
            //_avm.OdabraniVM = io;
        }

        private void Odustani()
        {           
            _duovm.GVM.OdabraniVM = _duovm;
        }

        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }
        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SviKomitenti"); } }

        public IGRE Igra { get => igra; set { igra = value; OnPropertyChanged("Igra"); } }

        public OPCINE OdabranaOpcina { get => _odabranaOpstina; set { _odabranaOpstina = value; OnPropertyChanged("OdabranaOpcina"); } }
        public EOP_SIN OdabranaUplata { get => _odabranaUplataO; set { _odabranaUplataO = value; OnPropertyChanged("OdabranaUplata"); } }

        public DinoUplOsnovnihViewModel DUOVM { get => _duovm; set { _duovm = value; OnPropertyChanged("DUOVM"); } }
    }
}
