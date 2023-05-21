public static class Locations
{
    #region Combat
    public static CombatLocation GoblinEncounter => new GoblinEncounter();
    public static CombatLocation GoblinAmbush => new GoblinAmbush();
    public static CombatLocation GoblinLair => new GoblinLair();

    public static CombatLocation HellChunk => new HellChunk();
    public static CombatLocation HellIsland => new HellIsland();
    public static CombatLocation ArchdemonLair => new ArchdemonLair();

    public static CombatLocation Graveyard => new Graveyard();
    public static CombatLocation Chappel => new Chappel();
    public static CombatLocation Cathedral => new Cathedral();
    #endregion    

    #region Filler
    public static SkillShop AeroSkillShop => new SkillShop(SkillGroup.AEROMANCY);
    public static SkillShop CryoSkillShop => new SkillShop(SkillGroup.CRYOMANCY);
    public static SkillShop PyroSkillShop => new SkillShop(SkillGroup.PYROMANCY);
    public static SkillShop TerraSkillShop => new SkillShop(SkillGroup.TERRAMANCY);
    public static SkillShop MartialRangedSkillShop => new SkillShop(SkillGroup.RANGED);
    public static SkillShop MartialMeleeSkillShop => new SkillShop(SkillGroup.MELEE);
    public static SkillShop NecroSkillShop => new SkillShop(SkillGroup.NECROMANCY);
    public static SkillShop MycoSkillShop => new SkillShop(SkillGroup.MYCOMANCY);
    public static RestingPlace RestingPlace => new RestingPlace();
    public static PowerWell PowerWell => new PowerWell();
    #endregion

    public static IWorldLocation[] CombatLocations = new CombatLocation[] {
        GoblinEncounter, HellChunk, Graveyard
    };

    public static IWorldLocation[] EliteCombatLocations = new CombatLocation[] {
        GoblinAmbush, HellIsland, Chappel
    };

    public static IWorldLocation[] BossCombatLocations = new CombatLocation[] {
        GoblinLair, ArchdemonLair, Cathedral
    };

    public static IWorldLocation[] FillerLocations = new IWorldLocation[] {
        AeroSkillShop,
        CryoSkillShop,
        PyroSkillShop,
        TerraSkillShop,
        MartialRangedSkillShop,
        MartialMeleeSkillShop,
        NecroSkillShop,
        MycoSkillShop,
        RestingPlace,
        RestingPlace,
        RestingPlace,
        RestingPlace,
        RestingPlace,
        RestingPlace,
        RestingPlace,
        RestingPlace,
        PowerWell,
        PowerWell,
        PowerWell,
        PowerWell,
        PowerWell,
        PowerWell,
        PowerWell,
        PowerWell,
    };
}
