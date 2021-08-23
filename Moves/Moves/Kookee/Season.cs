﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Season : MoveButton
{
    protected const int COST = 2;

    protected override void SetUpMove() {
        action.TargetType = "Season";

        action.GutsCost = COST;
    }
}
