using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCornRecipe : Recipe
{
    public PopCornRecipe ()
    {
        SetName("Popcorn");

        cookCount = 0;
        cookTime  = 3;

        stirCount = 0;
        stirGoal = 1;

        result = new PopCornItem();

        reqIngredients = new Inventory();
        reqIngredients.AddItem("Corn");
        reqIngredients.AddItem("Grease");
    }
}
