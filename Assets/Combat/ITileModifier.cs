public interface ITileModifier
{
    bool BlocksMovement { get; }
    bool BlocksLOS { get; }
    void OnEnter(ICombatActor actor) { }
    void OnExit(ICombatActor actor) { }

    interface IState
    {
        byte[] Bytes { get; }
        void PopulateFromBytes(byte[] bytes);
    }
}