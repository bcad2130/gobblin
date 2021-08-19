using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    protected BattleManager bm;
    protected UnitStats parentUnit;
    protected Action action;
    protected Action altAction;
    protected Recipe recipe;
    protected int gutsCost;
    protected List<Item> resourceCost = null;
    protected string ingredientCost = null;

    // public void Awake()
    // {
    //     Debug.Log('C');
    //     SetUpButtonAction();
    // }

    public void AttemptAction()
    {
        // SetUpButtonAction();

        // TODO add a checkAfford here, then we dont need to check in the bm

        // or move everything to bm
        SetUpButtonAction();

        bm.TryAction(action);

        // if (bm.CheckAfford(action)) {
        //     bm.RouteAction(action);
        // } else {
        //     Debug.Log("Cannot do Move");
        // }
    }

//  maybe this is isn't overridden, instead we just set up with any of the vars that the variants override
    protected virtual void SetUpMove()
    {
        print("should you have overridden this?");
    }

    // public bool CheckAfford() {
    //     if (bm.CanAffordGutsCost(action) && bm.CanAffordResourceCost(action)) {
    //         return true;
    //     }

    //     return false;
    // }

    public void SetParentUnit(UnitStats unit)
    {
        parentUnit = unit;
    }

    private void NewAction()
    {
        action = new Action();
    }

    private void NewAltAction()
    {
        altAction = new Action();
    }

    private void FindBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public Image GetImage()
    {
        return this.GetComponent<Image>();
    }

    public void SetUpButtonAction()
    {
        NewAction();
        FindBattleManager();
        SetUpMove();
    }

    public Action GetAction()
    {
        return action;
    }

    public void SetRecipe(Recipe recipeToSet)
    {
        recipe = new Recipe();
        recipe = recipeToSet;
    }

    public Recipe GetRecipe()
    {
        return recipe;
    }

    public void SetIngredient(string ingredient)
    {
        ingredientCost = ingredient;
    }

    public string GetIngredient()
    {
        return ingredientCost;
    }

    public void SetResourceCost(List<Item> itemList)
    {
        if (resourceCost == null) {
            resourceCost = new List<Item>();
        }
        resourceCost = itemList;
    }

    public List<Item> GetResourceCost()
    {
        return resourceCost;
    }

    public void AddResourceCost(Item item)
    {
        if (resourceCost == null) {
            resourceCost = new List<Item>();
        }
        resourceCost.Add(item);
    }

    public string GetName()
    {
        return gameObject.name;
    }

    public int GetGutsCost()
    {
        return gutsCost;
    }

    public void SetGutsCost(int actionGutsCost)
    {
        gutsCost = actionGutsCost;
    }
}
