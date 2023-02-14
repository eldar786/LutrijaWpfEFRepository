using LutrijaWpfEF.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LutrijaWpfEF.UI.Windows
{
    public partial class OrgViewWindow : Form
    {
        OrgViewModel _orgViewModel;
        List<OrgViewModel> _list;


        public OrgViewWindow(OrgViewModel orgViewModel, List<OrgViewModel> list)
        {
            InitializeComponent();
            _orgViewModel = orgViewModel;
            _list = list;
        }

        private void OrgViewWindow_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
        }

        private void reportViewer_Load(object sender, EventArgs e)
        {

            //OrgViewModelBindingSource.DataSource = _list;
            Microsoft.Reporting.WinForms.ReportParameter[] p = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("KomitentParameter", _orgViewModel.Od.ToString())
            };
            this.reportViewer.LocalReport.SetParameters(p);
            this.reportViewer.RefreshReport();
        }
    }
}
