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

namespace LutrijaWpfEF.ViewModel
{


    public class IzvjestajViewModel : ObservableObject
    {
        private ApplicationViewModel _avm;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private GlavniViewModel _gvm;
       
        private OrgViewModel _org;


        public ICommand PrintCommand { get; set; }

        public IzvjestajViewModel(OrgViewModel org)
        {
            
            _org = org;
            _odabraniKomitent = _org.OdabraniKomitent;
            //_odabraniKomitent.IME = "Odaberi Komitenta";
           

        }

        public void Print()
        {
            if (OdabraniKomitent != null)
            {
               


              

            }
        }




        public void Odustani()
        {
            //_avm.OdabraniVM = new IzvjestajViewModel(_avm);
        }

        public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; OnPropertyChanged("OdabraniKomitent"); } }


    }

}
