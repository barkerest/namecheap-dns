

using System.Xml;
using OneBarker.NamecheapDns.Exceptions;

namespace OneBarker.NamecheapDns.Utility;

/// <summary>
/// Extension methods for XmlElements.
/// </summary>
internal static partial class XmlElementExtensions
{   
    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool GetAttributeAsBoolean(this XmlElement element, string name, bool defaultValue = default)
        => bool.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool GetContentAsBoolean(this XmlElement element, bool defaultValue = default)
        => bool.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool GetChildContentAsBoolean(this XmlElement element, string name, bool defaultValue = default)
        => bool.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static bool GetRequiredAttributeAsBoolean(this XmlElement element, string name)
        => bool.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool GetRequiredContentAsBoolean(this XmlElement element)
        => bool.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static bool GetRequiredChildContentAsBoolean(this XmlElement element, string name)
        => bool.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static sbyte GetAttributeAsInt8(this XmlElement element, string name, sbyte defaultValue = default)
        => sbyte.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static sbyte GetContentAsInt8(this XmlElement element, sbyte defaultValue = default)
        => sbyte.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static sbyte GetChildContentAsInt8(this XmlElement element, string name, sbyte defaultValue = default)
        => sbyte.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static sbyte GetRequiredAttributeAsInt8(this XmlElement element, string name)
        => sbyte.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static sbyte GetRequiredContentAsInt8(this XmlElement element)
        => sbyte.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static sbyte GetRequiredChildContentAsInt8(this XmlElement element, string name)
        => sbyte.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static byte GetAttributeAsUInt8(this XmlElement element, string name, byte defaultValue = default)
        => byte.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static byte GetContentAsUInt8(this XmlElement element, byte defaultValue = default)
        => byte.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static byte GetChildContentAsUInt8(this XmlElement element, string name, byte defaultValue = default)
        => byte.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static byte GetRequiredAttributeAsUInt8(this XmlElement element, string name)
        => byte.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static byte GetRequiredContentAsUInt8(this XmlElement element)
        => byte.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static byte GetRequiredChildContentAsUInt8(this XmlElement element, string name)
        => byte.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static short GetAttributeAsInt16(this XmlElement element, string name, short defaultValue = default)
        => short.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static short GetContentAsInt16(this XmlElement element, short defaultValue = default)
        => short.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static short GetChildContentAsInt16(this XmlElement element, string name, short defaultValue = default)
        => short.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static short GetRequiredAttributeAsInt16(this XmlElement element, string name)
        => short.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static short GetRequiredContentAsInt16(this XmlElement element)
        => short.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static short GetRequiredChildContentAsInt16(this XmlElement element, string name)
        => short.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ushort GetAttributeAsUInt16(this XmlElement element, string name, ushort defaultValue = default)
        => ushort.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ushort GetContentAsUInt16(this XmlElement element, ushort defaultValue = default)
        => ushort.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ushort GetChildContentAsUInt16(this XmlElement element, string name, ushort defaultValue = default)
        => ushort.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static ushort GetRequiredAttributeAsUInt16(this XmlElement element, string name)
        => ushort.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static ushort GetRequiredContentAsUInt16(this XmlElement element)
        => ushort.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static ushort GetRequiredChildContentAsUInt16(this XmlElement element, string name)
        => ushort.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetAttributeAsInt32(this XmlElement element, string name, int defaultValue = default)
        => int.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetContentAsInt32(this XmlElement element, int defaultValue = default)
        => int.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetChildContentAsInt32(this XmlElement element, string name, int defaultValue = default)
        => int.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static int GetRequiredAttributeAsInt32(this XmlElement element, string name)
        => int.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static int GetRequiredContentAsInt32(this XmlElement element)
        => int.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static int GetRequiredChildContentAsInt32(this XmlElement element, string name)
        => int.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static uint GetAttributeAsUInt32(this XmlElement element, string name, uint defaultValue = default)
        => uint.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static uint GetContentAsUInt32(this XmlElement element, uint defaultValue = default)
        => uint.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static uint GetChildContentAsUInt32(this XmlElement element, string name, uint defaultValue = default)
        => uint.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static uint GetRequiredAttributeAsUInt32(this XmlElement element, string name)
        => uint.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static uint GetRequiredContentAsUInt32(this XmlElement element)
        => uint.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static uint GetRequiredChildContentAsUInt32(this XmlElement element, string name)
        => uint.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long GetAttributeAsInt64(this XmlElement element, string name, long defaultValue = default)
        => long.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long GetContentAsInt64(this XmlElement element, long defaultValue = default)
        => long.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long GetChildContentAsInt64(this XmlElement element, string name, long defaultValue = default)
        => long.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static long GetRequiredAttributeAsInt64(this XmlElement element, string name)
        => long.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static long GetRequiredContentAsInt64(this XmlElement element)
        => long.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static long GetRequiredChildContentAsInt64(this XmlElement element, string name)
        => long.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ulong GetAttributeAsUInt64(this XmlElement element, string name, ulong defaultValue = default)
        => ulong.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ulong GetContentAsUInt64(this XmlElement element, ulong defaultValue = default)
        => ulong.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ulong GetChildContentAsUInt64(this XmlElement element, string name, ulong defaultValue = default)
        => ulong.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static ulong GetRequiredAttributeAsUInt64(this XmlElement element, string name)
        => ulong.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static ulong GetRequiredContentAsUInt64(this XmlElement element)
        => ulong.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static ulong GetRequiredChildContentAsUInt64(this XmlElement element, string name)
        => ulong.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetAttributeAsFloat(this XmlElement element, string name, float defaultValue = default)
        => float.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetContentAsFloat(this XmlElement element, float defaultValue = default)
        => float.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetChildContentAsFloat(this XmlElement element, string name, float defaultValue = default)
        => float.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static float GetRequiredAttributeAsFloat(this XmlElement element, string name)
        => float.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static float GetRequiredContentAsFloat(this XmlElement element)
        => float.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static float GetRequiredChildContentAsFloat(this XmlElement element, string name)
        => float.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double GetAttributeAsDouble(this XmlElement element, string name, double defaultValue = default)
        => double.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double GetContentAsDouble(this XmlElement element, double defaultValue = default)
        => double.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double GetChildContentAsDouble(this XmlElement element, string name, double defaultValue = default)
        => double.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static double GetRequiredAttributeAsDouble(this XmlElement element, string name)
        => double.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static double GetRequiredContentAsDouble(this XmlElement element)
        => double.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static double GetRequiredChildContentAsDouble(this XmlElement element, string name)
        => double.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal GetAttributeAsDecimal(this XmlElement element, string name, decimal defaultValue = default)
        => decimal.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal GetContentAsDecimal(this XmlElement element, decimal defaultValue = default)
        => decimal.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal GetChildContentAsDecimal(this XmlElement element, string name, decimal defaultValue = default)
        => decimal.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static decimal GetRequiredAttributeAsDecimal(this XmlElement element, string name)
        => decimal.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static decimal GetRequiredContentAsDecimal(this XmlElement element)
        => decimal.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static decimal GetRequiredChildContentAsDecimal(this XmlElement element, string name)
        => decimal.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime GetAttributeAsDateTime(this XmlElement element, string name, DateTime defaultValue = default)
        => DateTime.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime GetContentAsDateTime(this XmlElement element, DateTime defaultValue = default)
        => DateTime.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime GetChildContentAsDateTime(this XmlElement element, string name, DateTime defaultValue = default)
        => DateTime.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static DateTime GetRequiredAttributeAsDateTime(this XmlElement element, string name)
        => DateTime.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static DateTime GetRequiredContentAsDateTime(this XmlElement element)
        => DateTime.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static DateTime GetRequiredChildContentAsDateTime(this XmlElement element, string name)
        => DateTime.Parse(element.GetRequiredChildContent(name));

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Guid GetAttributeAsGuid(this XmlElement element, string name, Guid defaultValue = default)
        => Guid.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Guid GetContentAsGuid(this XmlElement element, Guid defaultValue = default)
        => Guid.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Guid GetChildContentAsGuid(this XmlElement element, string name, Guid defaultValue = default)
        => Guid.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static Guid GetRequiredAttributeAsGuid(this XmlElement element, string name)
        => Guid.Parse(element.GetRequiredAttribute(name));

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Guid GetRequiredContentAsGuid(this XmlElement element)
        => Guid.Parse(element.GetContent());
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static Guid GetRequiredChildContentAsGuid(this XmlElement element, string name)
        => Guid.Parse(element.GetRequiredChildContent(name));

}

