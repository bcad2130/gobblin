using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public int StrengthDamage;
    public int TumDamage;
    public int TrueDamage;
    public int Heal;
    public int GutsCost;
    public int Sate;
    public int Speed;
    public int Tantalize;

    protected string skillType = null; // TODO
    public string TargetType;

    public Dictionary<string,int> StatusEffects;
    public Dictionary<string,int> StatChanges;

    public List<UnitStats> Targets;
    public List<UnitStats> PossibleTargets;
    public List<Item> ResourceCost;

    public bool isRecipe;
    public Recipe Recipe;

    public bool isBasicAttack;
    public bool checkIfCookin;

    private string ingredientCost;

    //these should be Lists
    public bool isSummon;
    public GameObject Summon;
    public Item GiveItem;
    public bool DestroySelf;

    public Action selfAction;


    // CONSTRUCTOR

    public Action ()
    {
        StrengthDamage  = 0;
        TumDamage       = 0;
        Speed           = 0;
        Heal            = 0;
        GutsCost        = 0;

        ingredientCost  = "";
        // skillType       = "";
        TargetType      = "";
        
        DestroySelf     = false;
        isRecipe        = false;
        isSummon        = false;
        isBasicAttack   = false;
        checkIfCookin   = false;

        Targets         = new List<UnitStats>();
        PossibleTargets = new List<UnitStats>();
        StatusEffects   = new Dictionary<string,int>();
        StatChanges     = new Dictionary<string,int>();
        ResourceCost    = new List<Item>();

        // selfAction = new Action();
    }

    // GETTERS

    public string GetSkillType()
    {
        return skillType;
    }

    public void SetSkillType(string type)
    {
        skillType = type;
    }

    // SELECTED TARGETS

    public void AddTarget(ref UnitStats targetUnit)
    {
        // print("AddTarget");

        Targets.Add(targetUnit);
    }

    public void AddTargets(ref List<UnitStats> unitList)
    {
        // print("AddTarget");

        Targets.AddRange(unitList);
    }

    public void RemoveTarget(ref UnitStats targetUnit)
    {
        Targets.Remove(targetUnit);
    }


    // POSSIBLE TARGETS

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


    // STATUS EFFECTS

    public void AddStatusEffect(string statusEffect, int stacks)
    {
        StatusEffects.Add(statusEffect, stacks);
    }

    // public void UpdateStatusEffect(string statusEffect, int stacks)
    // {
    //     if (StatusEffects.ContainsKey(statusEffect)) {
    //         StatusEffects[statusEffect] = stacks;
    //     } else {
    //         StatusEffects.Add(statusEffect, stacks);
    //     }
    // }

    public Dictionary<string,int> GetAllStatusEffects()
    {
        return StatusEffects;
    }

    // STAT CHANGES

    public void AddStatChange(string statChange, int stacks)
    {
        StatChanges.Add(statChange, stacks);
    }

    // public void UpdateStatChange(string statChange, int stacks)
    // {
    //     if (StatChanges.ContainsKey(statChange)) {
    //         StatChanges[statChange] = stacks;
    //     } else {
    //         StatChanges.Add(statChange, stacks);
    //     }
    // }

    public Dictionary<string,int> GetAllStatChanges()
    {
        return StatChanges;
    }    

    // INGREDIENTS

    public void AddIngredientCost(string ingredient)
    {
        ingredientCost = ingredient;
    }

    // ITEM/RESOURCE COST

    public string GetIngredientCost()
    {
        return ingredientCost;
    }

    public void SetResourceCost(List<Item> itemList)
    {
        ResourceCost = itemList;
    }

    public int GetGutsCost()
    {
        return GutsCost;
    }

    public Action GetSelfAction()
    {
        return selfAction;
    }

    public void SetSelfAction(Action action)
    {
        // selfAction = new Action();
        selfAction = action;
    }

    // public void AISetUpMove()
    // {
        
    // }
}
