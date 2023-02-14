using LutrijaWpfEF.UI.Windows;
using LutrijaWpfEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LutrijaWpfEF.ViewModel.OrgViewModel;

namespace LutrijaWpfEF.UI
{
    /// <summary>
    /// Interaction logic for OrgView.xaml
    /// </summary>
    public partial class OrgView
    {

        private int _errors = 0;
        string _pocStanje;
        string _automati;
        public OrgView()
        {
            InitializeComponent();
            
        }

        private void Confirm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {   
            e.CanExecute = _errors == 0;
            e.Handled = true;
        }

        private void Confirm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;

        }
        public void OtvoriKomitente()
        {
            //KomitentiWindow komitenti = new KomitentiWindow(KomitentiView view);
        }
        private void datOd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void datDo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

      
        private void stampa_button(object sender, RoutedEventArgs e)
        {

            

            if (PocStanjePlus.Text !="0")
            {
                _pocStanje = PocStanjePlus.Text;
            }
            else if (PocStanjeMinus.Text !="0")
            {
                _pocStanje = PocStanjeMinus.Text;
            }


            string Od = dtpOd.Text;
            string Do = dtpDo.Text;
            string _KomImePrez = KomitentImePrez.Text;
            int _OpBrojInt;
            Int32.TryParse(OpBroj.Text, out _OpBrojInt);
            string _OpBroj = _OpBrojInt.ToString();

            string _Online = Online.Text;
            string _KladionicaUpl = KladionicaUpl.Text;
            string _Srecke = Srecke.Text;
            string _Porez = Porez.Text;
            string _PologPazara = PologPazara.Text;
            string _automati = Automati.Text;
            string _RealTimeKlIgre = RealTImeKlIgre.Text;
            string _KladionicaIspl = KladionicaIspl.Text;
            string _WebIspl = WebIsplata.Text;
            string _UkupanPrometUplata = UkupanPrometUplata.Text;
            string _UkupanIsplata = UkupanPrometIsplata.Text;
            string _Saldo = Saldo.Text;
            string _OstalaZaduzenja = OstZad.Text;

            string _NazivIgre;

            

            //DataTemplate cellTemplate = 
            OrgViewModel.UplataIsplataSkupa lll = new OrgViewModel.UplataIsplataSkupa();
            string kladUpl = lll.IznosUplate.ToString();

            UplataIsplataSkupa uplataIsplataSkupa = new UplataIsplataSkupa();

            string _IznosUplate = uplataIsplataSkupa.IznosUplate.ToString();

            //foreach (UplataIsplataSkupa item in uplataIsplataSkupas)
            //{
            //    string _NazivIgreKlad = item.NazivIgre;
            //    nazivIgara.Add(_NazivIgreKlad);
            //}

            string _Igre = Klad_Igra.DisplayMemberBinding.StringFormat;

            List<UplataIsplataSkupa> _uplIsplKlad = new List<UplataIsplataSkupa>();
            //OrgViewModel orgViewModelKladList = new OrgViewModel(_uplIsplKlad);


            //OrgViewModel orgViewModel = new OrgViewModel();
            //PRAZNO????
            //foreach (UplataIsplataSkupa item in orgViewModelKladList.UplateIsplateSkupaKladionica)
            //{
            //    _uplIsplKlad.Add(item);
            //}

            //Dobiti podatke za kladionicu??
            //string _NazivIgre = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Klad_Igra"));

            IzvjestajOrg izvjestajOrg = new IzvjestajOrg(Od, Do, _pocStanje, _KomImePrez, _Online, _KladionicaUpl, _Srecke, _Porez, _PologPazara, _automati, 
                _RealTimeKlIgre, _KladionicaIspl, _WebIspl, _UkupanPrometUplata, _UkupanIsplata, _Saldo, _OstalaZaduzenja, _Igre, _OpBroj);


            izvjestajOrg.Show();


            //using (OrgViewWindow orgViewWindow = new OrgViewWindow(orgViewModel, list))
            //{
            //    orgViewWindow.Show();
            //}


            //PrintOrg print = new PrintOrg();
            //print.Stampa();
        }

        private void NapraviPdf(object sender, RoutedEventArgs e)
        {
           
                new IzvjestajOrg();
            
        }

        private void Klad_Igra_Copying(object sender, DataObjectCopyingEventArgs e)
        {

        }
    }
}
