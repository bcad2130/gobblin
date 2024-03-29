﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    // will this work by ref?
    public UnitStats cauldron;

    private string recipeName;

    protected int cookCount = 0;
    protected int cookTime;

    protected int stirCount = 0;
    protected int stirGoal;

    private int flavor = 0;
    private int flavorGoal;
    private int flavorGoalBonus;


    protected Inventory reqIngredients;

    protected Item result;

    public Recipe ()
    {
        recipeName = "Default";
    }

    public int GetCookCount()
    {
        return cookCount;
    }

    public int GetCookTime()
    {
        return cookTime;
    }

    public int GetCookTurnsLeft()
    {
        return cookTime - cookCount;
    }

    public void AddCookTurn()
    {
        cookCount++;
    }

    public int GetStirCount()
    {
        return stirCount;
    }

    public void AddStir()
    {
        stirCount++;
    }

    public int GetStirGoal()
    {
        return stirGoal;
    }

    public int GetReqStirs()
    {
        return stirGoal - stirCount;
    }

    public string GetName()
    {
        return recipeName;
    }

    public void SetName(string name)
    {
        recipeName = name;
    }

    public int GetFlavor()
    {
        return flavor;
    }

    public void AddFlavor(int flavorToAdd)
    {
        flavor += flavorToAdd;
        Debug.Log("flavor: " + flavor);
    }

    public void LoseFlavor(int flavorToLose)
    {
        flavor -= flavorToLose;
    }

    public int GetFlavorGoal()
    {
        return flavorGoal;
    }

    public void SetFlavorGoal(int goal)
    {
        flavorGoal = goal;
    }

    public int GetFlavorGoalBonus()
    {
        return flavorGoalBonus;
    }

    public void SetFlavorGoalBonus(int goalBonus)
    {
        flavorGoalBonus = goalBonus;
    }

    public Inventory GetRequiredIngredients()
    {
        return reqIngredients;
    }

    public Item GetResult()
    {
        return result;
    }
}
