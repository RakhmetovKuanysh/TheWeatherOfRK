using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map
{
    class DatabaseConnection
    {
        private string sql_string;
        private string strCon;
        System.Data.SqlClient.SqlDataAdapter da_1;

        public string Sql
        {
            set { sql_string = value; }


            get
            {
                return sql_string;
            }
        }

        public string connection_string
        {

            set { strCon = value; }
        }

        public DataSet GetConnection
        {
            get
            { return MyDataSet(); }
        }

        private DataSet MyDataSet()
        {
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(strCon);

            con.Open();

            da_1 = new System.Data.SqlClient.SqlDataAdapter(sql_string, con);

            DataSet dat_set = new DataSet();
            da_1.Fill(dat_set, "Table_Data_1");
            con.Close();


            return dat_set;
        }

        public void UpdateDatabase(System.Data.DataSet ds)
        {
            //ds.Tables.Remove(ds.Tables[0])
            System.Data.SqlClient.SqlCommandBuilder cb = new System.Data.SqlClient.SqlCommandBuilder(da_1);
            cb.DataAdapter.Update(ds.Tables[0]);
        }
    }
}
