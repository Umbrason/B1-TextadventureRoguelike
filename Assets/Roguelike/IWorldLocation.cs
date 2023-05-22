public interface IWorldLocation : IBinarySerializable, IReadOnlyWorldLocation
{
    public void OnPickOption(int option, RunInfo runInfo);

    byte[] IBinarySerializable.ByteData
    {
        get => new byte[0];
        set { }
    }
}

public interface IReadOnlyWorldLocation
{
    public string Name { get; }
    public string LocationImage { get => Name; }
    public string storyText { get; }
    public string[] optionTexts { get; }
}