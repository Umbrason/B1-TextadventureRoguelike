using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatPredictor
{
    private IReadOnlyCombatState currentState;
    private IReadOnlyCombatActor AIActor;
    private AIProfile profile;
    public CombatPredictor(IReadOnlyCombatState state, Guid targetActorGuid, AIProfile profile)
    {
        this.currentState = state;
        this.AIActor = state.CombatActors[targetActorGuid];
        this.profile = profile;
    }

    public float PredictTurn(out IReadOnlySkill skillToUse, out object[] skillParameters, out Vector2Int positionToMoveAt)
    {
        skillToUse = null;
        skillParameters = new object[0];
        positionToMoveAt = AIActor.Position;

        var skillOptions = AIActor.Skills.Where(skill => skill.CanUse(currentState, AIActor)).ToArray();
        var highScore = float.MinValue;
        foreach (var skill in skillOptions)
        {
            var positions = FindLogicalPositionsForSkill(skill);
            foreach (var position in positions)
            {
                var parameters = FindLogicalParameters(skill, position);
                foreach (var parameter in parameters)
                {
                    var score = EvaluateTurnOption(skill, parameter, position);
                    if (score <= highScore) continue;
                    skillToUse = skill;
                    skillParameters = parameter;
                    positionToMoveAt = position;
                }
            }
        }
        if (highScore <= 0)
        {
            highScore = 0;
            skillToUse = null;
            skillParameters = new object[0];
            positionToMoveAt = AIActor.Position;
        }
        return highScore;
    }

    private Vector2Int[] FindLogicalPositionsForSkill(IReadOnlySkill skill)
    {
        var selfAlignment = AIActor.Alignment;
        var rangedPositionSelectors = skill.TargetSelectors.Where(selector => selector is RangedPositionSelector).Select(s => (RangedPositionSelector)s).ToArray();
        var minRange = rangedPositionSelectors.Min(selector => selector.Range);
        var targets = currentState.CombatActors.Values.Where(actor => skill.IsBuff == (actor.Alignment == selfAlignment));
        var pathfinder = new Pathfinder(currentState.IsTileWalkable);
        var positions = targets.Select(target => Vector2Int.RoundToInt(Vector2.ClampMagnitude((target.Position - AIActor.Position), minRange)))
                                .Where(position => pathfinder.FromTo(AIActor.Position, position, AIActor.MovementPoints.Value).Length > 0)
                                .Distinct()
                                .ToArray();
        return positions;
    }

    private object[][] FindLogicalParameters(IReadOnlySkill skill, Vector2Int fromPosition)
    {
        var targets = currentState.CombatActors.Values.Where(actor => skill.IsBuff == (actor.Alignment == AIActor.Alignment)).ToArray();
        var targetSelectors = skill.TargetSelectors;
        var options = targetSelectors.Select(FindValidParams);
        var queue = new Queue<object[]>(options);
        return RecursiveParamSets(queue);
    }

    private object[][] RecursiveParamSets(Queue<object[]> remainingParams)
    {
        var parameters = remainingParams.Dequeue();
        var parameterSets = new List<object[]>();
        foreach (var parameter in parameters)
            parameterSets.AddRange(RecursiveParamSets(new Queue<object[]>(remainingParams)).Select(set => set.Prepend(parameter).ToArray()).ToArray());
        return parameterSets.ToArray();
    }

    private object[] FindValidParams(ITargetSelector selector)
    {
        if (selector.ValueType == typeof(Vector2Int))
            return currentState.Room.Tiles.Where(tile => selector.IsValid(tile.Item1, currentState)).Select(tile => (object)tile.Item1).ToArray();
        return new object[0];
    }



    private float EvaluateTurnOption(IReadOnlySkill skillToCast, object[] skillParameters, Vector2Int fromPosition)
    {
        var state = currentState.DeepCopy();
        var log = new CombatLog(state);
        log.MoveActor(AIActor, fromPosition);
        log.CastSkill(AIActor, skillToCast, skillParameters);
        return log.CurrentReadOnlyCombatState.EvaluateCurrentState(AIActor, profile);
    }
}