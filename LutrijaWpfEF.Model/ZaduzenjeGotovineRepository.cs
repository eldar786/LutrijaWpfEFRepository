using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace LutrijaWpfEF.Model
{
    public class ZaduzenjeGotovineRepository
    {
        private LutrijaEntities1 _zadGotContext = null;
        private ZADUZENJE_GOTOVINE _promjeneZadGot;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private string _pretraga;

        public ZaduzenjeGotovineRepository(ZADUZENJE_GOTOVINE zADUZENJE_GOTOVINE, komitenti_ime_matbr_zracun odabraniKomitent)
        {

            _promjeneZadGot = zADUZENJE_GOTOVINE;
            _odabraniKomitent = odabraniKomitent;

        }

        public void ObrisiZadGot()
        {
            ZADUZENJE_GOTOVINE zADUZENJE_GOTOVINE = PronadjiZadGot(_promjeneZadGot.ID);
            zADUZENJE_GOTOVINE.ZADUZITI_BLAGAJNIKA = _promjeneZadGot.ZADUZITI_BLAGAJNIKA;
            //...
        }


        public ZADUZENJE_GOTOVINE PronadjiZadGot(int id)
        {
            ZADUZENJE_GOTOVINE zADUZENJE_GOTOVINE = _zadGotContext.ZADUZENJE_GOTOVINE.Find(id);
            return zADUZENJE_GOTOVINE;
        }

        public void DodajZaduzenjeGotovine()
        {
            using (_zadGotContext = new LutrijaEntities1())
            {
                if (_promjeneZadGot.ID <= 0)
                {
                    ZADUZENJE_GOTOVINE zADUZENJE_GOTOVINE = new ZADUZENJE_GOTOVINE();
                    zADUZENJE_GOTOVINE.ZADUZITI_BLAGAJNIKA = _promjeneZadGot.ZADUZITI_BLAGAJNIKA;
                    zADUZENJE_GOTOVINE.ODOBRITI_BLAGAJNIKA = _promjeneZadGot.ODOBRITI_BLAGAJNIKA;
                    zADUZENJE_GOTOVINE.DATUM = _promjeneZadGot.DATUM;
                    zADUZENJE_GOTOVINE.IZNOS = _promjeneZadGot.IZNOS;
                    zADUZENJE_GOTOVINE.AKTIVAN = 1;
                    _zadGotContext.ZADUZENJE_GOTOVINE.Add(zADUZENJE_GOTOVINE);
                }
                else
                {
                    IzmijeniZadGot();
                }
                SpasiBazuZadGot();
            }
        }


        private void SpasiBazuZadGot()
        {
            _zadGotContext.SaveChanges();
        }

        private void IzmijeniZadGot()
        {
            ZADUZENJE_GOTOVINE zADUZENJE_GOTOVINE = PronadjiZadGot(_promjeneZadGot.ID);
            zADUZENJE_GOTOVINE.ZADUZITI_BLAGAJNIKA = _promjeneZadGot.ZADUZITI_BLAGAJNIKA;
            zADUZENJE_GOTOVINE.ODOBRITI_BLAGAJNIKA = _promjeneZadGot.ODOBRITI_BLAGAJNIKA;
            zADUZENJE_GOTOVINE.DATUM = _promjeneZadGot.DATUM;
            zADUZENJE_GOTOVINE.IZNOS = _promjeneZadGot.IZNOS;
            SpasiBazuZadGot();
        }

    }

}
