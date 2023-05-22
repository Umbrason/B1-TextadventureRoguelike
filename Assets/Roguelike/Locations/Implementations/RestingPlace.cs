public class RestingPlace : IWorldLocation
{
    public string Name { get; private set; } = "RestingPlace";
    public string storyText => "You feel at ease in this location. You decide to stay for a short while bevor moving on.\nHow would you like to spend your time?";
    public string[] optionTexts => new string[] { "Rest", "Train", "Eat" };
    public void OnPickOption(int option, RunInfo run)
    {
        switch (option)
        {
            case 0:
                foreach (var actor in run.party.CombatActors)
                    actor.Health.Value = actor.Health.Max;
                ConsoleOutput.Println("You recovered from all your injuries");
                RunManager.ShowWorldMap();
                break;
            case 1:
                run.party.PartyLeader.ActionPoints.Max += 1;
                run.party.PartyLeader.MovementPoints.Max += 2;
                run.party.PartyLeader.Initiative += 2;
                ConsoleOutput.Println("Your feel faster");
                RunManager.ShowWorldMap();
                break;
            case 2:
                run.party.PartyLeader.Health.Max += 5;
                run.party.PartyLeader.Health.Value += 5;
                ConsoleOutput.Println("Your vitality increased");
                RunManager.ShowWorldMap();
                break;
        }
    }
}