using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using OneBarker.NamecheapDns.Models;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapDns.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class HostRecordImplementations_Should
{
    private static readonly (Type type, ConstructorInfo? ctor, bool usesPref, string testValue)[] RecordTypes;

    static HostRecordImplementations_Should()
    {
        var t = typeof(IHostRecord);
        var m = typeof(HostMailRecord);
        var c = typeof(HostCAAuthRecord);
        RecordTypes = t.Assembly
                       .GetTypes()
                       .Where(
                           x => x.IsClass &&
                                !x.IsAbstract &&
                                t.IsAssignableFrom(x) &&
                                (!t.IsGenericType || (t.GetGenericArguments() is { Length: 1 } args && t.IsAssignableFrom(args[0])))
                       )
                       .Select(x => x.IsGenericType ? x.MakeGenericType(typeof(HostTextRecord)) : x)
                       .Select(
                           x =>
                               (
                                   type: x,
                                   ctor: x.GetConstructor(BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes),
                                   usesPref: m.IsAssignableFrom(x),
                                   testValue: c.IsAssignableFrom(x) ? "2 issue \"or.not.2.issue\"" : "b"
                               )
                       )
                       .ToArray();
    }

    public static IEnumerable<object?[]> GetTestData()
        => RecordTypes.Select(recType => new object?[] { recType.type, recType.ctor, recType.usesPref, recType.testValue });

    private static readonly PropertyInfo[] InterfaceProperties = typeof(IHostRecord).GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.Name).ToArray();
    private static readonly MethodInfo[]   InterfaceMethods    = typeof(IHostRecord).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => !x.IsSpecialName).OrderBy(x => x.Name).ToArray();

    private readonly ITestOutputHelper _output;

    public HostRecordImplementations_Should(ITestOutputHelper outputHelper)
    {
        _output = outputHelper;
    }

    // We are only testing full implementations of IHostRecord.
    //  1) they must have a public parameterless constructor (makes them work with generic types).
    //  2) they must have all interface properties as read-only (limit the possibility for unexpected data changes).
    //  3) Preference should be explicit unless it is being used (we want it for internal use always, but externally it's only useful when used).
    //  4) all interface methods should be explicit (the methods are used internally, but the interface allows for extensibility).
    //  5a) SetValues must return a value of the same type.
    //  5b) SetValues must set the corresponding properties in the returned object.


    [Theory]
    [MemberData(nameof(GetTestData))]
    public void HavePublicDefaultConstructor(Type type, ConstructorInfo? ctor, bool usesPref, string testValue)
    {
        Assert.NotNull(ctor);
        var o = ctor!.Invoke(null);
        Assert.NotNull(o);
        Assert.True(o is IHostRecord);
        Assert.Equal(type, o.GetType());
        _output.WriteLine($"{type} has a public default constructor.");
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void HaveReadOnlyProperties(Type type, ConstructorInfo? ctor, bool usesPref, string testValue)
    {
        var myProps = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                          .Where(x => InterfaceProperties.Any(y => y.Name == x.Name))
                          .OrderBy(x => x.Name)
                          .ToArray();

        var props = myProps.Where(x => x.CanWrite)
                           .Select(x => x.Name)
                           .ToArray();

        if (props.Any())
        {
            _output.WriteLine($"The following public properties in {type} are read/write: " + string.Join(", ", props));
        }
        else
        {
            _output.WriteLine($"All public properties in {type} are read-only.");
        }

        Assert.Empty(props);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void HaveExplicitPreferenceUnlessUsed(Type type, ConstructorInfo? ctor, bool usesPref, string testValue)
    {
        var myProps = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                          .Where(x => InterfaceProperties.Any(y => y.Name == x.Name))
                          .OrderBy(x => x.Name)
                          .ToArray();

        // explicit properties are those with private/final getters and those defined in the interface but missing from the implementation.
        var props = myProps.Where(x => x.GetMethod is { IsPrivate: true, IsFinal: true })
                           .Select(x => x.Name)
                           .Concat(
                               InterfaceProperties.Where(x => myProps.All(y => y.Name != x.Name))
                                                  .Select(x => x.Name)
                           )
                           .OrderBy(x => x)
                           .ToArray();

        if (props.Any())
        {
            _output.WriteLine($"The following interface properties in {type} are defined explicitly: " + string.Join(", ", props));
        }
        else
        {
            _output.WriteLine($"All interface properties in {type} are publicly implemented.");
        }

        // Preference should only be public when we know it is being used.
        if (!usesPref)
        {
            Assert.Contains(props, x => x == nameof(IHostRecord.Preference));
        }
        else
        {
            Assert.DoesNotContain(props, x => x == nameof(IHostRecord.Preference));
        }
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void HaveExplicitInterfaceMethods(Type type, ConstructorInfo? ctor, bool usesPref, string testValue)
    {
        var myMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                            .Where(
                                x => InterfaceMethods.Any(
                                    y => y.Name == x.Name
                                         && y.GetParameters().Select(z => z.ParameterType).SequenceEqual(x.GetParameters().Select(z => z.ParameterType))
                                )
                            )
                            .OrderBy(x => x.Name)
                            .ToArray();

        var methods = myMethods.Where(x => x is not { IsPrivate: true, IsFinal: true })
                               .Select(x => x.Name)
                               .ToArray();

        if (methods.Any())
        {
            _output.WriteLine($"The following interface methods in {type} are not defined explicitly: " + string.Join(", ", methods));
        }
        else
        {
            _output.WriteLine($"All interface methods in {type} are defined explicitly.");
        }

        Assert.Empty(methods);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void HaveValidReturnFromSetValues(Type type, ConstructorInfo? ctor, bool usesPref, string testValue)
    {
        Assert.NotNull(ctor);
        var item = (IHostRecord)ctor!.Invoke(null);
        Assert.Equal(type, item.GetType());

        var item2 = item.Create("a", testValue, 0xc, 0xd);
        _output.WriteLine($"The SetValues method of {type} returns {item2.GetType()} objects.");
        // allow for descendants of {type}.
        Assert.True(type.IsInstanceOfType(item2));
        // should be a new object.
        Assert.False(ReferenceEquals(item, item2), $"The SetValues method of {type} returns the caller.");

        Assert.Equal("a", item2.Name);
        Assert.Equal(testValue, item2.Value);
        Assert.Equal(0xc, item2.TimeToLive);
        Assert.Equal(usesPref ? 0xd : 0, item2.Preference);
    }
}
