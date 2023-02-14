using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
    //Ova klasa Observable collection, To radimo da bi ova klasa imala mogucnost emitovanja promjene vrijednosti 
    public class PologPazarCollection : ObservableCollection<POLOG_PAZAR>
    {

        public static PologPazarCollection DohvatiPolog()
        {
            PologPazarCollection pologLista = new PologPazarCollection();
            POLOG_PAZAR polog = null;
            using (SqlConnection conn = new SqlConnection(@"Server=data source=192.168.1.213\LUTRIJASQL,1433;Network Library=DBMSSOCN;initial catalog=Lutrija;multipleactiveresultsets=True;application name=EntityFramework;User Id=sa;Password=Lutrija1;".ToString()))
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT ID, TIP_DOKUMENTA, DATUM, OP_BROJ_PROD, IZNOS, OPIS FROM dbo.POLOG_PAZAR", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //while (reader.Read())
                    //{
                    //    polog = new POLOG_PAZAR((int)reader["ID"], (string)reader["TIP_DOKUMENTA"], (DateTime)reader["DATUM"], (string)reader["OP_BROJ_PROD"],
                    //            (decimal)reader["IZNOS"], (string)reader["OPIS"]);

                    //    pologLista.Add(polog);
                    //}

                }
                return pologLista;
            }
        }
    }
}
