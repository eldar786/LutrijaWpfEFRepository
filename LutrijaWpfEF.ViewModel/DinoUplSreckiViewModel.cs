using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

namespace LutrijaWpfEF.ViewModel
{
    public class DinoUplSreckiViewModel : ObservableObject
    {
        private List<EOP_SIN> _pretragaSrecki;
        private ObservableCollection<EOP_SIN> _sveUplSrecki;
        private EOP_SIN _odabranaUplSrecki;
        private ApplicationViewModel _avm;
        private string _pretraga;
        public ICommand DodajCommand { get; set; }
        public ICommand IzmijeniCommand { get; set; }

        public DinoUplSreckiViewModel(ApplicationViewModel avm)
        {
            _avm = avm;
            SveUplSrecki = new ObservableCollection<EOP_SIN>();
            _pretragaSrecki = NapuniUplSrecki();

            foreach (EOP_SIN p in _pretragaSrecki)
            {
                SveUplSrecki.Add(p);
            }

            Sortiraj();

            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);
        }

        private void Sortiraj()
        {
            ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_sveUplSrecki);
            komitentiView.SortDescriptions.Clear();
            komitentiView.SortDescriptions.Add(new SortDescription("DATUM", ListSortDirection.Descending));
        }
        public void TraziUplSrecki(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {
                SveUplSrecki = new ObservableCollection<EOP_SIN>(from i in _sveUplSrecki
                                                                 where i.OP_BROJ.ToString().IndexOf(_pretraga) >= 0
                                                                 select i);
            }
            else
            {
                SveUplSrecki.Clear();

                if (_pretragaSrecki != null)
                {

                    foreach (EOP_SIN uplata in _pretragaSrecki)
                    {
                        SveUplSrecki.Add(uplata);
                    }
                }
            }
            Sortiraj();
        }
        private void Dodaj()
        {

            if (_avm.OdabraniVM == this)
            {

                _odabranaUplSrecki = new EOP_SIN();

                _avm.OdabraniVM = new IzmijeniUplSreckiDinoViewModel(_avm, _odabranaUplSrecki);

            }
        }

        private void Izmijeni()
        {

            if (_avm.OdabraniVM == this)
            {
                _avm.OdabraniVM = new IzmijeniUplSreckiDinoViewModel(_avm, _odabranaUplSrecki);

            }
        }

        private List<EOP_SIN> NapuniUplSrecki()
        {
            //List<EOP_SIN> upl = new List<EOP_SIN>();
            //List<IGRE> igre = this._avm.Gr.Igre;
            var context1 = new LutrijaEntities1();

            List<EOP_SIN> upl = context1.EOP_SIN.ToList();
            List<IGRE> igre = context1.IGRE.ToList();

            List<EOP_SIN> sve = this._avm.Gr.UplateOsnovneIgre;
            using (var context = new LutrijaEntities1())
            {

                // upl = context.EOP_SIN.Where(s => s.SRECKA > 0).ToList();

                foreach (EOP_SIN es in sve)
                {
                    int? sr = (from i in igre
                               where i.SIF_UPL == es.IGRA
                               select i.SIF_BAZ_IGR).First();

                    if (sr == 4)
                    {
                        upl.Add(es);
                    }
                }


                //foreach(EOP_SIN es in sve)
                //{
                //    int? sr = (from i in igre
                //              where i.SIF_UPL == es.IGRA
                //              select i.SIF_BAZ_IGR).First();

                //    if (sr==4)
                //    {
                //        upl.Add(es);
                //    }
                //}



                foreach (EOP_SIN u in upl)
                {
                    u.NazivIgre = "Srecka";
                }
            }
            return upl;
        }

        public ObservableCollection<EOP_SIN> SveUplSrecki { get => _sveUplSrecki; set { _sveUplSrecki = value; OnPropertyChanged("SveUplSrecki"); } }
        public EOP_SIN OdabranaUplSrecki { get => _odabranaUplSrecki; set { _odabranaUplSrecki = value; OnPropertyChanged("OdabranaUplSrecki"); } }

        public string Pretraga { get => _pretraga; set { _pretraga = value; TraziUplSrecki(_pretraga); } }
    }
}
