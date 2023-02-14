using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LutrijaWpfEF.ViewModel
{
    public class Poruke
    {     
            public void Uspjeh()
            {
                MessageBox.Show("Podaci spašeni!");
            }

            public void Greska(Exception e)
            {
            MessageBox.Show("Došlo je do greške " + e);
            }
    }
}
