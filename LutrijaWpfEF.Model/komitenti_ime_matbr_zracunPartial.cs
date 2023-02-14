using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    partial class komitenti_ime_matbr_zracun:ObservableObject
    {
        private int _sifRegiona;
        public int SifRegiona { get => _sifRegiona; set { _sifRegiona = value; OnPropertyChanged("SifRegiona"); } }
    }
}
