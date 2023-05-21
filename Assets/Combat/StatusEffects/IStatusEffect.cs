using System.IO;

public interface IStatusEffect : IReadOnlyStatusEffect, IBinarySerializable
{
    public new int Duration { get; set; }
    bool IReadOnlyStatusEffect.IsStackable => false;
    public void OnApply(ICombatActor actor, CombatState state) { }
    public void OnRemove(ICombatActor actor, CombatState state) { }
    public void OnBeginTurn(ICombatActor actor, CombatState state) { }
    public void OnMove(ICombatActor actor, CombatState state) { }
    public void OnUseSkill(ICombatActor actor, ISkill skill, CombatState state) { }
    public void OnDie(ICombatActor actor, CombatState state) { }
    public DamageInfo OnBeforeDealDamage(ICombatActor actor, DamageInfo attackInfo) => attackInfo;
    public void OnAfterDealDamage(ICombatActor actor, DamageResultInfo attackResultInfo) { }
    public DamageInfo OnBeforeRecieveDamage(ICombatActor actor, DamageInfo attackInfo) => attackInfo;
    public void OnAfterRecieveDamage(ICombatActor actor, DamageResultInfo attackResultInfo) { }
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
    bool IsStackable { get; }
}
