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
        action.TargetType = "Targetless";

        action.GutsCost = COST;
        action.Damage = DAMAGE;

        action.Summon = playerCharacter; 
    }
}
