public interface ICombatActor
{
    public void NotifyTurnStart(TurnInfo turnInfo);
    public int Alignment { get; }
    public IState State { get; }
    interface IState
    {
        byte[] Bytes { get; }
        void PopulateFromBytes(byte[] bytes);
    }
}