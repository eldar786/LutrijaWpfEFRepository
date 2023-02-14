using LutrijaWpfEF.Model;
using LutrijaWpfEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.UI.Izvjestaji
{
    public class ReportOrg
    {
        public komitenti_ime_matbr_zracun OdabraniKomitent { get; set; }
        public DateTime Od { get; set; }
        public DateTime Do { get; set; }

        public OrgViewModel orgViewModel = new OrgViewModel();

        public List<OrgViewModel> GetAllOrgViewModel()
        {
            var list = new List<OrgViewModel>();

            list.Add(new OrgViewModel { OdabraniKomitent = orgViewModel.OdabraniKomitent });
            list.Add(new OrgViewModel { Od = orgViewModel.Od });
            list.Add(new OrgViewModel { Do = orgViewModel.Do });

            return list;
        }

    }
}
