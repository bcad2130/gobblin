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
        bm.AddTargetToAction(ref parentUnit);

        bm.TakeAction();
    }

    public void SetParentUnit(ref UnitStats unit) {
        parentUnit = unit;
    }
}
