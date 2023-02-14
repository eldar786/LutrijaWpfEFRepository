using LutrijaWpfEF.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LutrijaWpfEF.UI
{
    /// <summary>
    /// Interaction logic for NewEditPologPazaraWindow.xaml
    /// </summary>
    public partial class NewEditPologPazaraWindow : Window
    {
        public NewEditPologPazaraWindow()
        {
            InitializeComponent();
        }

        //Unutar handler metode cemo se pretplatiti na ovaj event 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewEditPologPazarViewModel newEditPologPazarViewModel = (NewEditPologPazarViewModel)DataContext;
            newEditPologPazarViewModel.Done += NewEditPologPazarViewModel_Done;
        }

        private void NewEditPologPazarViewModel_Done(object sender, NewEditPologPazarViewModel.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);
        }



        //private void NewEditPologPazaraWindow_Done(object sender, NewEditPologPazarViewModel.DoneEventArgs e)
        //{
        //    //pristupamo direktno poruci koja je prosljedjena iz viewmodel klase
        //    MessageBox.Show(e.Message);
        //}
    }
}
