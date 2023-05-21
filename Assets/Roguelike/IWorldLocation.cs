public interface IWorldLocation : IBinarySerializable, IReadOnlyWorldLocation
{
    public void OnPickOption(int option, RunInfo runInfo);
}

public interface IReadOnlyWorldLocation
{
    public string Name { get; }
    public string LocationImage { get => Name; }
    public string storyText { get; }
    public string[] optionTexts { get; }
}