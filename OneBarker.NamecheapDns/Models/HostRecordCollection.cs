using System.Collections;
using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A collection of host records of the same type and name.
/// </summary>
/// <typeparam name="T"></typeparam>
public class HostRecordCollection<T> : IHostRecordCollectionWithPreference where T : class, IHostRecord, new()
{
    private static readonly T Default = new T();
    
    private          int                      _preference;
    private readonly ICollection<IHostRecord> _allHostRecords;
    private readonly bool                     _ownAllHostRecords;

    private static IHostRecord CreateItem(string name, string value, int? timeToLive)
    {
        return Default.Create(name, value, timeToLive ?? Default.TimeToLive, Default.Preference);
    }
    
    /// <summary>
    /// Create a collection.
    /// </summary>
    public HostRecordCollection()
        : this(new List<IHostRecord>(), "")
    {
        _ownAllHostRecords = true;
    }

    /// <summary>
    /// Create a collection from the supplied values.
    /// </summary>
    /// <param name="values"></param>
    public HostRecordCollection(IEnumerable<string> values)
        : this(values.Select(x => CreateItem("", x, null)).ToList(), "")
    {
        _ownAllHostRecords = true;
    }

    /// <summary>
    /// Create a collection from the supplied values.
    /// </summary>
    /// <param name="name">The name for this collection.</param>
    /// <param name="values">The values for this collection.</param>
    /// <param name="timeToLive">The TTL for this collection.</param>
    /// <param name="preference">The preference increment for this collection.</param>
    public HostRecordCollection(string name, IEnumerable<string> values, int? timeToLive = null, int? preference = null)
        : this(values.Select(x => CreateItem(name, x, timeToLive)).ToList(), name, timeToLive, preference)
    {
        _ownAllHostRecords = true;
    }

    /// <summary>
    /// Create a collection.
    /// </summary>
    /// <param name="name">The name of this collection.</param>
    /// <param name="timeToLive">The TTL for this collection.</param>
    /// <param name="preference">The preference increment for this collection.</param>
    public HostRecordCollection(string name, int? timeToLive = null, int? preference = null)
        : this(new List<IHostRecord>(), name, timeToLive, preference)
    {
        _ownAllHostRecords = true;
    }

    /// <summary>
    /// Create a collection tied to a parent collection.
    /// </summary>
    /// <param name="allHostRecords">The parent collection.</param>
    /// <param name="name">The name of this collection.</param>
    /// <param name="timeToLive">The TTL for this collection.</param>
    /// <param name="preference">The preference increment for this collection.</param>
    public HostRecordCollection(ICollection<IHostRecord> allHostRecords, string name, int? timeToLive = null, int? preference = null)
    {
        Type            = Default.Type;
        _preference     = Default.Preference <= 0 ? 0 : (preference ?? Default.Preference);
        _allHostRecords = allHostRecords;
        Name            = name;
        var matchedRecords = GetHostRecords().ToArray();
        _timeToLive     = timeToLive ?? (matchedRecords.Any() ? matchedRecords.Max(x => x.TimeToLive) :  Default.TimeToLive);
    }

    /// <inheritdoc />
    public void ChangeTimeToLive(int newTimeToLive)
    {
        _timeToLive = newTimeToLive;
        var toUpdate = GetHostRecords().ToArray();
        foreach (var record in toUpdate)
        {
            _allHostRecords.Remove(record);
            _allHostRecords.Add(Default.Create(record.Name, record.Value, newTimeToLive, record.Preference));
        }
    }

    /// <inheritdoc />
    public void ChangePreference(int newPreferenceStep)
    {
        _preference = Default.Preference > 0 ? newPreferenceStep : 0;
        if (Default.Preference <= 0) return;
        var toUpdate = GetSortedHostRecords().ToArray();
        var pref     = 0;
        foreach (var record in toUpdate)
        {
            pref += _preference;
            _allHostRecords.Remove(record);
            _allHostRecords.Add(Default.Create(record.Name, record.Value, record.TimeToLive, pref));
        }
    }

    /// <inheritdoc />
    public void SetValue(string newValue)
    {
        Clear();
        if (string.IsNullOrWhiteSpace(newValue)) return;
        foreach (var val in newValue.Split('\n').Select(x => x.Trim()))
        {
            Add(val);
        }
    }

    IEnumerable<IHostRecord> IHostRecordAccessor.ToHostRecords() => GetSortedHostRecords();

    private void UpdatePreferences()
    {
        if (_preference <= 0) return;
        var pref     = _preference;
        var toUpdate = GetSortedHostRecords().ToArray();
        foreach (var record in toUpdate)
        {
            _allHostRecords.Remove(record);
            _allHostRecords.Add(Default.Create(record.Name, record.Value, record.TimeToLive, pref));
            pref += _preference;
        }
    }

    /// <inheritdoc />
    public RecordType Type { get; }

    /// <inheritdoc />
    public string Name { get; }

    string IHostRecord.Value => string.Join("\n", GetSortedHostRecordValues());

    private int _timeToLive;

    /// <inheritdoc />
    public int TimeToLive => _timeToLive;

    /// <summary>
    /// Incremental preference value for this collection.
    /// </summary>
    int IHostRecord.Preference
        => _preference;

    private IEnumerable<IHostRecord> GetHostRecords()            => _allHostRecords.Where(x => x.Type == Type && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
    private IEnumerable<IHostRecord> GetSortedHostRecords()      => GetHostRecords().OrderBy(x => x.Preference).ThenBy(x => x.Value);
    private IEnumerable<string>      GetSortedHostRecordValues() => GetSortedHostRecords().Select(x => x.Value);

    /// <inheritdoc />
    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
    {
        var newValues = string.IsNullOrWhiteSpace(value) ? Array.Empty<string>() : value.Split('\n').Select(x => x.Trim()).ToArray();
        
        // if we own the parent collection, then we're just returning a new collection.
        if (_ownAllHostRecords)
        {
            return new HostRecordCollection<T>(name, newValues, timeToLive);
        }

        // the new collection we'll be returning.
        var ret = new HostRecordCollection<T>(_allHostRecords, name, timeToLive);
        
        // clear out any existing records with the specified name.
        ret.ClearValue();
        
        // then add in all the new values.
        foreach (var val in newValues)
        {
            ret.Add(val);
        }
        
        return ret;
    }

    IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetSortedHostRecordValues().GetEnumerator();
    IEnumerator IEnumerable.                GetEnumerator() => GetSortedHostRecordValues().GetEnumerator();

    /// <inheritdoc />
    public void Add(string item)
    {
        if (Contains(item)) return;
        var nextPref = _preference <= 0 ? 0 : (_preference + GetSortedHostRecords().Aggregate(0, (p, r) => r.Preference > p ? r.Preference : p));
        _allHostRecords.Add(Default.Create(Name, item, TimeToLive, nextPref));
    }


    /// <inheritdoc />
    public void ClearValue() => Clear();
    
    /// <inheritdoc  />
    public void Clear()
    {
        var toRemove = GetHostRecords().ToArray();
        foreach (var item in toRemove)
        {
            _allHostRecords.Remove(item);
        }
    }

    /// <inheritdoc />
    public bool Contains(string item) => GetHostRecords().Any(x => x.Value.Equals(item, StringComparison.Ordinal));

    void ICollection<string>.CopyTo(string[] array, int arrayIndex)
    {
        foreach (var item in GetSortedHostRecordValues())
        {
            array[arrayIndex++] = item;
        }
    }

    internal void CopyFrom(IHostRecordCollection other)
    {
        if (ReferenceEquals(this, other)) return;

        if (other is HostRecordCollection<T> c &&
            ReferenceEquals(_allHostRecords, c._allHostRecords) &&
            Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)) return;

        ChangeTimeToLive(other.TimeToLive);
        Clear();
        foreach (var val in other)
        {
            Add(val);
        }
    }

    /// <inheritdoc />
    public bool Remove(string item)
    {
        var toRemove = GetHostRecords().Where(x => x.Value.Equals(item, StringComparison.Ordinal)).ToArray();

        foreach (var record in toRemove)
        {
            _allHostRecords.Remove(record);
        }
        
        if (toRemove.Any()) UpdatePreferences();

        return toRemove.Any();
    }

    /// <inheritdoc />
    public int Count => GetHostRecords().Count();

    bool ICollection<string>.IsReadOnly => false;
    
    public static implicit operator HostRecordCollection<T>(string[] a) => new(a);
    public static implicit operator string[](HostRecordCollection<T> a) => a.ToArray();
}
