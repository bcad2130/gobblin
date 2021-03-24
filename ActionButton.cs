using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : ScriptableObject
{
    public int Damage;
    public int Heal;
    public int ManaCost;
    public string TargetType;
    public bool DestroySelf;
    
    // public bool TargetedAction { get; set; }
    // public List<string> StatusEffects { get; set; }
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

    // public StatusEffects Status { get; set; }


    private void Awake() {
        NewAction();
        SetBattleManager();

        // SetBattleManager();
        // Targets = new List<UnitStats>();
        // PossibleTargets = new List<UnitStats>();
        // StatusEffects = new Dictionary<string,int>();
        // ResourceCost = new List<Item>();
    }

    private void NewAction() {
        action = new Action();
    }

    public void SetBattleManager() {
        // print('g');
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public void AddTarget(ref UnitStats targetUnit) {
        // print("AddTarget");
        // if (Targets == null) {
        //     Targets = new List<UnitStats>();
        // }

        Targets.Add(targetUnit);
    }

    public void AddPossibleTarget(ref UnitStats targetUnit) {
        // print("AddPossibleTarget");
        // if (PossibleTargets == null) {
        //     PossibleTargets = new List<UnitStats>();
        // }

        PossibleTargets.Add(targetUnit);
    }    

// // status effects should be a hash table so that all status effects are predefined
// // and more importantly, so status effects can stack multiple times
    public void AddStatusEffect(string statusEffect, int stacks) {
        // if (StatusEffects == null) {
        //     StatusEffects = new Dictionary<string,int>();
        // }

        StatusEffects.Add(statusEffect, stacks);
    }

    public void UpdateStatusEffect(string statusEffect, int stacks) {
        // if (StatusEffects == null) {
        //     StatusEffects = new Dictionary<string,int>();
        // }

        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] = stacks;
        } else {
            StatusEffects.Add(statusEffect, stacks);
        }
    }

    public Dictionary<string,int> GetAllStatusEffects() {
        // if (StatusEffects == null) {
        //     // return null;
        //     StatusEffects = new Dictionary<string,int>();
        // }
        
        return StatusEffects;
    }

// maybe i should only set the requirements first, then check those, then if you can use the action, it sets the rest of these
    public void Clear() {
        Damage      = 0;
        Heal        = 0;
        ManaCost    = 0;
        TargetType  = null;
        DestroySelf = false;

        Summon       = null;
        ResourceCost = null;

        Targets = new List<UnitStats>();
        PossibleTargets = new List<UnitStats>();
        StatusEffects = new Dictionary<string,int>();
        ResourceCost = new List<Item>();

        // if (Targets.Count > 0 && Targets != null) {
        //     // print(Targets);
        //     Targets.Clear();
        // }

        // if (PossibleTargets.Count > 0 || PossibleTargets != null) {
        //     PossibleTargets.Clear();
        // }
        // Debug.Log (StatusEffects);

        // if (StatusEffects.Count > 0 || StatusEffects != null) {
        // if (StatusEffects != null) {
        //     StatusEffects.Clear();
        // }
    }

    public void AttemptAction() {

        // bm.CurrentAction = this;
        
        // SetUpMove();
        // bm.CurrentAction = this.GetComponent<Action>();
        // bm.CurrentAction.ParentButtonImage = this.GetComponent<Image>();
        // bm.SetAction(this);

        // bm.AttemptAction();
    }

    public virtual void SetUpMove() {
        Debug.Log ("should you have overridden this?");
    }

    public void AISetUpMove() {
        SetBattleManager();
        SetUpMove();
    }
    // public Action Self() {
    //     return this;
    // }
}
