using System.Collections.Generic;

public class CastCommandParser : ICommandParser
{
    public string HelpText => @"Use a skill.
args: string skillID";

    private readonly SkillTargetSelector skillTargetSelector = new();
    private Queue<ITargetSelector> skillTargetSelectors;
    private readonly List<ITargetSelector> finishedTargetSelectors = new();

    public ICommandParser.ParseResult Parse(ref Queue<string> args, CombatLog log)
    {
        if (!skillTargetSelector.IsDone && skillTargetSelector.ParseInput(ref args, log.CurrentReadOnlyCombatState) == ITargetSelector.TargetSelectionResult.INVALID) return ICommandParser.ParseResult.CANCEL;
        if (!skillTargetSelector.IsDone) return ICommandParser.ParseResult.INCOMPLETE;
        var skill = (IReadOnlySkill)skillTargetSelector.Value;
        if (skill.Cooldown.Value > 0)
        {
            ConsoleOutput.Println($"{skill.GetType().Name} is still on cooldown.");
            return ICommandParser.ParseResult.CANCEL;
        }
        if (!skill.CanUse(CombatManager.CombatLog.CurrentReadOnlyCombatState, CombatManager.CombatLog.CurrentReadOnlyCombatState.ActiveActor))
        {
            ConsoleOutput.Println($"Could not cast {skill.GetType().Name}");
            return ICommandParser.ParseResult.CANCEL;
        }

        if (skillTargetSelectors == null)
            skillTargetSelectors = new Queue<ITargetSelector>(skill.TargetSelectors);

        if (skillTargetSelectors.Count > 0) skillTargetSelectors.Peek().Reset();
        while (skillTargetSelectors.Count > 0)
        {
            var parseResult = skillTargetSelectors.Peek().ParseInput(ref args, log.CurrentReadOnlyCombatState);
            if (parseResult == ITargetSelector.TargetSelectionResult.INVALID) return ICommandParser.ParseResult.CANCEL;
            if (parseResult == ITargetSelector.TargetSelectionResult.INCOMPLETE) return ICommandParser.ParseResult.INCOMPLETE;
            finishedTargetSelectors.Add(skillTargetSelectors.Dequeue());
            if (skillTargetSelectors.Count > 0) skillTargetSelectors.Peek().Reset();
        }
        var targetActor = log.CurrentReadOnlyCombatState.ActiveActor;
        log.CastSkill(targetActor, skill, finishedTargetSelectors.ToArray());

        return ICommandParser.ParseResult.FINISHED;
    }

}
