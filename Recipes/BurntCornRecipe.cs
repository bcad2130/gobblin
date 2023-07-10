using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntCornRecipe : Recipe
{
    public BurntCornRecipe ()
    {
        // Debug.Log("BurntCornRecipe");
        SetName("Burnt Corn");

        cookCount = 0;
        cookTime  = 1;

        stirCount = 0;
        stirGoal = 0;

        SetFlavorGoal(7);
        SetFlavorGoalBonus(3);

        result = new BurntCornItem();

        reqIngredients = new Inventory();
        reqIngredients.AddItem("Corn");
    }
}
