using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    protected BattleManager bm;
    protected UnitStats parentUnit;

    private void Awake() {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public void AttemptAction() {
        bm.CurrentAction = this.GetComponent<Action>();
        
        SetUpMove();
        // bm.CurrentAction = this.GetComponent<Action>();
        bm.CurrentAction.ParentButtonImage = this.GetComponent<Image>();

        bm.AttemptAction();
    }

    protected virtual void SetUpMove() {
        print("should you have overridden this?");
    }

    public void SetParentUnit(UnitStats unit) {
        parentUnit = unit;
    }
}
