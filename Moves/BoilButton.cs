using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilButton : MoveButton
{
    // public Item NewItem;

    protected const int COST = 0;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        action.TargetType = "Self";
        // manaCost = COST;

        action.ManaCost = COST;
        action.Damage = DAMAGE;

        // action.AddStatusEffect("BOIL", 1);

        // NewItem = new SoupItem();

        // action.GiveItem = NewItem;

        // action.DestroySelf = true;
        // action.TargetedAction = true;
    }
}
