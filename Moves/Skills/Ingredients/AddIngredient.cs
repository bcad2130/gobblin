using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredient : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "AllyPot";

        action.SetSkillType("AddIngredient");

        action.AddIngredientCost(ingredientCost);
    }
}
