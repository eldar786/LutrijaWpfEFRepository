using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    public class UplSreckiRepository
    {
        private LutrijaEntities1 _uplSContext = null;
        private EOP_SIN _promjeneUplS;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private IGRE _igra;
        private string igraString;

        private string serija;
        private string _sifIspl;
        string sreckaPuniNaziv;


        public UplSreckiRepository(EOP_SIN promjeneUplS, komitenti_ime_matbr_zracun odabraniKomitent, IGRE igra, int odabranaSerijaSrecke)
        {
            _promjeneUplS = promjeneUplS;
            _odabraniKomitent = odabraniKomitent;
            _igra = igra;
            igraString = _igra.NAZIV;
            int? idIgra = _igra.SIF_ISP;

            _sifIspl = _igra.SIF_ISP.ToString();
            serija = odabranaSerijaSrecke.ToString();

            sreckaPuniNaziv = _sifIspl + "00" + serija;




        }

        public void DodajUplS()
        {
            using (_uplSContext = new LutrijaEntities1())
            {
                if (_promjeneUplS.ID <= 0)
                {
                    EOP_SIN UplataS = new EOP_SIN();
                    UplataS.DATUM = _promjeneUplS.DATUM;
                    UplataS.UPL = _promjeneUplS.UPL;
                    //UplataS.OPIS = _promjeneUplS.OPIS;
                    UplataS.IGRA = _igra.SIF_UPL.GetValueOrDefault();
                    UplataS.RED_KOLO = _promjeneUplS.RED_KOLO;
                    UplataS.OPSTINA = "003";
                    UplataS.OP_BROJ = _promjeneUplS.OP_BROJ;
                    UplataS.SRECKA = Int32.Parse(sreckaPuniNaziv);

                    //UplataS.SRECKA = _igra.SIF_ISP.GetValueOrDefault();

                    //UplataS.SRECKA = _promjeneUplS.SRECKA;
                    _uplSContext.EOP_SIN.Add(UplataS);
                }
                else
                {
                    IzmjeniUplS();
                }
                SpasiBazuUplS();
            }
        }

        public void IzmjeniUplS()
        {
            EOP_SIN uplsKojiSeMijenja = PronadjiUplS(_promjeneUplS.ID);
            uplsKojiSeMijenja.DATUM = _promjeneUplS.DATUM;
            uplsKojiSeMijenja.UPL = _promjeneUplS.UPL;
            //uplsKojiSeMijenja.OPIS = _promjeneUplS.OPIS;
            uplsKojiSeMijenja.OP_BROJ = _promjeneUplS.OP_BROJ;
            //uplsKojiSeMijenja.SRECKA = Int32.Parse(sreckaPuniNaziv);
            uplsKojiSeMijenja.SRECKA = _promjeneUplS.SRECKA;
            uplsKojiSeMijenja.IGRA = _igra.SIF_UPL.GetValueOrDefault();
            uplsKojiSeMijenja.RED_KOLO = _promjeneUplS.RED_KOLO;
            uplsKojiSeMijenja.OPSTINA = _promjeneUplS.OPSTINA;



            SpasiBazuUplS();
        }

        public EOP_SIN PronadjiUplS(int id)
        {
            EOP_SIN nadenaUplS = _uplSContext.EOP_SIN.Find(id);
            return nadenaUplS;
        }

        public void SpasiBazuUplS()
        {
            _uplSContext.SaveChanges();
        }
    }
}
