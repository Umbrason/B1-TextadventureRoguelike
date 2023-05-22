public struct AIProfile
{
    //weights
    public float selfAliveWeight;
    public float selfHealthWeight;
    public float selfArmorWeight;

    public float alliesHealthWeight;
    public float alliesArmorWeight;
    public float alliesAliveCountWeight;

    public float enemiesHealthWeight;
    public float enemiesArmorWeight;
    public float enemiesAliveCountWeight;

    public float distanceToNearestEnemyWeight;
    public float distanceToNearestAllyWeight;

    public float preferredDistanceToEnemy;
}

public static class AIProfiles
{
    public static AIProfile DEFAULT => new()
    {
        selfAliveWeight = 1000,
        selfHealthWeight = 100,
        alliesHealthWeight = 10,
        alliesArmorWeight = 5,
        alliesAliveCountWeight = 10,
        enemiesHealthWeight = -10,
        enemiesArmorWeight = -5,
        enemiesAliveCountWeight = -999,
        distanceToNearestEnemyWeight = -1,
        distanceToNearestAllyWeight = 1,
        preferredDistanceToEnemy = 0,
    };

    public static AIProfile RANGED => new()
    {
        selfAliveWeight = 1000,
        selfHealthWeight = 100,
        alliesHealthWeight = 10,
        alliesArmorWeight = 5,
        enemiesHealthWeight = -10,
        enemiesArmorWeight = -5,
        alliesAliveCountWeight = 10,
        enemiesAliveCountWeight = -99,
        distanceToNearestEnemyWeight = -1,
        distanceToNearestAllyWeight = 2,
        preferredDistanceToEnemy = 10,
    };

    public static AIProfile MELEE => new()
    {
        selfAliveWeight = 1000,
        selfHealthWeight = 100,
        alliesHealthWeight = 10,
        alliesArmorWeight = 5,
        enemiesHealthWeight = -10,
        enemiesArmorWeight = -5,
        alliesAliveCountWeight = 10,
        enemiesAliveCountWeight = -99,
        distanceToNearestEnemyWeight = -20,
        distanceToNearestAllyWeight = 1,
        preferredDistanceToEnemy = 0,
    };
}

