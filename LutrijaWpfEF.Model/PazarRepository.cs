using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    public class PazarRepository
    {
        private LutrijaEntities1 _pazarContext = null;
        private POLOG_PAZAR _promjenePazar;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        //private POLOG_PAZAR _pologPazar;
        public PazarRepository(POLOG_PAZAR promjenePazar, komitenti_ime_matbr_zracun odabraniKomitent)
        {
          
            _promjenePazar = promjenePazar;
            _odabraniKomitent = odabraniKomitent;
        }

        public void DodajPazar()
        {
            using (_pazarContext = new LutrijaEntities1())
            { 
            if (_promjenePazar.ID<=0)
            {
                POLOG_PAZAR Pazar = new POLOG_PAZAR();
                Pazar.DATUM = _promjenePazar.DATUM;
                Pazar.IZNOS = _promjenePazar.IZNOS;
                Pazar.OPIS = _promjenePazar.OPIS;
                Pazar.TIP_DOKUMENTA = _promjenePazar.TIP_DOKUMENTA;
                Pazar.OP_BROJ_PROD = _odabraniKomitent.KOMITENT;
                _pazarContext.POLOG_PAZAR.Add(Pazar);
            }
            else
            {
                IzmjeniPazar();
            }
            SpasiBazuPazar();
            }
        }

        public void IzmjeniPazar()
        {
            POLOG_PAZAR pazarKojiSeMijenja =PronadjiPazar(_promjenePazar.ID);
            pazarKojiSeMijenja.DATUM = _promjenePazar.DATUM;
            pazarKojiSeMijenja.IZNOS = _promjenePazar.IZNOS;
            pazarKojiSeMijenja.OPIS = _promjenePazar.OPIS;
            pazarKojiSeMijenja.OP_BROJ_PROD = _promjenePazar.OP_BROJ_PROD;
            pazarKojiSeMijenja.TIP_DOKUMENTA = _promjenePazar.TIP_DOKUMENTA;

            SpasiBazuPazar();
        }

        public POLOG_PAZAR PronadjiPazar(int id)
        {
          POLOG_PAZAR nadeniPazar= _pazarContext.POLOG_PAZAR.Find(id);
          return nadeniPazar;
        }

        public void SpasiBazuPazar()
        {
            _pazarContext.SaveChanges();
        }
}
}
