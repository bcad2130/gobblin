using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempt : MoveButton
{
    protected const int COST = 6;
    protected const int DAMAGE = 0;
    protected const int FAMISH = 10;

    protected override void SetUpMove() {
        // Debug.Log("test')");

        action.TargetType = "MeleeEnemy";

        action.GutsCost = COST;

        action.Famish = FAMISH;

        action.checkIfCookin = true;

        SetIsMove(true);
        //TODO you can only use if you have a meal, but it doesn't use up the meal
    }
}
