using System.Collections.Generic;
using UnityEngine;

public class CombatTest : MonoBehaviour
{
    [SerializeField] private TextAsset roomLayout;
    void Start()
    {
        var corpse1 = new RaisedCorpse() { Position = new(5, 8) };
        var corpse2 = new RaisedCorpse() { Position = new(7, 8) };
        var tileModifiers = new Dictionary<Vector2Int, ITileModifier>();
        tileModifiers.Add(new(3, 4), new Ice());
        tileModifiers.Add(new(3, 5), new Ice());
        tileModifiers.Add(new(4, 4), new Ice());
        tileModifiers.Add(new(4, 5), new Ice());
        var encounterInfo = new CombatEncounterInfo()
        {
            allyStartPositions = new Vector2Int[] { new(1, 1) },
            enemies = new ICombatActor[] {
                corpse1,
                corpse2
            },
            room = new RoomInfo(roomLayout.text),
            tileModifiers = tileModifiers
        };
        RunManager.BeginRun(new(), Random.Range(int.MinValue, int.MaxValue));
        //RunManager.StartCombat(encounterInfo);
    }

    /* void PredictCombatTurn(IReadOnlyCombatState state)
    {
        Debug.Log("Predicting");
        if (state.ActiveActorGuid == null) return;
        var predictor = new CombatPredictor(state, state.ActiveActorGuid.Value, AIProfiles.DEFAULT);
        predictor.PredictTurn(out var skill, out var parameters, out var targetPosition);
        Debug.Log(targetPosition);
        Debug.Log(skill?.GetType().Name);
    } */
}
