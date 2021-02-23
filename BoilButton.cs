using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilButton : MoveButton
{
    public Item NewItem;

    protected const int COST = 0;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        bm.CurrentAction.TargetType = "Self";
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;


        NewItem = new SoupItem();

        bm.CurrentAction.GiveItem = NewItem;

        bm.CurrentAction.DestroySelf = true;
        // bm.CurrentAction.TargetedAction = true;
    }
}
