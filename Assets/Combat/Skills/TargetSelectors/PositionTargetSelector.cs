using System;

public class PositionTargetSelector : ISkill.ITargetSelector
{
    const string USER_QUERRY = "at what position?";
    public void OnBeginSelection(Action<object> NotifySelectionDone)
    {
        throw new NotImplementedException();
    }
}