﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skewer : MoveButton
{
    protected const int COST = 4;
    protected const int DAMAGE = 5;

    protected override void SetUpMove() {

        action.TargetType = "MeleeEnemyPierce";

        action.GutsCost = COST;
        action.StrengthDamage = bm.getCurrentUnit().GetNetStrength() + DAMAGE;
        SetIsMove(true);
    }
}
