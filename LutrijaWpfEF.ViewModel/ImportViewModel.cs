using LutrijaWpfEF.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;


namespace LutrijaWpfEF.ViewModel
{
    public class ImportViewModel : ObservableObject
    {
        private GlavniViewModel _gvm;
        private ApplicationViewModel _avm;
        string putanjaDatumIme = @"\\192.168.1.213\Users\Share Import\Import EOP UPLATE\Arhiva\import" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".csv";
        private string _sFileName;
        private string sFileName;
        private DateTime datumIsp;
        string _ime;
        private string dc = @"Server=192.168.1.213;Database=Lutrija;MultipleActiveResultSets=true;User ID=sa;Password=Lutrija1;
                        TrustServerCertificate=True;";
        string output = @"\\192.168.1.213\Users\Share Import\Import EOP UPLATE\Arhiva\import" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".csv";
        private string query = @"INSERT INTO PROVJERA_KLADIONICA VALUES (@ime)";
        string arhivaGreska = @"\\192.168.1.213\Users\Share Import\Import EOP UPLATE\ArhivaGreska\import" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + "greska";
        private List<string> lista;
        private string brojListica;
        private double sumaIsplata;
        private readonly object _lock = new object();
        CancellationTokenSource cts;
        private DateTime _datumKladionicaUplate;
        private string _sifraKladionicaIgre;

        private int _kladionicaID;
        private System.Threading.Timer timer;

        public ICommand EopAnaCommand { get; set; }
        //  public ICommand EopSinCommand { get; set; }
        public ICommand KladUplIsplCommand { get; set; }
        public ICommand IsplataCommand { get; set; }

        public ICommand Eop_SinCommand { get; set; }

        public ICommand PazarCommand { get; set; }

        public ICommand AutomatiCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand PonistiSinCommand { get; set; }

        public ICommand PonistiIsplatuCommand { get; set; }

        public ICommand PonistiKladionicuCommand { get; set; }

        public ICommand PonistiPazarCommand { get; set; }

        public ICommand PonistiAutomateCommand { get; set; }

        public ICommand MultiIsplata { get; set; }

        public ICommand MultiUplata { get; set; }

        public ICommand MultiKladionica { get; set; }

        //public ImportViewModel()
        //{

        //    //this.EopAnaCommand = new RelayCommand(Imp_Eop_Ana);
        //    //this.EopSinCommand = new RelayCommand(EOP_Sintetika);
        //    this.KladUplIsplCommand = new RelayCommand(Kladionica);
        //    this.IsplataCommand = new RelayCommand(Isplata);
        //}

        public ImportViewModel(GlavniViewModel gvm)
        {
            _gvm = gvm;
            this.Eop_SinCommand = new RelayCommand(async () => await EOP_Sintetika());
            this.KladUplIsplCommand = new RelayCommand(async () => await Kladionica1());
            this.IsplataCommand = new RelayCommand(async () => await Isplata1());
            this.PazarCommand = new RelayCommand(async () => await Pazar());
            this.AutomatiCommand = new RelayCommand(async () => await Automati());
            this.CancelCommand = new RelayCommand(Cancel);
            this.PonistiSinCommand = new RelayCommand(PonistiZadnjiImportSin);
            this.PonistiIsplatuCommand = new RelayCommand(PonistiZadnjiImportIsplate);
            this.PonistiKladionicuCommand = new RelayCommand(PonistiZadnjiImportKladionice);
            this.PonistiPazarCommand = new RelayCommand(PonistiZadnjiImportPazara);
            this.PonistiAutomateCommand = new RelayCommand(PonistiZadnjiImportAutomata);
            this.MultiIsplata = new RelayCommand(async () => await UbaciViseIsplata());
            this.MultiUplata = new RelayCommand(async () => await UbaciViseUplata());
            this.MultiKladionica= new RelayCommand(async () => await UbaciViseKladionica());
        }

     
     

        public async Task UbaciViseIsplata()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT files (*.txt)|*.txt";

            // Allow the user to select multiple images.
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Ubaci Vise Fileova";

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    try
                    {
                        await ImpIspAsync(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Doslo je do greske.\n\n" +
                                   "Greska: " + ex.Message + "\n\n" +
                                   "Detalji (Posalji Informatici):\n\n" + ex.StackTrace);
                    }
                }
            }
        }

        public async Task UbaciViseUplata()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT files (*.txt)|*.txt";

            // Allow the user to select multiple images.
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Ubaci Vise Fileova";

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    try
                    {
                        await ImpSinAsync(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Doslo je do greske.\n\n" +
                                   "Greska: " + ex.Message + "\n\n" +
                                   "Detalji (Posalji Informatici):\n\n" + ex.StackTrace);
                    }
                }
            }
        }

        public async Task UbaciViseKladionica()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT files (*.txt)|*.txt";

            // Allow the user to select multiple images.
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Ubaci Vise Fileova";

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    try
                    {
                        string ime = Path.GetFileName(file);
                        await ImpKladAsync(file, ime);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Doslo je do greske.\n\n" +
                                   "Greska: " + ex.Message + "\n\n" +
                                   "Detalji (Posalji Informatici):\n\n" + ex.StackTrace);
                    }
                }
            }
        }

        private async Task Automati()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                _sFileName = openFileDialog.FileName;
                _ime = openFileDialog.SafeFileName;
            }

            try
            {
                await ImpAutomatiAsync(_sFileName, _ime);
            }
            catch (AggregateException ex)
            {
                //loadingPopup.IsOpen = false;
                MessageBox.Show("Korisnik otkazao unos!");
                KopirajFileGreska(_sFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}.{Environment.NewLine} Stacktrace je: {ex.StackTrace}");
                KopirajFileGreska(_sFileName);
            }
        }
        private async Task Pazar()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {

                _sFileName = openFileDialog.FileName;
                _ime = openFileDialog.SafeFileName;
            }

            try
            {
                await Imp_Pazar(_sFileName, _ime);
                _gvm.AVM.Gr.NapuniPazar();
            }
            catch (AggregateException ex)
            {
                //loadingPopup.IsOpen = false;
                MessageBox.Show("Korisnik otkazao unos!");
                KopirajFileGreska(_sFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}.{Environment.NewLine} Stacktrace je: {ex.StackTrace}");
            }
        }
        private async Task Isplata1()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                _sFileName = openFileDialog.FileName;
                _ime = openFileDialog.SafeFileName;
            }

            try
            {
                await ImpIspAsync(_sFileName);
                _gvm.AVM.Gr.NapuniIsplateOI();
            }
            catch (AggregateException ex)
            {
               // loadingPopup.IsOpen = false;
                MessageBox.Show("Korisnik otkazao unos!");
                KopirajFileGreska(_sFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}.{Environment.NewLine} Stacktrace je: {ex.StackTrace}");
               KopirajFileGreska(_sFileName);
            }

        }
        private async Task Kladionica1()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {

                _sFileName = openFileDialog.FileName;
                _ime = openFileDialog.SafeFileName;
            }
            try
            {
                //await this.Presenter.ImpKladAsync(_sFileName, _ime);
                await ImpKladAsync(_sFileName, _ime);
                _gvm.AVM.Gr.NapuniKladionicu();
            }
            catch (AggregateException ex)
            {
                if (openFileDialog.ShowDialog() == false)
                {
                    MessageBox.Show("Korisnik otkazao unos!");
                    //this.Presenter.KopirajFileGreska(_sFileName);
                    KopirajFileGreska(_sFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}.{Environment.NewLine} Stacktrace je: {ex.StackTrace}");
                //this.Presenter.KopirajFileGreska(_sFileName);
                KopirajFileGreska(_sFileName);
            }
        }
        private async Task EOP_Sintetika()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "DAT files (*.dat)|*.dat";

            if (openFileDialog.ShowDialog() == true)
            {
                _sFileName = openFileDialog.FileName;
            }

            try
            {
                await ImpSinAsync(_sFileName);
                _gvm.AVM.Gr.NapuniUplateOI();
            }
            catch (AggregateException ex)
            {
                
                MessageBox.Show("Korisnik otkazao unos!");
                KopirajFileGreska(_sFileName);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Došlo je do greške: {ex.Message}, stacktrace je: {ex.StackTrace}");
                KopirajFileGreska(_sFileName);
            }
        }

        private string[] RazdvojiLinijuNaKolone(string line)
        {
            string[] razdvojeno = line.Split('$');

            return razdvojeno;
        }

        private DateTime DohvatiDatumZaAna(string datum)
        {
            DateTime d = DateTime.Parse(datum);

            return d;
        }

        private double DohvatiIznos(string broj)
        {
            double Br = Double.Parse(broj, CultureInfo.InvariantCulture);
            double iznos = Br / 100;
            return iznos;
        }

        private int ProvjeriDatum(DateTime dat, string procedura)
        {
            int rezultat;

            using (SqlConnection con = new SqlConnection(dc))
            {
                con.Open();


                using (SqlCommand cmd = new SqlCommand(procedura, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@datum", SqlDbType.Date).Value = dat;

                    rezultat = (int)cmd.ExecuteScalar();

                    con.Close();

                    return rezultat;
                }

            }
        }



        public async Task Imp_Eop_Ana(string sFileName)
        {
            List<double> Iznosi = new List<double>();
            List<string[]> linije = new List<string[]>();
            double suma = 0;
            int red = 0;
            double iznos = 0;
            int rezultat = 0;

            var linesRead = await UcitajTekst(sFileName);
            string[] datum = RazdvojiLinijuNaKolone(linesRead[1]);
            DateTime dat = DohvatiDatumZaAna(datum[16]);

            foreach (string line in linesRead)
            {
                red++;
                string[] formatiranaLinija = RazdvojiLinijuNaKolone(line);
                linije.Add(formatiranaLinija);

                if (red > 1)
                {
                    iznos = DohvatiIznos(formatiranaLinija[16]);
                    Iznosi.Add(iznos);
                }
            }

            suma = Iznosi.Sum();

            string procedura = "dbo.provjera_datum_ana";
            rezultat = ProvjeriDatum(dat, procedura);


            if (rezultat == 1)
            {
                MessageBox.Show("Taj datum već postoji u bazi. Molimo probajte odabrati drugi file", "Greška", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }

            else
            {
                procedura = "dynamic_import";
                try
                {
                    UnosUPripremnuTabelu(procedura, sFileName);
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Greška u importu u pripremnu tabelu:{Environment.NewLine}{ex}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }

                MessageBoxResult dialogResult = MessageBox.Show("Unijeli ste " + red + "redova. Ukupan iznos je: " + suma, "Želite li nastaviti?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    procedura = "dbo.analitika_copy";
                    UnosUPravuTabelu(procedura, sFileName);

                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    MessageBox.Show("Import otkazan");
                }

            }
        }


        //private void EOP_Sintetika()
        //{
        //    Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
        //    openFileDialog.Filter = "DAT files (*.dat)|*.dat";

        //    if (openFileDialog.ShowDialog() == true)
        //    {

        //        try
        //        {
        //            _sFileName = openFileDialog.FileName;
        //            double suma = 0;
        //            int i = 0;
        //            int rezultat = 0;

        //            StreamWriter file = new System.IO.StreamWriter(@output);

        //            var linesRead = File.ReadAllLines(_sFileName);
        //            string dat = linesRead[0];

        //            char[] date = new char[8];
        //            date[0] = dat[74];
        //            date[1] = dat[75];
        //            date[2] = dat[76];
        //            date[3] = dat[77];
        //            date[4] = dat[78];
        //            date[5] = dat[79];
        //            date[6] = dat[80];
        //            date[7] = dat[81];

        //            string d = new string(date);
        //            DateTime datum = DateTime.ParseExact(d, "yyyyMMdd", CultureInfo.InvariantCulture).Date;

        //            foreach (var line in linesRead)
        //            {
        //                string red = line.Insert(5, "$").Insert(28, "$").Insert(33, "$").Insert(37, "$").Insert(42, "$").Insert(46, "$")
        //               .Insert(51, "$").Insert(56, "$").Insert(70, "$").Insert(77, "$").Insert(81, "$").Insert(84, "$").Insert(94, "$").Insert(102, "$").Replace(",", ".");

        //                char[] array = new char[7];
        //                array[0] = line[55];
        //                array[1] = line[56];
        //                array[2] = line[57];
        //                array[3] = line[58];
        //                array[4] = line[59];
        //                array[5] = line[60];
        //                array[6] = line[61];

        //                string uplata = new string(array).Trim(' ').Replace(",", ".");

        //                double iznos = Double.Parse(uplata, CultureInfo.InvariantCulture);

        //                suma = suma + iznos;

        //                red = Regex.Replace(red, @"\s+", String.Empty);
        //                i++;
        //                file.WriteLine(red);
        //            }

        //            file.Close();

        //            using (SqlConnection con = new SqlConnection(dc))
        //            {

        //                using (SqlCommand cmd = new SqlCommand("dbo.provjera_datum_sin", con))
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add("@datum", SqlDbType.Date).Value = datum;
        //                    con.Open();

        //                    rezultat = (int)cmd.ExecuteScalar();

        //                }

        //                if (rezultat == 1)
        //                {
        //                    System.Windows.MessageBox.Show("Taj datum već postoji u bazi. Molimo probajte odabrati drugi file");
        //                }
        //                else
        //                {
        //                    using (SqlCommand cmd = new SqlCommand("dbo.sintetika_import", con))
        //                    {
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.Add("@Path", SqlDbType.VarChar).Value = output;
        //                        cmd.ExecuteNonQuery();

        //                        System.IO.File.Delete(_sFileName);
        //                    }

        //                    MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Unijeli ste " + i + "redova. Ukupan iznos je: " + suma, "Želite li nastaviti?", MessageBoxButton.YesNo);

        //                    if (dialogResult == MessageBoxResult.Yes)
        //                    {
        //                        using (SqlCommand cmd = new SqlCommand("dbo.sintetika_copy", con))
        //                        {
        //                            cmd.CommandType = CommandType.StoredProcedure;
        //                            cmd.Parameters.Add("@Path", SqlDbType.VarChar).Value = output;
        //                            cmd.ExecuteNonQuery();
        //                        }
        //                    }
        //                    else if (dialogResult == MessageBoxResult.No)
        //                    {
        //                        System.Windows.MessageBox.Show("Import otkazan");
        //                    }
        //                    con.Close();
        //                }
        //            }
        //        }
        //        catch (Exception greska)
        //        {
        //            System.Windows.MessageBox.Show("Greška: " + greska.Message);
        //        }

        //    }
        //}

        public DateTime DohvatiDatumZaSin(string dat)
        {
            char[] date = new char[8];
            date[0] = dat[75];
            date[1] = dat[76];
            date[2] = dat[77];
            date[3] = dat[78];
            date[4] = dat[79];
            date[5] = dat[80];
            date[6] = dat[81];
            date[7] = dat[82];

            string d = new string(date);
            DateTime datum = DateTime.ParseExact(d, "yyyyMMdd", CultureInfo.InvariantCulture).Date;

            return datum;
        }

        public string NapraviLinijuZaUnosUplate(string line)
        {
            string red = line.Insert(5, "$").Insert(28, "$").Insert(33, "$").Insert(37, "$").Insert(43, "$").Insert(47, "$")
           .Insert(52, "$").Insert(57, "$").Insert(71, "$").Insert(78, "$").Insert(82, "$").Insert(85, "$").Insert(95, "$").Insert(103, "$").Replace(",", ".");

            red = Regex.Replace(red, @"\s+", String.Empty);

            return red;
        }

        public double DohvatiIznosUplate(string line)
        {
            char[] array = new char[8];
            array[0] = line[55];
            array[1] = line[56];
            array[2] = line[57];
            array[3] = line[58];
            array[4] = line[59];
            array[5] = line[60];
            array[6] = line[61];
            array[7] = line[62];

            string uplata = new string(array).Trim(' ').Replace(",", ".");

            double iznos = Double.Parse(uplata, CultureInfo.InvariantCulture);

            return iznos;
        }


        public async Task Imp_Eop_SinA(IProgress<ProgressReportModel> progres, string sFileName, CancellationToken ct)
        {
            StreamWriter file = new System.IO.StreamWriter(@putanjaDatumIme);
            List<double> ListaUplata = new List<double>();
            string[] linesRead = await UcitajTekst(sFileName);
            int brojRedova = linesRead.Count();

            double suma = 0;
            int i = 0;
            int rezultat = 0;

            ParallelOptions po = new ParallelOptions();
            po.CancellationToken = cts.Token;
            po.MaxDegreeOfParallelism = Environment.ProcessorCount;
            ProgressReportModel report = new ProgressReportModel();

            DateTime datum = DohvatiDatumZaSin(linesRead[0]);

            await Task.Run(() =>
            {
                Parallel.ForEach(linesRead, (line) =>
                {
                    string red = NapraviLinijuZaUnosUplate(line);


                    lock (_lock)
                    {
                        file.WriteLine(red);
                        double iznos = DohvatiIznosUplate(line);
                        ListaUplata.Add(iznos);
                        i++;
                        report.procenatZavrsen = (i * 100) / brojRedova;
                        report.Linija = red;
                        progres.Report(report);
                    }

                    ct.ThrowIfCancellationRequested();

                });
            });

            suma = ListaUplata.Sum();
            file.Close();

            string procedura = "dbo.provjera_datum_sin";
            rezultat = ProvjeriDatum(datum, procedura);

            if (rezultat == 1)
            {
                MessageBox.Show("Taj datum već postoji u bazi. Molimo probajte odabrati drugi file", "Greška", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }

            else
            {
                procedura = "dbo.sintetika_import";
                UnosUPripremnuTabelu(procedura, putanjaDatumIme);

                MessageBoxResult dialogResult = MessageBox.Show($"Unijeli ste {i} redova. Ukupan iznos je:{suma}", "Želite li nastaviti?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    procedura = "dbo.sintetika_copy";
                    UnosUPravuTabelu(procedura, putanjaDatumIme);
                }

                else if (dialogResult == MessageBoxResult.No)
                {
                    MessageBox.Show("Import otkazan");
                }
            }
        }


        public async Task ImpSinAsync(string sFileName)
        {
            Progress<ProgressReportModel> progres = new Progress<ProgressReportModel>();
            progres.ProgressChanged += Progres_ProgressChanged;
            cts = new CancellationTokenSource();

            // await Task.Run(() => Test(progres, sFileName));

            try
            {
                await Task.Run(() => Imp_Eop_SinA(progres, sFileName, cts.Token));
            }
            catch (AggregateException ae)
            {
                var ignoredExceptions = new List<Exception>();
                // This is where you can choose which exceptions to handle.
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    if (ex is ArgumentException)
                        Console.WriteLine(ex.Message);
                    else
                        ignoredExceptions.Add(ex);
                }
                if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
            }


        }

        private (string, double, string) NapraviLinijuAutomati(string ime, string line, string datumAutomati)
        {
            string imeFilea = ime;
            string opbroj = $"{line.Substring(0, 5)}$";

            string prod = line.Substring(0, 5);

            string imePrezime = $"{line.Substring(5, 30)}$";


            string igra = $"{line.Substring(35, 3)}$";


            string igraNaziv = $"{line.Substring(42, 20)}$";
            //                    listicTemp.IgraNaziv = t.Trim();

            string iznos = "0" ;

            if (imeFilea.Contains("JP"))
            {
                 iznos = "-" + line.Substring(79, 13);
            }
            else
            { 
             iznos = line.Substring(62, 13);
            }
            double iznDob = double.Parse(iznos);
            iznos = $"{iznDob.ToString(CultureInfo.InvariantCulture)}$";

            string datum = datumAutomati.ToString();

            string priprema = opbroj + imePrezime + igra + igraNaziv + iznos + datum;

            string red = Regex.Replace(priprema, @"\s+", "");
            red = red.Replace("*", "");

            return (red, iznDob, prod);
        }
        private (string, double, string) NapraviLinijuIsplate(string line)
        {

            string godina = $"{line.Substring(0, 2)}$";

            string lis_kolo = $"{line.Substring(2, 4)}$";


            string lis_igra = $"{line.Substring(6, 3)}$";


            string igraNaziv = $"{line.Substring(9, 15)}$";
            //                    listicTemp.IgraNaziv = t.Trim();

            string listicIsplatio = $"{line.Substring(24, 6)}$";

            string prod = line.Substring(25, 5);

            //Provjeri prodavače u bazi
            //if (!prodavaciUValuti.Contains(listicTemp.ListicIsplatio))
            //    prodavaciUValuti.Add(listicTemp.ListicIsplatio);

            string listicUplatio = $"{line.Substring(30, 6)}$";

            brojListica = $"{line.Substring(36, 9)}$";


            string dobitak = $"{line.Substring(45, 4)}$";


            string brojDobitaka = $"{line.Substring(49, 11)}$";


            string iznosDobitka = line.Substring(60, 13);

            double iznDob = double.Parse(iznosDobitka);
            iznosDobitka = $"{iznDob.ToString(CultureInfo.InvariantCulture)}$";

            string iznos = line.Substring(73, 13);
            double izn = double.Parse(iznos);

            iznos = $"{izn.ToString(CultureInfo.InvariantCulture)}$";

            string porez = line.Substring(86, 13);
            double por = double.Parse(porez);
            porez = $"{por.ToString(CultureInfo.InvariantCulture)}$";


            string t1 = line.Substring(99, 9).Trim();
            string t2 = line.Substring(108, 7).Trim();
            int god, mj, dan, sat, min, sec;
            string vrUpl;
            if (t1 != "00000000")
            {

                god = Convert.ToInt16(t1.Substring(0, 4));
                mj = Convert.ToInt16(t1.Substring(4, 2));
                dan = Convert.ToInt16(t1.Substring(6, 2));


                sat = Convert.ToInt16(t2.Substring(0, 2));
                min = Convert.ToInt16(t2.Substring(2, 2));
                sec = Convert.ToInt16(t2.Substring(4, 2));
                DateTime VrijemeUplate = new DateTime(god, mj, dan, sat, min, sec);

                vrUpl = $"{VrijemeUplate.ToString("yyyyMMdd HH:mm:ss")}$";
            }
            else
            {
                vrUpl = "$";
            }
            t1 = line.Substring(115, 9).Trim();
            god = Convert.ToInt16(t1.Substring(0, 4));
            mj = Convert.ToInt16(t1.Substring(4, 2));
            dan = Convert.ToInt16(t1.Substring(6, 2));

            t2 = line.Substring(124, 7).Trim();
            sat = Convert.ToInt16(t2.Substring(0, 2));
            min = Convert.ToInt16(t2.Substring(2, 2));
            sec = Convert.ToInt16(t2.Substring(4, 2));
            DateTime VrijemeIsplate = new DateTime(god, mj, dan, sat, min, sec);
            datumIsp = VrijemeIsplate.Date;

            string vrIsp = $"{VrijemeIsplate.ToString("yyyyMMdd HH:mm:ss")}$";

            string uMUpl = $"{line.Substring(131, 9)}$";

            string uMIsp = $"{line.Substring(140, 5)}$";

            string tipSre = $"{line.Substring(145, 4)}$";

            string serSre = $"{line.Substring(149, 4)}$";

            string nazivSre = $"{line.Substring(153, 30)}$";

            string priprema = godina + lis_kolo + lis_igra + igraNaziv + listicIsplatio + listicUplatio + brojListica + dobitak + brojDobitaka + iznosDobitka + iznos + porez;

            string priprema2 = uMUpl + uMIsp + tipSre + serSre + nazivSre;

            string red = Regex.Replace(priprema, @"\s+", "") + vrUpl + vrIsp + Regex.Replace(priprema2, @"\s+", "");

            red = red.Replace("*", "");

            return (red, iznDob, prod);
        }

        public List<int> DohvatiIgreIsplata()
        {
            using (SqlConnection con = new SqlConnection(dc))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT SIF_ISP FROM dbo.IGRE", con))
                {
                    List<int> sifIsp = new List<int>();
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (rdr["SIF_ISP"] != DBNull.Value)
                            {
                                sifIsp.Add(Convert.ToInt32(rdr["SIF_ISP"]));
                            }
                        }
                    }
                    return sifIsp;
                }
            }
        }


        private bool ProvjeriIgru(List<int> listaIgri, List<int> listaIgriBaza)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var listaUnikatnihProdavaca = listaIgri.Distinct().ToList();

            var listaPresjek = listaUnikatnihProdavaca.Except(listaIgriBaza);

            watch.Stop();
            var vrijeme = watch.ElapsedMilliseconds;

            if (listaPresjek.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        public async Task ImpIspAsync(string sFileName)
        {
            Progress<ProgressReportModel> progres = new Progress<ProgressReportModel>();
            progres.ProgressChanged += Progres_ProgressChanged;
            cts = new CancellationTokenSource();

            // await Task.Run(() => Test(progres, sFileName));

            try
            {
                await Task.Run(() => Imp_Isp(progres, sFileName, cts.Token));             
            }
            catch (AggregateException ae)
            {
                var ignoredExceptions = new List<Exception>();

                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    if (ex is ArgumentException)
                        Console.WriteLine(ex.Message);
                    else
                        ignoredExceptions.Add(ex);
                }
                if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
            }


        }

        public async Task Imp_Isp(IProgress<ProgressReportModel> progres, string sFileName, CancellationToken ct)
        {
            StreamWriter file = new System.IO.StreamWriter(@putanjaDatumIme);
            List<double> listaIznosa = new List<double>();
            List<string> listaProdavaca = new List<string>();
            List<string> listaProdBaza = DohvatiProdavace();
            List<int> listaIgreBaza = DohvatiIgreIsplata();
            List<int> listaIgara = new List<int>();
            List<int> listaRez = new List<int>();
            //  List<int> provjera= new List<>
            string[] linesRead = await UcitajTekst(sFileName);

            int brojRedova = linesRead.Count();
            int rezultat = 0;
            int i = 0;

            ParallelOptions po = new ParallelOptions();
            po.CancellationToken = cts.Token;
            po.MaxDegreeOfParallelism = Environment.ProcessorCount;
            ProgressReportModel report = new ProgressReportModel();

            await Task.Run(() =>
            {
                Parallel.ForEach(linesRead, (line) =>
                {
                    var (red, iznosDob, prod) = NapraviLinijuIsplate(line);

                    lock (_lock)
                    {
                        file.WriteLine(red);

                        i++;
                        listaIznosa.Add(iznosDob);
                        listaProdavaca.Add(prod);
                        report.procenatZavrsen = (i * 100) / brojRedova;
                        report.Linija = red;
                        progres.Report(report);
                    }

                    ct.ThrowIfCancellationRequested();

                });
            });
            double suma = listaIznosa.Sum();
            file.Close();

            string procedura = "dbo.provjera_datum_isplata";

            rezultat = ProvjeriDatum(datumIsp, procedura);

            if (rezultat == 1)
            {
                MessageBox.Show("Taj file već postoji u bazi. Molimo probajte odabrati drugi file");
            }

            else
            {
                bool prodavacPostoji = ProvjeriProdavaca(listaProdavaca, listaProdBaza);

                if (prodavacPostoji != true)
                {
                    MessageBox.Show("Pokušali ste unijeti nepostojećeg prodavača!");
                }
                else
                {
                    bool igraPostoji = ProvjeriIgru(listaIgara, listaIgreBaza);

                    if (igraPostoji != true)
                    {
                        MessageBox.Show("Pokušali ste unijeti nepostojeću igru!");
                    }
                    else
                    {
                        procedura = "dbo.isplata_import";

                        UnosUPripremnuTabelu(procedura, putanjaDatumIme);

                        MessageBoxResult dialogResult = MessageBox.Show($"Broj unesenih redova je {i}, suma iznosa dobitaka je {suma}. Da li želite ubaciti podatke u finalnu tabelu?", "Želite li nastaviti=", MessageBoxButton.YesNo);

                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            procedura = "dbo.isplata_copy";
                            UnosUPravuTabelu(procedura, putanjaDatumIme);
                           // _gvm.AVM.Gr.NapuniIsplateOI();
                        }

                        else if (dialogResult == MessageBoxResult.No)
                        {
                            MessageBox.Show("Import otkazan");
                        }
                    }
                }
            }
        }


        private (string, double) linijaPazar(string line)
        {
            var parts = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+")
               .Cast<Match>()
               .Select(m => m.Value)
               .ToList();

            string tip_dok = parts[0] + "$";
            string t1 = parts[1];
            string t2 = parts[2];
            int god, mj, dan, sat, min, sec;
            string dat;
            if (t1 != "00000000")
            {
                dan = Convert.ToInt16(t1.Substring(0, 2));
                mj = Convert.ToInt16(t1.Substring(3, 2));
                god = Convert.ToInt16(t1.Substring(6, 4));

                sat = Convert.ToInt16(t2.Substring(0, 2));
                min = Convert.ToInt16(t2.Substring(3, 2));
                sec = Convert.ToInt16(t2.Substring(6, 2));
                DateTime Datum = new DateTime(god, mj, dan, sat, min, sec);

                dat = Datum.ToString("yyyyMMdd HH:mm:ss") + '$';
            }
            else
            {
                dat = "" + '$';
            }

            string op_broj = parts[3] + '$';
            string iznos = parts[4];
            double iznDob = double.Parse(iznos);
            iznos = iznDob.ToString(CultureInfo.InvariantCulture) + '$';

            string bezzareza = parts[5].Replace(",", "");
            string opis = bezzareza + '$';

            string priprema = tip_dok + dat + op_broj + iznos + opis;
            string red = priprema.Replace("\"", "");

            // sumaPazara = sumaPazara + iznDob;

            return (red, iznDob);
        }


        public async Task Imp_Pazar(string sFileName, string ime)
        {

            StreamWriter file = new System.IO.StreamWriter(@putanjaDatumIme);
            int i = 0;
            List<double> listaDobitaka = new List<double>();
            // string[] linesRead = File.ReadAllLines(sFileName);
            string[] linesRead = await UcitajTekst(sFileName);

            double sumaPazara = 0;

            int rezultat = 0;

            List<string> neka = new List<string>();




            Parallel.ForEach(linesRead, (line) =>
            {
                var (red, dob) = linijaPazar(line);


                //string red = Regex.Replace(priprema, @"\s+", "") ;
                lock (_lock)
                {
                    file.WriteLine(red);
                    i++;
                    listaDobitaka.Add(dob);
                }

            });

            sumaPazara = listaDobitaka.Sum();

            file.Close();

            using (SqlConnection con = new SqlConnection(dc))
            {
                string procedura = "dbo.pazar_import";
                UnosUPripremnuTabelu(procedura, putanjaDatumIme);

                MessageBoxResult dialogResult = MessageBox.Show($"Broj unesenih redova je {i}. Ukupan iznos je: {sumaPazara}, Unos podataka u tabelu?", "Želite li nastaviti?", MessageBoxButton.YesNo);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    procedura = "dbo.pazar_copy";
                    UnosUPravuTabelu(procedura, putanjaDatumIme);
                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    MessageBox.Show("Import otkazan");
                }

            }

        }


        private void UnosUPravuTabelu(string procedura, string sFileName)
        {
            using (SqlConnection con = new SqlConnection(dc))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(procedura, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Path", SqlDbType.VarChar).Value = sFileName;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Uspiješno uneseni podaci u tabelu");
                    con.Close();
                }
                KopirajFile(sFileName);
                File.Move(sFileName, putanjaDatumIme);
            }
        }


        private void UnosUPravuTabeluKlad(string sFileName, string procedura, int igraID, DateTime datumImporta)
        {
            using (SqlConnection con = new SqlConnection(dc))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(procedura, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@igraID", SqlDbType.VarChar).Value = igraID;
                    cmd.Parameters.Add("@datumImporta", SqlDbType.Date).Value = datumImporta;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Uspješno uneseni podaci u tabelu");
                    con.Close();
                }
                //KopirajFile(sFileName);
                //File.Move(sFileName, $"{putanjaDatumIme}");
            }
        }

        private void UnosUPravuTabeluAut(string sFileName, string procedura,  DateTime datumImporta)
        {
            using (SqlConnection con = new SqlConnection(dc))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(procedura, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;                 
                    cmd.Parameters.Add("@datumImporta", SqlDbType.Date).Value = datumImporta;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Uspješno uneseni podaci u tabelu");
                    con.Close();
                }
                //KopirajFile(sFileName);
                //File.Move(sFileName, $"{putanjaDatumIme}");
            }
        }

        // *** UBACENO OD DINE ***

        //Greska
        //public async Task ImpKladAsync(string sFileName, string ime)
        //{
        //    Progress<ProgressReportModel> progres = new Progress<ProgressReportModel>();
        //    progres.ProgressChanged += Progres_ProgressChanged;
        //    cts = new CancellationTokenSource();

        //    // await Task.Run(() => Test(progres, sFileName));

        //    try
        //    {
        //        await Task.Run(() => Imp_Klad(progres, sFileName, ime, cts.Token));
        //    }
        //    catch (AggregateException ae)
        //    {
        //        var ignoredExceptions = new List<Exception>();

        //        foreach (var ex in ae.Flatten().InnerExceptions)
        //        {
        //            if (ex is ArgumentException)
        //                Console.WriteLine(ex.Message);
        //            else
        //                ignoredExceptions.Add(ex);
        //        }
        //        if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
        //    }


        //}

        public void UbaciVrstuIgreKladionica(string IME)
        {

            if (IME.Contains("UTRKEPASA"))
            {
                _kladionicaID = 1;
            }
            else if (IME.Contains("LOTOKLAĐENJE") || IME.Contains("LOTOKLADJENJE"))
            {
                _kladionicaID = 2;
            }
            else if (IME.Contains("LUCKYSIX"))
            {
                _kladionicaID = 3;
            }
            else if (IME.Contains("SPORTSKOKLADJENJE"))
            {
                _kladionicaID = 4;
            }

            else if (IME.Contains("LIVEKLADJENJE"))
            {
                _kladionicaID = 5;
            }
            else
            {
                MessageBox.Show("Naziv filea nije odgovarajući molimo obratite se Administraciji");
            }
        }

        public List<string> DohvatiObjekte()
        {
            using (SqlConnection con = new SqlConnection(dc))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT BROBJ FROM dbo.GR_ORGANIZACIJE_VIEW", con))
                {
                    List<string> brObjekata = new List<string>();
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            brObjekata.Add(rdr["BROBJ"].ToString());
                        }
                    }
                    return brObjekata;
                }
            }
        }

        public List<string> DohvatiProdavace()
        {
            using (SqlConnection con = new SqlConnection(dc))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT KOMITENT FROM dbo.komitenti_ime_matbr_zracun", con))
                {
                    List<string> opBrojevi = new List<string>();
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            opBrojevi.Add(rdr["KOMITENT"].ToString());
                        }
                    }
                    return opBrojevi;
                }
            }
        }

        public DateTime DatumKladionica(string IME)
        {
            DateTime datumUplate;
            Match match = Regex.Match(IME, @"\d{2}\.\d{2}\.\d{4}");
            Match match2 = Regex.Match(IME, @"\d{2}\.\d{1}\.\d{4}");
            string date = match.Value;
            string date2 = match2.Value;

            if (date != String.Empty)
            {
                datumUplate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            else if (date2 != String.Empty)
            {
                DateTime datum = DateTime.ParseExact(date2, "dd.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                string d = datum.ToString("dd.MM.yyyy");
                datumUplate = DateTime.ParseExact(d, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            else
            {
                throw new Exception("Datum u nazivu filea nije pravilno formatiran. Pravilan format je dd.MM.yyyy ili dd.M.yyyy");
            }
            return datumUplate;
        }

        public async Task<string[]> UcitajTekst(string sFileName)
        {
            if (sFileName != null)
            {
                string[] linesRead = await Task.Run(() => File.ReadAllLines(sFileName));

                MessageBoxResult dialogResult = MessageBox.Show($"Učitali ste {linesRead.Count()} redova, želite li nastaviti?", "Nastavak?", MessageBoxButton.YesNo);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    return linesRead;
                }

                else
                {
                    MessageBox.Show("Import otkazan");
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali file!");
                return null;
            }
        }

        private static string ObrisiSlova(string slova)
        {
            return new string(slova.Where(c => char.IsDigit(c)).ToArray());
        }

        public (string, string, string, double, double, double) NapraviLinijuKladionica(string line)
        {

            var result = Regex.Split(line, (",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"));
            //if (result.Count() == 11)
            //{
            //    List<string> red = new List<string>();

            //    string brObj = result[0].Replace("\"", "").Replace(@"\", "");
            //    int broj = Convert.ToInt16(brObj);
            //    string brojObj = broj.ToString("D5");          
            //    red.Add(brojObj);

            //    string opBroj = result[3].Replace("\"", "").Replace(@"\", "");
            //    string oB = opBroj.PadLeft(5, '0');               
            //    red.Add(oB);

            //    string brojListica = result[5].Replace("\"", "").Replace(@"\", "");
            //    red.Add(brojListica);

            //    string uplata = result[6].Replace("\"", "").Replace(@"\", "");
            //    double upl = double.Parse(uplata);
            //    result[6] = upl.ToString(CultureInfo.InvariantCulture);
            //    red.Add(result[6]);

            //    string naknada = result[7].Replace("\"", "").Replace(@"\", "");
            //    double nak = double.Parse(naknada);
            //    result[7] = nak.ToString(CultureInfo.InvariantCulture);
            //    red.Add(result[7]);

            //    string isplata = result[8].Replace("\"", "").Replace(@"\", "");
            //    double isp = double.Parse(isplata);
            //    result[8] = isp.ToString(CultureInfo.InvariantCulture);
            //    red.Add(result[8]);

            //    string porez = result[9].Replace("\"", "").Replace(@"\", "");
            //    double por = double.Parse(porez);
            //    result[9] = por.ToString(CultureInfo.InvariantCulture);
            //    red.Add(result[9]);

            //    string razlika = result[10].Replace("\"", "").Replace(@"\", "");
            //    double raz = double.Parse(razlika);
            //    result[10] = raz.ToString(CultureInfo.InvariantCulture);
            //    red.Add(result[10]);

            //    double bla = upl - isp;
            //    string blagajna = bla.ToString(CultureInfo.InvariantCulture);
            //    red.Add(blagajna);

            //    //var lista = result.ToList<string>();
            //    //lista.Add(blagajna);
            //    //lista.RemoveAt(lista.Count - 1);



            //    string combindedString = string.Join("$", red);
            //    combindedString = combindedString.Replace("\"", "");

            //    return (combindedString, oB, brojObj, upl, isp, bla);


            //}

            //if (result.Count() == 12)
            //{
            List<string> red = new List<string>();
            double blag = 0;

            string brObj = result[0].Replace("\"", "").Replace(@"\", "");
            int broj = 0;
            int.TryParse(brObj, out broj);
            //int broj = Convert.ToInt16(brObj);
            string brojObj = broj.ToString("D5");
            result[0] = brojObj;
            red.Add(brojObj);

            string operativniBroj = ObrisiSlova(result[3].Replace("\"", "").Replace(@"\", "").Trim());
            string oB = operativniBroj.PadLeft(5, '0');
            red.Add(oB);

            //Ovo je za kolonu Ime prezime operatora ako nam bude trebalo
            //string imePrezOper = result[4].Replace("\"", "").Replace(@"\", "").TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9').Trim();
            //red.Add(imePrezOper);




            string brojListica = result[5].Replace("\"", "").Replace(@"\", "");
            red.Add(brojListica);

            string uplata = result[6].Replace("\"", "").Replace(@"\", "");
            double upl = double.Parse(uplata);
            result[6] = upl.ToString(CultureInfo.InvariantCulture);
            red.Add(result[6]);

            string naknada = result[7].Replace("\"", "").Replace(@"\", "");
            double nak = double.Parse(naknada);
            result[7] = nak.ToString(CultureInfo.InvariantCulture);
            red.Add(result[7]);

            string isplata = result[8].Replace("\"", "").Replace(@"\", "");
            double isp = double.Parse(isplata);
            result[8] = isp.ToString(CultureInfo.InvariantCulture);
            red.Add(result[8]);

            string porez = result[9].Replace("\"", "").Replace(@"\", "");
            double por = double.Parse(porez);
            result[9] = por.ToString(CultureInfo.InvariantCulture);
            red.Add(result[9]);

            string razlika = result[10].Replace("\"", "").Replace(@"\", "");
            double raz = double.Parse(razlika);
            result[10] = raz.ToString(CultureInfo.InvariantCulture);
            red.Add(result[10]);

            if (result[11] != string.Empty)
            {
                string blagajna = result[11].Replace("\"", "").Replace(@"\", "");
                blag = double.Parse(blagajna);
                result[11] = blag.ToString(CultureInfo.InvariantCulture);
                red.Add(result[11]);
            }
            else
            {
                blag = upl - isp;
                string blagajna = blag.ToString(CultureInfo.InvariantCulture);
                red.Add(blagajna);
            }
            //var lista = result.ToList<string>();
            //lista.RemoveAt(lista.Count - 1);

            string combindedString = string.Join("$", red);
            combindedString = combindedString.Replace("\"", "");

            return (combindedString, oB, brojObj, upl, isp, blag);
            //}

            //else 
            //{
            //    throw new Exception("Greška, file kladionice koji ste probali učitati nema odgovarajući broj kolona");
            //}
        }

        private int ProvjeriDatumKlad(int igra, DateTime datum_importa, string procedura)
        {
            int rezultat;

            using (SqlConnection con = new SqlConnection(dc))
            {
                con.Open();


                using (SqlCommand cmd = new SqlCommand(procedura, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@igra", SqlDbType.Int).Value = igra;
                    cmd.Parameters.Add("@datumImporta", SqlDbType.Date).Value = datum_importa;

                    rezultat = (int)cmd.ExecuteScalar();

                    con.Close();

                    return rezultat;
                }

            }
        }

        private bool ProvjeriProdavaca(List<string> listaProdavaca, List<string> listaProdBaza)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var listaUnikatnihProdavaca = listaProdavaca.Distinct().ToList();

            var listaPresjek = listaUnikatnihProdavaca.Except(listaProdBaza);

            watch.Stop();
            var vrijeme = watch.ElapsedMilliseconds;

            if (listaPresjek.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        private bool ProvjeriObjekat(List<string> listaObjekata, List<string> listaObjekataBaza)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var listaUnikatnihObjekata = listaObjekata.Distinct().ToList();

            var listaPresjek = listaUnikatnihObjekata.Except(listaObjekataBaza);

            watch.Stop();
            var vrijeme = watch.ElapsedMilliseconds;

            if (listaPresjek.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        private void UnosUPripremnuTabelu(string procedura, string sFileName)
        {

            using (SqlConnection con = new SqlConnection(dc))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(procedura, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Path", SqlDbType.VarChar).Value = sFileName;
                    cmd.ExecuteNonQuery();
                }

                con.Close();

                //ObrisiFile(sFileName);

            }
        }

        public void Cancel()
        {
            cts.Cancel();
        }

        private void KopirajFile(string sFileName)
        {
            //System.IO.File.Copy(sFileName, $@"\\192.168.1.213\Users\Share Import\Import EOP UPLATE\Arhiva\{sFileName}{DateTime.Now.ToString("dd.MM.yyyy")}.csv");


            //ovo nesto ne radi iz nekog razloga ne javi gresku ali ne kopira file...



            //      \\192.168.1.213\Users\Share Import\Import EOP UPLATE\Arhiva
        }

        public void KopirajFileGreska(string sFileName)
        {
            //ovo nesto ne radi iz nekog razloga ne javi gresku ali ne kopira file...
            // System.IO.File.Copy(sFileName, $"{arhivaGreska}Greska");
        }

        // *** IPRAVITI GRESKU ***
        private void Progres_ProgressChanged(object sender, ProgressReportModel e)
        {
            //this.View.loadingPopup.IsOpen = true;

            //this.View.progresBar.Value = e.procenatZavrsen;
            //this.View.tekstPB.Text = $"Upisao red: {e.Linija}";
            //if (e.procenatZavrsen == 100)
            //{
            //    this.View.loadingPopup.IsOpen = false;
            //}
        }

        private async Task Kladionica()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {

                _sFileName = openFileDialog.FileName;
                _ime = openFileDialog.SafeFileName;
            }
            try
            {
                await ImpKladAsync(_sFileName, _ime);
            }
            catch (AggregateException ex)
            {
                //loadingPopup.IsOpen = false;
                if (openFileDialog.ShowDialog() == false) {

                    MessageBox.Show("Korisnik otkazao unos!");
                    KopirajFileGreska(_sFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}.{Environment.NewLine} Stacktrace je: {ex.StackTrace}");
                KopirajFileGreska(_sFileName);
            }
        }

        public async Task ImpKladAsync(string sFileName, string ime)
        {

            Progress<ProgressReportModel> progres = new Progress<ProgressReportModel>();
            progres.ProgressChanged += Progres_ProgressChanged;
            cts = new CancellationTokenSource();

            //await Task.Run(() => Test(progres, sFileName));

            try
            {
                await Task.Run(() => Imp_Klad(progres, sFileName, ime, cts.Token));
            }
            catch (AggregateException ae)
            {
                var ignoredExceptions = new List<Exception>();

                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    if (ex is ArgumentException)
                        Console.WriteLine(ex.Message);
                    else
                        ignoredExceptions.Add(ex);
                }
                if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
            }


        }



        public async Task Imp_Klad(IProgress<ProgressReportModel> progres, string sFileName, string ime, CancellationToken ct)
        {

            StreamWriter file = new System.IO.StreamWriter(@putanjaDatumIme);
            List<double> listaUplata = new List<double>();
            List<double> listaIsplata = new List<double>();
            List<string> listaProdavaca = new List<string>();
            List<string> listaObjekata = new List<string>();
            List<string> listaObjekataBaza = DohvatiObjekte();
            List<string> listaProdBaza = DohvatiProdavace();
            string IME = "";
            List<int> listaRez = new List<int>();

            ParallelOptions po = new ParallelOptions();
            po.CancellationToken = cts.Token;
            po.MaxDegreeOfParallelism = Environment.ProcessorCount;
            ProgressReportModel report = new ProgressReportModel();
            if (ime != null)
            { 
            string I = ime.ToUpper();
            IME = Regex.Replace(I, @"\s", "");
            UbaciVrstuIgreKladionica(IME);
            }
            double blagajna = 0;

            int i = 0;
            int rezultat = 0;
            List<string> neka = new List<string>();

            //  StreamWriter file = new System.IO.StreamWriter(@output);

            DateTime datumUplate = DatumKladionica(IME);
            _datumKladionicaUplate = datumUplate;
            string[] linesRead = await UcitajTekst(sFileName);
            linesRead = linesRead.Skip(1).ToArray();
            int brojRedova = linesRead.Count();



            // var result = Regex.Split(line, ",(?=(?:[^']*'[^']*')*[^']*$)");
            await Task.Run(() =>
            {
                Parallel.ForEach(linesRead, (line) =>
                {
                    var (red, prod, objekat, upl, isp, bla) = NapraviLinijuKladionica(line);

                    lock (_lock)
                    {
                        if (Double.Parse(prod) != 0)
                        {
                            file.WriteLine(red);

                            i++;
                            listaUplata.Add(upl);
                            listaIsplata.Add(isp);

                            listaProdavaca.Add(prod);

                            listaObjekata.Add(objekat);
                            blagajna = bla;
                            report.procenatZavrsen = (i * 100) / brojRedova;
                            report.Linija = red;
                            progres.Report(report);
                        }
                    }

                    ct.ThrowIfCancellationRequested();

                });
            });

            double uplataUkupno = listaUplata.Sum();
            double isplataUkupno = listaIsplata.Sum();
            file.Close();
            string procedura = "dbo.provjera_datum_klad";

            rezultat = ProvjeriDatumKlad(_kladionicaID, datumUplate, procedura);

            if (rezultat == 1)
            {
                MessageBox.Show("Taj file već postoji u bazi. Molimo probajte odabrati drugi file");
            }

            else
            {
                bool prodavacPostoji = ProvjeriProdavaca(listaProdavaca, listaProdBaza);

                if (prodavacPostoji != true)
                {
                    MessageBox.Show("Pokušali ste unijeti nepostojećeg prodavača!");
                }
                else
                {
                    bool objPostoji = ProvjeriObjekat(listaObjekata, listaObjekataBaza);

                    if (objPostoji != true)
                    {
                        MessageBox.Show("Pokušali ste unijeti nepostojeći objekat!");
                    }
                    else
                    {
                        procedura = "dbo.kladionica_import";

                        UnosUPripremnuTabelu(procedura, putanjaDatumIme);

                        MessageBoxResult dialogResult = MessageBox.Show($"Broj unesenih redova je {i}, ukupna uplata je {uplataUkupno}, ukupna isplata je {isplataUkupno} . Da li želite ubaciti podatke u finalnu tabelu?", "Želite li nastaviti=", MessageBoxButton.YesNo);

                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            procedura = "dbo.kladionica_copy";
                            UnosUPravuTabeluKlad(sFileName, procedura, _kladionicaID, datumUplate);
                        }

                        else if (dialogResult == MessageBoxResult.No)
                        {
                            MessageBox.Show("Import otkazan");
                        }
                    }
                }
            }
        }

        public void PonistiZadnjiImportSin()
        {
            MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Da li ste sigurni da želite poništiti zadnji import?", "Vraćanje stanja", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
              
                using (SqlConnection con = new SqlConnection(dc))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.ponisti_sin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    System.Windows.MessageBox.Show("Zadnji import uplate je poništen!");
                }
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                System.Windows.MessageBox.Show("Import uplate NIJE poništen");
            }


        }

        public void PonistiZadnjiImportIsplate()
        {

            MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Da li ste sigurni da želite poništiti zadnji import?", "Vraćanje stanja", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(dc))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.ponisti_isplatu", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        
                    }
                    con.Close();
                    _gvm.AVM.Gr.NapuniIsplateOI();
                    System.Windows.MessageBox.Show("Zadnji import isplate je poništen!");
                }
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                System.Windows.MessageBox.Show("Import isplate NIJE poništen");
            }

        }

        public void PonistiZadnjiImportKladionice()
        {

            MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Da li ste sigurni da želite poništiti zadnji import?", "Vraćanje stanja", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
              //  var (dat, id) = this._gvm.AVM.Gr.NadjiNajveciDatumKladionica();
                using (SqlConnection con = new SqlConnection(dc))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.ponisti_kladionicu", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                       
                        cmd.ExecuteNonQuery();
                       
                    }
                    _gvm.AVM.Gr.NapuniKladionicu();
                    con.Close();
                    
                    System.Windows.MessageBox.Show("Zadnji import kladionice je poništen!");
                }
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                System.Windows.MessageBox.Show("Import kladionice NIJE poništen");
            }
        }

      
        public void PonistiZadnjiImportPazara()
        {

            MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Da li ste sigurni da želite poništiti zadnji import?", "Vraćanje stanja", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(dc))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.ponisti_pazar", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    _gvm.AVM.Gr.NapuniPazar();
                    con.Close();
                    System.Windows.MessageBox.Show("Zadnji import pazara je poništen!");
                }
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                System.Windows.MessageBox.Show("Import pazara NIJE poništen");
            }
        }

        public void PonistiZadnjiImportAutomata()
        {

            MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Da li ste sigurni da želite poništiti zadnji import?", "Vraćanje stanja", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(dc))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.ponisti_automate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    _gvm.AVM.Gr.NapuniAutomate();
                    con.Close();
                    System.Windows.MessageBox.Show("Zadnji import automata je poništen!");
                }
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                System.Windows.MessageBox.Show("Import automata NIJE poništen");
            }
        }


        public async Task ImpAutomatiAsync(string sFileName, string ime)
        {

            Progress<ProgressReportModel> progres = new Progress<ProgressReportModel>();
            progres.ProgressChanged += Progres_ProgressChanged;
            cts = new CancellationTokenSource();

            //await Task.Run(() => Test(progres, sFileName));
            if (sFileName !=null)
            { 
            try
            {
                await Task.Run(() => Imp_Automati(progres, sFileName, ime, cts.Token));
               // _gvm.AVM.Gr.NapuniAutomate();
            }
            catch (AggregateException ae)
            {
                var ignoredExceptions = new List<Exception>();

                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    if (ex is ArgumentException)
                        Console.WriteLine(ex.Message);
                    else
                        ignoredExceptions.Add(ex);
                }
                if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
            }
            }


        }

        public async Task Imp_Automati(IProgress<ProgressReportModel> progres, string sFileName, string ime, CancellationToken ct)
        {
            StreamWriter file = new System.IO.StreamWriter(@putanjaDatumIme);
           
            List<double> listaIznosa = new List<double>();
            List<string> listaProdavaca = new List<string>();
            List<string> listaProdBaza = DohvatiProdavace();
            List<int> listaIgreBaza = DohvatiIgreIsplata();
            List<int> listaIgara = new List<int>();
            List<int> listaRez = new List<int>();
            //  List<int> provjera= new List<>
            string[] linesRead = await UcitajTekst(sFileName);
            string datumAutomati = (ime.Substring(2, 6));
            DateTime dAut = new DateTime();
            DateTime.TryParseExact(datumAutomati, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None,  out dAut);
            int brojRedova = linesRead.Count();
            int rezultat = 0;
            int i = 0;

            ParallelOptions po = new ParallelOptions();
            po.CancellationToken = cts.Token;
            po.MaxDegreeOfParallelism = Environment.ProcessorCount;
            ProgressReportModel report = new ProgressReportModel();

            await Task.Run(() =>
            {
                Parallel.ForEach(linesRead, (line) =>
                {
                    var (red, iznosDob, prod) = NapraviLinijuAutomati(ime, line, datumAutomati);

                    lock (_lock)
                    {
                        file.WriteLine(red);

                        i++;
                        listaIznosa.Add(iznosDob);
                        listaProdavaca.Add(prod);
                        report.procenatZavrsen = (i * 100) / brojRedova;
                        report.Linija = red;
                        progres.Report(report);
                    }

                    ct.ThrowIfCancellationRequested();

                });
            });
            double suma = listaIznosa.Sum();
            file.Close();

            string procedura = "dbo.provjera_datum_automati";

            rezultat = ProvjeriDatum(dAut, procedura);

            if (rezultat == 1)
            {
                MessageBox.Show("Taj file već postoji u bazi. Molimo probajte odabrati drugi file");
            }

            else
            {
                bool prodavacPostoji = ProvjeriProdavaca(listaProdavaca, listaProdBaza);

                if (prodavacPostoji != true)
                {
                    MessageBox.Show("Pokušali ste unijeti nepostojećeg prodavača!");
                }
                else
                {
                        procedura = "dbo.automati_import";

                        UnosUPripremnuTabelu(procedura, putanjaDatumIme);

                        MessageBoxResult dialogResult = MessageBox.Show($"Broj unesenih redova je {i}, suma iznosa dobitaka je {suma}. Da li želite ubaciti podatke u finalnu tabelu?", "Želite li nastaviti=", MessageBoxButton.YesNo);

                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            procedura = "dbo.automati_copy";
                            UnosUPravuTabeluAut(putanjaDatumIme, procedura, dAut);

                        }

                        else if (dialogResult == MessageBoxResult.No)
                        {
                            MessageBox.Show("Import otkazan");
                        }
                    }
                }
            }
        }

    
    public class ProgressReportModel
    {
        public int procenatZavrsen { get; set; } = 0;
        public string Linija { get; set; } = "";
    }
}
