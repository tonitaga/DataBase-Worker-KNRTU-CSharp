using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    internal class DBController
    {
        public DBController() {}

        public DBInfo ConnectToDataBase(string connection_key)
        {
            if (connection_key == null)
            {
                return DBInfo.DBConnectionError;
            }
            return DBFacadeModel.GetModel().ConnectToDataBase(connection_key);
        }

        public DBInfo InsertToDataBase(DBMainTableItem row)
        {
            return DBFacadeModel.GetModel().InsertToDataBase(row);
        }

        public DBInfo RemoveFromDataBase(DBMainTableItem row)
        {
            return DBFacadeModel.GetModel().RemoveFromDataBase(row);
        }

        public DBInfo UpdateTheDataBase(DBMainTableItem o,  DBMainTableItem n)
        { 
            return DBFacadeModel.GetModel().UpdateTheDataBase(o, n);
        }

        public DBInfo SelectFromDataBase(string commantText)
        {
            return DBFacadeModel.GetModel().SelectFromDataBase(commantText);
        }

        public DBInfo GetCurrentDBConnectionState()
        {
            return DBFacadeModel.GetModel().GetCurrentDBConnectionState();
        }

        public string GetDBInfoString(DBInfo info)
        {
            return DBFacadeModel.GetModel().GetDBInfoString(info);
        }

        public KeyValuePair<string, string> GetLastAddedHistory()
        {
            return DBFacadeModel.GetModel().GetLastAddedHistory();
        }
    }
}
