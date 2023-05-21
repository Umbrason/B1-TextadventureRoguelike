using System.IO;
using UnityEngine;

public class TippedArrows : IStatusEffect
{
    public int Duration { get; set; } = 2;

    public TippedArrows() { }
    public TippedArrows(Element element)
    {
        this.element = element;
    }

    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteInt(Duration);
            stream.WriteEnum<Element>(element);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Duration = stream.ReadInt();
            element = stream.ReadEnum<Element>();
        }
    }



    public Element element = Element.PHYSICAL;
    private bool isArrowSkill;
    public void OnUseSkill(ICombatActor actor, ISkill skill, CombatState state)
    {
        isArrowSkill = skill.SkillGroup == SkillGroup.RANGED;
    }
    public DamageInfo OnBeforeDealDamage(ICombatActor actor, DamageInfo attackInfo)
    {
        attackInfo.element = element;
        attackInfo.amount = Mathf.CeilToInt(attackInfo.amount * 1.2f);
        return attackInfo;
    }
}

