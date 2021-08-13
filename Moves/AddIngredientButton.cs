﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddIngredientButton : MoveButton
{
    protected const int COST = 0;
    protected const int DAMAGE = 0;

    protected List<Item> resourceCost = new List<Item>();


    protected override void SetUpMove() {
        resourceCost.Add(new PotatoItem());
        action.ResourceCost = resourceCost;

        action.TargetType = "Ingredient";

        action.GutsCost = COST;
        action.Damage = DAMAGE;
    }
}
