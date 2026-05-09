using System.Collections.Generic;

public class Blackboard
{
    private readonly Dictionary<string, object> data = new();

    public void Set<T>(string key, T value) => data[key] = value;

    public T GetOrDefault<T>(string key, T defaultValue = default)
    {
        if (!data.TryGetValue(key, out var value))
            return defaultValue;

        if (value is T typed)
            return typed;

        return defaultValue;
    }

    public bool Has(string key) => data.ContainsKey(key);
    public void Remove(string key) => data.Remove(key);
    public void Clear() => data.Clear();
}