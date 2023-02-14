using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
   public class NewEditPologPazarViewModel : INotifyPropertyChanged
    {

        private POLOG_PAZAR currentPologPazar;
        private string windowTitle;

        //Ovo polje ce dobivati vrijednosti injektovanjem kroz konstruktor. TO je princip koji se zove DEPENDENCY INJECTION (to je jos jedan 
        //softwareski DESIGN PATTERN)
        private Mediator mediator;




        #region Property


        public POLOG_PAZAR CurrentPologPazar    
        {
            get { return currentPologPazar; }
            set
            {
                if (currentPologPazar == value)
                {
                    return;
                }
                currentPologPazar = value;

                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPologPazar"));
            }
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                if (windowTitle == value)
                {
                    return;
                }
                windowTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WinwowTitle"));
            }
        }

        #endregion


        #region Constructor

        //Konstruktor za editovanje
        public NewEditPologPazarViewModel(POLOG_PAZAR pOLOG_PAZAR, Mediator mediator)
        {
            this.mediator = mediator;

            SaveCommand = new NewRelayCommand(SaveExecute, CanSave);

            CurrentPologPazar = pOLOG_PAZAR;
            WindowTitle = "Izmjena pologa pazara";
        }

        //Konstruktor za unos novog
        public NewEditPologPazarViewModel(Mediator mediator)
        {
            this.mediator = mediator;

            SaveCommand = new NewRelayCommand(SaveExecute, CanSave);

            CurrentPologPazar = new POLOG_PAZAR();
            WindowTitle = "Unos novog pologa pazara";
        }

        #endregion

        public ICommand saveCommand;

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand == value)
                {
                    return;
                }
                saveCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaveCommand"));
            }
        }

        void SaveExecute(object obj)
        {
            //if (CurrentPologPazar != null && !CurrentPologPazar.HasErrors)
            //{
            //    CurrentPologPazar.Save();
            //    OnDone(new DoneEventArgs("Polog pazara spašen."));
                //Nakon uspjesnog cuvanja
                //Ovom metodom Notify Mediator instance i prosljedjujem naslov poruke i instancu objekta PologPazar
                //Sve zainteresovane strane koje su se pretplatile na poruku PologPromjena, dobit cu dojavu da je doslo do promjene PologPazar objekta i 
                //dobit ce instancu ovog objekta 
            //    mediator.Notify("PologPromjena", CurrentPologPazar);
            //}
            //  else
            //{
            //    OnDone(new DoneEventArgs("Provjerite unesene podatke"));
            //}
        }

        bool CanSave(object obj)
        {
            //Ovo bi trebalo napraviti bolje, jer ideja je da se error okine odma po otvaranju NewEditPologPazaraWindow, tako da ne dozvoli da se spasi dok ne bude sve uneseno
            if (currentPologPazar.OP_BROJ_PROD == null || currentPologPazar.OP_BROJ_PROD == 0.ToString() || currentPologPazar.IZNOS == null || currentPologPazar.OPIS == null || currentPologPazar.TIP_DOKUMENTA == null)
            {
                return false;
            }
            //Uvijek vraca True, to znaci da je komanda uvijek dostupna
            return true;
        }
        //Custom event handler 
        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            private string message;

            public string Message
            {
                get { return message; }
                set
                {
                    if (message == value)
                    {
                        return;
                    }
                    message = value;
                }
            }
            public DoneEventArgs(string message)
            {
                this.message = message;
            }
        }

       

        //Ova event se podize kada je potrebno informisati korisnika prilikom pokusaja cuvanja Polog_Pazar objekta 
        public event DoneEventHandler Done;


        //Metoda za podizanje eventa
        public void OnDone(DoneEventArgs e)
        {
            if (Done != null)
            {
                Done(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
