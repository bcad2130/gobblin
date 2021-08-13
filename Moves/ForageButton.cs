using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [System.Serializable]
public class ForageButton : MoveButton
{
    // [SerializeField]
    public Item NewItem;
    // public GameObject NewItem;

    protected const int COST = 0;
    protected const int DAMAGE = 0;


    protected override void SetUpMove() {
        action.TargetType = "Forage";

        action.GutsCost = COST;
        action.Damage = DAMAGE;

        // List<Item> localIngredients = new List<Item>();

        // localIngredients.Add
        // NewItem = new PotatoItem();

        // action.GiveItem = NewItem;
    }
}
