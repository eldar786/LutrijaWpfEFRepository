using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;


//ubaceno
using LutrijaWpfEF;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;

using System.Data;
using System.Globalization;
using System.IO;

using System.Text;
using System.Text.RegularExpressions;



using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LutrijaWpfEF.ViewModel;

namespace LutrijaWpfEF.UI
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : UserControl
    {
        //        string output = "\\\\192.168.1.213\\Users\\Share Import\\Import EOP UPLATE\\Arhiva\\import" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".csv";
        //        public string sFileName;
        //        public string red;
        //private string dc = @"Server=DB-SRV-01;Database=Lutrija;MultipleActiveResultSets=true;User ID=sa;Password=Lutrija1;
        //                TrustServerCertificate=True;";
        //        private string query = @"INSERT INTO PROVJERA_KLADIONICA VALUES (@ime)";
        //        private List<string> lista;
        //        private string brojListica;
        //        private double sumaIsplata;
        //        private readonly object _lock = new object();
        
      
        //ImportViewModel importViewModel = new ImportViewModel();

        public ImportView()
        {
            InitializeComponent();
            
        }

     

        //private void EOP_Analitika(object sender, RoutedEventArgs e)
        //{

        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    //openFileDialog.Filter = "CSV files (*.csv)|*.csv";
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        _sFileName = openFileDialog.FileName;
        //    }

        //    _sFileName = Helpers.Pathing.GetUNCPath(_sFileName);


        //    try
        //    {
        //        this.importViewModel.Imp_Eop_Ana(_sFileName);
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show($"Došlo je do greške: {ex.Message}, stacktrace je: {ex.StackTrace}");
        //    }
        //}

      

       

       

    }
}
