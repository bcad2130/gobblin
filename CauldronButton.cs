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
        bm.CurrentAction.ResourceCost = resourceCost;

        // print('q');
        bm.CurrentAction.TargetType = "Targetless";
        // targetType = "Self";
        // targetType = "OneAlly";
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;

        // bm.CurrentAction.TargetedAction = true;

        // bm.CurrentAction.TargetedAction = false;

        bm.CurrentAction.Summon = Cauldron; 
    }
}
