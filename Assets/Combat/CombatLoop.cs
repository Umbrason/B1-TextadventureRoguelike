using System;
using System.Collections.Generic;
using System.Linq;

public class CombatQueue
{
    

    private ICombatActor activeActor;
    private void EndTurn()
    {
        activeActor = null;
        NextTurn();
    }

    

    private void NextTurn()
    {
       
    }
}
