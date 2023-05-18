using UnityEngine;

public interface IWorldLocation : IBinarySerializable, IReadOnlyWorldLocation
{
    public void OnPickOption(int option);
}

public interface IReadOnlyWorldLocation
{
    public string Name { get; }
    public Sprite Image { get; }
    public string storyText { get; }
    public string[] optionTexts { get; }
}