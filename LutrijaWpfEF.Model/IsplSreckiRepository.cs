using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    public class IsplSreckiRepository
    {
        private LutrijaEntities1 _isplOContext = null;
        private ISPLATA _promjeneIsplS;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private IGRE _igra;
        private string igraString;


        public IsplSreckiRepository(ISPLATA promjeneIsplS, komitenti_ime_matbr_zracun odabraniKomitent, IGRE igra)
        {

            _promjeneIsplS = promjeneIsplS;
            _odabraniKomitent = odabraniKomitent;
            _igra = igra;
            igraString = _igra.NAZIV;
            int? idIgra = _igra.SIF_ISP;
        }

        public void DodajIsplO()
        {
            using (_isplOContext = new LutrijaEntities1())
            {
                if (_promjeneIsplS.ID <= 0)
                {
                    ISPLATA Iplata = new ISPLATA();
                    Iplata.LIS_VRISPL = _promjeneIsplS.LIS_VRISPL;
                    Iplata.LIS_IZNOS = _promjeneIsplS.LIS_IZNOS;
                    //Iplata.OPIS = _promjeneIsplS.OPIS;
                    Iplata.LIS_IGRA = igraString;
                    Iplata.LIS_KOLO = _promjeneIsplS.LIS_KOLO;
                    Iplata.LIS_ISPL = _odabraniKomitent.KOMITENT;
                    Iplata.SIF_IGRE = _igra.SIF_ISP;
                    _isplOContext.ISPLATA.Add(Iplata);
                }
                else
                {
                    IzmjeniIsplO();
                }
                SpasiBazuIsplO();
            }
        }

        public void IzmjeniIsplO()
        {
            ISPLATA isplSreckiKojiSeMijenja = PronadjiIsplO(_promjeneIsplS.ID);
            isplSreckiKojiSeMijenja.LIS_VRISPL = _promjeneIsplS.LIS_VRISPL;
            isplSreckiKojiSeMijenja.LIS_IZNOS = _promjeneIsplS.LIS_IZNOS;
            //isplSreckiKojiSeMijenja.OPIS = _promjeneIsplS.OPIS;
            isplSreckiKojiSeMijenja.LIS_ISPL = _promjeneIsplS.LIS_ISPL;
            isplSreckiKojiSeMijenja.LIS_IGRA = igraString;
            isplSreckiKojiSeMijenja.SIF_IGRE = _igra.SIF_ISP;

            SpasiBazuIsplO();
        }

        public ISPLATA PronadjiIsplO(int id)
        {
            ISPLATA nadenaIsplS = _isplOContext.ISPLATA.Find(id);
            return nadenaIsplS;
        }

        public void SpasiBazuIsplO()
        {
            _isplOContext.SaveChanges();
        }

    }
}