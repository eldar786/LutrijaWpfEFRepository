using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    public class StanjeRepository
    {
        private LutrijaEntities1 _stanjeContext = null;
        private POC_STANJA _promjeneStanje;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        //private POLOG_PAZAR _pologPazar;
        public StanjeRepository(POC_STANJA promjeneStanje, komitenti_ime_matbr_zracun odabraniKomitent)
        {

            _promjeneStanje = promjeneStanje;
            _odabraniKomitent = odabraniKomitent;
        }

        public void DodajStanje()
        {
            using (_stanjeContext = new LutrijaEntities1())
            {
                if (_promjeneStanje.ID <= 0)
                {
                    POC_STANJA Stanje = new POC_STANJA();
                    Stanje.MJESEC = _promjeneStanje.MJESEC;
                    Stanje.POC_STANJE = _promjeneStanje.POC_STANJE;
                    Stanje.GODINA = _promjeneStanje.GODINA;
                    
                    Stanje.OP_BROJ = int.Parse(_odabraniKomitent.KOMITENT);
                    _stanjeContext.POC_STANJA.Add(Stanje);
                }
                else
                {
                    IzmjeniStanje();
                }
                SpasiBazuStanje();
            }
        }

        public void IzmjeniStanje()
        {
            POC_STANJA stanjeKojeSeMijenja = PronadjiStanje(_promjeneStanje.ID);
            stanjeKojeSeMijenja.MJESEC = _promjeneStanje.MJESEC;
            stanjeKojeSeMijenja.GODINA = _promjeneStanje.GODINA;
            stanjeKojeSeMijenja.POC_STANJE = _promjeneStanje.POC_STANJE;
            
            SpasiBazuStanje();
        }

        public POC_STANJA PronadjiStanje(int id)
        {
            POC_STANJA nadenoStanje = _stanjeContext.POC_STANJA.Find(id);
            return nadenoStanje;
        }

        public void SpasiBazuStanje()
        {
            _stanjeContext.SaveChanges();
        }
    }
}
