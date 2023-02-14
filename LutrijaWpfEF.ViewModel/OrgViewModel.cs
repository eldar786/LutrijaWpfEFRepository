using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
    public class OrgViewModel : ObservableObject
    {
        private List<OPCINE> _opcine;
        private ApplicationViewModel _avm;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private OdaberiKomitentaViewModel _oKVM;
        private UplataIsplataSkupa _upIsp;
        private List<ISPLATA> _isplateOsnovneIgre;
        private List<ISPLATA> _poreziZaOrgOsnovneIgre;
        private List<EOP_SIN> _uplateOsnovneIgre;
        private List<KLADIONICA> _uplateZaOrgKladionica;
        private List<KLADIONICA> _listiciKladionica;
        private List<ImpKlad2> _importKlad2;
        private List<KLADIONICA> _isplateZaOrgKladionica;
        private List<KLADIONICA> _poreziZaOrgKladionica;
        private List<ISPLATA> _isplateZaOrg;
        private List<ISPLATA> _isplateZaOrgSrecke;
        private List<POLOG_PAZAR> _pazarZaOrg;
        private List<POLOG_PAZAR> _pazar;
        private List<IGRE> _igre;
        private List<EOP_SIN> _uplateZaOrgOsnovneIgre;
        private List<EOP_SIN> _uplateZaOrgSrecke;
        private List<EOP_SIN> _uplateIsplateZaOrgWeb;
        private decimal? _isplataOsnovneIgreSrecke;
        private decimal? _uplataOsnovneIgre;
        private decimal? _uplataSreckeSve;
        private decimal? _ukupnaUplata;
        private decimal? _ukupnaIsplata;
        private decimal? _porezOsnovneIgre;
        private decimal? _porez;
        private decimal? _porezKladionica;
        private decimal? _isplataKladionica;
        private decimal? _uplataKladionica;
        private decimal? _saldo;
        private decimal? _pologPazara;
        private decimal? _webIsplata;
        private decimal? _nazivIgre;
        private decimal? _iznosIgre;
        private decimal? _pocetnoStanje;
        private decimal? _pocetnoMinus;
        private decimal? _pocetnoPlus;
        private decimal? _automatiUkupno;
        private decimal? _uplataKladOsnIg;
        private decimal? _ostalaZaduzenja;
        private DateTime _od = DateTime.Now.AddDays(-30);
        private DateTime _do = DateTime.Now;
        private Org _org;
        private decimal? _isplataSuperloto;
        private List<ISPLATA> _isplataPoIgri;
        private List<ISPLATA> _isplataPoSrecki;
        private List<ZADUZENJE_GOTOVINE> _zaduzenja;
        private List<ZADUZENJE_GOTOVINE> _zaduzenjaZaOrg;
        private List<KLADIONICA> _kladionicaUplataIsplata;
        private List<ImpKlad2> _importKlad2UplataIsplata;
        private List<komitenti_ime_matbr_zracun> _komitentiZaOrg;
        private List<EOP_SIN> _uplataPoIgri;
        private List<EOP_SIN> _uplataPoIgriSrecke;
        private List<UplataIsplataSkupa> _uplateIsplateSkupa;
        private List<UplataIsplataSkupa> _uplateIsplateSkupa2;
        private List<UplataIsplataSkupa> _uplateIsplateSkupaSrecke; //<------------

        private List<UplataIsplataSkupa> _uplateIsplateSkupaSrecke2;
        
        private List<UplataIsplataSkupa> _uplateIsplateSkupaKladionica;

        public static List<UplataIsplataSkupa> _uplateIsplateSkupaKladionica2;

        private List<POC_STANJA> _pocetnaStanja;
        private List<KLADIONICA_IGRE> _kladionicaIgre;
        private List<AUTOMATI_SABINA> _automati;
        private List<AUTOMATI_SABINA> _automatiZaOrg;
        private GlavniViewModel _gVM;
        private PRIJAVA_ORG _korisnik;
        private Poruke _poruke;
        string putanja = @"\\192.168.1.213\Users\Share Import\OrgTxtPdf\";
        public ICommand KomitentiCommand { get; set; }
        public ICommand KreirajOrgCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }
        private bool _pdfDugme = false;

        public OrgViewModel(ApplicationViewModel avm, GlavniViewModel gVM, PRIJAVA_ORG korisnik)
        {
            
            _poruke = new Poruke();
            _gVM = gVM;
           // _gVM.AVM.Gr = new GlavniRepository();
            _avm = avm;
            _korisnik = korisnik;
            if (_korisnik.VRSTA != 10)
            {
                PDFDugme = true;
            }
            
            _odabraniKomitent = new komitenti_ime_matbr_zracun();
            _odabraniKomitent.IME = "Odaberi Komitenta";

            this.KomitentiCommand = new RelayCommand(Komitenti);
            this.KreirajOrgCommand = new RelayCommand(KreirajOrg);
            this.OdustaniCommand = new RelayCommand(Odustani);
        }

        public OrgViewModel(ApplicationViewModel avm)
        {
            _avm = avm;
        }

        

        public OrgViewModel()
        {
           
        }
        //List<UplataIsplataSkupa> uplataIsplataSkupas = new List<UplataIsplataSkupa>();
        //uplataIsplataSkupas.Add();
        

        public void Odustani()
        {
            _gVM.OdabraniVM = new OrgViewModel(_avm, _gVM, _korisnik);
        }

        private void Komitenti()
        {

            if (_gVM.OdabraniVM == this)
            {

                _gVM.OdabraniVM = new OdaberiKomitentaViewModel(_gVM, this, _korisnik);

            }
        }

        public void KreirajOrgZaKomitente(List<komitenti_ime_matbr_zracun> komitentiZaOrg)
        {
            _komitentiZaOrg = komitentiZaOrg;

            foreach (komitenti_ime_matbr_zracun kom in _komitentiZaOrg)
            {
                int op = int.Parse(kom.KOMITENT);
                _uplateOsnovneIgre = this._avm.Gr.UplateOsnovneIgre.Where(x => op == x.OP_BROJ).ToList();
                _isplateOsnovneIgre = this._avm.Gr.NapuniIsplateZaKomitenta(kom);
                _listiciKladionica = this._avm.Gr.NapuniKladionicuZaKomitenta(kom);           
                _igre = this._avm.Gr.Igre;
                _pazar = this._avm.Gr.NapuniPazarZaKomitenta(kom);
                _kladionicaIgre = this._avm.Gr.KladionicaIgre;
                _automati = this._avm.Gr.NapuniAutomateZaKomitenta(kom);          
                _zaduzenja = this._avm.Gr.NapuniZadZaKomitenta(kom);               
                _uplateIsplateSkupaKladionica = new List<UplataIsplataSkupa>();
                _uplateIsplateSkupaSrecke = new List<UplataIsplataSkupa>();
                _uplateIsplateSkupa = new List<UplataIsplataSkupa>();
                _isplateZaOrg = new List<ISPLATA>();
                _isplateZaOrgSrecke = new List<ISPLATA>();
                _uplateZaOrgOsnovneIgre = new List<EOP_SIN>();
                _uplateZaOrgSrecke = new List<EOP_SIN>();
                _uplateZaOrgKladionica = new List<KLADIONICA>();
                _isplateZaOrgKladionica = new List<KLADIONICA>();
                _poreziZaOrgKladionica = new List<KLADIONICA>();
                _poreziZaOrgOsnovneIgre = new List<ISPLATA>();
                _pazarZaOrg = new List<POLOG_PAZAR>();
                _uplateIsplateZaOrgWeb = new List<EOP_SIN>();
                _isplataPoIgri = new List<ISPLATA>();
                _uplataPoIgri = new List<EOP_SIN>();
                _uplataPoIgriSrecke = new List<EOP_SIN>();
                _isplataPoSrecki = new List<ISPLATA>();
                _automatiZaOrg = new List<AUTOMATI_SABINA>();
                _kladionicaUplataIsplata = new List<KLADIONICA>();
                _importKlad2UplataIsplata = new List<ImpKlad2>();
                _zaduzenjaZaOrg = new List<ZADUZENJE_GOTOVINE>();

                _org = new Org();

                OdabraniKomitent = kom;

                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month, 1);
                var first = month.AddMonths(-1);
                var last = month.AddDays(-1);

                Od = first;
                Do = last;
                // Uplata Isplata Kladionica
                int mj = Od.Month - 1;

                decimal? pocStanje = this._avm.Gr.PocZaOpBroj(OdabraniKomitent.KOMITENT, mj);


               

                

                if (pocStanje != null)
                {
                    PocetnoStanje = pocStanje;
                    if (PocetnoStanje >= 0)
                    {
                        PocetnoPlus = PocetnoStanje;
                        PocetnoMinus = 0;
                    }
                    else
                    {
                        PocetnoMinus = PocetnoStanje;
                        PocetnoPlus = 0;
                    }
                }
                else
                {
                    PocetnoMinus = 0;
                    PocetnoPlus = 0;
                }


                foreach (ZADUZENJE_GOTOVINE i in _zaduzenja.Where(ios => ios.ZADUZITI_BLAGAJNIKA.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do))
                {
                    _zaduzenjaZaOrg.Add(i);
                }


                OstalaZaduzenja = (from i in _zaduzenjaZaOrg
                                   select i.IZNOS).Sum();

                foreach (KLADIONICA k in _listiciKladionica.Where(klad => klad.OP_BR_PROD.PadLeft(5, '0') == OdabraniKomitent.KOMITENT
                && klad.DATUM_IMPORTA >= Od && klad.DATUM_IMPORTA <= Do))
                {
                    string broj = k.OP_BR_PROD;
                    _kladionicaUplataIsplata.Add(k);
                }

                KladionicaUplataIsplata = _kladionicaUplataIsplata;

                //foreach (ImpKlad2 i in _importKlad2)
                //{
                //    _importKlad2UplataIsplata.Add(i);
                //}


                //ImpKlad2UplataIsplata = _importKlad2UplataIsplata; 


                // Isplata Osnovne Igre
                foreach (ISPLATA i in _isplateOsnovneIgre.Where(ios => ios.LIS_ISPL == OdabraniKomitent.KOMITENT && ios.LIS_VRISPL >= Od && ios.LIS_VRISPL <= Do.AddDays(1) && ios.LIS_TIP_SRE == 0))
                {
                    _isplateZaOrg.Add(i);
                }

                //IsplataSrecke
                foreach (ISPLATA i in _isplateOsnovneIgre.Where(ios => ios.LIS_ISPL == OdabraniKomitent.KOMITENT && ios.LIS_VRISPL >= Od && ios.LIS_VRISPL <= Do.AddDays(1) && ios.LIS_TIP_SRE != 0))
                {
                    _isplateZaOrgSrecke.Add(i);
                }

                decimal? ispOsn = (from i in _isplateZaOrg
                                   select i.LIS_IZNOS).Sum();

                decimal? ispSre = (from i in _isplateZaOrgSrecke
                                   select i.LIS_IZNOS).Sum();


                _org.IsplataOsnovneIgre = ispOsn + ispSre;
                IsplataOsnovneIgreSrecke = _org.IsplataOsnovneIgre;

                // Isplata po Igri
                IsplataPoIgri = _isplateZaOrg
                                        .GroupBy(l => l.SIF_IGRE)
                                        .Select(cl => new ISPLATA
                                        {
                                            SIF_IGRE = cl.First().SIF_IGRE,

                                            LIS_IZNOS = cl.Sum(c => c.LIS_IZNOS),
                                        }).ToList();


                IsplataPoSrecki = _isplateZaOrgSrecke
                                       .GroupBy(l => l.LIS_TIP_SRE)
                                       .Select(cl => new ISPLATA
                                       {
                                           LIS_TIP_SRE = cl.First().LIS_TIP_SRE,

                                           LIS_IZNOS = cl.Sum(c => c.LIS_IZNOS),
                                       }).ToList();




                foreach (ISPLATA isig in IsplataPoIgri)
                {

                    Org.NazivIgre = isig.LIS_IGRA;

                    Org.IsplataIgra = isig.LIS_IZNOS;
                }

                foreach (ISPLATA i in IsplataPoSrecki)
                {
                    i.LIS_NAZ_SRE = (from ig in _igre
                                     where i.LIS_TIP_SRE == ig.SIF_ISP
                                     select ig.NAZIV).First();
                }



                //Uplata Osnovne Igre (Kolo pretplate se racuna na datum uplate ne za uplaceno kolo)
                foreach (EOP_SIN es in _uplateOsnovneIgre.Where(ios => ios.OP_BROJ.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do && ios.SRECKA == 0 && ios.IGRA != 30))
                {
                    _uplateZaOrgOsnovneIgre.Add(es);
                }

                _org.UplataOsnovneIgre = (from i in _uplateZaOrgOsnovneIgre
                                          select i.UPL).Sum();


                foreach (EOP_SIN es in _uplateOsnovneIgre.Where(ios => ios.OP_BROJ.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do && ios.SRECKA == 0 && ios.IGRA == 30))
                {
                    _uplateIsplateZaOrgWeb.Add(es);
                }

                _org.WebIsplata = (from i in _uplateIsplateZaOrgWeb
                                   select i.UPL).Sum();

                WebIsplata = _org.WebIsplata;
                UplataOsnovneIgre = _org.UplataOsnovneIgre;
                //Uplata Srecke
                foreach (EOP_SIN esi in _uplateOsnovneIgre.Where(ios => ios.OP_BROJ.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do && ios.SRECKA != 0))
                {
                    _uplateZaOrgSrecke.Add(esi);
                }

                _org.UplataSrecke = (from i in _uplateZaOrgSrecke
                                     select i.UPL).Sum();
                UplataSreckeSve = _org.UplataSrecke;

                //SRECKE NEMAJU POREZ MORA SE RUCNO IZRACUNATI IZ FILEA ZA SVAKU SRECKU DOBITKA PREKO 100 KM 10%

                foreach (IGRE ig in _igre)
                {
                    if (ig.SIF_BAZ_IGR == 1 && ig.ID != 43)
                    {
                        UpIsp = new UplataIsplataSkupa();
                        UpIsp.sifraIgreUpl = ig.SIF_UPL;
                        UpIsp.sifraIgreIspl = ig.SIF_ISP;
                        UpIsp.NazivIgre = ig.NAZIV;

                        _uplateIsplateSkupa.Add(UpIsp);
                    }
                    else if (ig.SIF_BAZ_IGR == 4)
                    {
                        UpIsp = new UplataIsplataSkupa();
                        UpIsp.sifraIgreUpl = ig.SIF_UPL;
                        UpIsp.sifraIgreIspl = ig.SIF_ISP;
                        UpIsp.NazivIgre = ig.NAZIV;

                        _uplateIsplateSkupaSrecke.Add(UpIsp);
                    }
                    else if (ig.SIF_BAZ_IGR == 2)
                    {
                        UpIsp = new UplataIsplataSkupa();

                        if (ig.ID == 38)
                        {
                            UpIsp.sifraIgreKlad = 1;
                            UpIsp.NazivIgre = ig.NAZIV;
                        }
                        else if (ig.ID == 39)
                        {
                            UpIsp.sifraIgreKlad = 2;
                            UpIsp.NazivIgre = ig.NAZIV;
                        }
                        else if (ig.ID == 40)
                        {
                            UpIsp.sifraIgreKlad = 3;
                            UpIsp.NazivIgre = ig.NAZIV;
                        }
                        else if (ig.ID == 41)
                        {
                            UpIsp.sifraIgreKlad = 4;
                            UpIsp.NazivIgre = ig.NAZIV;
                        }
                        else if (ig.ID == 42)
                        {
                            UpIsp.sifraIgreKlad = 5;
                            UpIsp.NazivIgre = ig.NAZIV;
                        }

                        _uplateIsplateSkupaKladionica.Add(UpIsp);

                        _uplateIsplateSkupaKladionica2.Add(UpIsp);



                    }

                }
                

                UplataPoIgriSrecke = _uplateZaOrgSrecke
                                       .GroupBy(l => l.IGRA)
                                       .Select(cl => new EOP_SIN
                                       {
                                           IGRA = cl.First().IGRA,

                                           UPL = cl.Sum(c => c.UPL),
                                       }).ToList();

                //Uplata po igri
                UplataPoIgri = _uplateZaOrgOsnovneIgre
                                       .GroupBy(l => l.IGRA)
                                       .Select(cl => new EOP_SIN
                                       {
                                           IGRA = cl.First().IGRA,

                                           UPL = cl.Sum(c => c.UPL),
                                       }).ToList();

                var KladionicaPoIgri = _kladionicaUplataIsplata
                                      .GroupBy(l => l.KLAD_IGRA)
                                      .Select(cl => new KLADIONICA
                                      {
                                          KLAD_IGRA = cl.First().KLAD_IGRA,

                                          UPLATA = cl.Sum(c => c.UPLATA),
                                          ISPLATA = cl.Sum(c => c.ISPLATA),
                                          PLAC_POREZ = cl.Sum(c => c.PLAC_POREZ)
                                      }).ToList();

                foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaKladionica)
                {
                    ui.IznosIsplate = (from k in KladionicaPoIgri
                                       where k.KLAD_IGRA == ui.sifraIgreKlad
                                       select k.ISPLATA).FirstOrDefault();
                }

                foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaKladionica)
                {
                    ui.IznosUplate = (from k in KladionicaPoIgri
                                      where k.KLAD_IGRA == ui.sifraIgreKlad
                                      select k.UPLATA).FirstOrDefault();
                }

                foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaKladionica)
                {
                    ui.IznosPoreza = (from k in KladionicaPoIgri
                                      where k.KLAD_IGRA == ui.sifraIgreKlad
                                      select k.PLAC_POREZ).FirstOrDefault();
                }

                decimal? proba = (from bla in IsplataPoIgri
                                  select bla.LIS_IZNOS).Sum();
                //SKUPA UPLATA ISPLATA
                foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaSrecke)
                {
                    ui.IznosUplate = (from u in UplataPoIgriSrecke
                                      where ui.sifraIgreUpl == u.IGRA
                                      select u.UPL).FirstOrDefault();
                }

                foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaSrecke)
                {
                    ui.IznosIsplate = (from u in IsplataPoSrecki
                                       where ui.sifraIgreIspl == u.LIS_TIP_SRE
                                       select u.LIS_IZNOS).FirstOrDefault();
                }

                foreach (UplataIsplataSkupa ui in _uplateIsplateSkupa)
                {
                    ui.IznosUplate = (from u in UplataPoIgri
                                      where ui.sifraIgreUpl == u.IGRA
                                      select u.UPL).FirstOrDefault();
                }

                foreach (UplataIsplataSkupa ui in _uplateIsplateSkupa)
                {
                    ui.IznosIsplate = (from u in IsplataPoIgri
                                       where ui.sifraIgreIspl == u.SIF_IGRE
                                       select u.LIS_IZNOS).FirstOrDefault();
                }

                proba = (from bla in _uplateIsplateSkupa
                         select bla.IznosIsplate).Sum();
                decimal? proba2 = (from bla in _uplateIsplateSkupaSrecke
                                   select bla.IznosIsplate).Sum();
                decimal? proba3 = proba + proba2;


                //Porez Osnovne Igre i Srecke
                foreach (ISPLATA i in _isplateOsnovneIgre.Where(ios => ios.LIS_ISPL == OdabraniKomitent.KOMITENT && ios.LIS_VRISPL >= Od && ios.LIS_VRISPL <= Do && ios.LIS_POREZ != 0))
                {
                    _poreziZaOrgOsnovneIgre.Add(i);
                }

                PorezOsnovneIgre = (from i in _poreziZaOrgOsnovneIgre
                                    select i.LIS_POREZ).Sum();


                //Kladionica isplata(Datum treba u tabeli kladionica, nadat se da ce bit kad stigne novi program)
                foreach (KLADIONICA k in _listiciKladionica.Where(ios => ios.OP_BR_PROD == OdabraniKomitent.KOMITENT
                && ios.DATUM_IMPORTA >= Od && ios.DATUM_IMPORTA <= Do))
                {
                    _isplateZaOrgKladionica.Add(k);
                }

                decimal? kladIsp = (from i in _isplateZaOrgKladionica
                                    select i.ISPLATA).Sum();

                decimal? kladPorez = (from i in _isplateZaOrgKladionica
                                      select i.PLAC_POREZ).Sum();


                //Ukoliko je bio rucni unos 
                IsplataKladionica = kladIsp + kladPorez;

                //Kladionica uplata(Datum treba u tabeli kladionica, nadat se da ce bit kad stigne novi program)
                foreach (KLADIONICA k in _listiciKladionica.Where(ios => ios.OP_BR_PROD == OdabraniKomitent.KOMITENT
                && ios.DATUM_IMPORTA >= Od && ios.DATUM_IMPORTA <= Do))
                {
                    _uplateZaOrgKladionica.Add(k);
                }

                UplataKladionica = (from i in _uplateZaOrgKladionica
                                    select i.UPLATA).Sum();
                //Porez Kladionica (Datum treba u tabeli kladionica, nadat se da ce bit kad stigne novi program)
                foreach (KLADIONICA k in _listiciKladionica.Where(ios => ios.OP_BR_PROD == OdabraniKomitent.KOMITENT && ios.PLAC_POREZ != 0
                && ios.DATUM_IMPORTA >= Od && ios.DATUM_IMPORTA <= Do))
                {
                    _poreziZaOrgKladionica.Add(k);
                }

                PorezKladionica = (from i in _poreziZaOrgKladionica
                                   select i.PLAC_POREZ).Sum();

                //Polog Pazara
                foreach (POLOG_PAZAR p in _pazar.Where(pp => pp.OP_BROJ_PROD == OdabraniKomitent.KOMITENT && pp.DATUM >= Od && pp.DATUM <= Do))
                {
                    _pazarZaOrg.Add(p);
                }
                PologPazara = (from i in _pazarZaOrg
                               select i.IZNOS).Sum();



                foreach (AUTOMATI_SABINA a in _automati.Where(aus => aus.OP_BROJ == OdabraniKomitent.KOMITENT && aus.DATUM >= Od && aus.DATUM <= Do))
                {
                    _automatiZaOrg.Add(a);
                }

                AutomatiUkupno = (from a in _automatiZaOrg
                                  select a.IZNOS).Sum();


                //OsnIgre i Kladionica
                UplataKladOsnIg = UplataOsnovneIgre + UplataKladionica;
                //Ukupno Porez
                Org.Porez = PorezOsnovneIgre + PorezKladionica;

                Porez = _org.Porez;
                //UkupnaUplata
                UkupnaUplata = PocetnoPlus + UplataSreckeSve + UplataOsnovneIgre + UplataKladionica + Porez + AutomatiUkupno + OstalaZaduzenja;

                //UkupnaIsplata
                UkupnaIsplata = PocetnoMinus + IsplataOsnovneIgreSrecke + IsplataKladionica + PologPazara + WebIsplata;

                //Saldo
                Saldo = UkupnaUplata - UkupnaIsplata;

                UplateIsplateSkupa2 = UplateIsplateSkupa;
                UplateIsplateSkupaSrecke2 = UplateIsplateSkupaSrecke;
                UplateIsplateSkupaKladionica = _uplateIsplateSkupaKladionica;

                UplateIsplateSkupaKladionica = _uplateIsplateSkupaKladionica;

                NapraviTXTZaPDF();

            }
        }

        private void KreirajOrg()
        {
            if (OdabraniKomitent.ID != 0)
            {
                int op = int.Parse(OdabraniKomitent.KOMITENT);
                _uplateOsnovneIgre = this._avm.Gr.UplateOsnovneIgre.Where(x => op == x.OP_BROJ).ToList();
                _igre = this._avm.Gr.Igre;
                _kladionicaIgre = this._avm.Gr.KladionicaIgre;

                if (_korisnik.VRSTA == 10)
                {
                    _isplateOsnovneIgre = this._avm.Gr.IsplateOsnovneIgre.Where(x => OdabraniKomitent.KOMITENT == x.LIS_ISPL).ToList();
                    _listiciKladionica = this._avm.Gr.ListiciKladionica.Where(x => OdabraniKomitent.KOMITENT == x.OP_BR_PROD).ToList(); ;
                    _pazar = this._avm.Gr.Pazar.Where(x => OdabraniKomitent.KOMITENT == x.OP_BROJ_PROD).ToList();
                    _automati = this._avm.Gr.Automati.Where(x => OdabraniKomitent.KOMITENT == x.OP_BROJ).ToList();
                    _zaduzenja = this._avm.Gr.Zaduzenja.Where(x => op == x.ODOBRITI_BLAGAJNIKA || op == x.ZADUZITI_BLAGAJNIKA).ToList(); ;
                }

                else
                {
                    _isplateOsnovneIgre = this._avm.Gr.NapuniIsplateZaKomitenta(OdabraniKomitent);
                    _listiciKladionica = this._avm.Gr.NapuniKladionicuZaKomitenta(OdabraniKomitent);
                    _pazar = this._avm.Gr.NapuniPazarZaKomitenta(OdabraniKomitent);
                    _automati = this._avm.Gr.NapuniAutomateZaKomitenta(OdabraniKomitent);
                    _zaduzenja = this._avm.Gr.NapuniZadZaKomitenta(OdabraniKomitent);
                }

                // _uplateOsnovneIgre = this._avm.Gr.UplateOsnovneIgre;
                // _isplateOsnovneIgre = this._avm.Gr.IsplateOsnovneIgre;
                // //_isplateOsnovneIgre = this._avm.Gr.IsplateOIZaRegion;
                // _listiciKladionica = this._avm.Gr.ListiciKladionica;
                // _importKlad2 = this._avm.Gr.ImpKlad2UplataIsplata;
                // _igre = this._avm.Gr.Igre;
                // _pazar = this._avm.Gr.Pazar;
                // _kladionicaIgre = this._avm.Gr.KladionicaIgre;
                // _automati = this._avm.Gr.Automati;
                //// _pocetnaStanja = this._avm.Gr.PocetnaStanja;
                // _zaduzenja = this._avm.Gr.Zaduzenja;
                //var OrgViewModelContext = new LutrijaEntities1();
                //_uplateOsnovneIgre = OrgViewModelContext.EOP_SIN.ToList();
                //_isplateOsnovneIgre = OrgViewModelContext.ISPLATA.ToList();
                //_listiciKladionica = OrgViewModelContext.KLADIONICA.ToList();
                //_igre = OrgViewModelContext.IGRE.ToList();
                //_pazar = OrgViewModelContext.POLOG_PAZAR.ToList();
                _uplateIsplateSkupaKladionica = new List<UplataIsplataSkupa>();
            _uplateIsplateSkupaSrecke = new List<UplataIsplataSkupa>();
            _uplateIsplateSkupa = new List<UplataIsplataSkupa>();
            _isplateZaOrg = new List<ISPLATA>();
            _isplateZaOrgSrecke = new List<ISPLATA>();
            _uplateZaOrgOsnovneIgre = new List<EOP_SIN>();
            _uplateZaOrgSrecke = new List<EOP_SIN>();
            _uplateZaOrgKladionica = new List<KLADIONICA>();
            _isplateZaOrgKladionica = new List<KLADIONICA>();
            _poreziZaOrgKladionica = new List<KLADIONICA>();
            _poreziZaOrgOsnovneIgre = new List<ISPLATA>();
            _pazarZaOrg = new List<POLOG_PAZAR>();
            _uplateIsplateZaOrgWeb = new List<EOP_SIN>();
            _isplataPoIgri = new List<ISPLATA>();
            _uplataPoIgri = new List<EOP_SIN>();
            _uplataPoIgriSrecke = new List<EOP_SIN>();
            _isplataPoSrecki = new List<ISPLATA>();
            _automatiZaOrg = new List<AUTOMATI_SABINA>();
            _kladionicaUplataIsplata = new List<KLADIONICA>();
            _importKlad2UplataIsplata = new List<ImpKlad2>();
            _zaduzenjaZaOrg = new List<ZADUZENJE_GOTOVINE>();

            _org = new Org();
            // Uplata Isplata Kladionica
            int mj = Od.Month - 1;

            decimal? pocStanje = this._avm.Gr.PocZaOpBroj(OdabraniKomitent.KOMITENT, mj);


            //decimal? pocStanje = (from p in _pocetnaStanja
            //                     where p.MJESEC == Od.Month-1 && p.GODINA==Do.Year && p.OP_BROJ.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT
            //                     select p.POC_STANJE).FirstOrDefault();


            if (pocStanje != null)
            { 
            PocetnoStanje = pocStanje;
            if (PocetnoStanje >= 0)
            {
                PocetnoPlus = PocetnoStanje;
                PocetnoMinus = 0;
            }
            else
            {
                PocetnoMinus = PocetnoStanje;
                PocetnoPlus = 0;
            }
            }
            else
            {
                PocetnoMinus = 0;
                PocetnoPlus = 0;
            }


            foreach (ZADUZENJE_GOTOVINE i in _zaduzenja.Where(ios => ios.ZADUZITI_BLAGAJNIKA.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do))
            {
                _zaduzenjaZaOrg.Add(i);
            }

            
            OstalaZaduzenja = (from i in _zaduzenjaZaOrg
                                select i.IZNOS).Sum();

            foreach (KLADIONICA k in _listiciKladionica.Where(klad => klad.OP_BR_PROD.PadLeft(5, '0') == OdabraniKomitent.KOMITENT
            && klad.DATUM_IMPORTA >= Od && klad.DATUM_IMPORTA <= Do))
            {
                string broj = k.OP_BR_PROD;
                _kladionicaUplataIsplata.Add(k);
            }

            KladionicaUplataIsplata = _kladionicaUplataIsplata;

            //foreach (ImpKlad2 i in _importKlad2)
            //{
            //    _importKlad2UplataIsplata.Add(i);
            //}


            //ImpKlad2UplataIsplata = _importKlad2UplataIsplata; 


            // Isplata Osnovne Igre
            foreach (ISPLATA i in _isplateOsnovneIgre.Where(ios => ios.LIS_ISPL == OdabraniKomitent.KOMITENT && ios.LIS_VRISPL >= Od && ios.LIS_VRISPL <= Do.AddDays(1) && ios.LIS_TIP_SRE == 0))
            {
                _isplateZaOrg.Add(i);
            }

            //IsplataSrecke
            foreach (ISPLATA i in _isplateOsnovneIgre.Where(ios => ios.LIS_ISPL == OdabraniKomitent.KOMITENT && ios.LIS_VRISPL >= Od && ios.LIS_VRISPL <= Do.AddDays(1) && ios.LIS_TIP_SRE != 0))
            {
                _isplateZaOrgSrecke.Add(i);
            }

            decimal? ispOsn = (from i in _isplateZaOrg
                               select i.LIS_IZNOS).Sum();

            decimal? ispSre = (from i in _isplateZaOrgSrecke
                               select i.LIS_IZNOS).Sum();


            _org.IsplataOsnovneIgre = ispOsn + ispSre;
            IsplataOsnovneIgreSrecke = _org.IsplataOsnovneIgre;

            // Isplata po Igri
            IsplataPoIgri = _isplateZaOrg
                                    .GroupBy(l => l.SIF_IGRE)
                                    .Select(cl => new ISPLATA
                                    {
                                        SIF_IGRE = cl.First().SIF_IGRE,

                                        LIS_IZNOS = cl.Sum(c => c.LIS_IZNOS),
                                    }).ToList();


            IsplataPoSrecki = _isplateZaOrgSrecke
                                   .GroupBy(l => l.LIS_TIP_SRE)
                                   .Select(cl => new ISPLATA
                                   {
                                       LIS_TIP_SRE = cl.First().LIS_TIP_SRE,

                                       LIS_IZNOS = cl.Sum(c => c.LIS_IZNOS),
                                   }).ToList();




            foreach (ISPLATA isig in IsplataPoIgri)
            {

                Org.NazivIgre = isig.LIS_IGRA;

                Org.IsplataIgra = isig.LIS_IZNOS;
            }

            foreach (ISPLATA i in IsplataPoSrecki)
            {
                i.LIS_NAZ_SRE = (from ig in _igre
                                 where i.LIS_TIP_SRE == ig.SIF_ISP
                                 select ig.NAZIV).First();
            }



            //Uplata Osnovne Igre (Kolo pretplate se racuna na datum uplate ne za uplaceno kolo)
            foreach (EOP_SIN es in _uplateOsnovneIgre.Where(ios => ios.OP_BROJ.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do && ios.SRECKA == 0 && ios.IGRA != 30))
            {
                _uplateZaOrgOsnovneIgre.Add(es);
            }

            _org.UplataOsnovneIgre = (from i in _uplateZaOrgOsnovneIgre
                                      select i.UPL).Sum();


            foreach (EOP_SIN es in _uplateOsnovneIgre.Where(ios => ios.OP_BROJ.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do && ios.SRECKA == 0 && ios.IGRA == 30))
            {
                _uplateIsplateZaOrgWeb.Add(es);
            }

            _org.WebIsplata = (from i in _uplateIsplateZaOrgWeb
                               select i.UPL).Sum();

            WebIsplata = _org.WebIsplata;
            UplataOsnovneIgre = _org.UplataOsnovneIgre;
            //Uplata Srecke
            foreach (EOP_SIN esi in _uplateOsnovneIgre.Where(ios => ios.OP_BROJ.ToString().PadLeft(5, '0') == OdabraniKomitent.KOMITENT && ios.DATUM >= Od && ios.DATUM <= Do && ios.SRECKA != 0))
            {
                _uplateZaOrgSrecke.Add(esi);
            }

            _org.UplataSrecke = (from i in _uplateZaOrgSrecke
                                 select i.UPL).Sum();
            UplataSreckeSve = _org.UplataSrecke;

            //SRECKE NEMAJU POREZ MORA SE RUCNO IZRACUNATI IZ FILEA ZA SVAKU SRECKU DOBITKA PREKO 100 KM 10%

            foreach (IGRE ig in _igre)
            {
                if (ig.SIF_BAZ_IGR == 1 && ig.ID != 43)
                {
                    UpIsp = new UplataIsplataSkupa();
                    UpIsp.sifraIgreUpl = ig.SIF_UPL;
                    UpIsp.sifraIgreIspl = ig.SIF_ISP;
                    UpIsp.NazivIgre = ig.NAZIV;

                    _uplateIsplateSkupa.Add(UpIsp);
                }
                else if (ig.SIF_BAZ_IGR == 4)
                {
                    UpIsp = new UplataIsplataSkupa();
                    UpIsp.sifraIgreUpl = ig.SIF_UPL;
                    UpIsp.sifraIgreIspl = ig.SIF_ISP;
                    UpIsp.NazivIgre = ig.NAZIV;

                    _uplateIsplateSkupaSrecke.Add(UpIsp);
                }
                else if (ig.SIF_BAZ_IGR == 2)
                {
                    UpIsp = new UplataIsplataSkupa();

                    if (ig.ID == 38)
                    {
                        UpIsp.sifraIgreKlad = 1;
                        UpIsp.NazivIgre = ig.NAZIV;
                    }
                    else if (ig.ID == 39)
                    {
                        UpIsp.sifraIgreKlad = 2;
                        UpIsp.NazivIgre = ig.NAZIV;
                    }
                    else if (ig.ID == 40)
                    {
                        UpIsp.sifraIgreKlad = 3;
                        UpIsp.NazivIgre = ig.NAZIV;
                    }
                    else if (ig.ID == 41)
                    {
                        UpIsp.sifraIgreKlad = 4;
                        UpIsp.NazivIgre = ig.NAZIV;
                    }
                    else if (ig.ID == 42)
                    {
                        UpIsp.sifraIgreKlad = 5;
                        UpIsp.NazivIgre = ig.NAZIV;
                    }

                    _uplateIsplateSkupaKladionica.Add(UpIsp);
                }

            }

            UplataPoIgriSrecke = _uplateZaOrgSrecke
                                   .GroupBy(l => l.IGRA)
                                   .Select(cl => new EOP_SIN
                                   {
                                       IGRA = cl.First().IGRA,

                                       UPL = cl.Sum(c => c.UPL),
                                   }).ToList();

            //Uplata po igri
            UplataPoIgri = _uplateZaOrgOsnovneIgre
                                   .GroupBy(l => l.IGRA)
                                   .Select(cl => new EOP_SIN
                                   {
                                       IGRA = cl.First().IGRA,

                                       UPL = cl.Sum(c => c.UPL),
                                   }).ToList();

            var KladionicaPoIgri = _kladionicaUplataIsplata
                                  .GroupBy(l => l.KLAD_IGRA)
                                  .Select(cl => new KLADIONICA
                                  {
                                      KLAD_IGRA = cl.First().KLAD_IGRA,

                                      UPLATA = cl.Sum(c => c.UPLATA),
                                      ISPLATA = cl.Sum(c => c.ISPLATA),
                                      PLAC_POREZ = cl.Sum(c => c.PLAC_POREZ)
                                  }).ToList();

            foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaKladionica)
            {
                ui.IznosIsplate = (from k in KladionicaPoIgri
                                   where k.KLAD_IGRA == ui.sifraIgreKlad
                                   select k.ISPLATA).FirstOrDefault();
            }

            foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaKladionica)
            {
                ui.IznosUplate = (from k in KladionicaPoIgri
                                  where k.KLAD_IGRA == ui.sifraIgreKlad
                                  select k.UPLATA).FirstOrDefault();
            }

            foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaKladionica)
            {
                ui.IznosPoreza = (from k in KladionicaPoIgri
                                  where k.KLAD_IGRA == ui.sifraIgreKlad
                                  select k.PLAC_POREZ).FirstOrDefault();
            }

            decimal? proba = (from bla in IsplataPoIgri
                              select bla.LIS_IZNOS).Sum();
            //SKUPA UPLATA ISPLATA
            foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaSrecke)
            {
                ui.IznosUplate = (from u in UplataPoIgriSrecke
                                  where ui.sifraIgreUpl == u.IGRA
                                  select u.UPL).FirstOrDefault();
            }

            foreach (UplataIsplataSkupa ui in _uplateIsplateSkupaSrecke)
            {
                ui.IznosIsplate = (from u in IsplataPoSrecki
                                   where ui.sifraIgreIspl == u.LIS_TIP_SRE
                                   select u.LIS_IZNOS).FirstOrDefault();
            }

            foreach (UplataIsplataSkupa ui in _uplateIsplateSkupa)
            {
                ui.IznosUplate = (from u in UplataPoIgri
                                  where ui.sifraIgreUpl == u.IGRA
                                  select u.UPL).FirstOrDefault();
            }

            foreach (UplataIsplataSkupa ui in _uplateIsplateSkupa)
            {
                ui.IznosIsplate = (from u in IsplataPoIgri
                                   where ui.sifraIgreIspl == u.SIF_IGRE
                                   select u.LIS_IZNOS).FirstOrDefault();
            }

            proba = (from bla in _uplateIsplateSkupa
                     select bla.IznosIsplate).Sum();
            decimal? proba2 = (from bla in _uplateIsplateSkupaSrecke
                               select bla.IznosIsplate).Sum();
            decimal? proba3 = proba + proba2;


            //Porez Osnovne Igre i Srecke
            foreach (ISPLATA i in _isplateOsnovneIgre.Where(ios => ios.LIS_ISPL == OdabraniKomitent.KOMITENT && ios.LIS_VRISPL >= Od && ios.LIS_VRISPL <= Do && ios.LIS_POREZ != 0))
            {
                _poreziZaOrgOsnovneIgre.Add(i);
            }

            PorezOsnovneIgre = (from i in _poreziZaOrgOsnovneIgre
                                select i.LIS_POREZ).Sum();


            //Kladionica isplata(Datum treba u tabeli kladionica, nadat se da ce bit kad stigne novi program)
            foreach (KLADIONICA k in _listiciKladionica.Where(ios => ios.OP_BR_PROD == OdabraniKomitent.KOMITENT
            && ios.DATUM_IMPORTA >= Od && ios.DATUM_IMPORTA <= Do))
            {
                _isplateZaOrgKladionica.Add(k);
            }

            decimal? kladIsp = (from i in _isplateZaOrgKladionica
                                select i.ISPLATA).Sum();

            decimal? kladPorez = (from i in _isplateZaOrgKladionica
                                  select i.PLAC_POREZ).Sum();


            //Ukoliko je bio rucni unos 
            IsplataKladionica = kladIsp + kladPorez;

            //Kladionica uplata(Datum treba u tabeli kladionica, nadat se da ce bit kad stigne novi program)
            foreach (KLADIONICA k in _listiciKladionica.Where(ios => ios.OP_BR_PROD == OdabraniKomitent.KOMITENT
            && ios.DATUM_IMPORTA >= Od && ios.DATUM_IMPORTA <= Do))
            {
                _uplateZaOrgKladionica.Add(k);
            }

            UplataKladionica = (from i in _uplateZaOrgKladionica
                                select i.UPLATA).Sum();
            //Porez Kladionica (Datum treba u tabeli kladionica, nadat se da ce bit kad stigne novi program)
            foreach (KLADIONICA k in _listiciKladionica.Where(ios => ios.OP_BR_PROD == OdabraniKomitent.KOMITENT && ios.PLAC_POREZ != 0
            && ios.DATUM_IMPORTA >= Od && ios.DATUM_IMPORTA <= Do))
            {
                _poreziZaOrgKladionica.Add(k);
            }

            PorezKladionica = (from i in _poreziZaOrgKladionica
                               select i.PLAC_POREZ).Sum();

            //Polog Pazara
            foreach (POLOG_PAZAR p in _pazar.Where(pp => pp.OP_BROJ_PROD == OdabraniKomitent.KOMITENT && pp.DATUM >= Od && pp.DATUM <= Do))
            {
                _pazarZaOrg.Add(p);
            }
            PologPazara = (from i in _pazarZaOrg
                           select i.IZNOS).Sum();



            foreach (AUTOMATI_SABINA a in _automati.Where(aus => aus.OP_BROJ == OdabraniKomitent.KOMITENT && aus.DATUM >= Od && aus.DATUM <= Do))
            {
                _automatiZaOrg.Add(a);
            }

            AutomatiUkupno = (from a in _automatiZaOrg
                               select a.IZNOS).Sum();


            //OsnIgre i Kladionica
            UplataKladOsnIg = UplataOsnovneIgre + UplataKladionica;
            //Ukupno Porez
            Org.Porez = PorezOsnovneIgre + PorezKladionica;

            Porez = _org.Porez;
            //UkupnaUplata
            UkupnaUplata = PocetnoPlus + UplataSreckeSve + UplataOsnovneIgre + UplataKladionica + Porez + AutomatiUkupno + OstalaZaduzenja;

            //UkupnaIsplata
            UkupnaIsplata = PocetnoMinus + IsplataOsnovneIgreSrecke + IsplataKladionica + PologPazara + WebIsplata;

            //Saldo
            Saldo = UkupnaUplata - UkupnaIsplata;

            UplateIsplateSkupa2 = UplateIsplateSkupa;
            UplateIsplateSkupaSrecke2 = UplateIsplateSkupaSrecke;

                //Odavde kupiti podatke za kladionicu
            UplateIsplateSkupaKladionica = _uplateIsplateSkupaKladionica;
                UplateIsplateSkupaKladionica2 = _uplateIsplateSkupaKladionica;

                //Ovo ne treba, kupimo podatke iznad iz 'UplateIsplateSkupaKladionica2'
                //foreach (UplataIsplataSkupa item in UplateIsplateSkupaKladionica)
                //{
                //    _uplateIsplateSkupaKladionica2.Add(item);

                //}

                // NapraviTXTZaPDF();
            }

            else
            {
                Exception ex = new Exception("Niste Odabrali Komitenta");
                _poruke.Greska(ex);
            }


        }

        public void NapraviTXTZaPDF()
        {
            string message = Environment.NewLine;
            string od = Od.ToString("ddMMyyyy");
            string d = Do.ToString("ddMMyyyy");
            message += $"{od}${d}${_pocetnoStanje}${OdabraniKomitent.IME}${OdabraniKomitent.PREZIME}${UplataOsnovneIgre}${UplataKladionica}${UplataSreckeSve}${Porez}${PologPazara}${AutomatiUkupno}${IsplataKladionica}$";
            message += $"{WebIsplata}${UkupnaUplata}${UkupnaIsplata}${Saldo}${OstalaZaduzenja}${IsplataOsnovneIgreSrecke}$";          
            message += Environment.NewLine;
            int op = int.Parse(OdabraniKomitent.KOMITENT);
            
            string ime = putanja + op+"_"+od+"_"+d+".csv";

            using (StreamWriter writer = new StreamWriter(ime, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
            
        }
        //UplateIsplateSkupaKladionica
        //Pomocna klasa uplata isplata skupa
        public class UplataIsplataSkupa : ObservableObject
        {

            public decimal? IznosUplate { get; set; } = 0;

            public decimal? IznosIsplate { get; set; } = 0;

            public decimal? IznosPoreza { get; set; } = 0;
            public string NazivIgre { get; set; } = "";

            public int? sifraIgreUpl { get; set; } = 0;

            public int? sifraIgreIspl { get; set; } = 0;

            public int? sifraIgreKlad { get; set; } = 0;

        }



        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }

        public Org Org { get => _org; set { _org = value; OnPropertyChanged("Org"); } }

        public UplataIsplataSkupa UpIsp { get => _upIsp; set { _upIsp = value; OnPropertyChanged("UpIsp"); } }
        public DateTime Od { get => _od; set { _od = value; OnPropertyChanged("Od"); } }
        public DateTime Do { get => _do; set { _do = value; OnPropertyChanged("Do"); } }

        public decimal? IsplataOsnovneIgreSrecke { get => _isplataOsnovneIgreSrecke; set { _isplataOsnovneIgreSrecke = value; OnPropertyChanged("IsplataOsnovneIgreSrecke"); } }
        public decimal? UplataOsnovneIgre { get => _uplataOsnovneIgre; set { _uplataOsnovneIgre = value; OnPropertyChanged("UplataOsnovneIgre"); } }
        public decimal? UplataSreckeSve { get => _uplataSreckeSve; set { _uplataSreckeSve = value; OnPropertyChanged("UplataSreckeSve"); } }
        public decimal? UkupnaUplata { get => _ukupnaUplata; set { _ukupnaUplata = value; OnPropertyChanged("UkupnaUplata"); } }
        public decimal? UkupnaIsplata { get => _ukupnaIsplata; set { _ukupnaIsplata = value; OnPropertyChanged("UkupnaIsplata"); } }
        public decimal? Saldo { get => _saldo; set { _saldo = value; OnPropertyChanged("Saldo"); } }
        public decimal? PorezOsnovneIgre { get => _porezOsnovneIgre; set { _porezOsnovneIgre = value; OnPropertyChanged("PorezOsnovneIgre"); } }
        public decimal? PorezKladionica { get => _porezKladionica; set { _porezKladionica = value; OnPropertyChanged("PorezKladionica"); } }
        public decimal? UplataKladionica { get => _uplataKladionica; set { _uplataKladionica = value; OnPropertyChanged("UplataKladionica"); } }
        public decimal? IsplataKladionica { get => _isplataKladionica; set { _isplataKladionica = value; OnPropertyChanged("IsplataKladionica"); } }

        public decimal? Porez { get => _porez; set { _porez = value; OnPropertyChanged("Porez"); } }

        public decimal? PologPazara { get => _pologPazara; set { _pologPazara = value; OnPropertyChanged("PologPazara"); } }
        public decimal? WebIsplata { get => _webIsplata; set { _webIsplata = value; OnPropertyChanged("WebIsplata"); } }
        public decimal? IsplataSuperloto { get => _isplataSuperloto; set { _isplataSuperloto = value; OnPropertyChanged("IsplataSuperloto"); } }

        public decimal? AutomatiUkupno { get => _automatiUkupno; set { _automatiUkupno = value; OnPropertyChanged("AutomatiUkupno"); } }
        //public decimal? NazivIgre { get => _nazivIgre; set { _nazivIgre = value; OnPropertyChanged("NazivIgre"); } }

        public decimal? IznosIgre { get => _iznosIgre; set { _iznosIgre = value; OnPropertyChanged("IznosIgre"); } }

        public decimal? PocetnoStanje { get => _pocetnoStanje; set { _pocetnoStanje = value; OnPropertyChanged("PocetnoStanje"); } }

        public decimal? PocetnoMinus { get => _pocetnoMinus; set { _pocetnoMinus = value; OnPropertyChanged("PocetnoMinus"); } }
        public decimal? PocetnoPlus { get => _pocetnoPlus; set { _pocetnoPlus = value; OnPropertyChanged("PocetnoPlus"); } }

        public decimal? UplataKladOsnIg { get => _uplataKladOsnIg; set { _uplataKladOsnIg = value; OnPropertyChanged("UplataKladOsnIg"); } }

        public decimal? OstalaZaduzenja { get => _ostalaZaduzenja; set { _ostalaZaduzenja = value; OnPropertyChanged("OstalaZaduzenja"); } }

        public List<ISPLATA> IsplataPoIgri { get => _isplataPoIgri; set { _isplataPoIgri = value; OnPropertyChanged("IsplataPoIgri"); } }

        public List<EOP_SIN> UplataPoIgri { get => _uplataPoIgri; set { _uplataPoIgri = value; OnPropertyChanged("UplataPoIgri"); } }

        public List<EOP_SIN> UplataPoIgriSrecke { get => _uplataPoIgriSrecke; set { _uplataPoIgriSrecke = value; OnPropertyChanged("UplataPoIgriSrecke"); } }

        public List<ISPLATA> IsplataPoSrecki { get => _isplataPoSrecki; set { _isplataPoSrecki = value; OnPropertyChanged("IsplataPoSrecki"); } }
        public List<KLADIONICA> KladionicaUplataIsplata { get => _kladionicaUplataIsplata; set { _kladionicaUplataIsplata = value; OnPropertyChanged("KladionicaUplataIsplata"); } }
        public List<ImpKlad2> ImpKlad2UplataIsplata { get => _importKlad2UplataIsplata; set { _importKlad2UplataIsplata = value; OnPropertyChanged("ImpKlad2UplataIsplata"); } }

        public List<UplataIsplataSkupa> UplateIsplateSkupa { get => _uplateIsplateSkupa; set { _uplateIsplateSkupa = value; OnPropertyChanged("UplateIsplateSkupa"); } }

        public List<UplataIsplataSkupa> UplateIsplateSkupa2 { get => _uplateIsplateSkupa2; set { _uplateIsplateSkupa2 = value; OnPropertyChanged("UplateIsplateSkupa2"); } }

        public List<UplataIsplataSkupa> UplateIsplateSkupaSrecke { get => _uplateIsplateSkupaSrecke; set { _uplateIsplateSkupaSrecke = value; OnPropertyChanged("UplateIsplateSkupaSrecke"); } }

        public List<UplataIsplataSkupa> UplateIsplateSkupaSrecke2 { get => _uplateIsplateSkupaSrecke2; set { _uplateIsplateSkupaSrecke2 = value; OnPropertyChanged("UplateIsplateSkupaSrecke2"); } }

        public List<UplataIsplataSkupa> UplateIsplateSkupaKladionica { get => _uplateIsplateSkupaKladionica; set { _uplateIsplateSkupaKladionica = value; OnPropertyChanged("UplateIsplateSkupaKladionica"); } }
        public static List<UplataIsplataSkupa> UplateIsplateSkupaKladionica2 { get => _uplateIsplateSkupaKladionica2; set { _uplateIsplateSkupaKladionica2 = value; } }
        public ApplicationViewModel AVM { get => _avm; set { _avm = value; OnPropertyChanged("AVM"); } }

        public bool PDFDugme { get => _pdfDugme; set { _pdfDugme = value; OnPropertyChanged("PDFDugme"); } }


    }

}
