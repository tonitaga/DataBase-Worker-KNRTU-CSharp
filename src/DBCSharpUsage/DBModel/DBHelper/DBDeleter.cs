using DBCSharpUsage.DBModel.DBHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBDeleter
    {
        private static DBDeleter _deleter = null;
        private SqlConnection _connection = null;
        private DBSelecter _selecter = null;
        private SqlCommand _command = null;

        private DBDeleter() { }

        public static DBDeleter GetDeleter(SqlConnection connection)
        {
            if (_deleter == null)
            {
                _deleter = new DBDeleter();
            }
            _deleter.SetConnection(connection);
            _deleter._selecter = new DBSelecter(connection);
            _deleter._command = new SqlCommand();
            return _deleter;
        }

        private void SetConnection(SqlConnection connection)
        {
            _connection = connection;
        }

        public DBInfo Delete(DBMainTableItem item)
        {
            if (!DBChangeDataChecker.GetChecker(item).IsCorrect())
            {
                return DBInfo.DBInvalidInput;
            }

            DBInfo info = new DBInfo();
            item = GetIdentifiersFromNames(item);
            DBExistsChecker existsChecker = new DBExistsChecker(_connection);
            if (existsChecker.IsExistsInDB(item) == DBInfo.DBRowExists)
            {
                _command.Connection = _connection;
                try
                {
                    _command.CommandText = $"DELETE FROM [Students]\n" +
                                           $"WHERE Surname=N'{item.surname}' AND\n" +
                                                 $"Name=N'{item.name}' AND\n" +
                                                 $"Lastname=N'{item.lastname}' AND\n" +
                                                 $"Faccode={int.Parse(item.faccode)} AND\n" +
                                                 $"Groupid= {int.Parse(item.groupid)} AND\n" +
                                                 $"Otdelcode={short.Parse(item.otdelcode)} AND\n" +
                                                 $"Obuchcode={short.Parse(item.obuchcode)} AND\n" +
                                                 $"Obrcode={short.Parse(item.obrcode)} AND\n" +
                                                 $"Statuscode={short.Parse(item.statuscode)}";
                    _command.ExecuteNonQuery();

                    DBQueryHistory.GetHistory().Add(DBQueryType.DELETE, _command.CommandText);
                    info = DBInfo.DBRemoveSuccess;
                }
                catch
                {
                    info = DBInfo.DBRemoveError;
                }
            } else
            {
                info = DBInfo.DBRemoveError;
            }
            return info;
        }

        private DBMainTableItem GetIdentifiersFromNames(DBMainTableItem item)
        {
            string faccode = "", otdelcode = "", obuchcode = "", obrcode = "", statuscode = "", groupid = "";
            try
            {
                _selecter.Select($"SELECT Faccode FROM Faculties WHERE Name = N'{item.faccode}'");
                faccode = _selecter.GetRowColElement(0, 0);

                _selecter.Select($"SELECT Otdelcode FROM Departments WHERE Name = N'{item.otdelcode}'");
                otdelcode = _selecter.GetRowColElement(0, 0);

                _selecter.Select($"SELECT Obuchcode FROM EducationForm WHERE Name = N'{item.obuchcode}'");
                obuchcode = _selecter.GetRowColElement(0, 0);

                _selecter.Select($"SELECT Obrcode FROM Educations WHERE Name = N'{item.obrcode}'");
                obrcode = _selecter.GetRowColElement(0, 0);

                _selecter.Select($"SELECT Statuscode FROM StudentsStatus WHERE Name = N'{item.statuscode}'");
                statuscode = _selecter.GetRowColElement(0, 0);

                _selecter.Select($"SELECT Groupid FROM Groups WHERE Groupnum = '{item.groupid}'");
                groupid = _selecter.GetRowColElement(0, 0);
            }
            catch
            {
                return new DBMainTableItem();
            }

            return new DBMainTableItem(item.name, item.surname, item.lastname, item.birthday, faccode,
                                       groupid, otdelcode, obuchcode, obrcode, statuscode);
        }
    }
}
