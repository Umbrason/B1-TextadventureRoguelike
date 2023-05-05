public interface ITileModifier
{
    public bool BlocksMovement { get; }
    public bool BlocksLOS { get; }
    public void OnEnter(ICombatActor actor) { }
    public void OnExit(ICombatActor actor) { }

    interface IState
    {
        byte[] Bytes { get; }
        void PopulateFromBytes(byte[] bytes);
    }
}