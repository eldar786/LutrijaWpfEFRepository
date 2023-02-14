using LutrijaWpfEF.Model;
using LutrijaWpfEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LutrijaWpfEF.UI
{
    /// <summary>
    /// Interaction logic for PologPazara.xaml
    /// </summary>
    public partial class PologPazara : UserControl
    {
        //private ApplicationViewModel _avm;
        private string dc = @"Server=data source=192.168.1.213\LUTRIJASQL,1433;
                            Network Library=DBMSSOCN;initial catalog=Lutrija;multipleactiveresultsets=True;application name=EntityFramework";
        
        private ApplicationViewModel _av;
        private komitenti_ime_matbr_zracun _odabraniKomitent;
        private ObservableCollection<komitenti_ime_matbr_zracun> _sviKomitenti;
        public ICommand KomitentiCommand { get; set; }

        LutrijaEntities1 db = new LutrijaEntities1();
        private POLOG_PAZAR _polog = new POLOG_PAZAR();

        private int _errors = 0;

        public decimal IZNOS { get; set; }


        public PologPazara(ApplicationViewModel avm)
        {
            //View model kreiramo explicitno unutar konstruktora 
            PologPazaraViewModel pologPazaraViewModel = new PologPazaraViewModel(Mediator.Instance);
            this.DataContext = pologPazaraViewModel;

            _av = avm;
            _odabraniKomitent = new komitenti_ime_matbr_zracun();
            _odabraniKomitent.IME = "Odaberi Komitenta";

            this.KomitentiCommand = new RelayCommand(Komitenti);
        }

        private void Komitenti()
        {

            if (_av.OdabraniVM == this)
            {

              //  _av.OdabraniVM = new OdaberiKomitentaViewModel(_av);

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnpropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public PologPazara()
        {
            InitializeComponent();

            //Ovo zakomentarisemo ako zelimo da se promjene vide na listView i na pogledu koment Datacontext-DataTemplate
            //Ovdje je zakomentarisano, jer cemo ipak podatke da dobivamo instaniranjem kroz pogled, jer nam treba Dinin Odaberi Komitent       
            //"<DataTemplate DataType="{x:Type vm:PologPazaraViewModel}">"


            //ViewModel kreiramo Eksplicitno unutar konstruktora i njemu prosljedjujemo instancu Mediatora 
            //PologPazaraViewModel pologPazaraViewModel = new PologPazaraViewModel(Mediator.Instance);
            //////ovakav ViewModel postavljamo za DataContext trenutnog prozora 
            //this.DataContext = pologPazaraViewModel;

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

        private void datOd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        //private int ProvjeriDatum(DateTime dat, string procedura)
        //{
        //    int rezultat;

        //    using (SqlConnection con = new SqlConnection(dc))
        //    {
        //        con.Open();

        //        using (SqlCommand cmd = new SqlCommand(procedura, con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@datum", SqlDbType.Date).Value = dat;
        //            rezultat = (int)cmd.ExecuteScalar();
        //            con.Close();
        //            return rezultat;
        //        }
        //    }
        //}


        private void btnSacuvajFormu_Click(object sender, RoutedEventArgs e)
        {
            #region Funkcionalnost SACUVAJ(INSERT)

            //string Komitent = txt_komitent.Text;
            //string Datum = txt_datum.Text;
            //string Iznos = txt_iznos.Text;
            //string Opis = txt_opis.Text;
            //string tipDok = txt_tipDokumenta.Text;                                                                  

            //using (SqlConnection conn = new SqlConnection())
            //{
            //    conn.ConnectionString = @"Server=DB-SRV-01;Database=Lutrija;MultipleActiveResultSets=true;User ID=sa;Password=Lutrija1; TrustServerCertificate=True;".ToString();
            //    conn.Open();

            //    SqlCommand command = new SqlCommand("INSERT INTO dbo.POLOG_PAZAR(TIP_DOKUMENTA,DATUM,OP_BROJ_PROD,IZNOS,OPIS) " +
            //        "values(@tip_dokumenta, @datum, @op_broj_prod, @iznos, @opis)", conn);

            //    SqlParameter tipDokumentaParam = new SqlParameter("@tip_dokumenta", SqlDbType.NVarChar);
            //    tipDokumentaParam.Value = tipDok;

            //    SqlParameter datumParam = new SqlParameter("@datum", SqlDbType.DateTime);
            //    datumParam.Value = Datum;

            //    SqlParameter opBrProdParam = new SqlParameter("@op_broj_prod", SqlDbType.NVarChar);
            //    opBrProdParam.Value = Komitent;

            //    SqlParameter iznosParam = new SqlParameter("@iznos", SqlDbType.Decimal);
            //    iznosParam.Value = Iznos;

            //    SqlParameter opisParam = new SqlParameter("@opis", SqlDbType.NVarChar);
            //    opisParam.Value = Opis;

            //    command.Parameters.Add(tipDokumentaParam);
            //    command.Parameters.Add(datumParam);
            //    command.Parameters.Add(opBrProdParam);
            //    command.Parameters.Add(iznosParam);
            //    command.Parameters.Add(opisParam);

            //    command.ExecuteNonQuery();

            //    MessageBox.Show("Uspješno uneseni podaci u tabelu");
            //    conn.Close();
            //}
            #endregion

            NewEditPologPazaraWindow newEditPologPazaraWindow = new NewEditPologPazaraWindow();

            //INJEKTUJU SE PODACI IZ VIEWMODELA U POGLED
            //Koristimo prazan kostr jer unosimo novu vrijednost
            newEditPologPazaraWindow.DataContext = new NewEditPologPazarViewModel(Mediator.Instance);
            newEditPologPazaraWindow.ShowDialog();
        }

        private void btnIzmjeniFormu_Click(object sender, RoutedEventArgs e)
        {
            PologPazaraViewModel pologPazaraViewModel = (PologPazaraViewModel)DataContext;
            NewEditPologPazaraWindow newEditPologPazaraWindow = new NewEditPologPazaraWindow();
            //Ovom metodom clone, ne isporucujem direktno objekat iz kolekcije, vec njegovu kopiju
           // newEditPologPazaraWindow.DataContext = new NewEditPologPazarViewModel(pologPazaraViewModel.CurrentPologPazar.Clone(), Mediator.Instance);
            newEditPologPazaraWindow.ShowDialog();
        }

        private void btnOdustani_Click(object sender, RoutedEventArgs e)
        {
            txt_datum.Text = String.Empty;
            txt_iznos.Text = String.Empty;
            txt_komitent.Text = String.Empty;
            txt_opis.Text = String.Empty;
            txt_tipDokumenta.Text = String.Empty;
        }

        //public komitenti_ime_matbr_zracun OdabraniKomitent { get => _odabraniKomitent; set { _odabraniKomitent = value; 
        //        OnPropertyChanged("OdabraniKomitent"); } }

        //private void datDo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //}
    }
}
