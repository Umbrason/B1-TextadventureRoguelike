
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatTest : MonoBehaviour
{
    [SerializeField] private TextAsset roomLayout;
    void Start()
    {
        var combatants = new Dictionary<Guid, ICombatActor>();
        var actor = new PlayableCombatActor() { Position = Vector2Int.one + Vector2Int.up };
        combatants.Add(actor.Guid, actor);
        var combatState = new CombatState(new Room(roomLayout.text), combatants, new Dictionary<Vector2Int, ITileModifier>());
        CombatManager.Instance.StartCombat(combatState);
    }
}
