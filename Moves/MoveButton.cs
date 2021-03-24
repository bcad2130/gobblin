using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    protected BattleManager bm;
    protected UnitStats parentUnit;
    protected Action action;

    // private void Awake() {
    //     bm = GameObject.FindObjectOfType<BattleManager>();
    // }

    public void AttemptAction() {
        NewAction();
        // action.SetParentButton();

        FindBattleManager();

        // action = this.GetComponent<Action>();
        
        SetUpMove();
        // action = this.GetComponent<Action>();
        // action.ParentButtonImage = this.GetComponent<Image>();

        bm.AttemptAction(action);
    }

    protected virtual void SetUpMove() {
        print("should you have overridden this?");
    }

    public bool CheckAfford() {
        NewAction();
        FindBattleManager();
        SetUpMove();

        if (bm.CurrentUnit.CanAffordManaCost(action.ManaCost) && bm.CurrentUnit.CanAffordResourceCost(action.ResourceCost)) {
            return true;
        }

        return false;


    }

    public void SetParentUnit(UnitStats unit) {
        parentUnit = unit;
    }

    private void NewAction() {
        action = new Action();
    }

    private void FindBattleManager() {
        // print('g');
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public Image GetImage() {
        return this.GetComponent<Image>();
    }

    public Action GetAction() {
        NewAction();
        FindBattleManager();
        SetUpMove();
        return action;
    }
}
