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
        action.ResourceCost = resourceCost;

        action.TargetType = "Self";

        action.ManaCost = COST;
        action.Damage = DAMAGE;
        action.Heal = 10;
    }
}
