using UnityEngine;

public struct ClampedInt
{
    public ClampedInt(int min, int max, int value)
    {
        this.Min = min;
        this.Max = max;
        this.Value = value;
    }

    public int Min { get; set; }
    public int Max { get; set; }
    public int Value { get; set; }

    public static ClampedInt operator +(ClampedInt a, int b)
        => new ClampedInt(a.Min, a.Max, Mathf.Clamp(a.Value + b, a.Min, a.Max));
    public static ClampedInt operator -(ClampedInt a, int b)
        => new ClampedInt(a.Min, a.Max, Mathf.Clamp(a.Value - b, a.Min, a.Max));
    public static ClampedInt operator *(ClampedInt a, int b)
        => new ClampedInt(a.Min, a.Max, Mathf.Clamp(a.Value * b, a.Min, a.Max));
    public static ClampedInt operator /(ClampedInt a, int b)
        => new ClampedInt(a.Min, a.Max, Mathf.Clamp(a.Value / b, a.Min, a.Max));

    public static bool operator ==(ClampedInt a, int b) => a.Value == b;
    public static bool operator !=(ClampedInt a, int b) => a.Value != b;
    public static bool operator >(ClampedInt a, int b) => a.Value > b;
    public static bool operator <(ClampedInt a, int b) => a.Value < b;
    public static bool operator >=(ClampedInt a, int b) => a.Value >= b;
    public static bool operator <=(ClampedInt a, int b) => a.Value <= b;
    public static implicit operator int(ClampedInt clampedInt) => clampedInt.Value;
    public override bool Equals(object obj) => Value.Equals(obj);
    public override int GetHashCode() => Value.GetHashCode();
}