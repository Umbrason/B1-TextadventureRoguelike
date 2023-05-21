using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

public class AICombatActorController : MonoBehaviour
{
    void Awake()
    {
        CombatManager.OnCombatStateChanged += OnCombatStateChanged;
        OnCombatStateChanged(CombatManager.CombatLog.CurrentReadOnlyCombatState);
    }

    void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= OnCombatStateChanged;
    }

    private const float DelayBetweenMoves = .5f;
    private void OnCombatStateChanged(IReadOnlyCombatState state)
    {
        if (!(state.ActiveActor is AICombatActor)) return;
        TakeActionForActiveAIActor();
    }

    private struct AITurnAction
    {
        public IReadOnlySkill skillToCast;
        public object[] parametersToUse;
        public Vector2Int positionToMoveTo;
    }

    private readonly ConcurrentQueue<AITurnAction> AITurnActions = new();

    private void TakeActionForActiveAIActor() => TakeAIAction(CombatManager.CombatLog?.CurrentReadOnlyCombatState, (CombatManager.CombatLog?.CurrentReadOnlyCombatState?.ActiveActor as AICombatActor).Profile);
    private void TakeAIAction(IReadOnlyCombatState state, AIProfile profile)
    {
        if (!(state != null && state.ActiveActor is AICombatActor)) return;
        var predictor = new CombatPredictor(state, state.ActiveActorGuid.Value, profile);

        var thread = new Thread(() =>
        {
            var turnAction = new AITurnAction();
            predictor.PredictTurn(out turnAction.skillToCast, out turnAction.parametersToUse, out turnAction.positionToMoveTo);
            AITurnActions.Enqueue(turnAction);
        });
        thread.Start();
    }


    private float lastMove = -1;
    void Update()
    {
        if (Time.time < lastMove + DelayBetweenMoves || AITurnActions.Count == 0) return;
        if (!AITurnActions.TryDequeue(out var turnAction)) return;
        lastMove = Time.time;
        var state = CombatManager.CombatLog.CurrentReadOnlyCombatState;
        if (turnAction.skillToCast == null && turnAction.positionToMoveTo == state.ActiveActor.Position)
        {
            CombatManager.CombatLog.EndTurn();
            return;
        }
        if (turnAction.positionToMoveTo != state.ActiveActor.Position)
        {
            CombatManager.CombatLog.MoveActor(state.ActiveActor, turnAction.positionToMoveTo);
        }
        if (turnAction.skillToCast != null)
        {
            CombatManager.CombatLog.CastSkill(state.ActiveActor, turnAction.skillToCast, turnAction.parametersToUse);
        }
    }
}
