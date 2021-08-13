using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : ScriptableObject
{
    public int Damage;
    public int Heal;
    public int GutsCost;
    public string TargetType;
    public bool DestroySelf;
    
    public List<UnitStats> Targets;
    public List<UnitStats> PossibleTargets;
    public UnitStats ParentUnit;
    public Image ParentButtonImage;
    public GameObject Summon;
    public Item GiveItem;
    public List<Item> ResourceCost;
    public Dictionary<string,int> StatusEffects;

    protected BattleManager bm;

    protected Action action;

    private void Awake()
    {
        NewAction();
        InitializeBattleManager();
    }

    private void NewAction()
    {
        action = new Action();
    }

    public void InitializeBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public void AddTarget(ref UnitStats targetUnit)
    {
        Targets.Add(targetUnit);
    }

    public void AddPossibleTarget(ref UnitStats targetUnit)
    {
        PossibleTargets.Add(targetUnit);
    }    

    public void AddStatusEffect(string statusEffect, int stacks)
    {
        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] = stacks;
        } else {
            StatusEffects.Add(statusEffect, stacks);
        }
    }

    // public void UpdateStatusEffect(string statusEffect, int stacks) {
    //     // if (StatusEffects == null) {
    //     //     StatusEffects = new Dictionary<string,int>();
    //     // }

    //     if (StatusEffects.ContainsKey(statusEffect)) {
    //         StatusEffects[statusEffect] = stacks;
    //     } else {
    //         StatusEffects.Add(statusEffect, stacks);
    //     }
    // }

    public Dictionary<string,int> GetAllStatusEffects() {
        return StatusEffects;
    }

// maybe i should only set the requirements first, then check those, then if you can use the action, it sets the rest of these
    public void Clear() {
        Damage      = 0;
        Heal        = 0;
        GutsCost    = 0;
        TargetType  = null;
        DestroySelf = false;

        Summon       = null;
        ResourceCost = null;

        Targets = new List<UnitStats>();
        PossibleTargets = new List<UnitStats>();
        StatusEffects = new Dictionary<string,int>();
        ResourceCost = new List<Item>();
    }

    public void AttemptAction() {

    }

    public virtual void SetUpMove() {
        Debug.Log ("should you have overridden this?");
    }

    public void AISetUpMove() {
        InitializeBattleManager();
        SetUpMove();
    }
}
