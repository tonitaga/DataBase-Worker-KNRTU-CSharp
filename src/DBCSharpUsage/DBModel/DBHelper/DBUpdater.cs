using DBCSharpUsage.DBModel.DBHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBUpdater
    {
        private static DBUpdater _updater = null;
        private SqlConnection _connection = null;
        private DBSelecter _selecter = null;
        private SqlCommand _command = null;

        private DBUpdater() { }

        public static DBUpdater GetUpdater(SqlConnection connection)
        {
            if (_updater == null)
            {
                _updater = new DBUpdater();
                
            }
            _updater.SetConnection(connection);
            _updater._selecter = new DBSelecter(connection);
            _updater._command = new SqlCommand();
            return _updater;
        }

        public DBInfo Update(DBMainTableItem o, DBMainTableItem n)
        {
            if (!DBChangeDataChecker.GetChecker(o).IsCorrect() || !DBChangeDataChecker.GetChecker(n).IsCorrect())
            {
                return DBInfo.DBInvalidInput;
            }

            DBInfo info = new DBInfo();
            o = GetIdentifiersFromNames(o);
            n = GetIdentifiersFromNames(n);
            DBExistsChecker existsChecker = new DBExistsChecker(_connection);
            if (existsChecker.IsExistsInDB(o) == DBInfo.DBRowExists && existsChecker.IsExistsInDB(n) == DBInfo.DBRowNotFound)
            {
                _command.Connection = _connection;
                try
                {
                    _command.CommandText = $"UPDATE [Students]\n" +
                                           $"SET Surname = N'{n.surname}',\n" +
                                               $"Name = N'{n.name}',\n" +
                                               $"Lastname = N'{n.lastname}',\n" +
                                               $"Birthday = '{n.birthday}',\n" +
                                               $"Faccode = {int.Parse(n.faccode)},\n" +
                                               $"Groupid = {int.Parse(n.groupid)},\n" +
                                               $"Otdelcode = {short.Parse(n.otdelcode)},\n" +
                                               $"Obuchcode = {short.Parse(n.obuchcode)},\n" +
                                               $"Obrcode = {short.Parse(n.obrcode)},\n" +
                                               $"Statuscode = {short.Parse(n.statuscode)}\n" +
                                           $"WHERE Name = N'{o.name}' AND\n" +
                                                 $"Surname = N'{o.surname}' AND\n" +
                                                 $"Lastname = N'{o.lastname}' AND\n" +
                                                 $"Birthday = N'{o.birthday}' AND\n" +
                                                 $"Faccode = {int.Parse(o.faccode)} AND\n" +
                                                 $"Groupid = {int.Parse(o.groupid)} AND\n" +
                                                 $"Otdelcode = {short.Parse(o.otdelcode)} AND\n" +
                                                 $"Obuchcode = {short.Parse(o.obuchcode)} AND\n" +
                                                 $"Obrcode = {short.Parse(o.obrcode)} AND\n" +
                                                 $"Statuscode = {short.Parse(o.statuscode)}";
                    _command.ExecuteNonQuery();

                    DBQueryHistory.GetHistory().Add(DBQueryType.UPDATE, _command.CommandText);
                    info = DBInfo.DBUpdateSuccess;
                } catch
                {
                    info = DBInfo.DBUpdateError;
                }
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
