
using UnityEngine;

public static class Locations
{
    #region Combat
    public static CombatLocation GoblinAmbush => new CombatLocation(
        LoadLocationData("Goblin Ambush"),
        new(new RoomInfo())
    );
    #endregion    

    private static CombatEncounterInfo LoadCombatEncounter(string name)
    {
        var data = Resources.Load<CombatEncounterInfoData>($"CombatEncounters/{name}");
        
    }

    private static LocationData LoadLocationData(string name)
    {
        return Resources.Load<LocationData>($"LocationData/{name}");
    }
    
}
