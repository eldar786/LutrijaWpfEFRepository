using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
    public class ApplicationViewModel : ObservableObject
    {
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

        //GlavniView.xaml
        //<ContentControl x:Name="odabraniContent" Content="{Binding OdabraniVM}" Grid.Row="1" Grid.Column="1" Grid.RowSpan="10"/>
        private object _odabraniVM;

        private object _odabraniVMOpBroj;
        private object _odabraniVMOrgView;
        private object _odabraniVMImport;
        private GlavniRepository _gr;
       // private GlavniViewModel _gVM;
        private bool _prikaziStatistiku;
        //GlavniView.xaml
        //<ContentControl x:Name="odabraniContent" Content="{Binding OdabraniVM}" Grid.Row="1" Grid.Column="1" Grid.RowSpan="10"/>
        public object OdabraniVM
        {

            get { return _odabraniVM; }

            set
            {
                _odabraniVM = value;
                OnPropertyChanged("OdabraniVM");
            }

        }

        public bool PrikaziStatistiku
        {

            get { return _prikaziStatistiku; }

            set { _prikaziStatistiku = value; OnPropertyChanged("PrikaziStatistiku"); }

        }
        public object OdabraniVMOpBroj
        {

            get { return _odabraniVMOpBroj; }

            set { _odabraniVMOpBroj = value; OnPropertyChanged("OdabraniVMOpBroj"); }

        }

        public object OdabraniVMOrgView
        {

            get { return _odabraniVMOrgView; }

            set { _odabraniVMOrgView = value; OnPropertyChanged("OdabraniVMOrgView"); }

        }

        public object OdabraniVMImport
        {

            get { return _odabraniVMImport; }

            set { _odabraniVMImport = value; OnPropertyChanged("OdabraniVMImport"); }

        }

        public ApplicationViewModel()
        {
           
            _gr = new GlavniRepository();
          
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

           
            //SacuvajPologCommand = new RelayCommand(SacuvajPologPazara);
        }


        public GlavniRepository Gr
        {
            get { return _gr; }
            set { _gr = value; OnPropertyChanged("Gr"); }
        }

       
        public void OtvoriGlavni()

        {
            //OdabraniVM = new GlavniViewModel();

        }

        private void OtvoriOpBroj()

        {
            OdabraniVM = new OdaberiOpBrojViewModel(this);
        }

        public void OtvoriOrg()

        {
            //OdabraniVM = new OrgViewModel(this);
        }

        private void OtvoriStatistiku()

        {

            OdabraniVM = new StatistikaProdavaciViewModel(this);

        }

        private void OtvoriImport()
        {
           // OdabraniVM = new ImportViewModel(this);
        }

        private void OtvoriEopAna()
        {
            OdabraniVM = new EopAnaViewModel();
        }

        public void OtvoriGrOrg()
        {
            OdabraniVM = new GrOrgViewModel();
        }

        private void OtvoriGrKom()
        {
            OdabraniVM = new GrKomViewModel();
        }

        private void OtvoriRucniUnos()
        {
            OdabraniVM = new RucniUnosViewModel(this);
        }

        private void OtvoriPologPazara()
        {
            //OdabraniVM = new RUDinoPologPazaraViewModel(this);
        }

        private void SpasiIzmijenePazarUBazu()
        {
           

        }

        private void OtvoriZaduzenjeGot()
        {
           // OdabraniVM = new ZaduzenjeGotViewModel(this);
        }
        private void OtvoriIsplOsnovnih()
        {
            //OdabraniVM = new DinoIsplOsnovnihViewModel(this);
        }
        private void OtvoriUplOsnovnih()
        {
            OdabraniVM = new DinoUplOsnovnihViewModel(this);
        }
        private void OtvoriIsplSrecki()
        {
            OdabraniVM = new IsplataSreckiViewModel(this);
        }
        private void OtvoriUplSrecki()
        {
            OdabraniVM = new DinoUplSreckiViewModel(this);
        }

        private void OtvoriOpcinu()
        {
            //EOP_SIN eOP_SIN = new EOP_SIN();
            //OdabraniVM = new OpcineViewModel(this, eOP_SIN);
        }
    }
}
