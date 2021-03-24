using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    private BattleManager bm;
    private UnitStats parentUnit;

    private void Awake() {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public void SelectTarget() {
        // print("SelectTarget");
        bm.AddTargetToAction(ref parentUnit);

        // bm.CurrentAction.AddTarget(ref parentUnit);
        // bm.CurrentAction.ParentUnit.SpendMana(bm.CurrentAction.ManaCost);

        bm.TakeAction();
    }

    public void SetParentUnit(ref UnitStats unit) {
        parentUnit = unit;
    }
}
