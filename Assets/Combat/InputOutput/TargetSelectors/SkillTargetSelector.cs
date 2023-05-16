using System;
using System.Collections.Generic;
using System.Linq;

public class SkillTargetSelector : ITargetSelector
{
    public object Value => skill;
    public bool IsDone => skill != null;
    public Type ValueType => typeof(IReadOnlySkill);
    private IReadOnlySkill skill;

    public ITargetSelector.TargetSelectionResult ParseInput(ref Queue<string> args, IReadOnlyCombatState state)
    {
        var targetActor = state.ActiveActor;
        if (args.Count == 0)
        {
            var availableSkillOptions = targetActor.Skills.Where(skill => skill.CanUse(state, targetActor)).Select(skill => skill.GetType().Name).OrderBy(s => s).ToArray();
            if (availableSkillOptions.Length == 0)
            {
                ConsoleOutput.Println("No valid skills!");
                return ITargetSelector.TargetSelectionResult.INVALID;
            }
            ConsoleOutput.Println("Enter skill name:");
            foreach (var id in availableSkillOptions)
                ConsoleOutput.Println($" {id}");
            return ITargetSelector.TargetSelectionResult.INCOMPLETE;
        }
        var skillID = args.Dequeue();
        var valid = targetActor.Skills.Any(skill => string.Compare(skill.GetType().Name, skillID, true) == 0);
        if (!valid) return ITargetSelector.TargetSelectionResult.INVALID;
        skill = targetActor.Skills.First(skill => string.Compare(skill.GetType().Name, skillID, true) == 0);
        return ITargetSelector.TargetSelectionResult.FINISHED;
    }

    public void Reset() => skill = null;

    public bool IsValid(object value, IReadOnlyCombatState state)
    {        
        return state.ActiveActor?.Skills.Contains((IReadOnlySkill)value) ?? false;
    }
}