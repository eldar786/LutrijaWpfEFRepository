using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.ViewModel
{
    public class LoadingWindowViewModel : ObservableObject
    {
        private ApplicationViewModel _avm;
        private int _procenatZavrsen;
        private ProgressReportModel _report;
        private string _ispisProgresa;
        private object _odabraniVMW;
        private object _dataContext;
        private LoadingWindowViewModel _LWVM;
        private LoadingWindowViewModel()
        {
            _odabraniVMW = this;
            _report = new ProgressReportModel();
            _avm = new ApplicationViewModel();
        }

       
        public static async Task<LoadingWindowViewModel> Create()
        {
            var l = new LoadingWindowViewModel();
            await l.NapuniSve();
            l.Login();
            return new LoadingWindowViewModel();  
        }

        public  async Task NapuniSve()
        {
            await Task.Run(() => _avm.Gr.NapuniKomitente());
            _report.Linija = "Napunio tabelu komitenata";
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

            await Task.Run(() => _avm.Gr.NapuniUplateOI());
            _report.Linija = "Napunio tabelu uplata osnovnih igara";
            _report.procenatZavrsen = 50;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniKladionicu());
            _report.Linija = "Napunio tabelu uplata i isplata kladionice";
            _report.procenatZavrsen = 60;
            UpdateValueInProgressBar(_report);

            await Task.Run(() => _avm.Gr.NapuniIsplateOI());
            _report.Linija = "Napunio tabelu isplata osnovnih igara";
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
        public void UpdateValueInProgressBar (ProgressReportModel report)
        {
            ProcenatZavrsen = report.procenatZavrsen;
            IspisProgresa = report.Linija; 
        }

        public void Login()
        {
            if (_report.procenatZavrsen == 100)
            {
                OdabraniVMW = new PrijavaViewModel();
            }
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

        public object OdabraniVMW
        {
            get { return _odabraniVMW; }
            set
            {
                _odabraniVMW = value;
                OnPropertyChanged("OdabraniVMW");
            }
        }

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                _dataContext = value;
                OnPropertyChanged("DataContext");
            }
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

    }
}
