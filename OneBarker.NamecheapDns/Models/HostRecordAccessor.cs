using OneBarker.NamecheapDns.Enums;

namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// An accessor for a single host record.
/// </summary>
/// <typeparam name="T"></typeparam>
public class HostRecordAccessor<T> : IHostRecordAccessor where T : class, IHostRecord, new()
{
    private static readonly T Default = new T();

    private readonly ICollection<IHostRecord> _allHostRecords;
    private readonly bool                     _ownAllHostRecords;
    private readonly int                      _preference;
    private          int                      _timeToLive;

    public HostRecordAccessor()
        : this(new List<IHostRecord>(), "")
    {
        _ownAllHostRecords = true;
    }

    public HostRecordAccessor(string name)
        : this(new List<IHostRecord>(), name)
    {
        _ownAllHostRecords = true;
    }

    private static IHostRecord CreateItem(string name, string value, int? timeToLive = null, int? preference = null)
    {
        return Default.Create(name, value, timeToLive ?? Default.TimeToLive, preference ?? Default.Preference);
    }

    public HostRecordAccessor(string name, string value, int? timeToLive = null, int? preference = null)
        : this(new List<IHostRecord>() { CreateItem(name, value) }, name, timeToLive, preference)
    {
        _ownAllHostRecords = true;
    }

    public HostRecordAccessor(ICollection<IHostRecord> allHostRecords, string name, int? timeToLive = null, int? preference = null)
    {
        _allHostRecords = allHostRecords;
        Name            = name;
        Type            = Default.Type;
        var instance = GetInstance();
        _timeToLive = timeToLive ?? instance?.TimeToLive ?? Default.TimeToLive;
        _preference = Default.Preference <= 0 ? 0 : (preference ?? instance?.Preference ?? Default.Preference);
    }

    private IHostRecord? GetInstance() => _allHostRecords.FirstOrDefault(x => x.Type == Type && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));

    /// <inheritdoc />
    public RecordType Type { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string Value => GetInstance()?.Value ?? "";

    /// <inheritdoc />
    public int TimeToLive
        => _timeToLive;

    /// <inheritdoc />
    int IHostRecord.Preference => _preference;

    /// <inheritdoc />
    IHostRecord IHostRecord.Create(string name, string value, int timeToLive, int preference)
    {
        if (_ownAllHostRecords)
        {
            return new HostRecordAccessor<T>(name, value, timeToLive, preference);
        }

        var ret = new HostRecordAccessor<T>(_allHostRecords, name, timeToLive, preference);
        ret.SetValue(value);
        return ret;
    }

    /// <inheritdoc />
    public void ChangeTimeToLive(int newTimeToLive)
    {
        _timeToLive = newTimeToLive;
        var instance = GetInstance();
        if (instance is not null)
        {
            _allHostRecords.Remove(instance);
            _allHostRecords.Add(Default.Create(instance.Name, instance.Value, newTimeToLive, instance.Preference));
        }
    }
    
    /// <inheritdoc />
    public void SetValue(string newValue)
    {
        var instance = GetInstance();
        if (instance is not null)
        {
            _allHostRecords.Remove(instance);
        }

        _allHostRecords.Add(Default.Create(Name, newValue, _timeToLive, _preference));
    }

    /// <inheritdoc />
    public void ClearValue()
    {
        var instance = GetInstance();
        if (instance is not null)
        {
            _allHostRecords.Remove(instance);
        }
    }

    internal void CopyFrom(IHostRecordAccessor other)
    {
        if (ReferenceEquals(this, other)) return;

        if (other is HostRecordAccessor<T> h &&
            ReferenceEquals(_allHostRecords, h._allHostRecords) &&
            Name.Equals(h.Name, StringComparison.OrdinalIgnoreCase)) return;

        ChangeTimeToLive(other.TimeToLive);
        
        if (string.IsNullOrWhiteSpace(other.Value))
        {
            ClearValue();
        }
        else
        {
            SetValue(other.Value);
        }
    }
    
    /// <inheritdoc />
    IEnumerable<IHostRecord> IHostRecordAccessor.ToHostRecords()
    {
        var instance = GetInstance();
        if (instance is not null) yield return instance;
    }
}
