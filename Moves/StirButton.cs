using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirButton : MoveButton
{
    protected const int COST = 0;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {

        action.TargetType = "Stir";

        action.ManaCost = COST;
        action.Damage = DAMAGE;
    }
}
