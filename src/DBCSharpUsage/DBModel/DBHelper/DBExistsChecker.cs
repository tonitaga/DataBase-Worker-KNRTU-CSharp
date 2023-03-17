using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBExistsChecker
    {
        private SqlConnection _connection = null;
        private DBSelecter _selection = null;
        private int _founded_row = -1;
        
        public DBExistsChecker(SqlConnection connection)
        {
            this._connection = connection;
            _selection = new DBSelecter(this._connection);
        }

        public DBInfo IsExistsInDB(DBMainTableItem item)
        {
            if (item == null) return DBInfo.DBInvalidInput;

            var selection_status = _selection.Select($"SELECT * FROM Students");
            if (selection_status == DBInfo.DBSelectionError) return DBInfo.DBSelectionError;

            int row = 0;
            while (true)
            {

                if (_selection.GetRowColElement(row, 0) == "")
                    break;

                if (_selection.GetRowColElement(row, 1) == item.surname && _selection.GetRowColElement(row, 2) == item.name &&
                    _selection.GetRowColElement(row, 3) == item.lastname && DateTime.Parse(_selection.GetRowColElement(row, 4)).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) == item.birthday &&
                    _selection.GetRowColElement(row, 5) == item.faccode && _selection.GetRowColElement(row, 6) == item.groupid &&
                    _selection.GetRowColElement(row, 7) == item.otdelcode && _selection.GetRowColElement(row, 8) == item.obuchcode &&
                    _selection.GetRowColElement(row, 9) == item.obrcode && _selection.GetRowColElement(row, 10) == item.statuscode)
                {
                    _founded_row = row;
                    return DBInfo.DBRowExists;
                }
                row++;
            }
            _founded_row = -1;
            return DBInfo.DBRowNotFound;
        }

        public int GetFoundedRowIndex() => _founded_row;
    }
}
