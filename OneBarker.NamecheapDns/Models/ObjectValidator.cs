using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OneBarker.NamecheapDns.Models;

internal class ObjectValidator
{
    /// <summary>
    /// The object type this validator is for.
    /// </summary>
    public Type ObjectType { get; }

    private readonly (PropertyInfo Property, ValidationAttribute[] Validations)[] _propertiesToCheck;

    private ObjectValidator(Type objectType)
    {
        ObjectType = objectType;
        
        _propertiesToCheck = ObjectType
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(x => x.CanRead)
                                    .Select(x => (Property: x, Validations: x.GetCustomAttributes<ValidationAttribute>().ToArray()))
                                    .Where(x => x.Validations.Any())
                                    .ToArray();
    }

    
    public void ValidateObject(object data, ICollection<string> errors, string prefix)
    {
        if (data.GetType() != ObjectType)
            throw new ArgumentException("Command being checked does not match type of validator.");

        foreach (var prop in _propertiesToCheck)
        {
            var value = prop.Property.GetValue(data);
            var name  = string.IsNullOrWhiteSpace(prefix) ? prop.Property.Name : $"{prefix}.{prop.Property.Name}";
            
            foreach (var attrib in prop.Validations)
            {
                if (!attrib.IsValid(value))
                {
                    errors.Add(attrib.FormatErrorMessage(name));
                }
            }
        }

        if (data is IValidatableObject validatableObject)
        {
            foreach (var result in validatableObject.Validate(new ValidationContext(data)))
            {
                var mems = result.MemberNames.Select(x => string.IsNullOrWhiteSpace(prefix) ? x : $"{prefix}.{x}").ToArray();
                var err  = string.IsNullOrWhiteSpace(result.ErrorMessage) ? "appears to be invalid" : result.ErrorMessage;
                
                if (mems.Length == 1)
                {
                    err = $"The {mems[0]} field {err}";
                }
                else if (mems.Length == 0)
                {
                    err = "The object " + err;
                }
                else if (mems.Length == 2)
                {
                    err = $"The {mems[0]} and {mems[1]} fields {err}.";
                }
                else
                {
                    var members = string.Join(", ", mems[..^1]) + ", and " + mems[^1];
                    err = $"The {members} fields {err}.";
                }
                errors.Add(err);
            }
        }
        
    }
    
    private static readonly Dictionary<Type, ObjectValidator> Validators = new();

    /// <summary>
    /// Find or create the validator for the object.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ObjectValidator FindOrCreate(object value)
    {
        var t = value.GetType();
        if (Validators.ContainsKey(t)) return Validators[t];
        return Validators[t] = new ObjectValidator(t);
    }

    /// <summary>
    /// Validates the specified object.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static bool Validate(object value, out string[] errors)
        => Validate(value, "", out errors);

    /// <summary>
    /// Validates the specified object.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="prefix"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static bool Validate(object value, string prefix, out string[] errors)
    {
        var e = new List<string>();
        FindOrCreate(value).ValidateObject(value, e, prefix);
        errors = e.ToArray();
        return e.Count == 0;
    }

}
