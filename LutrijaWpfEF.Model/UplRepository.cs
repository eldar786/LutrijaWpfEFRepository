using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    public class UplRepository
    {
        private LutrijaEntities1 _uplOContext = null;
        private EOP_SIN _promjeneUplO;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private IGRE _igra;
        private string igraString;

        public UplRepository(EOP_SIN promjeneUplO,  IGRE igra)
        {
            _promjeneUplO = promjeneUplO;
            //_odabraniKomitent = odabraniKomitent;
            _igra = igra;
            igraString = _igra.NAZIV;
            int? idIgra = _igra.SIF_ISP;
        }

        public void DodajUplO()
        {
            using (_uplOContext = new LutrijaEntities1())
            {
                if (_promjeneUplO.ID <= 0)
                {
                    EOP_SIN Uplata = new EOP_SIN();
                    Uplata.DATUM = _promjeneUplO.DATUM;
                    Uplata.UPL = _promjeneUplO.UPL;
                    //Iplata.OPIS = _promjeneUplO.OPIS;
                    Uplata.IGRA = _igra.SIF_UPL.GetValueOrDefault();
                    Uplata.RED_KOLO = _promjeneUplO.RED_KOLO;
                    Uplata.OPSTINA = _promjeneUplO.OPSTINA;
                    Uplata.OP_BROJ = _promjeneUplO.OP_BROJ;
                    Uplata.SRECKA = 0;
                    _uplOContext.EOP_SIN.Add(Uplata);
                }
                else
                {
                    IzmjeniUplO();
                }
                SpasiBazuUplO();
            }
        }

        public void IzmjeniUplO()
        {
            EOP_SIN uplOsnovnihKojiSeMijenja = PronadjiUplO(_promjeneUplO.ID);
            uplOsnovnihKojiSeMijenja.DATUM = _promjeneUplO.DATUM;
            uplOsnovnihKojiSeMijenja.UPL = _promjeneUplO.UPL;
            //uplOsnovnihKojiSeMijenja.OPIS = _promjeneUplO.OPIS;
            uplOsnovnihKojiSeMijenja.OP_BROJ = _promjeneUplO.OP_BROJ;
            uplOsnovnihKojiSeMijenja.IGRA = _igra.SIF_UPL.GetValueOrDefault();
            uplOsnovnihKojiSeMijenja.RED_KOLO = _promjeneUplO.RED_KOLO;
            uplOsnovnihKojiSeMijenja.OPSTINA = _promjeneUplO.OPSTINA;
            uplOsnovnihKojiSeMijenja.SRECKA = 0;

            SpasiBazuUplO();
        }

        public EOP_SIN PronadjiUplO(int id)
        {
            EOP_SIN nadenaUplO = _uplOContext.EOP_SIN.Find(id);
            return nadenaUplO;
        }

        public void SpasiBazuUplO()
        {
            _uplOContext.SaveChanges();
        }

    }
}
