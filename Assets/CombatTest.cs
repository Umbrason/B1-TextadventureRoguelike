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
        var corpse1 = new RaisedCorpse() { Position = new(5, 8) };
        var corpse2 = new RaisedCorpse() { Position = new(7, 8) };
        combatants.Add(actor.Guid, actor);
        combatants.Add(corpse1.Guid, corpse1);
        combatants.Add(corpse2.Guid, corpse2);
        var TileModifiers = new Dictionary<Vector2Int, ITileModifier>();
        TileModifiers.Add(new(3, 4), new Ice());
        TileModifiers.Add(new(3, 5), new Ice());
        TileModifiers.Add(new(4, 4), new Ice());
        TileModifiers.Add(new(4, 5), new Ice());
        var combatState = new CombatState(new RoomInfo(roomLayout.text), combatants, TileModifiers);        
        CombatManager.Instance.StartCombat(combatState);
        CombatManager.Instance.OnCombatStateChanged += PredictCombatTurn;
    }

    void PredictCombatTurn(IReadOnlyCombatState state)
    {
        Debug.Log("Predicting");
        if (state.ActiveActorGuid == null) return;
        var predictor = new CombatPredictor(state, state.ActiveActorGuid.Value, AIProfiles.DEFAULT);
        predictor.PredictTurn(out var skill, out var parameters, out var targetPosition);
        Debug.Log(targetPosition);
        Debug.Log(skill?.GetType().Name);
    }
}
