using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    private string recipeName;

    public int cookCount;
    public int cookTime;

    public int stirCount;
    public int stirGoal;

    public Item result;

    // public Dictionary<string,int> ingredients;
    public Inventory reqIngredients;

    // will this work by ref?
    public UnitStats cauldron;

    public Recipe ()
    {
        // Debug.Log("Error : Recipe : Don't use the default recipe constructor");

        recipeName = "Default";

        cookCount = 0;
        cookTime  = 1;

        stirCount = 0;
        stirGoal = 0;

        // result = new SoupItem();

        // ingredients = new Dictionary<string,int>();
        // ingredients.Add("Potato", 1);

        result = new CornCobItem();

        reqIngredients = new Inventory();
        reqIngredients.AddItem("Corn");
    }

    public void AddCookTurn()
    {
        cookCount++;
        // Debug.Log(cookCount);
    }

    public int GetStirCount()
    {
        return stirCount;
    }

    public int GetStirGoal()
    {
        return stirGoal;
    }

    public string GetName()
    {
        return recipeName;
    }

    public void SetName(string name)
    {
        recipeName = name;
    }
}
