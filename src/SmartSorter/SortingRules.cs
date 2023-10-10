using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartSorter;

public class SortingRules : ReadOnlyDictionary<string, SortDirection>
{
    public SortingRules(IDictionary<string, SortDirection> dictionary) : base(dictionary)
    {
    }
}