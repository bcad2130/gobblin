using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{

    public int cookCount;
    public int cookTime;

    public int stirCount;
    public int stirGoal;

    public Item result;

    public Dictionary<string,int> ingredients;

    // will this work by ref?
    public UnitStats cauldron;

    public Recipe ()
    {
        cookCount = 0;
        cookTime  = 1;

        stirCount = 0;
        stirGoal = 0;

        result = new SoupItem();

        ingredients = new Dictionary<string,int>();
        ingredients.Add("Potato", 1);
    }

    public void AddCookTurn()
    {
        cookCount++;
        Debug.Log(cookCount);
    }

    public int GetStirCount() {
        return stirCount;
    }

    public int GetStirGoal() {
        return stirGoal;
    }

    // public 
}
