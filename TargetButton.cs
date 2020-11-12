using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    private BattleManager bm;
    private UnitStats parentUnit;
    private int damage;
    private string statusEffect;
    private List<UnitStats> targets;

    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
        targets = new List<UnitStats>();
    }

    public void SelectTarget() {
        targets.Add(parentUnit);

        bm.TakeAction(targets, damage, statusEffect);
    }

    public void EndTurn() {
        bm.EndTurn();
    }

    public void SetParentUnit(UnitStats unit) {
        parentUnit = unit;
    }

    public void SetMove(int moveDamage, string moveStatusEffect = null) {
        damage = moveDamage;
        statusEffect = moveStatusEffect;
    }
}
