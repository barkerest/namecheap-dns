namespace OneBarker.NamecheapDns.Models;

/// <summary>
/// A collection of host records of the same type.
/// </summary>
public interface IHostRecordCollection : IHostRecordAccessor, ICollection<string>
{
    
}

/// <summary>
/// A collection of host records of the same type.
/// </summary>
public interface IHostRecordCollectionWithPreference : IHostRecordCollection
{
    /// <summary>
    /// Change the preference for all accessible records if the records use preference.
    /// </summary>
    /// <param name="newPreferenceStep"></param>
    public void ChangePreference(int newPreferenceStep);
}