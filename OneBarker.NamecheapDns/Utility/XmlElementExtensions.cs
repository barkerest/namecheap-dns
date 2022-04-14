using System.Xml;
using OneBarker.NamecheapDns.Exceptions;

namespace OneBarker.NamecheapDns.Utility;

/// <summary>
/// Extension methods for XmlElements.
/// </summary>
internal static partial class XmlElementExtensions
{
    /// <summary>
    /// Returns the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetContent(this XmlElement element) => element.InnerText.Trim();

    /// <summary>
    /// Returns the content of the child element with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetChildContent(this XmlElement element, string name)
        => element.ChildNodes
                  .OfType<XmlElement>()
                  .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                  ?.InnerText ?? "";

    /// <summary>
    /// Returns the content of the child element with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static string GetRequiredChildContent(this XmlElement element, string name)
        => GetChildContent(element, name) ?? throw new MissingElementException(element.Name, name);
    
    /// <summary>
    /// Returns the specified child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static XmlElement? GetChild(this XmlElement element, string name)
        => element.ChildNodes
                  .OfType<XmlElement>()
                  .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Returns the specified child element. 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static XmlElement GetRequiredChild(this XmlElement element, string name)
        => GetChild(element, name) ?? throw new MissingElementException(name);

    /// <summary>
    /// Returns the value for the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static string GetRequiredAttribute(this XmlElement element, string name)
        => element.HasAttribute(name) ? element.GetAttribute(name) : throw new MissingAttributeException(element.Name, name);
    
    
    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TEnum GetAttributeAsEnum<TEnum>(this XmlElement element, string name, TEnum defaultValue = default) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum type.");
        return Enum.TryParse<TEnum>(element.GetAttribute(name), true, out var result) ? result : defaultValue;
    }
    
    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingAttributeException"></exception>
    public static TEnum GetRequiredAttributeAsEnum<TEnum>(this XmlElement element, string name) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum type.");
        return Enum.Parse<TEnum>(element.GetRequiredAttribute(name), true);
    }

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TEnum GetContentAsEnum<TEnum>(this XmlElement element, TEnum defaultValue = default) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new InvalidOperationException("TEnum must be an enum type.");
        return Enum.TryParse<TEnum>(element.GetContent(), true, out var result) ? result : defaultValue;
    }   
    
    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static TEnum GetRequiredContentAsEnum<TEnum>(this XmlElement element) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new InvalidOperationException("TEnum must be an enum type.");
        return Enum.Parse<TEnum>(element.GetContent(), true);
    }   
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="MissingElementException"></exception>
    public static TEnum GetRequiredChildContentAsEnum<TEnum>(this XmlElement element, string name) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new InvalidOperationException("TEnum must be an enum type.");
        return Enum.Parse<TEnum>(element.GetRequiredChildContent(name), true);
    }

    /// <summary>
    /// Ensures the element has the required name (case-insensitive).
    /// </summary>
    /// <param name="element"></param>
    /// <param name="requiredName"></param>
    /// <exception cref="IncorrectXmlElementException"></exception>
    public static void RequireName(this XmlElement element, string requiredName)
    {
        if (!element.Name.Equals(requiredName, StringComparison.OrdinalIgnoreCase))
            throw new IncorrectXmlElementException(requiredName, element.Name);
    }
}
