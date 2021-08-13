using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public int Damage;
    public int Heal;
    public int GutsCost;
    public int Sate;
    public int Speed;

    public string TargetType;
    public bool DestroySelf;

    public Dictionary<string,int> StatusEffects;

    public List<UnitStats> Targets;
    public List<UnitStats> PossibleTargets;
    public List<Item> ResourceCost;

    public Recipe Recipe;

    //these should be Lists
    public GameObject Summon;
    public Item GiveItem;


    // public Button ParentButton;

    public Action ()
    {
        Damage = 0;
        Heal = 0;
        GutsCost = 0;
        TargetType = "";
        DestroySelf = false;

        Targets = new List<UnitStats>();
        PossibleTargets = new List<UnitStats>();
        StatusEffects = new Dictionary<string,int>();
        ResourceCost = new List<Item>();
    }

    public void AddTarget(ref UnitStats targetUnit)
    {
        // print("AddTarget");

        Targets.Add(targetUnit);
    }

    public void RemoveTarget(ref UnitStats targetUnit)
    {
        Targets.Remove(targetUnit);
    }

    public void AddPossibleTarget(ref UnitStats targetUnit)
    {
        // print("AddPossibleTarget");

        PossibleTargets.Add(targetUnit);
    }

    public void AddPossibleTargets(ref List<UnitStats> unitList)
    {
        PossibleTargets.AddRange(unitList);
    }

    public void RemovePossibleTarget(ref UnitStats targetUnit)
    {
        PossibleTargets.Remove(targetUnit);
    }

    public void RemovePossibleTargets(ref List<UnitStats> unitList)
    {
        PossibleTargets.AddRange(unitList);
    }


// // status effects should be a hash table so that all status effects are predefined
// // and more importantly, so status effects can stack multiple times
    public void AddStatusEffect(string statusEffect, int stacks)
    {
        StatusEffects.Add(statusEffect, stacks);
    }

    public void UpdateStatusEffect(string statusEffect, int stacks)
    {
        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] = stacks;
        } else {
            StatusEffects.Add(statusEffect, stacks);
        }
    }

    public Dictionary<string,int> GetAllStatusEffects()
    {        
        return StatusEffects;
    }

    public void AISetUpMove()
    {
        
    }
}
