﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRecipe : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Pick";
        action.SetSkillType("PickRecipe");
    }
}
