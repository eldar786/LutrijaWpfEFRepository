using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
    public class OdaberiKomitentaViewModel : ObservableObject
    {
        private ObservableCollection<komitenti_ime_matbr_zracun> _sviKomitenti;

        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private List<komitenti_ime_matbr_zracun> _komitentiPretraga;
        private string _pretraga;
        private bool _omoguciDodavanje;
        private ApplicationViewModel _avm;
        private OrgViewModel _org;
        //private RucniUnosViewModel _ruc;
        private ZaduzenjeGotViewModel _zad;
        private PologPazaraViewModel _polPazara;
        private IsplOsnovnihViewModel _isplOsn;
        private IzmijeniIsplOsnovnihViewModel _izmIspOsn;
        private IzmijeniUplOsnovnihViewModel _izmUplOsn;
        private UplOsnovnihViewModel _uplOsn;
        private IsplataSreckiViewModel _isplSr;
        private UplsreckiViewModel _uplSr;
        private IzmijeniPazarDinoViewModel _izmPazDino;
        private IzmijeniZadGotovineViewModel _izmZadGot;
        private NewEditPologPazarViewModel _newEditPologPazar;
        private POLOG_PAZAR _pazar;
        private ISPLATA _isplOsnovnih;
        private POC_STANJA _stanje;
        private int _odaberiMetodu;
        private int _vrstaZad;
        private object _posiljaoc;
        private ISPLATA _isplSrecki;
        private EOP_SIN _uplSrecki;
        private EOP_SIN _uplOsnovnih;
        private ZADUZENJE_GOTOVINE _zadGotovine;
        private ZADUZENJE_GOTOVINE odabrani_komitentZaduzenja_komitentIsplata;
        
        private GlavniViewModel _gVM;
        private PRIJAVA_ORG _korisnik;
        private int odabrani_komitentZad_komIsplata;
        public ICommand OdaberiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }

        public OdaberiKomitentaViewModel(IzmijeniPazarDinoViewModel vm, POLOG_PAZAR pazar)
        {
            _pazar = pazar;
            _izmPazDino = vm;
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.RPP.GVM.AVM.Gr.Komitenti;
            _avm = vm.AVM;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiDino);
            this.OdustaniCommand = new RelayCommand(OdustaniDino);
        }

        public OdaberiKomitentaViewModel(IzmijeniPocStanjeViewModel vm, POC_STANJA stanje)
        {
            _stanje = stanje;
            // var OrgViewModelContext = new LutrijaEntities1();
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.AVM.Gr.Komitenti;
            _avm = vm.AVM;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiStanje);
            this.OdustaniCommand = new RelayCommand(OdustaniStanje);
        }

        public OdaberiKomitentaViewModel(GlavniViewModel vm, int i)
        {
          
            // var OrgViewModelContext = new LutrijaEntities1();
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.AVM.Gr.Komitenti;
            //_avm = vm.AVM;
            _gVM = vm;
            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();
            if (i==1)
            { 
            this.OdaberiCommand = new RelayCommand(OdaberiFilterIsplate);
              // this.OdustaniCommand = new RelayCommand(OdustaniFilter);
            }
            else if (i==2)
            {
                this.OdaberiCommand = new RelayCommand(OdaberiFilterUplate);
            }
            else if (i == 3)
            {
                this.OdaberiCommand = new RelayCommand(OdaberiFilterPazar);
            }
            //else if (i==3)
            //{
            //    this.OdaberiCommand = new RelayCommand(OdaberiFilterZaduzenja);
            //}
        }

        public OdaberiKomitentaViewModel(IzmijeniIsplOsnovnihViewModel vm, ISPLATA isplata)
        {
            _izmIspOsn = vm;
            _isplOsnovnih = isplata;
            //var OrgViewModelContext = new LutrijaEntities1();
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.DIOVM.GVM.AVM.Gr.Komitenti;
            //_avm = vm.AVM;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiIsplatuOsnovnih);
            this.OdustaniCommand = new RelayCommand(OdustaniIsplata);
        }

        public OdaberiKomitentaViewModel(IzmijeniZadGotovineViewModel vm, ZADUZENJE_GOTOVINE zADUZENJE_GOTOVINE)
        {
            
            _zadGotovine = zADUZENJE_GOTOVINE;           
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.AVM.Gr.Komitenti;
            _avm = vm.AVM;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiZaduzenje);
            this.OdustaniCommand = new RelayCommand(OdustaniDino);
        }

        public OdaberiKomitentaViewModel(IzmijeniIsplSreckiViewModel vm, ISPLATA isplata)
        {
            _isplSrecki = isplata;
            var OrgViewModelContext = new LutrijaEntities1();
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();

            _komitentiPretraga = vm.AVM.Gr.Komitenti;
            _avm = vm.AVM;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiDino);
            this.OdustaniCommand = new RelayCommand(OdustaniDino);
        }

        public OdaberiKomitentaViewModel(IzmijeniUplOsnovnihViewModel vm, EOP_SIN uplataO)
        {
            _uplOsnovnih = uplataO;
           _izmUplOsn = vm;
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.DUOVM.GVM.AVM.Gr.Komitenti;

         //   _avm = vm.AVM;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiDino);
            this.OdustaniCommand = new RelayCommand(OdustaniDino);
        }

        public OdaberiKomitentaViewModel(IzmijeniUplSreckiDinoViewModel vm, EOP_SIN uplataS)
        {
            _uplSrecki = uplataS;
            var OrgViewModelContext = new LutrijaEntities1();
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.AVM.Gr.Komitenti;
            _avm = vm.AVM;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiDino);
            this.OdustaniCommand = new RelayCommand(OdustaniDino);
        }


        public OdaberiKomitentaViewModel(IzmijeniZadGotovineViewModel vm, ZADUZENJE_GOTOVINE zADUZENJE_GOTOVINE, int _odabrani_komitentZaduzenja_komitentIsplata)
        {
            _izmZadGot = vm;
            _zadGotovine = zADUZENJE_GOTOVINE;
            _vrstaZad = _odabrani_komitentZaduzenja_komitentIsplata;
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            _komitentiPretraga = vm.ZGVM.GVM.AVM.Gr.Komitenti;
            
            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiZaduzenje);
            this.OdustaniCommand = new RelayCommand(OdustaniDino);
        }
        public OdaberiKomitentaViewModel(GlavniViewModel gVM, OrgViewModel org, PRIJAVA_ORG korisnik)
        {
            var OrgViewModelContext = new LutrijaEntities1();
            _gVM = gVM;
            _org = org;
            _korisnik = korisnik;
            //_ruc = new RucniUnosViewModel();
            //_zad = new ZaduzenjeGotViewModel();
            //_polPazara = new PologPazaraViewModel();
            //_isplOsn = new IsplOsnovnihViewModel();
            //_uplOsn = new UplOsnovnihViewModel();
            //_isplSr = new IsplataSreckiViewModel();
            //_uplSr = new UplsreckiViewModel();
           
            SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>();
            // _komitentiPretraga = gVM.AVM.Gr.KomitentiZaRegion(_korisnik);
            _komitentiPretraga = gVM.AVM.Gr.Komitenti;

            foreach (komitenti_ime_matbr_zracun k in _komitentiPretraga)
            {
                SviKomitenti.Add(k);
            }

            Sortiraj();

            this.OdaberiCommand = new RelayCommand(OdaberiOrg);
            this.OdustaniCommand = new RelayCommand(Odustani);
        }

        private void Sortiraj()
        {
            ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_sviKomitenti);
            komitentiView.SortDescriptions.Clear();
            komitentiView.SortDescriptions.Add(new SortDescription("IME", ListSortDirection.Ascending));
        }
        public void TraziKomitenta(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {
                SviKomitenti = new ObservableCollection<komitenti_ime_matbr_zracun>(from i in _sviKomitenti
                                                                                    where i.IME.IndexOf(_pretraga) >= 0 || i.KOMITENT.IndexOf(_pretraga) >= 0
                                                                                    select i);
            }
            else
            {
                SviKomitenti.Clear();

                if (_komitentiPretraga != null)
                {

                    foreach (komitenti_ime_matbr_zracun komitent in _komitentiPretraga)
                    {
                        SviKomitenti.Add(komitent);
                    }
                }
            }
            Sortiraj();
        }
        private void Odustani()
        {
            if (_odaberiMetodu == 1)
            {
                //OrgViewModel org = new OrgViewModel(_avm);
                //org.OdabraniKomitent = null;

                //_avm.OdabraniVM = org;
            }
            else if (_odaberiMetodu == 2)
            {
                //ZaduzenjeGotViewModel ruc = new ZaduzenjeGotViewModel(_gvm);
                //ruc.OdabraniKomitent = null;
                //_avm.OdabraniVM = ruc;
            }
            else if (_odaberiMetodu == 3)
            {
                PologPazaraViewModel polPazara = new PologPazaraViewModel(Mediator.Instance);
                polPazara.OdabraniKomitent = null;
                _avm.OdabraniVM = polPazara;
            }
            else if (_odaberiMetodu == 4)
            {
                IsplOsnovnihViewModel isplOsn = new IsplOsnovnihViewModel(_avm);
                isplOsn.OdabraniKomitent = null;
                _avm.OdabraniVM = isplOsn;
            }
            else if (_odaberiMetodu == 5)
            {
                UplOsnovnihViewModel uplOsn = new UplOsnovnihViewModel(_avm);
                uplOsn.OdabraniKomitent = null;
                _avm.OdabraniVM = uplOsn;
            }
            //else if (_odaberiMetodu == 6)
            //{
            //    IsplataSreckiViewModel isplSr = new IsplataSreckiViewModel(_avm);
            //    isplSr.OdabraniKomitent = null;
            //    _avm.OdabraniVM = isplSr;
            //}
            else if (_odaberiMetodu == 7)
            {
                UplsreckiViewModel uplSr = new UplsreckiViewModel(_avm);
                uplSr.OdabraniKomitent = null;
                _avm.OdabraniVM = uplSr;
            }
        }

        private void OdaberiOrg ()
        {
            //OrgViewModel izm = new OrgViewModel(_avm, _gVM);
            //izm.OdabraniKomitent = _odabraniKomitent;

            //_gVM.OdabraniVM = izm;

            _org.OdabraniKomitent = _odabraniKomitent;
            _gVM.OdabraniVM = _org;
              
        }
        //private void Odaberi()
        //{
        //    if (OdabraniKomitent != null)
        //    {
        //        OdabraniKomitent = _odabraniKomitent;

        //        OmoguciDodavanje = true;

        //        if (_odaberiMetodu == 1)
        //        {
        //            OrgViewModel org = new OrgViewModel(_avm);
        //            org.OdabraniKomitent = _odabraniKomitent;

        //            _avm.OdabraniVM = org;
        //        }
        //        else if (_odaberiMetodu == 2)
        //        {
        //            ZaduzenjeGotViewModel ruc = new ZaduzenjeGotViewModel(_avm);
        //            ruc.OdabraniKomitent = _odabraniKomitent;
        //            _avm.OdabraniVM = ruc;
        //        }
        //        else if (_odaberiMetodu == 3)
        //        {
        //            PologPazaraViewModel polPazara = new PologPazaraViewModel(Mediator.Instance);
        //            polPazara.OdabraniKomitent = _odabraniKomitent;
        //            _avm.OdabraniVM = polPazara;
        //        }
        //        else if (_odaberiMetodu == 4)
        //        {
        //            IsplOsnovnihViewModel isplOsn = new IsplOsnovnihViewModel(_avm);
        //            isplOsn.OdabraniKomitent = _odabraniKomitent;
        //            _avm.OdabraniVM = isplOsn;
        //        }
        //        else if (_odaberiMetodu == 5)
        //        {
        //            UplOsnovnihViewModel uplOsn = new UplOsnovnihViewModel(_avm);
        //            uplOsn.OdabraniKomitent = _odabraniKomitent;
        //            _avm.OdabraniVM = uplOsn;
        //        }
        //        else if (_odaberiMetodu == 6)
        //        {
        //            IsplataSreckiViewModel isplSr = new IsplataSreckiViewModel(_avm);
        //            isplSr.OdabraniKomitent = _odabraniKomitent;
        //            _avm.OdabraniVM = isplSr;
        //        }
        //        else if (_odaberiMetodu == 7)
        //        {
        //            UplsreckiViewModel uplSr = new UplsreckiViewModel(_avm);
        //            uplSr.OdabraniKomitent = _odabraniKomitent;
        //            _avm.OdabraniVM = uplSr;
        //        }
        //    }
        //    else
        //    {
        //        MessageBoxResult result = MessageBox.Show("Morate odabrati bar jednog komitenta", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}
    
        private void OdaberiFilterIsplate()
        {
            DinoIsplOsnovnihViewModel divm = new DinoIsplOsnovnihViewModel(this, OdabraniKomitent.KOMITENT);
           // divm.Pretraga = OdabraniKomitent.KOMITENT;
            _gVM.OdabraniVM = divm;
        }

        private void OdaberiFilterUplate()
        {
            if (OdabraniKomitent != null)
            { 
            DinoUplOsnovnihViewModel duovm = new DinoUplOsnovnihViewModel(this, OdabraniKomitent.KOMITENT);
            // divm.Pretraga = OdabraniKomitent.KOMITENT;
            _gVM.OdabraniVM = duovm;
            }
        }

        private void OdaberiFilterPazar()
        {
            if (OdabraniKomitent != null)
            {
                RUDinoPologPazaraViewModel rudppvm = new RUDinoPologPazaraViewModel(this, OdabraniKomitent.KOMITENT);
                // divm.Pretraga = OdabraniKomitent.KOMITENT;
                _gVM.OdabraniVM = rudppvm;
            }
        }
        private void OdaberiDino ()
        {
            if (_pazar != null)
            {

                // IzmijeniPazarDinoViewModel izm = new IzmijeniPazarDinoViewModel(_avm, _pazar);
                _izmPazDino.OdabraniKomitent = _odabraniKomitent;
                _pazar.OP_BROJ_PROD = OdabraniKomitent.KOMITENT;
                _izmPazDino.RPP.GVM.OdabraniVM = _izmPazDino;
            }

            if (_isplOsnovnih != null)
            {
                _izmIspOsn.OdabraniKomitent = _odabraniKomitent;
                _isplOsnovnih.LIS_ISPL = OdabraniKomitent.KOMITENT;
                _izmIspOsn.DIOVM.GVM.OdabraniVM = _izmIspOsn;
            }

            if (_isplSrecki != null)
            {
                IzmijeniIsplSreckiViewModel izmS = new IzmijeniIsplSreckiViewModel(_avm, _isplSrecki);
                izmS.OdabraniKomitent = _odabraniKomitent;
                _isplSrecki.LIS_ISPL = OdabraniKomitent.KOMITENT;
                _avm.OdabraniVM = izmS;
            }

            if (_uplOsnovnih != null)
            {
                _izmUplOsn.OdabraniKomitent = _odabraniKomitent;
                _uplOsnovnih.OP_BROJ = int.Parse(OdabraniKomitent.KOMITENT);
                _izmUplOsn.DUOVM.GVM.OdabraniVM = _izmUplOsn;            
            }

            if (_uplSrecki != null)
            {
                IzmijeniUplSreckiDinoViewModel izmS = new IzmijeniUplSreckiDinoViewModel(_avm, _uplSrecki);
                izmS.OdabraniKomitent = _odabraniKomitent;
                int komInt;
                Int32.TryParse(OdabraniKomitent.KOMITENT, out komInt);
                _uplSrecki.OP_BROJ = komInt;
                _avm.OdabraniVM = izmS;

            }

            if (_zadGotovine != null)
            {
                //kroz konstruktor reci dali je 1 ili 2
                //IzmijeniZadGotovineViewModel _izmZadGot = new IzmijeniZadGotovineViewModel(_avm, _zadGotovine);
                //_izmZadGot.OdabraniKomitent = _odabraniKomitent;

                IzmijeniZadGotovineViewModel izmZadGot = new IzmijeniZadGotovineViewModel(_avm, _zadGotovine, odabrani_komitentZad_komIsplata);
                izmZadGot.OdabraniKomitent = _odabraniKomitent;

                int.TryParse(OdabraniKomitent.KOMITENT, out int result);
                if (odabrani_komitentZad_komIsplata == 1)
                {
                    _zadGotovine.ZADUZITI_BLAGAJNIKA = result;
                }
                if (odabrani_komitentZad_komIsplata == 2)
                {
                    _zadGotovine.ODOBRITI_BLAGAJNIKA = result;
                }
                //_zadGotovine.KOMITENT_ZADUZENJA = result;
                //_zadGotovine.KOMITENT_ISPLATA = result;
                _avm.OdabraniVM = izmZadGot;
            }  

    }

        private void OdaberiZaduzenje()
        {

            if (_zadGotovine != null)
            {
                _izmZadGot.OdabraniKomitent = _odabraniKomitent;
                int.TryParse(OdabraniKomitent.KOMITENT, out int result);
                if (_vrstaZad == 1)
                {
                    _zadGotovine.ZADUZITI_BLAGAJNIKA = result;
                }
                if (_vrstaZad == 2)
                {
                    _zadGotovine.ODOBRITI_BLAGAJNIKA = result;
                }

                _izmZadGot.ZGVM.GVM.OdabraniVM = _izmZadGot;               
            }
        }

        private void OdaberiStanje()
        {
            IzmijeniPocStanjeViewModel izm = new IzmijeniPocStanjeViewModel(_avm, _stanje);
            izm.OdabraniKomitent = _odabraniKomitent;
            int OpBroj = int.Parse(_odabraniKomitent.KOMITENT);
            _stanje.OP_BROJ = OpBroj;
            //  izm.OmogucenoDugme = true;
            _avm.OdabraniVM = izm;
        }

        private void OdustaniStanje()
        {
            IzmijeniPocStanjeViewModel izm = new IzmijeniPocStanjeViewModel(_avm, _stanje);
            izm.OdabraniKomitent = null;

            _avm.OdabraniVM = izm;
        }

        private void OdaberiIsplatuOsnovnih()
        {
            _izmIspOsn.OdabraniKomitent = _odabraniKomitent;
            _isplOsnovnih.LIS_ISPL = OdabraniKomitent.KOMITENT;
            _izmIspOsn.DIOVM.GVM.OdabraniVM = _izmIspOsn;
            
        }

        private void OdustaniIsplata()
        {

            _avm.OdabraniVM = _posiljaoc;
        }
        private void OdustaniDino()
        {
            //OrgViewModel org = new OrgViewModel(_avm);
            //org.OdabraniKomitent = null;

            //_avm.OdabraniVM = org;

            _org.OdabraniKomitent = null;
            _avm.OdabraniVM = _org;


            if (_pazar != null)
            {
                _izmPazDino.OdabraniKomitent = _odabraniKomitent;
                _pazar.OP_BROJ_PROD = OdabraniKomitent.KOMITENT;
                _gVM.OdabraniVM = _izmPazDino;
            }

            if (_isplOsnovnih != null)
            {
                _izmIspOsn.OdabraniKomitent = _odabraniKomitent;
                _isplOsnovnih.LIS_ISPL = OdabraniKomitent.KOMITENT;
                _gVM.OdabraniVM = _izmIspOsn;
            }

            if (_isplSrecki != null)
            {
                IzmijeniIsplSreckiViewModel izmS = new IzmijeniIsplSreckiViewModel(_avm, _isplSrecki);
                izmS.OdabraniKomitent = null;
                _avm.OdabraniVM = izmS;
            }

            if (_uplOsnovnih != null)
            {
                _izmUplOsn.OdabraniKomitent = _odabraniKomitent;
                _uplOsnovnih.OP_BROJ = int.Parse(OdabraniKomitent.KOMITENT);
                _gVM.OdabraniVM = _izmUplOsn;
            }

            if (_uplSrecki != null)
            {
                IzmijeniUplSreckiDinoViewModel izmUS = new IzmijeniUplSreckiDinoViewModel(_avm, _uplSrecki);
                izmUS.OdabraniKomitent = null;

                _avm.OdabraniVM = izmUS;
            }

        }


        //    private List<komitenti_ime_matbr_zracun> NapuniKomitente()
        //{
        //    List<komitenti_ime_matbr_zracun> kom = new List<komitenti_ime_matbr_zracun>();
        //    using (var context = new LutrijaEntities1())
        //    {
        //        kom = context.komitenti_ime_matbr_zracun.ToList();
        //    }
        //    return kom;
        //}

        public ObservableCollection<komitenti_ime_matbr_zracun> SviKomitenti { get => _sviKomitenti; set { _sviKomitenti = value; OnPropertyChanged("SviKomitenti"); } }

        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }

        public GlavniViewModel GVM { get => _gVM; set { _gVM = value; OnPropertyChanged("GVM"); } }
        public string Pretraga { get => _pretraga; set { _pretraga = value; TraziKomitenta(_pretraga); } }
        public bool OmoguciDodavanje { get => _omoguciDodavanje; set { _omoguciDodavanje = value; OnPropertyChanged("OmoguciDodavanje"); } }
    }
}