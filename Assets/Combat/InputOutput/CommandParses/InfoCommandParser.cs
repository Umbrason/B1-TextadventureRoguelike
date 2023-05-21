using System.Collections.Generic;


public class InfoCommandParser : ICommandParser
{
    public string HelpText => @"Provides information on a CombatActor or tile.
args: int x, y";

    private PositionTargetSelector targetSelector = new();

    ICommandParser.ParseResult ICommandParser.Parse(ref Queue<string> args, CombatLog log)
    {
        if (!targetSelector.IsDone)
        {
            var parseResult = targetSelector.ParseInput(ref args, log.CurrentReadOnlyCombatState);
            if (parseResult == ITargetSelector.TargetSelectionResult.INVALID) return ICommandParser.ParseResult.CANCEL;
        }
        if (!targetSelector.IsDone) return ICommandParser.ParseResult.INCOMPLETE;
        var isActorAtPosition = log.CurrentReadOnlyCombatState.ActorPositions.TryGetValue(targetSelector.Position, out var actorGuid);
        if (isActorAtPosition)
            PrintInfo(log.CurrentReadOnlyCombatState.CombatActors[actorGuid]);
        return ICommandParser.ParseResult.FINISHED;
    }

    private void PrintInfo(IReadOnlyCombatActor actor)
    {
        ConsoleOutput.Println(actor.Name);
        ConsoleOutput.Println($"HEALTH:       {actor.Health}");
        ConsoleOutput.Println($"ARMOR:        {actor.Armor}");
        ConsoleOutput.Println($"ACTIONPOINTS: {actor.ActionPoints}");
        ConsoleOutput.Println($"MOVEMENT:     {actor.MovementPoints}");        
        ConsoleOutput.Println($"SKILLS:");
        foreach (var skill in actor.Skills)
        {
            ConsoleOutput.Println($"{skill.GetType().Name}");
            ConsoleOutput.Println($"{skill.Description}");
            ConsoleOutput.Println($"  COSTS:    {skill.APCost}AP");
            ConsoleOutput.Println($"  COOLDOWN: {skill.Cooldown} TURNS");
        }
        ConsoleOutput.Println($"STATUS EFFECTS:");
        foreach (var status in actor.StatusEffects)
        {
            ConsoleOutput.Println($"{status.GetType().Name}");
            ConsoleOutput.Println($"  DURATION: {status.Duration} TURNS");
        }
    }
}