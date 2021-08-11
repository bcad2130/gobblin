using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoSoupRecipe : Recipe
{
    public PotatoSoupRecipe ()
    {
        cookCount = 0;
        cookTime  = 2;

        stirCount = 0;
        stirGoal = 1;

        result = new SoupItem();

        ingredients = new Dictionary<string,int>();
        ingredients.Add("Potato", 2);
    }
}
