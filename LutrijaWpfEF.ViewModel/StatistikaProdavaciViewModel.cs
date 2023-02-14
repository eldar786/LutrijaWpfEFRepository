using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.ViewModel
{
    public class StatistikaProdavaciViewModel : ObservableObject
    {
        private ApplicationViewModel _avm;
        private List<POC_STANJA> _pocetnaStanja;
        private List<POC_STANJA> _uplateOsnIgre;
        private List<komitenti_ime_matbr_zracun> _komitenti;
        private List<PrikazUplate> _prikazi;
        private PrikazUplate _prikaz;
      

        public StatistikaProdavaciViewModel(ApplicationViewModel avm)
        {
            _avm = avm;
            
            _prikazi = new List<PrikazUplate>();
            _pocetnaStanja = this._avm.Gr.NapuniPocStanja();
            _komitenti = this._avm.Gr.Komitenti;
            _uplateOsnIgre = (from p in _pocetnaStanja
                              where p.OP_BROJ  != 1537 && p.MJESEC == DateTime.Now.Month && p.GODINA == DateTime.Now.Year
                              orderby p.UPLATA_OSN_IGRE descending
                              select p).Take(5).ToList();


           
            foreach (POC_STANJA up in _uplateOsnIgre)
            {
                _prikaz = new PrikazUplate();
                _prikaz.imePrezimeUpl = (from komitenti_ime_matbr_zracun kom in _komitenti
                               where kom.KOMITENT == up.OP_BROJ.ToString().PadLeft(5, '0')
                               select kom.IME + " " + kom.PREZIME).First();

                _prikaz.opBrojUpl = up.OP_BROJ;

                _prikaz.mjesecUpl = up.MJESEC;
                _prikaz.godinaUpl = up.GODINA;
                _prikaz.uplataOsIg = up.UPLATA_OSN_IGRE;

                _prikazi.Add(_prikaz);
            }
           
        }

        public class PrikazUplate : ObservableObject
        {
            public string imePrezimeUpl { get; set; }
            public int? opBrojUpl { get; set; }
            public int? mjesecUpl { get; set; }
            public int? godinaUpl { get; set; }
            public decimal? uplataOsIg { get; set; }


        }



        public List<POC_STANJA> UplateOsnIgre { get => _uplateOsnIgre; set { _uplateOsnIgre = value; OnPropertyChanged("UplateOsnIgre"); } }
        public List  <PrikazUplate> Prikazi{ get => _prikazi; set { _prikazi = value; OnPropertyChanged("Prikazi"); } }

       
    }
}
