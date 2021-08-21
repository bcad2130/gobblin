﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeMeal : MoveButton
{
    // protected const int COST = 4;
    protected const int DAMAGE = 0;

    protected override void SetUpMove()
    {
        action.TargetType = "ServeMeal";

        // action.GutsCost = COST;

        action.SetResourceCost(GetResourceCost());
    }
}