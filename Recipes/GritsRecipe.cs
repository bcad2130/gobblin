using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GritsRecipe : Recipe
{
    public GritsRecipe ()
    {
        SetName("Grits");

        cookCount = 0;
        cookTime  = 3;

        stirCount = 0;
        stirGoal = 1;

        result = new GritsItem();

        reqIngredients = new Inventory();
        reqIngredients.AddItem("Corn");
        reqIngredients.AddItem("Water");
    }
}
