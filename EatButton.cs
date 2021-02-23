using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatButton : MoveButton
{
    protected const int COST = 0;
    protected const int DAMAGE = 0;

    protected List<Item> resourceCost = new List<Item>();

    protected override void SetUpMove() {
        resourceCost.Add(new SoupItem());
        bm.CurrentAction.ResourceCost = resourceCost;

        bm.CurrentAction.TargetType = "Self";

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;
        bm.CurrentAction.Heal = 10;
    }
}
