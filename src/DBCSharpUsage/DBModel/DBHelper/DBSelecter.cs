using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBSelecter
    {
        private SqlConnection _connection = null;
        private DataSet _dataSet = null;

        public DBSelecter(SqlConnection connection)
        {
            _connection = connection;
            _dataSet = new DataSet();
        }

        public class DBLoginSelecter { }

        public DBInfo Select(string selection)
        {
            _dataSet = new DataSet();
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selection, _connection);
                dataAdapter.Fill(_dataSet);
            }
            catch
            {
                return DBInfo.DBSelectionError;
            }
            
            try
            {
                GetRowColElement(0, 0);
            }
            catch
            {
                return DBInfo.DBSelectionIsEmpty;
            }
            return DBInfo.DBSelectionSuccess;
        }

        public DBInfo Select(string selection, DBLoginSelecter _)
        {

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(selection, _connection);
            DataTable table = new DataTable();

            adapter.SelectCommand = command;

            try
            {
                adapter.Fill(table);

                if (table.Rows.Count == 1)
                {
                    return DBInfo.DBUserLoginSuccess;
                }
            } catch
            {
                return DBInfo.DBError;
            }
            return DBInfo.DBUserLoginError;
        }

        public string GetRowColElement(int row, int column)
        {
            string element = "";
            try
            {
                element = _dataSet.Tables[0].Rows[row][column].ToString();
                return element;
            }
            catch {}
            return element;
        }
    }
}
