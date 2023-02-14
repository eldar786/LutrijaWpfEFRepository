using Caliburn.Micro;
using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
    public class IsplataSreckiViewModel : ObservableObject
    {
        private List<ISPLATA> _pretragaIsplS;
        private ObservableCollection<ISPLATA> _sveIsplateSrecki;
        private ISPLATA _odabranaIsplataS;
        private ApplicationViewModel _avm;
        private string _pretraga;
        public ICommand DodajCommand { get; set; }
        public ICommand IzmijeniCommand { get; set; }

        public IsplataSreckiViewModel(ApplicationViewModel avm)
        {
            _avm = avm;
            SveIsplateSrecki = new ObservableCollection<ISPLATA>();
            _pretragaIsplS = NapuniIsplS();

            foreach (ISPLATA i in _pretragaIsplS)
            {
                SveIsplateSrecki.Add(i);
            }

            //Sortiraj();

            this.DodajCommand = new RelayCommand(Dodaj);
            this.IzmijeniCommand = new RelayCommand(Izmijeni);
        }

        private void Sortiraj()
        {
            ICollectionView komitentiView = CollectionViewSource.GetDefaultView(_sveIsplateSrecki);
            komitentiView.SortDescriptions.Clear();
            komitentiView.SortDescriptions.Add(new SortDescription("LIS_VRISPL", ListSortDirection.Descending));
        }

        public void TraziIsplS(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {
                SveIsplateSrecki = new ObservableCollection<ISPLATA>(from i in _sveIsplateSrecki
                                                                     where i.LIS_ISPL.IndexOf(_pretraga) >= 0
                                                                     select i);
            }
            else
            {
                SveIsplateSrecki.Clear();

                if (_sveIsplateSrecki != null)
                {

                    foreach (ISPLATA isplata in _sveIsplateSrecki)
                    {
                        SveIsplateSrecki.Add(isplata);
                    }
                }
            }
            Sortiraj();
        }

        private void Dodaj()
        {

            if (_avm.OdabraniVM == this)
            {

                _odabranaIsplataS = new ISPLATA();

                _odabranaIsplataS.LIS_VRISPL = DateTime.Now;


                _avm.OdabraniVM = new IzmijeniIsplSreckiViewModel(_avm, _odabranaIsplataS);

            }
        }

        private void Izmijeni()
        {

            if (_avm.OdabraniVM == this)
            {
                _avm.OdabraniVM = new IzmijeniIsplSreckiViewModel(_avm, _odabranaIsplataS);

            }
        }

        private List<ISPLATA> NapuniIsplS()
        {
            List<ISPLATA> ispl = new List<ISPLATA>();
            using (var context = new LutrijaEntities1())
            {
                ispl = context.ISPLATA.Where(s => s.LIS_SERIJA_SRE == 1).ToList();


            }
            return ispl;
        }

        public ObservableCollection<ISPLATA> SveIsplateSrecki { get => _sveIsplateSrecki; set { _sveIsplateSrecki = value; OnPropertyChanged("SveIsplateSrecki"); } }
        public ISPLATA OdabranaIsplataS { get => _odabranaIsplataS; set { _odabranaIsplataS = value; OnPropertyChanged("OdabranaIsplataS"); } }

        public string Pretraga { get => _pretraga; set { _pretraga = value; TraziIsplS(_pretraga); } }

    }
}
