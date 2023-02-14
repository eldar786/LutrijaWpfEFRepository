using LutrijaWpfEF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LutrijaWpfEF.ViewModel
{
    public class OpcineViewModel : ObservableObject
    {
        private List<OPCINE> _opcineList;
        private ObservableCollection<OPCINE> _sveOpcine;
        private OPCINE _odabranaOpcina;
        private IzmijeniUplOsnovnihViewModel _iuovm;
        private IzmijeniUplSreckiDinoViewModel _iusdvm;
        private EOP_SIN _uplSrecke;
        private EOP_SIN _uplOsn;
        private List<OPCINE> _opcinePretraga;

        private ApplicationViewModel _avm;
        


        public ICommand OdaberiCommand { get; set; }
        public ICommand OdustaniCommand { get; set; }



        public OpcineViewModel(IzmijeniUplOsnovnihViewModel iuovm, EOP_SIN eOP_SIN)
        {
            _iuovm = iuovm;          
            _uplOsn = eOP_SIN;


            //_avm = avm;
            //_uplSrecke = eOP_SIN;
            SveOpcine = new ObservableCollection<OPCINE>();

            _opcineList = _iuovm.DUOVM.GVM.AVM.Gr.Opcine;

           

            foreach (OPCINE item in _opcineList)
            {
                _sveOpcine.Add(item);
            }


            this.OdaberiCommand = new RelayCommand(Odaberi);
            this.OdustaniCommand = new RelayCommand(Odustani);

        }

        public OpcineViewModel(IzmijeniUplSreckiDinoViewModel iusdvm, EOP_SIN eOP_SIN, int UplOsn1_UplSrc2)
        {
            _iusdvm = iusdvm;
            if (UplOsn1_UplSrc2 == 1)
            {
                _uplOsn = eOP_SIN;
            }
            else if (UplOsn1_UplSrc2 == 2)
            {
                _uplSrecke = eOP_SIN;
            }



            //_avm = avm;
            //_uplSrecke = eOP_SIN;
           

            SveOpcine = new ObservableCollection<OPCINE>();

            foreach (OPCINE item in _opcineList)
            {
                _sveOpcine.Add(item);
            }


            this.OdaberiCommand = new RelayCommand(Odaberi);
            this.OdustaniCommand = new RelayCommand(Odustani);

        }


        private void Odustani()
        {
            EOP_SIN uplS = new EOP_SIN();
            //IzmijeniUplSreckiDinoViewModel izmUplSrecki = new IzmijeniUplSreckiDinoViewModel(_avm);
            _avm.OdabraniVM = new IzmijeniUplSreckiDinoViewModel(_avm, uplS);
        }

        private void Odaberi()
        {
            //EOP_SIN uplS = new EOP_SIN();
            //uplS.OPSTINA = odabranaOpcina.OPC_SIF;


            if (_uplOsn != null && _odabranaOpcina != null)
            {
               
                _iuovm.OdabranaOpcina = _odabranaOpcina;
                _uplOsn.OPSTINA = _iuovm.OdabranaOpcina.OPC_SIF;
                _iuovm.DUOVM.GVM.OdabraniVM = _iuovm;
                
            }


            //if (_uplSrecke != null)
            //{
            //   _iusdvm.Oda
            //    _uplSrecke.OPSTINA = _odabranaOpcina.OPC_SIF;
            //    IzmijeniUplSreckiDinoViewModel izmUplSrecke = new IzmijeniUplSreckiDinoViewModel(_avm, _uplSrecke);
            //    _avm.OdabraniVM = izmUplSrecke;
           // }
        }

        private void Sortiraj()
        {
            ICollectionView opcineView = CollectionViewSource.GetDefaultView(_sveOpcine);
            opcineView.SortDescriptions.Clear();
            opcineView.SortDescriptions.Add(new SortDescription("OPC_NAZIV", ListSortDirection.Ascending));
        }

        public void TraziOpcinu(string _pretraga)
        {
            if (!string.IsNullOrEmpty(_pretraga) && _pretraga.Length > 0)
            {
                SveOpcine = new ObservableCollection<OPCINE>(from i in _sveOpcine
                                                                                    where i.OPC_NAZIV.IndexOf(_pretraga) >= 0 ||                                                                      i.OPC_SIF.IndexOf(_pretraga) >= 0
                                                                                    select i);
            }
            else
            {
                SveOpcine.Clear();

                if (_opcinePretraga != null)
                {

                    foreach (OPCINE opcina in _opcinePretraga)
                    {
                        SveOpcine.Add(opcina);
                    }
                }
            }
            Sortiraj();
        }

        //private List<OPCINE> NapuniOpcinu()
        //{
        //    List<OPCINE> opcinaList = new List<OPCINE>();
        //    using (var context = new LutrijaEntities1())
        //    {
        //        opcinaList = context.OPCINE.ToList();
        //    }
        //    return opcinaList;
        //}


        public ObservableCollection<OPCINE> SveOpcine { get => _sveOpcine; set { _sveOpcine = value; OnPropertyChanged("SveOpcine"); } }
        public OPCINE OdabranaOpcina { get => _odabranaOpcina; set { _odabranaOpcina = value; OnPropertyChanged("OdabranaOpcina"); } }

    }
}
