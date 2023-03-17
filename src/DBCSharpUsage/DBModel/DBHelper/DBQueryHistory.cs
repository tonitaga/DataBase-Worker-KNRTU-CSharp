using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCSharpUsage.DBModel.DBHelper
{
    internal class DBQueryHistory
    {
        private static DBQueryHistory _history = null;
        private static int _current = 0;
        List<KeyValuePair<string, string>> _queries = null;
        private DBQueryHistory() {}

        public static DBQueryHistory GetHistory()
        {
            if (_history == null )
            {
                _history = new DBQueryHistory();
                _history._queries = new List<KeyValuePair<string, string>>();
            }
            return _history;
        }

        public void Add(DBQueryType type, string query)
        {
            _queries.Add(new KeyValuePair<string, string>(type.ToString(), query));
        }

        public int Size() => _queries.Count();
        public KeyValuePair<string, string> Get()
        {
            if (_current < _queries.Count)
            {
                return _queries[_current++];
            }
            return new KeyValuePair<string, string>();
        }

        public void Clear()
        {
            _queries.Clear();
        }
    }
}
