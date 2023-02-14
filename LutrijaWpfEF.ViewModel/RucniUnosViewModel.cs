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
    public class RucniUnosViewModel : ObservableObject
    {
        private ApplicationViewModel _av;

        private komitenti_ime_matbr_zracun _odabraniKomitent;

        //private ObservableCollection<IGRE> sveIgre;
        //private IGRE igra;
        private List<IGRE> igreList;
        public ICommand KomitentiCommand { get; set; }


        public RucniUnosViewModel(ApplicationViewModel avm)
        {
            var IgreContext = new LutrijaEntities1();
            igreList = IgreContext.IGRE.ToList();
            //SveIgre = new ObservableCollection<IGRE>();

            _av = avm;
            _odabraniKomitent = new komitenti_ime_matbr_zracun();
            _odabraniKomitent.IME = "Odaberi Komitenta";

            //igra = new IGRE();
            //igra.NAZIV = "Odaberi naziv igre";

            this.KomitentiCommand = new RelayCommand(Komitenti);
        }

        public RucniUnosViewModel()
        {
            //GrKomList = GrKomCollection.GetAllGrKom();
            //GrKomListView = new ListCollectionView(GrKomList);
        }

        private void Komitenti()
        {

            if (_av.OdabraniVM == this)
            {

               // _av.OdabraniVM = new OdaberiKomitentaViewModel(_av);

            }
        }

        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }
        public List<IGRE> SveIgre { get => igreList; set { igreList = value; OnPropertyChanged("SviKomitenti"); } }

    }
}