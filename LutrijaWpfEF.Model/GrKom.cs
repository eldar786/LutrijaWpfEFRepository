using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutrijaWpfEF.Model
{
   public class GrKom
    {
        [StringLength(220)]
        public string IME { get; set; }

        [StringLength(220)]
        public string ZIRO_RACUN { get; set; }

        [StringLength(220)]
        public string STARA_SIFRA { get; set; }

        [StringLength(220)]
        public string KOMITENT { get; set; }

        [StringLength(220)]
        public string TELEFON_MOB { get; set; }

        public GrKom(string iME, string zIRO_RACUN, string sTARA_SIFRA, string kOMITENT, string tELEFON_MOB)
        {
            IME = iME;
            ZIRO_RACUN = zIRO_RACUN;
            STARA_SIFRA = sTARA_SIFRA;
            KOMITENT = kOMITENT;
            TELEFON_MOB = tELEFON_MOB;
        }

        public GrKom()
        {

        }

        public static GrKom GetKomFromResultSet(SqlDataReader reader)
        {

            string ime = Convert.IsDBNull(reader["IME"]) ? null : (string)reader["IME"];
            string prezime = Convert.IsDBNull(reader["PREZIME"]) ? null : (string)reader["PREZIME"];
            string ziroRacun = Convert.IsDBNull(reader["ZIRO_RACUN"]) ? null : (string)reader["ZIRO_RACUN"];
            string staraSifra = Convert.IsDBNull(reader["STARA_SIFRA"]) ? null : (string)reader["STARA_SIFRA"];
            string komitent = Convert.IsDBNull(reader["KOMITENT"]) ? null : (string)reader["KOMITENT"];
            string telefonMob = Convert.IsDBNull(reader["TELEFON_MOB"]) ? null : (string)reader["TELEFON_MOB"];

            GrKom grKom = new GrKom(ime,ziroRacun,staraSifra, komitent, telefonMob);

            return grKom;
        }
    }
}
