using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
    public class IzmijeniZadGotovineViewModel : ObservableObject
    {
        private ApplicationViewModel _av;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private List<ZADUZENJE_GOTOVINE> zadGotList;
        ZADUZENJE_GOTOVINE _odabranoZadGot;
        ApplicationViewModel _avm;
        private bool _omogucenoDugme = true;
        private ZADUZENJE_GOTOVINE odabranoZadGotovine;
        private ZADUZENJE_GOTOVINE odabranoZadGotovineZaKomIsplata;
        private ZaduzenjeGotViewModel _zgvm;
        private Poruke por = new Poruke();
        public int odabrani_komitentZaduzenja_komitentIsplata;

        public ICommand KomitentiCommand { get; set; }
        public ICommand KomitentiCommand_komIsplata { get; set; }
        public ICommand SpasiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }


        public IzmijeniZadGotovineViewModel(ZaduzenjeGotViewModel zgvm, ZADUZENJE_GOTOVINE zadGotovine)
        {
            _zgvm = zgvm;
            odabranoZadGotovine = zadGotovine;
            odabranoZadGotovineZaKomIsplata = zadGotovine;

            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.KomitentiCommand_komIsplata = new RelayCommand(Komitenti_komIsplata);
            this.SpasiCommand = new RelayCommand(async () => await Spasi());
            this.OdustaniCommand = new RelayCommand(Odustani);

            //odabranoZadGotovine.DATUM = DateTime.Now;
        }

        public IzmijeniZadGotovineViewModel(ApplicationViewModel avm, ZADUZENJE_GOTOVINE zadGotovine, int _odabrani_komitentZaduzenja_komitentIsplata)
        {
            _avm = avm;
            odabranoZadGotovine = zadGotovine;
            odabranoZadGotovineZaKomIsplata = zadGotovine;

            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.KomitentiCommand_komIsplata = new RelayCommand(Komitenti_komIsplata);
            this.SpasiCommand = new RelayCommand(async () => await Spasi());
            this.OdustaniCommand = new RelayCommand(Odustani);
        }

        public IzmijeniZadGotovineViewModel(ApplicationViewModel avm)
        {
            var zadGotContext = new LutrijaEntities1();
            _av = avm;
            _odabraniKomitent = new komitenti_ime_matbr_zracun();
            _odabraniKomitent.IME = "Odaberi Komitenta";

            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.KomitentiCommand_komIsplata = new RelayCommand(Komitenti_komIsplata);
        }

        private void Odustani()
        {
            _zgvm.GVM.OdabraniVM = _zgvm;
        }

        private void Komitenti()
        {
            odabrani_komitentZaduzenja_komitentIsplata = 1;
            if (_zgvm.GVM.OdabraniVM == this)
            {
                //_avm.OdabraniVM = new OdaberiKomitentaViewModel(this, odabranoZadGotovine);
                _zgvm.GVM.OdabraniVM = new OdaberiKomitentaViewModel(this, odabranoZadGotovine, odabrani_komitentZaduzenja_komitentIsplata);
            }
        }

        private void Komitenti_komIsplata()
        {
            odabrani_komitentZaduzenja_komitentIsplata = 2;

            if (_zgvm.GVM.OdabraniVM == this)
            {
                _zgvm.GVM.OdabraniVM = new OdaberiKomitentaViewModel(this, odabranoZadGotovineZaKomIsplata, odabrani_komitentZaduzenja_komitentIsplata);
            }
        }

        private async Task Spasi()
        {
            if (odabranoZadGotovine != null)
            {
                ZaduzenjeGotovineRepository zaduzenjeGotovineRepository = new ZaduzenjeGotovineRepository(odabranoZadGotovine, _odabraniKomitent);
                zaduzenjeGotovineRepository.DodajZaduzenjeGotovine();
                await Task.Run(() => this.ZGVM.GVM.AVM.Gr.IzmijeniPocStanje(odabranoZadGotovine.ODOBRITI_BLAGAJNIKA.ToString(), odabranoZadGotovine.DATUM));
                await Task.Run(() => this.ZGVM.GVM.AVM.Gr.IzmijeniPocStanje(odabranoZadGotovine.ZADUZITI_BLAGAJNIKA.ToString(), odabranoZadGotovine.DATUM));
                this.ZGVM.GVM.AVM.Gr.NapuniZaduzenja();
                por.Uspjeh();
            }
            
        }


        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }
        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public bool OmogucenoDugme { get => _omogucenoDugme; set { _omogucenoDugme = value; OnPropertyChanged("OmogucenoDugme"); } }
        public ZADUZENJE_GOTOVINE OdabranoZadGotovine { get => odabranoZadGotovine; set { odabranoZadGotovine = value; OnPropertyChanged("OdabranoZadGotovine"); } }

        public ZaduzenjeGotViewModel ZGVM { get => _zgvm; set { _zgvm = value; OnPropertyChanged("ZGVM"); } }
        public ZADUZENJE_GOTOVINE OdabranoZadGotovineZaKomIsplata { get => odabranoZadGotovineZaKomIsplata; set { odabranoZadGotovineZaKomIsplata = value; OnPropertyChanged("OdabranoZadGotovineZaKomIsplata"); } }



    }
}
