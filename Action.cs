using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public int Damage { get; set; }
    public int Heal { get; set; }
    public int ManaCost { get; set; }
    public string TargetType { get; set; }
    public bool DestroySelf {get; set; }
    
    // public bool TargetedAction { get; set; }
    // public List<string> StatusEffects { get; set; }
    public List<UnitStats> Targets { get; set; }
    public List<UnitStats> PossibleTargets { get; set; }
    public UnitStats ParentUnit { get; set; }
    public GameObject Summon { get; set; }
    public Item GiveItem { get; set; }
    public List<Item> ResourceCost { get; set; }

    // public StatusEffects Status { get; set; }

    private Dictionary<string,int> StatusEffects;

    public void AddTarget(UnitStats targetUnit) {
        if (Targets == null) {
            Targets = new List<UnitStats>();
        }

        Targets.Add(targetUnit);
    }

    public void AddPossibleTarget(UnitStats targetUnit) {
        if (PossibleTargets == null) {
            PossibleTargets = new List<UnitStats>();
        }

        PossibleTargets.Add(targetUnit);
    }    

// // status effects should be a hash table so that all status effects are predefined
// // and more importantly, so status effects can stack multiple times
    public void AddStatusEffect(string statusEffect, int stacks) {
        if (StatusEffects == null) {
            StatusEffects = new Dictionary<string,int>();
        }

        StatusEffects.Add(statusEffect, stacks);
    }

    public void UpdateStatusEffect(string statusEffect, int stacks) {
        if (StatusEffects == null) {
            StatusEffects = new Dictionary<string,int>();
        }

        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] = stacks;
        } else {
            StatusEffects.Add(statusEffect, stacks);
        }
    }

    public Dictionary<string,int> GetAllStatusEffects() {
        if (StatusEffects == null) {
            // return null;
            StatusEffects = new Dictionary<string,int>();
        }
        
        return StatusEffects;
    }

// maybe i should only set the requirements first, then check those, then if you can use the action, it sets the rest of these
    public void Clear() {
        Damage      = 0;
        Heal        = 0;
        ManaCost    = 0;

        Summon       = null;
        ResourceCost = null;


        // if (Targets.Count > 0 && Targets != null) {
        //     // print(Targets);
        //     Targets.Clear();
        // }

        // if (PossibleTargets.Count > 0 || PossibleTargets != null) {
        //     PossibleTargets.Clear();
        // }
        // Debug.Log (StatusEffects);

        // if (StatusEffects.Count > 0 || StatusEffects != null) {
        if (StatusEffects != null) {
            StatusEffects.Clear();
        }
    }
}
