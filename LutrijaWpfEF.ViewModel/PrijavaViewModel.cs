using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace LutrijaWpfEF.ViewModel
{

    public class PrijavaViewModel : ObservableObject
    {
        private string _username;
        private string _password;
        private ApplicationViewModel _avm;
        private LoadingWindowViewModel _LWVM;
        private string _prijavaStatus;
        public ICommand PrijavaCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }
        private object _odabraniVMW;
        private bool _pb;
        private int _procenatZavrsen;
        private ProgressReportModel _report;
        private string _ispisProgresa;
        public PrijavaViewModel()    
        {
            _username = Username;
            _password = Password;
            _avm= new ApplicationViewModel();
            _odabraniVMW = this;
            _prijavaStatus = "";
            this.PrijavaCommand = new RelayCommand(async () => await Prijava());
            this.OdustaniCommand = new RelayCommand(Odustani);
        }

        public async Task Prijava()
        {

            PrijavaStatus = "Prijava u toku...";

            var korisnik = _avm.Gr.Prijava(_username, _password);

            if (korisnik != null)
            {
                if (korisnik.VRSTA == 10)
                {
                    PB = true;
                    await NapuniSve(korisnik);
                    PB = false;
                    OdabraniVMW = new GlavniViewModel(_avm, korisnik);
                }
                else
                { 
                PB = true;
                await NapuniZaPdf(korisnik);
                PB = false;
                OdabraniVMW = new GlavniViewModel(_avm, korisnik);
                }
            }
            else
            {
                PrijavaStatus = "Greška: provjerite username ili password.";
            }

        }

        public async Task NapuniZaPdf(PRIJAVA_ORG kor)
        {
            _report = new ProgressReportModel();

            await Task.Run(() => _avm.Gr.NapuniUplateOI());
            _report.Linija = "Napunio tabelu uplata osnovnih igara";
            _report.procenatZavrsen = 40;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniIgre());
            _report.Linija = "Napunio tabelu osnovne igre";
            _report.procenatZavrsen = 50;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniKladionicaIgre());
            _report.Linija = "Napunio tabelu igara kladionice";
            _report.procenatZavrsen = 60;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniOpcine());
            _report.Linija = "Napunio tabelu opcina";
            _report.procenatZavrsen = 70;
            UpdateValueInProgressBar(_report);

            List<komitenti_ime_matbr_zracun> kom = await Task.Run(() => _avm.Gr.KomitentiZaRegion2(kor));
            _report.Linija = "Napunio tabelu komitenata";
            _report.procenatZavrsen = 100;
            UpdateValueInProgressBar(_report);
        }
        public async Task NapuniSve(PRIJAVA_ORG kor)
        {
            _report = new ProgressReportModel();

            await Task.Run(() => _avm.Gr.NapuniUplateOI());
            _report.Linija = "Napunio tabelu uplata osnovnih igara";
            _report.procenatZavrsen = 10;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniIgre());
            _report.Linija = "Napunio tabelu osnovne igre";
            _report.procenatZavrsen = 20;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniKladionicaIgre());
            _report.Linija = "Napunio tabelu igara kladionice";
            _report.procenatZavrsen = 30;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniOpcine());
            _report.Linija = "Napunio tabelu opcina";
            _report.procenatZavrsen = 40;
            UpdateValueInProgressBar(_report);

                await Task.Run(() => _avm.Gr.NapuniKomitente());
                _report.Linija = "Napunio tabelu komitenata";
                _report.procenatZavrsen = 50;
                UpdateValueInProgressBar(_report);

                await Task.Run(() => _avm.Gr.NapuniIsplateOI());
                _report.Linija = "Napunio tabelu isplata osnovnih igara";
                _report.procenatZavrsen = 60;
                UpdateValueInProgressBar(_report);

                await Task.Run(() => _avm.Gr.NapuniKladionicu());
                _report.Linija = "Napunio tabelu uplata i isplata kladionice";
                _report.procenatZavrsen = 70;
                UpdateValueInProgressBar(_report);

                await Task.Run(() => _avm.Gr.NapuniAutomate());
                _report.Linija = "Napunio tabelu uplata isplata automata";
                _report.procenatZavrsen = 80;
                UpdateValueInProgressBar(_report);

                await Task.Run(() => _avm.Gr.NapuniPazar());
                _report.Linija = "Napunio tabelu pologa pazara";
                _report.procenatZavrsen = 90;
                UpdateValueInProgressBar(_report);

                await Task.Run(() => _avm.Gr.NapuniZaduzenja());
                _report.Linija = "Napunio tabelu rucnih zaduzenja";
                _report.procenatZavrsen = 100;
                UpdateValueInProgressBar(_report);
        }
        public void UpdateValueInProgressBar(ProgressReportModel report)
        {
            ProcenatZavrsen = report.procenatZavrsen;
            IspisProgresa = report.Linija;
        }

        public void Odustani()
        {
            Environment.Exit(0);
        }

        public void OcistiPasswordPolje()
        {
            Password = "";
        }


        public void OcistiPorukuStatusa()
        {
            PrijavaStatus = "";
        }
        public int ProcenatZavrsen
        {
            get { return _procenatZavrsen; }
            set
            {
                _procenatZavrsen = value;
                OnPropertyChanged("ProcenatZavrsen");
            }
        }

        public string IspisProgresa
        {
            get { return _ispisProgresa; }
            set
            {
                _ispisProgresa = value;
                OnPropertyChanged("IspisProgresa");
            }
        }

        public bool PB
        {
            get { return _pb; }
            set
            {
                _pb = value;
                OnPropertyChanged("PB");
            }
        }
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string PrijavaStatus
        {
            get { return _prijavaStatus; }
            set
            {
                _prijavaStatus = value;
                OnPropertyChanged("PrijavaStatus");
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


    }
}
