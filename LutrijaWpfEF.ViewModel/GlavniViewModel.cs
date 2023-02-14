using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
    public class GlavniViewModel : ObservableObject
    {
        ApplicationViewModel _avm;
        PRIJAVA_ORG _korisnik;
        bool _prikaziStatistiku;
        public ICommand GlavniCommand { get; set; }

        public ICommand OpBrojCommand { get; set; }

        public ICommand OrgCommand { get; set; }

        public ICommand StatistikaCommand { get; set; }

        public ICommand ImportCommand { get; set; }

        public ICommand EopAnaCommand { get; set; }

        public ICommand GrOrgCommand { get; set; }

        public ICommand GrKomCommand { get; set; }

        public ICommand RucniCommand { get; set; }

        public ICommand PologCommand { get; set; }

        public ICommand ZaduzenjeCommand { get; set; }
        public ICommand IsplOsnovnihCommand { get; set; }
        public ICommand UplOsnovnihCommand { get; set; }
        public ICommand IsplSreckiCommand { get; set; }
        public ICommand UplSreckiCommand { get; set; }
        public ICommand SacuvajPologCommand { get; set; }

        public ICommand PrijavaCommand { get; set; }
        public ICommand OtvoriIsplatuOsnovnihSve { get; set; }

        private object _odabraniVM;

        private object _odabraniVMW;

        private bool _prikaziRU = false;


        public GlavniViewModel(ApplicationViewModel avm, PRIJAVA_ORG korisnik)
        {

            _korisnik = korisnik;

            if (_korisnik.VRSTA == 10)
            {
                PrikaziRU = true;
            }

            PrikaziStatistiku = false;

            _avm=avm;
            _avm.OdabraniVM = this;
           // PrikaziStatistiku = true;
            GlavniCommand = new RelayCommand(OtvoriGlavni);

            OpBrojCommand = new RelayCommand(OtvoriOpBroj);

            OrgCommand = new RelayCommand(OtvoriOrg);

            StatistikaCommand = new RelayCommand(OtvoriStatistiku);

            ImportCommand = new RelayCommand(OtvoriImport);

            EopAnaCommand = new RelayCommand(OtvoriEopAna);

            GrOrgCommand = new RelayCommand(OtvoriGrOrg);

            GrKomCommand = new RelayCommand(OtvoriGrKom);

            RucniCommand = new RelayCommand(OtvoriRucniUnos);

            PologCommand = new RelayCommand(OtvoriPologPazara);

            ZaduzenjeCommand = new RelayCommand(OtvoriZaduzenjeGot);

            IsplOsnovnihCommand = new RelayCommand(OtvoriIsplOsnovnih);

            UplOsnovnihCommand = new RelayCommand(OtvoriUplOsnovnih);
            IsplSreckiCommand = new RelayCommand(OtvoriIsplSrecki);
            UplSreckiCommand = new RelayCommand(OtvoriUplSrecki);

            OtvoriIsplatuOsnovnihSve = new RelayCommand(OtvoriIsplOsnovnihSve);
        }

        public void OtvoriGlavni()

        {
            //OdabraniVM = new GlavniViewModel();

        }

        private void OtvoriOpBroj()

        {
            OdabraniVM = new OdaberiOpBrojViewModel(_avm);
        }

        private void OtvoriOrg()
        {
            OdabraniVM = new OrgViewModel(_avm, this, _korisnik);
        }

        private void OtvoriStatistiku()
        {
            OdabraniVM = new StatistikaProdavaciViewModel(_avm);
        }

        private void OtvoriImport()
        {
            OdabraniVM = new ImportViewModel(this);
        }

        private void OtvoriEopAna()
        {
            OdabraniVM = new EopAnaViewModel();
        }

        private void OtvoriGrOrg()
        {
            OdabraniVM = new GrOrgViewModel();
        }

        private void OtvoriGrKom()
        {
            OdabraniVM = new GrKomViewModel();
        }

        private void OtvoriRucniUnos()
        {
            OdabraniVM = new RucniUnosViewModel(_avm);
        }

        private void OtvoriPologPazara()
        {
            OdabraniVM = new  OdaberiKomitentaViewModel(this, 3);
        }

        private void OtvoriZaduzenjeGot()
        {
            OdabraniVM = new ZaduzenjeGotViewModel(this);
        }
        private void OtvoriIsplOsnovnih()
        {

            OdabraniVM = new OdaberiKomitentaViewModel(this, 1);
        }

        private void OtvoriIsplOsnovnihSve()
        {
            OdabraniVM = new DinoIsplOsnovnihViewModel(this);
        }
        private void OtvoriUplOsnovnih()
        {
            OdabraniVM = new OdaberiKomitentaViewModel(this, 2);
        }
        private void OtvoriIsplSrecki()
        {
            OdabraniVM = new IsplataSreckiViewModel(_avm);
        }
        private void OtvoriUplSrecki()
        {
            OdabraniVM = new DinoUplSreckiViewModel(_avm);
        }

        public object OdabraniVM
        {

            get { return _odabraniVM; }

            set
            {
                _odabraniVM = value;
                OnPropertyChanged("OdabraniVM");
            }

        }

        public object OdabraniVMW
        {

            get { return _odabraniVMW; }

            set
            {
                _odabraniVMW = value;
                OnPropertyChanged("OdabraniVMW");
            }

        }

        public bool PrikaziStatistiku
        {

            get { return _prikaziStatistiku; }

            set { _prikaziStatistiku = value; OnPropertyChanged("PrikaziStatistiku"); }

        }

       public ApplicationViewModel AVM
        {

            get { return _avm; }

            set
            {
                _avm = value;
                OnPropertyChanged("AVM");
            }

        }

        public bool PrikaziRU
        {

            get { return _prikaziRU; }

            set
            {
                _prikaziRU = value;
                OnPropertyChanged("PrikaziRU");
            }

        }

    }
}
