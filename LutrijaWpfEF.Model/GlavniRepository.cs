using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{

    public class GlavniRepository : ObservableObject
    {
        private List<ISPLATA> _isplateOsnovneIgre;
        private List<ISPLATA> _isplateOIZaRegion;
        private List<EOP_SIN> _uplateOsnovneIgre;
       
        private List<KLADIONICA> _listiciKladionica;
        private List<ImpKlad2> _importKlad2UplataIsplata;
        private List<POLOG_PAZAR> _pazar;
        private List<IGRE> _igre;
        private List<IGRE> _osnovneIgre;
        private List<IGRE> _srecke;
        private List<KLADIONICA_IGRE> _kladionicaIgre;
        private List<POC_STANJA> _pocetnaStanja;
        private List<komitenti_ime_matbr_zracun> _komitenti;
        private List<komitenti_ime_matbr_zracun> _komitentiZaRegion;
        private List<AUTOMATI_SABINA> _automati;
        private List<ZADUZENJE_GOTOVINE> _zaduzenja;
        private POC_STANJA _pocStanje;
        private PRIJAVA_ORG _korisnik;
        private LutrijaEntities1 _context;
        private List<OPCINE> _opcine;
        private string dc = @"Server=192.168.1.213;Database=Lutrija;MultipleActiveResultSets=true;User ID=sa;Password=Lutrija1;
                        TrustServerCertificate=True;";
        private string procedura = "izmijeni_poc_stanja";

        CancellationTokenSource cts;
        public GlavniRepository()
        {
            _context = new LutrijaEntities1();
          
            //NapuniKomitente();         
            //NapuniIgre();            
            //NapuniKladionicaIgre();          
            //NapuniUplateOI();            
            //NapuniIsplateOI();
            //NapuniKladionicu();
            //NapuniAutomate();
            //NapuniPazar();
            //// NapuniPocStanja();
            //NapuniZaduzenja();
            //NapuniKomitenteZaRegion();
            //NapuniOpcine();
        }

        public void NapuniOpcine()
        {
            _opcine = _context.OPCINE.ToList();
        }

        public void NapuniKomitente()
        {
            
            _komitenti =  _context.komitenti_ime_matbr_zracun.ToList();
          
           
        }

        public List<komitenti_ime_matbr_zracun> KomitentiZaRegion2(PRIJAVA_ORG korisnik)
        {
            _komitentiZaRegion = new List<komitenti_ime_matbr_zracun>();
            List<string> listaOp = new List<string>();
            PRIJAVA_ORG kor = korisnik;
            if (kor.VRSTA == 10)
            {
                _komitentiZaRegion = _context.komitenti_ime_matbr_zracun.ToList();
                return _komitentiZaRegion;
            }
            else
            {
                var uplate = UplateOsnovneIgre.Where(d => d.REGION == kor.VRSTA).GroupBy(p => p.OP_BROJ).Select(g => g.First()).ToArray();

                foreach (EOP_SIN u in uplate)
                {

                    string op = u.OP_BROJ.ToString().PadLeft(5, '0');
                    listaOp.Add(op);
                }


                List<komitenti_ime_matbr_zracun> bla = _context.komitenti_ime_matbr_zracun.Where(p => listaOp.Contains(p.KOMITENT)).ToList();
                Komitenti = bla;
                return bla;
            }
        }

            public List<komitenti_ime_matbr_zracun> KomitentiZaRegion(PRIJAVA_ORG korisnik)
        {
            
            _komitentiZaRegion = new List<komitenti_ime_matbr_zracun>();
           
            PRIJAVA_ORG kor = korisnik;
            if (kor.VRSTA == 10)
            {
                _komitentiZaRegion = _context.komitenti_ime_matbr_zracun.ToList();
                return _komitentiZaRegion;
            }
            else
            {
             var unikatniKomitentiUplate = UplateOsnovneIgre
                                .Where(d => d.REGION == kor.VRSTA)
                                 .GroupBy(p => p.OP_BROJ)
                                 .Select(g => g.First()).ToList();


                foreach (EOP_SIN u in unikatniKomitentiUplate)
            {     
                var nekiKomitent = (from komitenti_ime_matbr_zracun k in _komitenti
                                    where int.Parse(k.KOMITENT) == u.OP_BROJ
                                    select k).First();

                _komitentiZaRegion.Add(nekiKomitent);  
            }
                
            return _komitentiZaRegion;     

            }
        }
        public void IsplateRegion(List<komitenti_ime_matbr_zracun> kom)
        {
            var komitenti = kom.Select(d => d.KOMITENT).ToArray();
            IsplateOsnovneIgre = _context.ISPLATA.Where(p=> komitenti.Contains(p.LIS_ISPL)).ToList();
        }

        public void KladionicaRegion(List<komitenti_ime_matbr_zracun> kom)
        {
            var komitenti = kom.Select(d => d.KOMITENT).ToArray();
            ListiciKladionica = _context.KLADIONICA.Where(p => komitenti.Contains(p.OP_BR_PROD)).ToList();
        }

        public void AutomatiRegion(List<komitenti_ime_matbr_zracun> kom)
        {
            var komitenti = kom.Select(d => d.KOMITENT).ToArray();
            Automati = _context.AUTOMATI_SABINA.Where(p => komitenti.Contains(p.OP_BROJ)).ToList();
        }

        public void PazarRegion(List<komitenti_ime_matbr_zracun> kom)
        {
            var komitenti = kom.Select(d => d.KOMITENT).ToArray();
            Pazar = _context.POLOG_PAZAR.Where(p => komitenti.Contains(p.OP_BROJ_PROD)).ToList();
        }
        public void ZaduzenjaRegion(List<komitenti_ime_matbr_zracun> kom)
        {
            var komitenti = kom.Select(d => int.Parse(d.KOMITENT)).ToArray();
            Zaduzenja = _context.ZADUZENJE_GOTOVINE.Where(p => komitenti.Contains(p.ODOBRITI_BLAGAJNIKA) || komitenti.Contains(p.ZADUZITI_BLAGAJNIKA)).ToList();
        }
        public void NapuniIgre()
        {
            _igre = _context.IGRE.ToList();
            _osnovneIgre = (from IGRE i in _igre
                            where i.SIF_BAZ_IGR == 1 || i.SIF_BAZ_IGR==5
                            select i).ToList();
            _srecke = (from IGRE i in _igre
                       where i.SIF_BAZ_IGR == 4
                       select i).ToList();
        }

        public void NapuniKladionicaIgre()
        {
            _kladionicaIgre = _context.KLADIONICA_IGRE.ToList();
        }
       
        public List<EOP_SIN> NapuniUplateZaKomitenta(komitenti_ime_matbr_zracun kom)
        {
            List<EOP_SIN> uplate = new List<EOP_SIN>();
            komitenti_ime_matbr_zracun k = kom;
            int op = int.Parse(k.KOMITENT);
            uplate = _context.EOP_SIN.Where(s=> s.OP_BROJ == op).ToList();

            return uplate;
        }

        public List<ISPLATA> NapuniIsplateZaKomitenta(komitenti_ime_matbr_zracun kom)
        {
            List<ISPLATA> isplate = new List<ISPLATA>();
            komitenti_ime_matbr_zracun k = kom;
            
            isplate = _context.ISPLATA.Where(s => s.LIS_ISPL == kom.KOMITENT).ToList();

            return isplate;
        }

        public List<KLADIONICA> NapuniKladionicuZaKomitenta(komitenti_ime_matbr_zracun kom)
        {
            List<KLADIONICA> lis_klad = new List<KLADIONICA>();
            komitenti_ime_matbr_zracun k = kom;

            lis_klad = _context.KLADIONICA.Where(s => s.OP_BR_PROD == kom.KOMITENT).ToList();

            return lis_klad;
        }

        public List<POLOG_PAZAR> NapuniPazarZaKomitenta(komitenti_ime_matbr_zracun kom)
        {
            List<POLOG_PAZAR> polozi = new List<POLOG_PAZAR>();
            komitenti_ime_matbr_zracun k = kom;

            polozi = _context.POLOG_PAZAR.Where(s => s.OP_BROJ_PROD == kom.KOMITENT).ToList();

            return polozi;
        }

        public List<AUTOMATI_SABINA> NapuniAutomateZaKomitenta(komitenti_ime_matbr_zracun kom)
        {
            List<AUTOMATI_SABINA> automati = new List<AUTOMATI_SABINA>();
            komitenti_ime_matbr_zracun k = kom;

            automati = _context.AUTOMATI_SABINA.Where(s => s.OP_BROJ == kom.KOMITENT).ToList();

            return automati;
        }

        public List<ZADUZENJE_GOTOVINE> NapuniZadZaKomitenta(komitenti_ime_matbr_zracun kom)
        {
            List<ZADUZENJE_GOTOVINE> zaduzenja = new List<ZADUZENJE_GOTOVINE>();
            komitenti_ime_matbr_zracun k = kom;
            int op = int.Parse(k.KOMITENT);
            zaduzenja = _context.ZADUZENJE_GOTOVINE.Where(s => s.ODOBRITI_BLAGAJNIKA == op || s.ZADUZITI_BLAGAJNIKA == op).ToList();

            return zaduzenja;
        }

        public void NapuniUplateOI()
        {
            UplateOsnovneIgre = _context.EOP_SIN.ToList();
        }

        public void NapuniIsplateOI()
        {
            _isplateOsnovneIgre = _context.ISPLATA.ToList();
          
        }

        public void NapuniKladionicu()
        {
            _listiciKladionica = _context.KLADIONICA.ToList();
        }

        public (DateTime?, int) NadjiNajveciDatumKladionica()
        {
            var result = _listiciKladionica.Max(i => i.ID);
            var najveci= _listiciKladionica.OrderByDescending(x => x.ID).FirstOrDefault();
            DateTime? naj = najveci.DATUM_IMPORTA;
            int id = najveci.KLAD_IGRA;
            return (naj, id);
        }
        public void NapuniAutomate()
        {
            _automati = _context.AUTOMATI_SABINA.ToList();
        }

        public void NapuniPazar()
        {
            _pazar = _context.POLOG_PAZAR.ToList();
        }

        public List<POC_STANJA> NapuniPocStanja()
        {
           return _pocetnaStanja = _context.POC_STANJA.ToList();
        }

        public void NapuniZaduzenja()
        {
            _zaduzenja = _context.ZADUZENJE_GOTOVINE.ToList();
        }

        public decimal? PocZaOpBroj(string op, int mj)
        {
            if (op != null)
            { 
            int opbr = int.Parse(op);
            decimal? poc = 0;

                    _pocStanje = _context.POC_STANJA.AsNoTracking().FirstOrDefault(p => p.OP_BROJ == opbr && p.MJESEC == mj);

                if (_pocStanje !=null)
                {     
                    poc = _pocStanje.POC_STANJE;

                }


                return poc;

            }
            else
            {
                decimal? p = 0;
                return p;
            }
          
           
        }

        public PRIJAVA_ORG Prijava(string user, string sifra)
        {
            return  _korisnik = _context.PRIJAVA_ORG.FirstOrDefault(p => p.KORISNICKO_IME == user && p.SIFRA == sifra);   
        }


        public async Task IzmijeniPocStanje(string opBroj, DateTime datum)
        {
            int god = datum.Year;
            int mjes = datum.Month;
            int opBr = int.Parse(opBroj);
            DateTime danas = DateTime.Now;

            await Task.Run(() =>
            {
            using (SqlConnection con = new SqlConnection(dc))
                {
                    con.Open();

                    while (mjes < danas.Month && god == danas.Year)
                    {
                        using (SqlCommand cmd = new SqlCommand(procedura, con))
                        {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@opBroj", SqlDbType.Int).Value = opBr;
                        cmd.Parameters.Add("@god", SqlDbType.Int).Value = god;
                        cmd.Parameters.Add("@mjes", SqlDbType.Int).Value = mjes;
                        cmd.ExecuteNonQuery();                
                        
                        mjes++;
                        }
                    }
                  con.Close();                
            }
            });
            NapuniPocStanja();
        }

        public List<EOP_SIN> UplateOsnovneIgre { get => _uplateOsnovneIgre; set { _uplateOsnovneIgre = value; OnPropertyChanged("UplateOsnovneIgre"); } }
        public List<ISPLATA> IsplateOsnovneIgre { get => _isplateOsnovneIgre; set { _isplateOsnovneIgre = value; OnPropertyChanged("IsplateOsnovneIgre"); } }

        public List<KLADIONICA> ListiciKladionica { get => _listiciKladionica; set { _listiciKladionica = value; OnPropertyChanged("ListiciKladionica"); } }
        public List<ImpKlad2> ImpKlad2UplataIsplata { get => _importKlad2UplataIsplata; set { _importKlad2UplataIsplata = value; OnPropertyChanged("ImpKlad2UplataIsplata"); } }


        public List<IGRE> Igre { get => _igre; set { _igre = value; OnPropertyChanged("Igre"); } }

        public List<IGRE> OsnovneIgre { get => _osnovneIgre; set { _osnovneIgre = value; OnPropertyChanged("OsnovneIgre"); } }

        public List<IGRE> Srecke { get => _srecke; set { _srecke = value; OnPropertyChanged("Srecke"); } }

        public List<KLADIONICA_IGRE> KladionicaIgre { get => _kladionicaIgre; set { _kladionicaIgre = value; OnPropertyChanged("KladionicaIgre"); } }

        public List<POC_STANJA> PocetnaStanja { get => _pocetnaStanja; set { _pocetnaStanja = value; OnPropertyChanged("PocetnaStanja"); } }

        public List<komitenti_ime_matbr_zracun> Komitenti { get => _komitenti; set { _komitenti = value; OnPropertyChanged("Komitenti"); } }

        public List<OPCINE> Opcine { get => _opcine; set { _opcine = value; OnPropertyChanged("Opcine"); } }

        public List<POLOG_PAZAR> Pazar { get => _pazar; set { _pazar = value; OnPropertyChanged("Pazar"); } }

        public List<AUTOMATI_SABINA> Automati { get => _automati; set { _automati = value; OnPropertyChanged("Automati"); } }

        public List<ZADUZENJE_GOTOVINE> Zaduzenja { get => _zaduzenja; set { _zaduzenja = value; OnPropertyChanged("Zaduzenja"); } }

        public List<ISPLATA> IsplateOIZaRegion { get => _isplateOIZaRegion; set { _isplateOIZaRegion = value; OnPropertyChanged("IsplateOIZaRegion"); } }

    }
}
