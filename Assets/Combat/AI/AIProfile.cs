public struct AIProfile
{
    //weights
    public float selfAliveWeight;
    public float selfHealthWeight;
    public float alliesHealthWeight;
    public float alliesAliveCountWeight;
    public float alliesHealthTotalWeight;

    public float enemiesHealthTotalWeight;
    public float enemiesAliveCountWeight;

    public float distanceToNearestEnemyWeight;
    public float distanceToNearestAllyWeight;
}

public static class AIProfiles
{
    public static AIProfile DEFAULT => new()
    {
        selfAliveWeight = 1000,
        selfHealthWeight = 100,
        alliesHealthWeight = 10,
        alliesAliveCountWeight = 10,
        alliesHealthTotalWeight = 10,
        enemiesHealthTotalWeight = -10,
        enemiesAliveCountWeight = -999,
        distanceToNearestEnemyWeight = -1,
        distanceToNearestAllyWeight = -1,
    };
}
