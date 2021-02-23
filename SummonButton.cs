using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonButton : MoveButton
{
    public GameObject playerCharacter;

    protected const int COST = 8;
    protected const int DAMAGE = 0;


    protected override void SetUpMove() {
        // print('q');
        bm.CurrentAction.TargetType = "Targetless";
        // targetType = "Self";
        // targetType = "OneAlly";
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;

        // bm.CurrentAction.TargetedAction = true;

        // bm.CurrentAction.TargetedAction = false;

        bm.CurrentAction.Summon = playerCharacter; 
    }
}
