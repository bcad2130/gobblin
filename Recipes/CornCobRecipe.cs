using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornCobRecipe : Recipe
{
    public CornCobRecipe ()
    {
        SetName("Corn Cob");

        cookCount = 0;
        cookTime  = 1;

        stirCount = 0;
        stirGoal = 1;

        SetFlavorGoal(7);
        SetFlavorGoalBonus(3);

        result = new CornCobItem();

        reqIngredients = new Inventory();
        reqIngredients.AddItem("Corn");
    }
}
