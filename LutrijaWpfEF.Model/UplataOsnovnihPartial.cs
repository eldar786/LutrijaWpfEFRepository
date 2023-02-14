using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    public partial class EOP_SIN : ObservableObject
    {
        private string _nazivIgre;
        public string NazivIgre { get => _nazivIgre; set { _nazivIgre = value; OnPropertyChanged("NazivIgre"); } }

    }
}
