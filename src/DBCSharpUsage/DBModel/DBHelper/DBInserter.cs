using DBCSharpUsage.DBModel.DBHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DBCSharpUsage
{
    internal class DBInserter
    {
        private static DBInserter _inserter = null;
        private SqlConnection _connection = null;
        private DBSelecter _selecter = null;
        private SqlCommand _command = null;

        private DBInserter() { }

        public static DBInserter GetInserter(SqlConnection connection)
        {
            if (_inserter == null)
            {
                _inserter = new DBInserter();
            }
            _inserter.SetConnection(connection);
            _inserter._selecter = new DBSelecter(connection);
            _inserter._command = new SqlCommand();
            return _inserter;
        }

        public DBInfo Insert(DBMainTableItem item)
        {
            if (!DBChangeDataChecker.GetChecker(item).IsCorrect())
            {
                return DBInfo.DBInvalidInput;
            }

            string group = item.groupid;
            string groupnum = item.groupid.Substring(item.groupid.Length - 2);

            DBInfo info = new DBInfo();
            item = GetIdentifiersFromNames(item);
            DBExistsChecker existsChecker = new DBExistsChecker(_connection);
            if (existsChecker.IsExistsInDB(item) == DBInfo.DBRowNotFound)
            {
                _command.Connection = _connection;
                try
                {
                    if (item.groupid == "")
                    {
                        _command.CommandText = $"INSERT INTO [GROUPS] (Groupnum, Faccode, Yearf, Course) " +
                            $"VALUES (N'{group}', " +
                            $"{int.Parse(group[0].ToString())}, " +
                            $"{short.Parse(DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture)) - short.Parse(group[1].ToString())}, " +
                            $"{short.Parse(group[1].ToString())})";
                        _command.ExecuteNonQuery();
                        item.groupid = GetIdentifierFromGroupNum(group);
                    }
                    _command.CommandText = $"INSERT INTO [Students] (Surname, Name, Lastname, Birthday, Faccode, Groupid, Otdelcode, Obuchcode, Obrcode, Statuscode) " +
                    $"VALUES (N'{item.surname}', N'{item.name}', N'{item.lastname}', '{item.birthday}', {int.Parse(item.faccode)}, {int.Parse(item.groupid)}, {short.Parse(item.otdelcode)}, " +
                    $"{short.Parse(item.obuchcode)}, {short.Parse(item.obrcode)}, {short.Parse(item.statuscode)})";
                    _command.ExecuteNonQuery();

                    DBQueryHistory.GetHistory().Add(DBQueryType.INSERT, _command.CommandText);
                    info = DBInfo.DBInsertionSuccess;
                }
                catch
                {
                    info = DBInfo.DBInsertionError;
                }
            } else
            {
                info = DBInfo.DBInsertionError;
            }
            return info;
        }

        private void SetConnection(SqlConnection connection)
        {
            _connection = connection;
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
            } catch
            {
                return new DBMainTableItem();
            }

            return new DBMainTableItem(item.name, item.surname, item.lastname, item.birthday, faccode,
                                       groupid, otdelcode, obuchcode, obrcode, statuscode);
        }

        private string GetIdentifierFromGroupNum(string group)
        {
            try
            {
                _selecter.Select($"SELECT Groupid FROM Groups WHERE Groupnum = '{group}'");
            } catch
            {
                return "";
            }
            return _selecter.GetRowColElement(0, 0);
        }
    }
}
