using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    // public Action action;
    private BattleManager bm;
    private UnitStats parentUnit;
    private List<UnitStats> targetedUnits;

    private void Start() {
        bm = GameObject.FindObjectOfType<BattleManager>();
        targetedUnits = new List<UnitStats>();
    }

    public void TakeAction() {
        // UnitStats target = null;
        // GameObject[] targetableObjects = null;


        // INVENT PROPER TARGETING SYSTEM
        if (parentUnit.tag == "Enemy") {

            // targetableObjects = GameObject.FindGameObjectsWithTag("Player");

            FindTargetsByTag("Player");
            // foreach (GameObject targetableObject in targetableObjects) {
            //     UnitStats unit = targetableObject.GetComponent<UnitStats>();
            //     if (!unit.isDead) {
            //         targetedUnits.Add(unit);
            //     }
            // }

            // print("now its the enemy turn");
            // target = GameObject.FindWithTag("Player").GetComponent<UnitStats>();
            // targetedUnits.Add(target);
        } else {
            FindTargetsByTag("Enemy");

            // target = GameObject.FindWithTag("Enemy").GetComponent<UnitStats>();
            // targetableObjects = GameObject.FindGameObjectsWithTag("Enemy");

            // foreach (GameObject targetableObject in targetableObjects) {
            //     targetedUnits.Add(targetableObject.GetComponent<UnitStats>());
            // }
        }

        // print(targetableUnits);
        bm.SelectTargets(targetedUnits, parentUnit.attack);

        

        // EndTurn();
    }

    private void FindTargetsByTag(string tag) {
        GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetableObject in targetableObjects) {
            UnitStats unit = targetableObject.GetComponent<UnitStats>();
            if (!unit.isDead) {
                targetedUnits.Add(unit);
            }
        }
    }

    private void EndTurn() {
        bm.RemoveAllButtons();
        
        bm.EndTurn();
    }

    public void SetParentUnit(UnitStats unit) {
        parentUnit = unit;
    }
}
