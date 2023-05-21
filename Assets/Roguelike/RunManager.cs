
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RunManager
{
    private const int CoreSceneBuildIndex = 1;
    private const int CombatSceneBuildIndex = 2;
    private const int LocationSceneBuildIndex = 3;
    private const int WorldmapSceneBuildIndex = 4;
    private const int GameOverSceneBuildIndex = 5;

    private static Scene CurrentAdditionalScene;

    private static RunInfo runInfo;
    public static IReadOnlyRunInfo ReadOnlyRunInfo => runInfo;
    public static event Action<int, int> OnLocationChanged;
    public static event Action OnLocationUpdate;

    public static void AdvanceLocation(int index)
    {
        runInfo.AdvanceLocation(index);
        EnterLocation(runInfo.CurrentLocation);
        OnLocationChanged?.Invoke(runInfo.CurrentDepth, runInfo.CurrentIndex);
    }

    public static void BeginRun(PlayableCombatActor PartyLeader, int worldSeed)
    {
        SceneManager.LoadScene(CoreSceneBuildIndex);
        runInfo = new RunInfo();
        runInfo.map = Worldmap.Generate(worldSeed);
        runInfo.party = PartyInfo.Empty;
        runInfo.party.PartyLeader = PartyLeader;
        ShowWorldMap();
    }

    public static void EndRun(bool victory)
    {
        runInfo.Won = victory;
        ChangeCoScene(GameOverSceneBuildIndex);
    }

    public static void EndCombat(bool victorious)
    {
        if (!victorious) { EndRun(false); return; }

        if (runInfo.Path.Count == runInfo.map.MapLocations.Length) //no more locations to go
        {
            EndRun(true);
            return;
        }
        //rewards?
        runInfo.Gold += UnityEngine.Random.Range(50, 100);
        ShowWorldMap();
    }

    private static void ChangeCoScene(int sceneBuildIndex)
    {
        var scene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
        if (scene.isLoaded) return;
        if (CurrentAdditionalScene.isLoaded) SceneManager.UnloadSceneAsync(CurrentAdditionalScene);
        if (sceneBuildIndex < 0 || sceneBuildIndex >= SceneManager.sceneCountInBuildSettings) return;
        SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        Camera.main.transform.position = new(0, 0, Camera.main.transform.position.z);
        CurrentAdditionalScene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
    }

    public static void ChooseLocationOption(int optionIndex)
    {
        runInfo.CurrentLocation.OnPickOption(optionIndex, runInfo);
        OnLocationUpdate?.Invoke();
    }

    public static void ShowWorldMap()
        => ChangeCoScene(WorldmapSceneBuildIndex);


    public static void EnterLocation(IWorldLocation location)
        => ChangeCoScene(LocationSceneBuildIndex);

    public static void StartCombat(CombatEncounterInfo encounterInfo)
    {
        ChangeCoScene(CombatSceneBuildIndex);
        var partyActors = runInfo.party.CombatActors;
        for (int i = 0; i < partyActors.Length; i++)
            partyActors[i].Position = encounterInfo.allyStartPositions[i];
        var combatActors = new List<ICombatActor>();
        combatActors.AddRange(encounterInfo.enemies);
        combatActors.AddRange(partyActors);
        var initialCombatState = new CombatState(encounterInfo.room, combatActors.ToDictionary(actor => actor.Guid));
        CombatManager.StartCombat(initialCombatState);
    }

    internal static void Reset()
    {
        runInfo = null;
    }
}