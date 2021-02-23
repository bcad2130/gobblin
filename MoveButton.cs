using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    protected BattleManager bm;
    protected UnitStats parentUnit;
    // protected string targetType;
    // protected int manaCost = 0;

    private void Awake() {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public void TakeAction() {
        SetUpMove();

        if (parentUnit.CanAffordManaCost(bm.CurrentAction.ManaCost) && parentUnit.CanAffordResourceCost(bm.CurrentAction.ResourceCost)) {
            // bm.CurrentAction.Damage = parentUnit.attack;

            SetTargets();

            switch (bm.CurrentAction.TargetType)
            {
                case "OneAlly": 
                case "OneEnemy":
                    bm.DisplayTargets();
                    break;
                case "Self":
                case "AllEnemies":
                case "Targetless":
                    bm.TakeAction();
                    break;
                default:
                    print("Invalid target");
                    break;
            }

            // if (targetType != "Targetless") {
            //     bm.DisplayTargets();
            // } else {
            //     bm.TakeAction();
            // }

        } else {
            this.GetComponent<Image>().color = Color.red;

            bm.CurrentAction.Clear();

            print("cannot afford");
        }
    }

    protected virtual void SetUpMove() {
        print("should you have overridden this?");

        // bm.CurrentAction.Damage = parentUnit.attack;
    }

    private void SetTargets() {
            switch (bm.CurrentAction.TargetType) 
            {
                case "Self":
                    // print('a');
                    SetTargetToSelf();
                    break;
                case "OneAlly": 
                    FindTargetsByTag(parentUnit.tag);
                    break;
                case "OneEnemy":
                    if (parentUnit.tag == "Enemy") {
                        FindTargetsByTag("Player");
                    } else {
                        FindTargetsByTag("Enemy");
                    }
                    break;
                case "AllEnemies":
                    if (parentUnit.tag == "Enemy") {
                        SetTargetsToAllTagged("Player");
                    } else {
                        SetTargetsToAllTagged("Enemy");
                    }
                    break;
                case "Targetless":
                    break;
                default:
                    print("Invalid target");
                    break;
            }

        // // Can we check the parent units tag and use that?
        // if (targetType == "OneAlly") {
        //     FindTargetsByTag(parentUnit.tag);
        //     // if (parentUnit.tag == "Player") {
        //     //     FindTargetsByTag("Player");

        //     // } else {
        //     //     FindTargetsByTag("Enemy");
        //     // }
        // } else if (targetType == "OneEnemy") {
        //     if (parentUnit.tag == "Enemy") {
        //         FindTargetsByTag("Player");
        //     } else {
        //         FindTargetsByTag("Enemy");
        //     }
        // }
    }

    private void SetTargetToSelf() {
        // print('a');
        bm.CurrentAction.AddTarget(parentUnit);
    }

    private void FindTargetsByTag(string tag) {
        // TODO use bm to grab the enemy list or ally list as appropriate
        // maybe actions should have a "targetable group" property, like enemies, allies, dead units, etc

        // thinking that units should be organized in squads
        // basically grouped into groups of 4, if you kill enough, the groups collapse into new groups

        GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetableObject in targetableObjects) {
            UnitStats unit = targetableObject.GetComponent<UnitStats>();
            if (!unit.isDead) {
                bm.CurrentAction.AddPossibleTarget(unit);
            }
        }
    }

    private void SetTargetsToAllTagged(string tag) {
        GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetableObject in targetableObjects) {
            UnitStats unit = targetableObject.GetComponent<UnitStats>();
            if (!unit.isDead) {
                bm.CurrentAction.AddTarget(unit);
            }
        }
    }

    public void SetParentUnit(UnitStats unit) {
        parentUnit = unit;
    }
}
