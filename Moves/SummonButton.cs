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
        action.TargetType = "Targetless";
        // targetType = "Self";
        // targetType = "OneAlly";
        // manaCost = COST;

        action.ManaCost = COST;
        action.Damage = DAMAGE;

        // action.TargetedAction = true;

        // action.TargetedAction = false;

        action.Summon = playerCharacter; 
    }
}
