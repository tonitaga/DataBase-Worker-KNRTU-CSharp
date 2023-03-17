using DBCSharpUsage.DBModel.DBHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBFacadeModel
    {
        private static DBFacadeModel _model = null;
        private SqlConnection _connection = null;
        private DBSelecter _selecter = null;
        private bool _isConnected = false;

        private DBFacadeModel() {}

        public static DBFacadeModel GetModel()
        {
            if (_model == null )
            {
                _model = new DBFacadeModel();
            }
            return _model;
        }

        public DBInfo ConnectToDataBase(string connection_key)
        {
            try
            {
                _connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connection_key].ConnectionString);
                _connection.Open();
                _isConnected = true;
            }
            catch
            {
                _isConnected = false;
                return DBInfo.DBConnectionError;
            }
            return DBInfo.DBConnectionSuccess;
        }

        public DBInfo GetCurrentDBConnectionState()
        {
            return _isConnected ? DBInfo.DBConnectionSuccess : DBInfo.DBConnectionError;
        }

        public DBInfo InsertToDataBase(DBMainTableItem row)
        {
            if (!_isConnected)
            {
                return DBInfo.DBConnectionError;
            }
            return DBInserter.GetInserter(_connection).Insert(row);
        }

        public DBInfo RemoveFromDataBase(DBMainTableItem row)
        {
            if (!_isConnected)
            {
                return DBInfo.DBConnectionError;
            }
            return DBDeleter.GetDeleter(_connection).Delete(row);
        }

        public DBInfo UpdateTheDataBase(DBMainTableItem o, DBMainTableItem n)
        {
            if (!_isConnected)
            {
                return DBInfo.DBConnectionError;
            }
            return DBUpdater.GetUpdater(_connection).Update(o, n);
        }

        public DBInfo SelectFromDataBase(string commantText)
        {
            _selecter = new DBSelecter(_connection);
            return _selecter.Select(commantText, new DBSelecter.DBLoginSelecter());
        }

        public KeyValuePair<string, string> GetLastAddedHistory()
        {
            return DBQueryHistory.GetHistory().Get();
        }

        public string GetDBInfoString(DBInfo info)
        {
            string str = info.ToString();
            switch (info)
            {
                case DBInfo.DBConnectionError:
                    str = "Не удалось подключиться к базе данных\n" +
                          "Ошибка: " + info.ToString();
                    break;
                case DBInfo.DBConnectionSuccess:
                    str = "Успешное подключение к базе данных\n" +
                          "Статус: " + info.ToString();
                    break;
                case DBInfo.DBInsertionError:
                    str = "Не удалось сделать вставку в таблицу\n" +
                          "Ошибка: " + info.ToString();
                    break;
                case DBInfo.DBInsertionSuccess:
                    str = "Успешная вставка в таблицу\n" +
                          "Статус: " + info.ToString();
                    break;
                case DBInfo.DBRemoveError:
                    str = "Не удалось сделать удаление из таблицы\n" +
                          "Ошибка: " + info.ToString();
                    break;
                case DBInfo.DBRemoveSuccess:
                    str = "Успешное удаление из таблицы\n" +
                          "Статус: " + info.ToString();
                    break;
                case DBInfo.DBSelectionError:
                    str = "Не удалось сделать выборку из таблицы\n" +
                          "Ошибка: " + info.ToString();
                    break;
                case DBInfo.DBSelectionIsEmpty:
                    str = "Пустая выборка из таблицы\n" +
                          "Статус: " + info.ToString();
                    break;
                case DBInfo.DBSelectionSuccess:
                    str = "Успешная выборка из таблицы\n" +
                          "Статус: " + info.ToString();
                    break;
                case DBInfo.DBInvalidInput:
                    str = "Некорректный ввод\n" +
                          "Ошибка: " + info.ToString();
                    break;
                case DBInfo.DBUpdateSuccess:
                    str = "Обновление данных строки прошло успешно\n" +
                          "Статус: " + info.ToString();
                    break;
                case DBInfo.DBUpdateError:
                    str = "Ошибка обновления данных строки\n" +
                          "Ошибка: " + info.ToString();
                    break;
                case DBInfo.DBUserLoginSuccess:
                    str = "Авторизация прошла успешно\n" +
                        "Статус: " + info.ToString();
                    break;
                case DBInfo.DBUserLoginError:
                    str = "Неправильный логин/пароль\n" +
                          "Ошибка: " + info.ToString();
                    break;
                case DBInfo.DBError:
                    str = "Неопознанная ошибка. Попробуйте позже\n" +
                          "Ошибка: " + info.ToString();
                    break;
            }
            return str;
        }
    }
}
