using System;
using System.Collections.Generic;
using System.Linq;

public static class IBinarySerializableFactory<T> where T : IBinarySerializable
{
    private static Dictionary<string, Type> cached_Types;
    public static Dictionary<string, Type> Types => cached_Types ??= GetTypes();

    private static readonly string[] assemblyBlacklist = {
        "unity",
        "mscorlib",
        "netstandard",
        "system",
        "bee.beedriver",
        "mono.security",
        "psdplugin",
        "niceio",
        "playerbuildprogramlibrary",
        "androidplayerbuildprogram",
        "webglplayerbuildprogram",
        "scriptcompilationbuildprogram",
        "beebuild",
    };

    private static Dictionary<string, Type> GetTypes()
    {
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().Where(asmbly => !assemblyBlacklist.Any(blacklistItem => asmbly.FullName.ToLower().Contains(blacklistItem))).ToArray();
        var types = assemblies.SelectMany(asmbly => asmbly.GetTypes()).ToArray();
        types = types.Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface).ToArray();
        return types.ToDictionary(type => type.Name.ToLower());

    }

    public static T CreateDefault(string name)
    {
        if (!Types.ContainsKey(name.ToLower())) return default;
        var type = Types[name.ToLower()];
        var instance = (T)Activator.CreateInstance(type);
        return instance;
    }

    public static T Create(string name, byte[] byteData)
    {
        var instance = CreateDefault(name);
        if (instance != null) instance.ByteData = byteData;
        return instance;
    }
}