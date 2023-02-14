using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
   public class GrKomCollection : ObservableCollection<GrKom>
    {
        

        public static GrKomCollection GetAllGrKom()
        {

            GrKomCollection grKomList = new GrKomCollection();
            GrKom grKom = null;
            //ime prezime, grad, sifra obj, tel
            string queryString = "SELECT IME, PREZIME, ZIRO_RACUN, STARA_SIFRA, KOMITENT, TELEFON_MOB FROM dbo.komitenti_ime_matbr_zracun";
            using (SqlConnection connection = new SqlConnection(
               "Server = 192.168.1.213; Database=Lutrija;Trusted_Connection=False;MultipleActiveResultSets=true;User ID=sa;Password=Lutrija1;"))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //foreach (var item in reader)
                    //{
                    //    GrKomitenti.Items.Add(item);
                    //}
                    while (reader.Read())
                    {
                        grKom = GrKom.GetKomFromResultSet(reader);
                        grKomList.Add(grKom);
                    }
                }
            }
            return grKomList;
        }
    }
}
