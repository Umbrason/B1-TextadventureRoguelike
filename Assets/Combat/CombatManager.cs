
public class CombatManager : SingletonBehaviour<CombatManager>
{
    public CombatLog CombatLog {get; private set;}
    private CombatLoop loop;

    public void StartCombat(CombatState state)
    {
        this.CombatLog = new CombatLog(state);
        this.loop = new CombatLoop(CombatLog);
    }
}
