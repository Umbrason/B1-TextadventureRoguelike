
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public static class RunManager
{
    private const int CombatSceneBuildIndex = 1;
    private const int LocationSceneBuildIndex = 2;
    private const int WorldmapSceneBuildIndex = 3;

    private static Scene CurrentAdditionalScene;

    private static RunInfo runInfo;
    public static IReadOnlyRunInfo ReadOnlyRunInfo => runInfo;
    public static event Action<int, int> OnLocationChanged;

    public static void AdvanceLocation(int index)
    {
        runInfo.AdvanceLocation(index);
        EnterLocation(runInfo.CurrentLocation);
        OnLocationChanged?.Invoke(runInfo.CurrentDepth, runInfo.CurrentIndex);
    }

    public static void BeginRun(PlayableCombatActor PartyLeader, int worldSeed)
    {
        runInfo = new RunInfo();
        runInfo.map = Worldmap.Generate(worldSeed);
        runInfo.party = PartyInfo.Empty;
        runInfo.party.PartyLeader = PartyLeader;
        ShowWorldMap();
    }

    private static void LoadSceneIfAbsent(int sceneBuildIndex)
    {
        var scene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
        if (scene.isLoaded) return;
        if (CurrentAdditionalScene.isLoaded) SceneManager.UnloadSceneAsync(CurrentAdditionalScene);
        SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        CurrentAdditionalScene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
    }

    public static void ChooseLocationOption(int optionIndex)
        => runInfo.CurrentLocation.OnPickOption(optionIndex);

    public static void ShowWorldMap()
        => LoadSceneIfAbsent(WorldmapSceneBuildIndex);


    public static void EnterLocation(IWorldLocation location)
        => LoadSceneIfAbsent(LocationSceneBuildIndex);

    public static void StartCombat(CombatEncounterInfo encounterInfo)
    {
        LoadSceneIfAbsent(CombatSceneBuildIndex);
        var partyActors = runInfo.party.CombatActors;
        for (int i = 0; i < partyActors.Length; i++)
            partyActors[i].Position = encounterInfo.allyStartPositions[i];
        var combatActors = new List<ICombatActor>();
        combatActors.AddRange(encounterInfo.enemies);
        combatActors.AddRange(partyActors);
        var initialCombatState = new CombatState(encounterInfo.room, combatActors.ToDictionary(actor => actor.Guid), encounterInfo.tileModifiers);
        CombatManager.StartCombat(initialCombatState);
    }
}