using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage
{
    public enum DBInfo
    {
        DBConnectionError,
        DBConnectionSuccess,
        DBInvalidInput,
        DBInsertionError,
        DBInsertionSuccess,
        DBSelectionError,
        DBSelectionIsEmpty,
        DBSelectionSuccess,
        DBRemoveError,
        DBRemoveSuccess,
        DBRowExists,
        DBRowNotFound,
        DBUpdateError,
        DBUpdateSuccess,
        DBUserLoginSuccess,
        DBUserLoginError,
        DBError
    }
}
