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
        ConsoleOutput.Disabled = true;
        skillToUse = null;
        skillParameters = new object[0];
        positionToMoveAt = AIActor.Position;

        var skillOptions = AIActor.Skills.Where(skill => skill.CanUse(currentState, AIActor)).ToArray();
        var highScore = EvaluateTurnOption(null, null, AIActor.Position); //baseline
        foreach (var position in Shapes.GridSquare(AIActor.Position, (AIActor.MovementPoints.Value + 1) / 2))
        {
            var pathfinder = new Pathfinder(currentState.IsTileWalkable);
            var path = pathfinder.FromTo(AIActor.Position, position);
            if (path.Length > AIActor.MovementPoints.Value || path.Length == 0) continue;
            var score = EvaluateTurnOption(null, null, position);
            if (score <= highScore) continue;
            highScore = score;
            skillToUse = null;
            skillParameters = null;
            positionToMoveAt = position;
        }
        foreach (var skill in skillOptions)
        {
            var positions = FindLogicalPositionsForSkill(skill);
            foreach (var position in positions)
            {
                var parameters = FindLogicalParameters(skill, position);
                foreach (var parameterGroup in parameters)
                {
                    var score = EvaluateTurnOption(skill, parameterGroup, position);
                    if (score <= highScore) continue;
                    highScore = score;
                    skillToUse = skill;
                    skillParameters = parameterGroup;
                    positionToMoveAt = position;
                }
            }
        }
        ConsoleOutput.Disabled = false;
        return highScore;
    }

    private Vector2Int[] FindLogicalPositionsForSkill(IReadOnlySkill skill)
    {
        var selfAlignment = AIActor.Alignment;
        var rangedPositionSelectors = skill.TargetSelectors.Where(selector => selector is RangedPositionSelector).Select(s => (RangedPositionSelector)s).ToArray();
        var minRange = rangedPositionSelectors.Count() > 0 ? rangedPositionSelectors.Min(selector => selector.Range) : 0;
        var targets = currentState.CombatActors.Values.Where(actor => skill.IsBuff == (actor.Alignment == selfAlignment));        
        var pathfinder = new Pathfinder(currentState.IsTileWalkable);
        var positions = targets.Select(target => Vector2Int.RoundToInt(Vector2.ClampMagnitude((target.Position - AIActor.Position), minRange)))
                                .Where(position => pathfinder.FromTo(AIActor.Position, position, AIActor.MovementPoints.Value).Length > 0)
                                .Append(AIActor.Position)
                                .Distinct()
                                .ToArray();
        return positions;
    }

    private ICollection<object[]> FindLogicalParameters(IReadOnlySkill skill, Vector2Int fromPosition)
    {
        var targets = currentState.CombatActors.Values.Where(actor => skill.IsBuff == (actor.Alignment == AIActor.Alignment)).ToArray();
        var targetSelectors = skill.TargetSelectors;
        var options = targetSelectors.Select(FindValidParams);
        var queue = new Queue<object[]>(options);
        return RecursiveParamSets(new object[0], queue);
    }

    private ICollection<object[]> RecursiveParamSets(object[] existingParams, Queue<object[]> remainingParameters)
    {
        if (remainingParameters.Count == 0) return new object[][] { existingParams };
        var parameterSets = new List<object[]>();
        var currentParameters = remainingParameters.Dequeue();
        foreach (var parameter in currentParameters)
            parameterSets.AddRange(RecursiveParamSets(existingParams.Append(parameter).ToArray(), new Queue<object[]>(remainingParameters)));
        return parameterSets;
    }

    private object[] FindValidParams(ITargetSelector selector)
    {
        if (selector is ActorTargetSelector)
            return currentState.ActorPositions.Keys.Where(pos => selector.IsValid(pos, currentState)).Select(pos => (object)pos).ToArray();
        if (selector is RangedPositionSelector)
            return Shapes.GridCircle(AIActor.Position, ((RangedPositionSelector)selector).Range).Where(pos => selector.IsValid(pos, currentState)).Select(pos => (object)pos).ToArray();
        if (selector is LineTargetSelector)
        {
            var positionsInRange = Shapes.GridCircle(AIActor.Position, ((RangedPositionSelector)selector).Range);
            var pairs = positionsInRange.Zip(Shapes.GridCircle(AIActor.Position, ((RangedPositionSelector)selector).Range), (a, b) => new Vector2Int[] { a, b });
            return pairs.Where(pair => selector.IsValid(pair, currentState)).Select(pos => (object)pos).ToArray();
        }
        if (selector is PositionTargetSelector)
            return currentState.Room.Tiles.Where(tile => selector.IsValid(tile.Item1, currentState)).Select(tile => (object)tile.Item1).ToArray();
        return new object[0];
    }



    private float EvaluateTurnOption(IReadOnlySkill skillToCast, object[] skillParameters, Vector2Int fromPosition)
    {
        var state = currentState.DeepCopy();
        var log = new CombatLog(state);
        if (fromPosition != AIActor.Position) log.MoveActor(AIActor, fromPosition);
        if (skillToCast != null) log.CastSkill(AIActor, skillToCast, skillParameters);
        return log.CurrentReadOnlyCombatState.EvaluateCurrentState(AIActor, profile);
    }
}