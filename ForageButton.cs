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
        bm.CurrentAction.TargetType = "Targetless";
        // targetType = "Self";
        // targetType = "OneAlly";
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;

        // bm.CurrentAction.TargetedAction = true;

        // bm.CurrentAction.TargetedAction = false;

        // bm.CurrentAction.Summon = playerCharacter;
        NewItem = new PotatoItem();

        bm.CurrentAction.GiveItem = NewItem;
    }
}
