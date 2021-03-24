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
        // print('q');
        action.TargetType = "Targetless";
        // targetType = "Self";
        // targetType = "OneAlly";
        // manaCost = COST;

        action.ManaCost = COST;
        action.Damage = DAMAGE;

        // action.TargetedAction = true;

        // action.TargetedAction = false;

        // action.Summon = playerCharacter;
        NewItem = new PotatoItem();

        action.GiveItem = NewItem;
    }
}
