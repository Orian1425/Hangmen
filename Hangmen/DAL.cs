using System.Data;
using System.Data.OleDb;
namespace DAL
{
    public class DAL
    {
        public static OleDbConnection GetConnection()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Alex\Downloads\Hangmen\Hangmen\db\db.accdb;Persist Security Info=True";
            return new OleDbConnection(connectionString);
        }
        public static OleDbCommand GetCommand(OleDbConnection con, string sqlstr)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = sqlstr;   
            return cmd;
        }

        public static DataTable GetDataTable(string sqlStr)
        {
            OleDbConnection con = GetConnection();
            OleDbCommand cmd = GetCommand(con, sqlStr);

            OleDbDataAdapter adp = new OleDbDataAdapter();
            adp.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adp.Fill(dt);

            return dt;
        }

        public static DataView GetDataView(string sqlStr)
        {
            return GetDataTable(sqlStr).DefaultView;
        }


    }
}
