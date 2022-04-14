using System.Collections;
using OneBarker.NamecheapDns.Utility;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A dictionary of host record sets.
/// </summary>
public class HostRecordDictionary : IHostRecordDictionary
{
    private readonly ICollection<IHostRecord> _allHostRecords;

    // we'll cache them as they are created.
    private readonly Dictionary<string, HostRecordSet> _hostRecordSets;

    public HostRecordDictionary()
        : this(new List<IHostRecord>())
    {
    }

    public HostRecordDictionary(ICollection<IHostRecord> allHostRecords)
    {
        _allHostRecords = allHostRecords;
        _hostRecordSets = new Dictionary<string, HostRecordSet>();
    }

    private IEnumerable<KeyValuePair<string, IHostRecordSet>> ToCollection()
        => Values.Select(x => new KeyValuePair<string, IHostRecordSet>(x.Name, x));

    IEnumerator<KeyValuePair<string, IHostRecordSet>> IEnumerable<KeyValuePair<string, IHostRecordSet>>.GetEnumerator() => ToCollection().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ToCollection().GetEnumerator();

    void ICollection<KeyValuePair<string, IHostRecordSet>>.Add(KeyValuePair<string, IHostRecordSet> item)
        => this[item.Key] = item.Value;

    /// <inheritdoc />
    public void Clear()
    {
        _allHostRecords.Clear();
        _hostRecordSets.Clear();
    }

    bool ICollection<KeyValuePair<string, IHostRecordSet>>.Contains(KeyValuePair<string, IHostRecordSet> item) => ContainsKey(item.Key);

    void ICollection<KeyValuePair<string, IHostRecordSet>>.CopyTo(KeyValuePair<string, IHostRecordSet>[] array, int arrayIndex)
    {
        foreach (var item in Values)
        {
            array[arrayIndex++] = new KeyValuePair<string, IHostRecordSet>(item.Name, item);
        }
    }

    bool ICollection<KeyValuePair<string, IHostRecordSet>>.Remove(KeyValuePair<string, IHostRecordSet> item) => Remove(item.Key);

    /// <inheritdoc />
    public int Count => GetKeys().Count();

    bool ICollection<KeyValuePair<string, IHostRecordSet>>.IsReadOnly => false;

    /// <summary>
    /// Adds or updates the host with the specified name (key).
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(string key, IHostRecordSet value)
        => this[key] = value;

    /// <inheritdoc />
    public bool ContainsKey(string key)
        => _allHostRecords.Any(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Removes the host with the specified name (key).
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Will always return true.</returns>
    public bool Remove(string key)
    {
        var cur = this[key];
        cur.Clear();
        _hostRecordSets.Remove(cur.Name);
        return true;
    }

    /// <summary>
    /// Gets the host with the specified name (key), creating a new host as necessary.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(string key, out IHostRecordSet value)
    {
        value = this[key];
        return true;
    }

    /// <summary>
    /// Gets or sets the host with the specified name (key), creating a new host as necessary.
    /// </summary>
    /// <param name="key"></param>
    public IHostRecordSet this[string key]
    {
        get
        {
            key = key.ToLower();
            if (!_hostRecordSets.ContainsKey(key))
            {
                _hostRecordSets.Add(key, new HostRecordSet(_allHostRecords, key));
            }

            return _hostRecordSets[key];
        }
        set => ((HostRecordSet)this[key]).CopyFrom(value);
    }

    private IEnumerable<string> GetKeys() => _allHostRecords.Select(x => x.Name.ToLower()).Distinct();

    /// <inheritdoc />
    public ICollection<string> Keys => GetKeys().OrderBy(x => x).ToArray();

    /// <inheritdoc />
    public ICollection<IHostRecordSet> Values => Keys.Select(x => this[x]).ToArray();


    /// <inheritdoc />
    public bool IsValid(out string[] errors)
    {
        var      allErrors = new List<string>();

        foreach (var key in Keys)
        {
            var      set = this[key];
            string[] currentErrors;
            
            if (!set.IsValid(out currentErrors))
            {
                foreach (var e in currentErrors)
                {
                    allErrors.Add($"Host: {key}, {e}");
                }
            }

            foreach (var record in _allHostRecords.Where(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Type).ThenBy(x => x.Preference).ThenBy(x => x.Value))
            {
                if (!ObjectValidator.Validate(record, out currentErrors))
                {
                    foreach (var e in currentErrors)
                    {
                        allErrors.Add($"Host: {key}, Type: {record.Type}, {e}");
                    }
                }
            }
        }

        errors = allErrors.ToArray();
        return (allErrors.Count == 0);
    }
}
