using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    public class IsplOsnovnihRepository
    {
        private LutrijaEntities1 _isplOContext = null;
        private ISPLATA _promjeneIsplO;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private IGRE _igra;
        private string igraString;

        public IsplOsnovnihRepository(ISPLATA promjeneIsplO, IGRE igra)
        {

            _promjeneIsplO = promjeneIsplO;
           // _odabraniKomitent = odabraniKomitent;
            _igra = igra;
            igraString = _igra.NAZIV;
            int? idIgra = _igra.SIF_ISP;
        }

        public void DodajIsplO()
        {
            using (_isplOContext = new LutrijaEntities1())
            {
                if (_promjeneIsplO.ID <= 0)
                {
                    ISPLATA Iplata = new ISPLATA();
                    Iplata.LIS_VRISPL = _promjeneIsplO.LIS_VRISPL;
                    Iplata.LIS_IZNOS = _promjeneIsplO.LIS_IZNOS;
                    //Iplata.OPIS = _promjeneIsplO.OPIS;
                    Iplata.LIS_IGRA = igraString;
                    Iplata.LIS_KOLO = _promjeneIsplO.LIS_KOLO;
                    Iplata.LIS_ISPL = _promjeneIsplO.LIS_ISPL;
                    Iplata.SIF_IGRE = _igra.SIF_ISP;
                    Iplata.LIS_SERIJA_SRE = 0;
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
            ISPLATA isplOsnovnihKojiSeMijenja = PronadjiIsplO(_promjeneIsplO.ID);
            isplOsnovnihKojiSeMijenja.LIS_VRISPL = _promjeneIsplO.LIS_VRISPL;
            isplOsnovnihKojiSeMijenja.LIS_IZNOS = _promjeneIsplO.LIS_IZNOS;
            //isplOsnovnihKojiSeMijenja.OPIS = _promjeneIsplO.OPIS;
            isplOsnovnihKojiSeMijenja.LIS_ISPL = _promjeneIsplO.LIS_ISPL;
            isplOsnovnihKojiSeMijenja.LIS_IGRA = igraString;
            isplOsnovnihKojiSeMijenja.LIS_KOLO = _promjeneIsplO.LIS_KOLO;
            isplOsnovnihKojiSeMijenja.SIF_IGRE = _igra.SIF_ISP;
            isplOsnovnihKojiSeMijenja.LIS_SERIJA_SRE = 0;
            SpasiBazuIsplO();
        }

        public ISPLATA PronadjiIsplO(int id)
        {
            ISPLATA nadenaIsplO = _isplOContext.ISPLATA.Find(id);
            return nadenaIsplO;
        }

        public void SpasiBazuIsplO()
        {
            _isplOContext.SaveChanges();
        }

    }
}
