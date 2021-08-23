using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCornRecipe : Recipe
{
    public PopCornRecipe ()
    {
        SetName("Popcorn");

        cookCount = 0;
        cookTime  = 2;

        stirCount = 0;
        stirGoal = 1;

        SetFlavorGoal(10);
        SetFlavorGoalBonus(5);

        result = new PopCornItem();

        reqIngredients = new Inventory();
        reqIngredients.AddItem("Corn");
        reqIngredients.AddItem("Grease");
    }
}
