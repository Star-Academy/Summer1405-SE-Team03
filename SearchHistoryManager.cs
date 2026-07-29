using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchHistoryApp
{
    public class SearchHistoryManager
    {
        private List<string> _history = new List<string>();
        private Dictionary<string, int> _searchStats = new Dictionary<string, int>();
        private int _currentIndex = -1;
        public string Search(string query)
        {
            if (_currentIndex < _history.Count - 1 && _currentIndex >= 0)
            {
                _history.RemoveRange(_currentIndex + 1, _history.Count - (_currentIndex + 1));
            }

            _history.Add(query);
            _currentIndex = _history.Count - 1;

            if (_searchStats.ContainsKey(query))
                _searchStats[query]++;
            else
                _searchStats.Add(query, 1);

            return _history[_currentIndex];
        }

        public string GoBack()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                return _history[_currentIndex];
            }
            return null;
        }

        public string GoForward()
        {
            if (_currentIndex < _history.Count - 1 && _currentIndex >= 0)
            {
                _currentIndex++;
                return _history[_currentIndex];
            }
            return null;
        }

        public string GetCurrent()
        {
            if (_currentIndex >= 0 && _currentIndex < _history.Count)
            {
                return _history[_currentIndex];
            }
            return null;
        }

        public IEnumerable<KeyValuePair<string, int>> GetTopStats(int count = 3)
        {
            return _searchStats.OrderByDescending(p => p.Value).Take(count);
        }
        public int GetUniqueCount()
        {
            return _searchStats.Count;
        }
    }
}