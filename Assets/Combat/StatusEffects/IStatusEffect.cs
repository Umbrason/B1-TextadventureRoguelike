using System.IO;

public interface IStatusEffect : IReadOnlyStatusEffect, IBinarySerializable
{
    public new int Duration { get; set; }
    public void OnApply(ICombatActor actor, CombatState state);
    public void OnRemove(ICombatActor actor, CombatState state);
    public void OnBeginTurn(ICombatActor actor, CombatState state);
    public void OnMove(ICombatActor actor, CombatState state);
    public void OnUseSkill(ICombatActor actor, CombatState state);
    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteInt(Duration);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Duration = stream.ReadInt();
        }
    }
}

public interface IReadOnlyStatusEffect
{
    int Duration { get; }
}
