using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronButton : MoveButton
{
    public GameObject Cauldron;

    protected const int COST = 3;
    protected const int DAMAGE = 0;

    protected List<Item> resourceCost = new List<Item>();


    protected override void SetUpMove() {
        resourceCost.Add(new PotatoItem());
        action.ResourceCost = resourceCost;

        // print('q');
        action.TargetType = "Targetless";
        // targetType = "Self";
        // targetType = "OneAlly";
        // manaCost = COST;

        action.ManaCost = COST;
        action.Damage = DAMAGE;

        // action.TargetedAction = true;

        // action.TargetedAction = false;

        action.Summon = Cauldron; 
    }
}
