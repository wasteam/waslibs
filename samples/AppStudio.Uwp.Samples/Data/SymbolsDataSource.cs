using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples
{
    class SymbolsDataSource
    {
        public IEnumerable<object> GetItems()
        {
            return GetSymbols().Select(r => r.Key).Cast<object>();
        }

        private Dictionary<string, Symbol> GetSymbols()
        {
            var symbols = Enum.GetValues(typeof(Symbol)).Cast<Symbol>().Select(r => new { Key = r.ToString(), Value = (Symbol)r });
            return symbols.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
