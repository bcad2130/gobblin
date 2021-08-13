  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    // ***************
    // ***VARIABLES***
    // ***************

    public GameObject targetButton;
    public GameObject foodButton;
    public GameObject HUD;

    public UnitStats CurrentUnit;

    private List<UnitStats> completeList;
    private List<UnitStats> nextUpList;
    private List<UnitStats> allyList;
    private List<UnitStats> enemyList;
    private List<UnitStats> meleeAllyList;
    private List<UnitStats> meleeEnemyList;
    private List<UnitStats> allyCookinList;
    // private List<UnitStats>
    
    private Dictionary<string,string> coverUnits;

    private bool myTurn = false;
    private bool canAct = false;
    // private bool movePicked = false;
    private bool mealPicked = false;
    private bool gameOver = false;

    public Action CurrentAction;
    public Inventory playerEquipment;
    public Inventory playerIngredients;
    public Inventory playerFood;
    public Inventory localIngredients;
    public Recipe recipe;







    // ***************
    // ***FUNCTIONS***
    // ***************


    // INITIALIZATION

    private void Awake()
    {
        StartStage();
    }

    private void StartStage()
    {
        InitializeItems();
        InitializeUnitLists();
        InitializeCurrentAction();

        SetHUD();
        // SortTurnList();
        // SetUnitTurnNumbers();


        // TODO: put this at start of round, check DT's round number (must add round number to DT)
        DialogueTrigger dt = FindObjectOfType<DialogueTrigger>();
        if (dt != null) {
            dt.TriggerDialogue();
        } else {
            StartCombat();
        }
        // FindObjectOfType<DialogueTrigger>().TriggerDialogue();


        // StartRound();
    }

    private void InitializeItems()
    {
        playerEquipment     = new Inventory();
        playerIngredients   = new Inventory();
        playerFood          = new Inventory();
        localIngredients    = new Inventory();

        // start with one cauldron
        playerEquipment.AddItem("Cauldron");

        // start with food (for testing)
        playerFood.AddItem("Cob");

        // populate local ingredients for forage (note that this system results in limited quantities of ingredients)
        localIngredients.AddItem("Potato");
        localIngredients.AddItem("Potato");
        localIngredients.AddItem("Potato");
        localIngredients.AddItem("Corn"  );
    }

    private void InitializeCurrentAction()
    {
        CurrentAction = new Action();
    }    

    private void InitializeUnitLists()
    {
        completeList    = new List<UnitStats>();
        nextUpList      = new List<UnitStats>();
        allyList        = new List<UnitStats>();
        enemyList       = new List<UnitStats>();
        meleeAllyList   = new List<UnitStats>();
        meleeEnemyList  = new List<UnitStats>();
        allyCookinList  = new List<UnitStats>();
        coverUnits      = new Dictionary<string, string>();

        PopulateList("Player", ref completeList);
        PopulateList("Enemy",  ref completeList);

        PopulateList("Player", ref allyList);
        PopulateList("Enemy",  ref enemyList);
        PopulateList("Player", ref meleeAllyList);
        PopulateList("Enemy",  ref meleeEnemyList);
    }




    // LISTS

    // TODO rename this to PopulateListByTag and reorder params
    private void PopulateList(string tag, ref List<UnitStats> list)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects) {
            list.Add(obj.GetComponent<UnitStats>());
        }
    }

    private void AddUnitToList(ref UnitStats unit, ref List<UnitStats> list)
    {
        list.Add(unit);
    }

    private void RemoveUnitFromList(UnitStats unit, ref List<UnitStats> list)
    {
        list.Remove(unit);
    }







    // TURN MANAGEMENT

    public void StartCombat()
    {
        StartRound();
    }

    private void StartRound()
    {
        // Debug.Log("Start Round");

        BuildTurnList();

        StartRoundUpdates();

        NextTurn();
    }

    private void StartRoundUpdates()
    {
        // Debug.Log("StartRoundUpdates");
        GutCheck();

        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
            unit.CheckIfDead();
        }

        HandleDeaths();
    }

    private void StartTurn()
    {
        // print("Turn Start");
        CurrentUnit = WhoseTurn();
        if (CurrentUnit.tag == "Player") {
            myTurn = true;
        }

        // print("Turn Start for: " + CurrentUnit.name);
        StartTurnUpdates();

        if (CurrentUnit.GetStatusEffectStacks("STUN") > 0 || CurrentUnit.NPC) {
            // Debug.Log(CurrentUnit.name + " is stunned due to hunger (or is food)");
            EndTurn();
        } else {
            // print("Turn for: " + CurrentUnit.name);

            PickUnitMove();
        }
    }

    private void StartTurnUpdates()
    {
        CurrentUnit.CheckGross();

        CurrentUnit.UpdateText();
        CurrentUnit.CheckIfDead();
    }

    private void PickUnitMove()
    {
        if (myTurn) {
            DisplayMoves();
        } else {
            AutoMove();
        }
    }

    private void EndTurn()
    {
        EndTurnUpdates();

        NextTurn();
    }

    private void EndTurnUpdates()
    {
        // TickCurrentUnitStatusEffects();

        // TURN MANAGEMENT
        myTurn = false;
        RemoveCurrentUnitFromNextUpList();
        SetUnitTurnNumbers();
        ResetCurrentAction();

        // UI UPDATES
        RemoveAllButtons();

        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
            unit.CheckIfDead();
        }

        HandleDeaths();
    }

    private void NextTurn()
    {
        if (!gameOver) {
            if (nextUpList.Count != 0){
                StartTurn();
            } else {
                EndRound();
            }
        }
    }

    private void EndRound()
    {
        EndRoundUpdates();

        StartRound();
        // Debug.Log("Round Over");
    }

    private void EndRoundUpdates()
    {
        CheckRecipe();

        Metabolism();

        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
            unit.CheckIfDead();
        }

        HandleDeaths();
    }

    private void RemoveCurrentUnitFromNextUpList()
    {
        if (nextUpList.Count != 0) {
            nextUpList[0].turnNumber = 0;
            nextUpList.RemoveAt(0);
        }
    }

    private UnitStats WhoseTurn()
    {
        // if (nextUpList.Count == 0) {
        //     EndRound();
        // }

        return nextUpList[0];
    }

    private void BuildTurnList()
    {
        nextUpList = completeList.OrderBy(w => w.speed).Reverse().ToList();

        SetUnitTurnNumbers();

        // TODO: MAKE SORTING SLIGHTLY RANDOM, BUT HIGHER CHANCE IF HIGH STAT?
    }

    private void SetUnitTurnNumbers()
    {
        int iter = 1;

        foreach (UnitStats unit in nextUpList)
        {
            unit.turnNumber = iter;
            iter++;
        }
    }

    private void ResetTurn()
    {
        PickUnitMove();
    }



    // EATING MANAGEMENT

    private void PickMeal()
    {
        DisplayMeals();
    }

    public void SetMealPicked(bool pick)
    {
        mealPicked = pick;
    }

    public bool GetMealPicked()
    {
        return mealPicked;
    }

    // COOKING MANAGEMENT
    public void SetRecipe(Recipe recipeToSet)
    {
        // recipe = new PotatoSoupRecipe();
        recipe = recipeToSet;
    }

    private void CheckRecipe()
    {
        if (recipe != null) { //exists
            // print("yes recipe");

            if (recipe.cookCount < recipe.cookTime) {
                // Debug.Log(recipe.cookTime);
                recipe.AddCookTurn();

            } else if (recipe.cookCount == recipe.cookTime) {

                Debug.Log("READY");
                if (CheckIngredients() && CheckStirring() ) {
                    playerIngredients.AddItem(recipe.result.Name);
                } else {
                    playerFood.AddItem("Burnt Mess");
                }
                // Debug.Log('A');
                RecipeCleanUp();
                // Debug.Log('A');
                


            } else if (recipe.cookCount > recipe.cookTime) {
                Debug.Log("What is this, Overcooked?");
            }



            // print(recipe.cookCount);
            // if (recipe.cookCount >= recipe.cookTime) {             // <- recipe is done cooking
            //     if (CheckIngredients()) {
            //         print("Recipe complete!");

            //         playerFood.AddItem(recipe.result.Name);
            //     }

            //     // cue taste test animation -> hand sign OK!

            //     // print "got <item>!"
            //     RecipeCleanUp();

            // } else { // recipe needs to cook more

            // }
        } else {
            // print("no recipe");
        }
    }

    private bool CheckStirring()
    {
        if ( recipe.GetStirCount() == recipe.GetStirGoal() ) {
            return true;
        } else {
            return false;
        }
    }

    private bool CheckIngredients()
    {
        Dictionary<string,int> requiredIngredients = recipe.reqIngredients.GetInventoryAsDictionary();

        foreach (KeyValuePair<string, int> ingredient in requiredIngredients) {
            if (ingredient.Value > 0) {
                print("Not enough ingredients");
                return false;
                // break
                // recipe explodes
                // did you forget the <which ever ingredient proc'ed this>
            } else if (ingredient.Value < 0) {
                print("Too many ingredients");
                return false;

                // break
                // recipe explodes
                // did you forget the <which ever ingredient proc'ed this>
            } else {
                print("right amount of this ingredient");
            }
        }

        return true;
    }

    private void RecipeCleanUp()
    {

        // TODO handle having more than one cauldron
        //      handle having enemy cauldrons?
                // Debug.Log('C');

        CurrentUnit = allyCookinList[0];
                // Debug.Log('B');

        allyCookinList.Remove(CurrentUnit);
        allyList.Remove(CurrentUnit);
        meleeAllyList.Remove(CurrentUnit);
        completeList.Remove(CurrentUnit);

        CurrentUnit.DestroySelf();

        recipe = null;

        // // restock cauldron item
        playerEquipment.AddItem("Cauldron");
    }

    // HUNGER MANAGEMENT

    private void Metabolism()
    {
        foreach (UnitStats unit in completeList) {
            if (!unit.isCookin && !unit.isDead) {
                unit.guts--;

                // guts should not be negative
                if (unit.guts < 0) {
                    unit.guts = 0;
                }

                unit.UpdateText();
            }
        }
    }

    private void GutCheck()
    {
        foreach (UnitStats unit in completeList) {
            // Debug.Log("Gut Check " + unit.name);
            if (unit.guts == 0 && !unit.isCookin) {
                Starve(unit);
            }
        }
    }

    private void Starve(UnitStats unit)
    {
        Debug.Log(unit.name + " is starving");
        unit.AddStatusEffect("STUN", 1);
        unit.TakeTumDamage(10);
        // unit.CheckIfDead();
    }



    // MISC

    private void DisplayCurrentUnit()
    {
        print(nextUpList[0]);
    }

    private void SetHUD() 
    {
        HUD = GameObject.FindWithTag("HUD");
    }

    private UnitStats GetUnitByName(string name)
    {
        foreach (UnitStats unit in completeList) {
            if (unit.name == name) {
                return unit;
            }
        }

        return null;
    }





    // ACTION

    public void SetAction(Action action)
    {
        CurrentAction = action;
    }

    public void ResetCurrentAction()
    {
        CurrentAction = null;
    }

    private void AutoMove()
    {
        // print("AutoMove");
        // GameObject canvas = CurrentUnit.GetCanvasObj();
        // GameObject canvas = GameObject.FindGameObjectsWithTag("HUD");


        List<MoveButton> currentUnitMoves = CurrentUnit.GetMoves();

        if (currentUnitMoves.Count > 0) {

            MoveButton currentMove = CurrentUnit.GetRandomAction();

            CurrentAction = currentMove.GetAction();

            // CurrentAction = CurrentUnit.GetRandomAction();

            // PickRandomMove();

            // print(currentMove);

            // USE THESE IF YOU WANT THE MOVE AND TARGET TO BE PICKED AUTOMAGICALLY (not working yet)
            // switch (CurrentAction.TargetType)
            // {
            //     case "OneAlly": 
            //     case "OneEnemy":
            //         TakeAction();
            //         break;
            //     case "Self":
            //     case "AllEnemies":
            //     case "Targetless":
            //         TakeAction();
            //         break;
            //     default:
            //         print("Invalid target type");
            //         break;
            // }
            
            SelectTargetsByActionType();

            TakeAction();

            // USE THESE IF YOU WANT THE BUTTON TO SHOW (working)
            // for some reason the bm.action isn't set in effect

            // var randomIndex = Random.Range(0, currentUnitMoves.Count);
            // Button selectedMove = currentUnitMoves[randomIndex];

            // Button instantButton = Instantiate(selectedMove, new Vector3(40, 0, 0), Quaternion.identity);
            // instantButton.transform.SetParent(canvas.transform, false);
        } else {
            print ("This unit has no moves");
        }
    }

    private void SelectRandomUnitFromPossibleTargets()
    {
        if (CurrentAction.PossibleTargets.Count != 0) {

            UnitStats selectedUnit = CurrentAction.PossibleTargets[Random.Range(0, CurrentAction.PossibleTargets.Count)];

            CurrentAction.AddTarget(ref selectedUnit);
        } else { 
            Debug.Log("NO UNIT SELECTABLE");
        }
    }

    public bool CanAffordResourceCost (List<Item> resourceCost) {
        // TODO: Make this work for requiring multiple of the same item
        if (resourceCost != null) {
            // print ("resource cost is not null");
            foreach (Item resource in resourceCost) {
                // print(CurrentAction.TargetType);
                switch(CurrentAction.TargetType) {
                    case "Cook":
                        if (!playerEquipment.CheckItem(resource.Name)) {
                            return false;
                        }
                        break;
                    case "Ingredient":
                        if (!playerIngredients.CheckItem(resource.Name)) {
                            return false;
                        }
                        break;
                    case "Eat":
                        if (!playerFood.CheckItem(resource.Name)) {
                            return false;
                        }
                        break;
                    default:
                        print("Invalid target type for action with resourceCost");
                        break;
                }
            }
        }

        return true;
    }

    public void SpendResource (List<Item> resourceCost)
    {
        if (resourceCost != null) {
            foreach (Item resource in resourceCost) {
                switch(CurrentAction.TargetType) {
                    case "Cook":
                        playerEquipment.RemoveItem(resource.Name);
                        break;
                    case "Ingredient":
                        playerIngredients.RemoveItem(resource.Name);
                        break;
                    case "Ally":
                        playerFood.RemoveItem(resource.Name);
                        break;
                    default:
                        print("Invalid target type for action with resourceCost");
                        break;
                }
            }
        }

        // print ("we made it this far");
    }

    public void AttemptAction(Action action)
    {
        if (CurrentUnit.CanAffordGutsCost(action.GutsCost) && CanAffordResourceCost(action.ResourceCost)) {
            // movePicked = true;
            SetAction(action);
            SelectTargetsByActionType();

            switch (CurrentAction.TargetType)
                {
                    case "Ally":
                    case "MeleeAlly":
                    case "CoverAlly":
                    case "CoveredAlly": // TODO
                    case "Enemy":
                    case "MeleeEnemy":
                    case "MeleeEnemyPierce":
                    case "CoveredEnemy": // TODO
                    case "Stir":
                    case "Ingredient":
                        DisplayTargets();
                        break;
                    case "Self":
                    case "AllMelee": // TODO
                    case "AllCoveredEnemies": // TODO
                    case "AllEnemies":
                    case "Targetless": // or "Untargeted"
                    case "Forage":
                    case "Cook":
                        TakeAction();
                        break;
                    case "Eat":
                        PickMeal();
                        break;
                    default:
                        print("Invalid target");
                        break;
                }
        } else {

            // TODO: reimplement the below, with new UI refactor
            // we need an indicator that you couldn't afford a move and the reason why
            // alt, we could not display unaffordable moves
            // this.GetComponent<Image>().color = Color.red;
            // CurrentAction.ParentButtonImage.color = Color.red;

            // ResetCurrentAction();

            // bm.CurrentAction.Clear();

            print("cannot afford");
        }
    }

    public void TakeAction()
    {
        canAct = true;
        // print(CurrentAction);

        if (CurrentAction.GutsCost != 0) {
            CurrentUnit.SpendGuts(CurrentAction.GutsCost);
        }

        // TODO make it so you can't cover yourself in targeting
        if (CurrentAction.TargetType == "CoverAlly") {
            if (CurrentAction.Targets.Count == 1) {
                foreach (UnitStats target in CurrentAction.Targets) {
                    CoverUnit(target);
                }
            } else {
                Debug.Log("Can only cover one ally!");
                canAct = false;
                // ResetTurn();
            }
        }


        if (CurrentAction.TargetType == "Forage") {
            // Debug.Log(localIngredients);
            if (localIngredients.CountItems() != 0) {
                string foragedIngredient = localIngredients.GetRandomItem();
                localIngredients.RemoveItem(foragedIngredient);
                playerIngredients.AddItem(foragedIngredient);
                Debug.Log(foragedIngredient);
            } else {
                Debug.Log("No ingredients to forage!");
                canAct = false;
                // ResetTurn();
            }
        }

        if (CurrentAction.TargetType == "Stir") {
            // if ()
            recipe.stirCount++;
        }

        if (CurrentAction.ResourceCost.Count != 0) {
            if (CurrentAction.TargetType == "Ingredient") {
                // foreach (UnitStats target in CurrentAction.Targets) {
                //     target.AddIngredient(CurrentAction.ResourceCost);
                // }

                //NEW METHOD
                // SetRecipe();

                foreach (Item ingredient in CurrentAction.ResourceCost) {
                    // recipe.ingredients[ingredient.Name]--;
                    recipe.reqIngredients.RemoveItem(ingredient.Name);
                }

            }

            SpendResource(CurrentAction.ResourceCost);
        }

        if (CurrentAction.Recipe != null) {
            SetRecipe(CurrentAction.Recipe);
        }

        // TODO: i should reorganize the units automatically. So if there are a ton of units summoned, they get properly organized on the screen
        // i should have certain slots premade (enumerated?) and then it fills in the next available ally/enemy spot
        // then i can have a certain number of ally spots available as you level
        // and maybe a certain number of cauldron spots, upgradeable
        if (CurrentAction.Summon) {
            SummonUnit();
        }

        // print(CurrentAction.GiveItem);

        // deprecated?
        // if (CurrentAction.GiveItem != null) {
        //     // print("succeess");
        //     // print (CurrentAction.GiveItem); 

        //     switch(CurrentAction.TargetType) {
        //         // case "Cook":
        //         //     playerEquipment.AddItem(CurrentAction.GiveItem.Name);
        //         //     break;
        //         case "Forage":
        //             playerIngredients.AddItem(CurrentAction.GiveItem.Name);
        //             break;
        //         // case "Cook":
        //         //     playerFood.AddItem(CurrentAction.GiveItem.Name);
        //         //     break;
        //         default:
        //             print("Invalid target type for action with resourceCost");
        //             break;
        //     }
        // }

        // HERE WE ARE NOT SENDING THE REAL UNITS TO EFFECT(), JUST COPIES OF THEIR VALUES, SO THE HEALTH DOESN'T UPDATE AND THE UNITSTATS AWAKE STEPS NEVER HAPPENED
        // IF I COULD INSTEAD GRAB THESE TARGETS VIA FIND COMPONENT, IT MIGHT GRAB THE REAL ONES
        
        // Debug.Log(CurrentAction.Targets.Count);
        if (canAct) {
            if ( CurrentAction.Targets.Count != 0) {
                // Debug.Log(CurrentUnit.name);
                foreach (UnitStats target in CurrentAction.Targets) {
                    // print(target);
                    // print(target.bm);
                    // target.Effect();
                    ApplyActionToUnit(target);
                }

            }
            
            EndTurn();
        } else {
            canAct = true;
            ResetTurn();
        }
    }

    public void ApplyActionToUnit(UnitStats unit)
    {

        // if (CurrentAction.Type == "Basic") {
        //     CurrentUnit.speed
        // }

        if (CurrentAction.Damage > 0) {
            // Debug.Log(unit.name + " receives " + CurrentAction.Damage + " raw damage");
            unit.TakeAttack(CurrentAction.Damage, CurrentUnit.speed);
        }

        if (CurrentAction.TargetType == "MeleeEnemyPierce") {
            if (coverUnits.ContainsKey(unit.name)) {
                Debug.Log(coverUnits[unit.name] + " receives " + CurrentAction.Damage/2 + " raw pierce damage");
                
                GetUnitByName(coverUnits[unit.name]).TakeAttack(CurrentAction.Damage/2, CurrentUnit.speed);
            } else {
                Debug.Log ("covered unit not found");
            }
        }

        if (CurrentAction.Heal > 0) {
            unit.HealDamage(CurrentAction.Heal);
        }

        if (CurrentAction.Sate > 0) {
            unit.SateHunger(CurrentAction.Sate);
        }

        // TODO: i should just use the status effect instead of having a local shield stat
        unit.ApplySingleEffectStatus(CurrentAction.GetAllStatusEffects());

        foreach(KeyValuePair<string, int> effect in CurrentAction.GetAllStatusEffects())
        {
            // do something with effect.Value or effect.Key

            unit.AddStatusEffect(effect.Key, effect.Value);
        }

        if (CurrentAction.DestroySelf) {
            // print('h');
            unit.DestroySelf();

            // TODO: must clear from unit lists etc (might have solved this now, will keep testing)
            // on this note, will i have revival? should i even keep dead units around?
            // if not, i should have a more effective way of clearing units, since i might want to move them around, or have them transform, or get "used up" like the cauldron
        }
    }

    private void CoverUnit(UnitStats unitToCover)
    {
        if (!coverUnits.ContainsKey(CurrentUnit.name)){
            coverUnits.Add(CurrentUnit.name, unitToCover.name);
            if (unitToCover.tag == "Player") { // TODO add functionality for NPC
                meleeAllyList.Remove(unitToCover);
            } else if (unitToCover.tag == "Enemy") {
                meleeEnemyList.Remove(unitToCover);
            }
        } else {
            Debug.Log("need to handle this case");
        }

    }

    // TODO: REMOVE UNIT FROM COVER FUNCTION
    //       TRIGGER THIS WHEN THE BLOCKING UNIT DIES

    private void SummonUnit()
    {
        GameObject summon = Instantiate(CurrentAction.Summon, new Vector3(-6, -3, 0), Quaternion.identity); // hardcoded location, should make dynamic
        UnitStats summonUnit = summon.GetComponent<UnitStats>();

        if (CurrentUnit.tag == "Player") {
        // if (summon.tag == "Player" || summon.tag == "NPC") {
            allyList.Add(summonUnit);
            meleeAllyList.Add(summonUnit);
        } else if (CurrentUnit.tag == "Enemy") {
            enemyList.Add(summonUnit);
        } else {
            print("who is this?");
        }

        completeList.Add(summonUnit);


        // this is where i check if its a recipe action, but i should move this higher in the work flow
        if (summonUnit.isCookin) {
            AddUnitToList(ref summonUnit, ref allyCookinList);
            // will this work by ref?
            // SetRecipe();

            // print("recipe");
            // print(recipe);
            // print("summon");
            // print(summon);

            recipe.cauldron = summonUnit;
        }
    }





    // TARGETTING

    public void AddTargetToAction(ref UnitStats unit)
    {
        CurrentAction.AddTarget(ref unit);
    }

    private void SetTargetToSelf()
    {
        CurrentAction.AddTarget(ref CurrentUnit);
    }


    private void SetTargetsToAllTagged(string tag)
    {
        GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetableObject in targetableObjects) {
            UnitStats unit = targetableObject.GetComponent<UnitStats>();
            if (!unit.isDead) {
                CurrentAction.AddTarget(ref unit);
            }
        }
    }


    private void FindTargetsByTag(string tag)
    {
            // print("FindTargetsByTag");
            // TODO: use bm to grab the enemy list or ally list as appropriate
            // maybe actions should have a "targetable group" property, like enemies, allies, dead units, etc

            // now rethinking, each unit can create a squad by 'covering' for another unit
            // then you take all (or most, TBD) melee damage for that unit
            // units that are being covered can't use melee attacks
            // they can still be hit by ranged attacks and magic

        GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetableObject in targetableObjects) {
            UnitStats unit = targetableObject.GetComponent<UnitStats>();
            if (!unit.isDead) {
                CurrentAction.AddPossibleTarget(ref unit);
            }
        }
    }

    private void FindTargetsByList(List<UnitStats> unitList)
    {
        CurrentAction.AddPossibleTargets(ref unitList);
    }

    public void SelectTargetsByActionType()
    {
        // TODO use FindTargetsByList instead of FindTargetsByTag when possible
        switch (CurrentAction.TargetType) 
        {
            case "Self":
                SetTargetToSelf();
                break;
            case "SelfOrAlly":
                FindTargetsByTag(CurrentUnit.tag);
                if (!myTurn) {
                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "CoverAlly":
            case "MeleeAlly":
                if (!myTurn) {
                    FindTargetsByList(meleeEnemyList);

                    CurrentAction.RemovePossibleTarget(ref CurrentUnit);

                    SelectRandomUnitFromPossibleTargets();
                } else {
                    FindTargetsByList(meleeAllyList);

                    CurrentAction.RemovePossibleTarget(ref CurrentUnit);
                }
                break;
            case "Ally":
                FindTargetsByTag(CurrentUnit.tag);
                if (!myTurn) {
                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "MeleeEnemy":
            case "MeleeEnemyPierce":
                if (!myTurn) {
                    FindTargetsByList(meleeAllyList);
                    SelectRandomUnitFromPossibleTargets();
                } else {
                    FindTargetsByList(meleeEnemyList);
                }
                break;
            case "Enemy":
                if (!myTurn) {
                    FindTargetsByTag("Player");
                    SelectRandomUnitFromPossibleTargets();
                } else {
                    FindTargetsByTag("Enemy");
                }
                break;
            case "AllEnemies":
                if (!myTurn) {
                    SetTargetsToAllTagged("Player");
                } else {
                    SetTargetsToAllTagged("Enemy");
                }
                break;
            case "Stir":
            case "Ingredient":
                FindTargetsByList(allyCookinList);
                break;
            case "Targetless":
            case "Forage":
            case "Cook":
            case "Eat":
                break;
            default:
                print("Invalid target");
                break;
        }
    }





    // UI

    private void DisplayMoves()
    {
        // GameObject canvas = CurrentUnit.GetCanvasObj();
        // GameObject canvas = GameObject.FindGameObjectsWithTag("HUD");

        // Iterate this value to make position lower
        float yPos = 50f;


        if (CurrentUnit.GetMoves().Count > 0) {
            foreach (MoveButton button in CurrentUnit.GetMoves()) {
                MoveButton instantButton = Instantiate(button, new Vector3(0, yPos, 0), Quaternion.identity);

                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                // instantButton.transform.position = new Vector3(20,5,0);
                instantButton.transform.SetParent(HUD.transform, false);
                // second param keeps scale etc the same


                yPos -= 30f;
            }
        } else {
            print ("This unit has no moves");
            EndTurn();
        }
    }

    private void DisplayMeals()
    {
        RemoveAllButtons();

        // Iterate this value to make position lower
        float yPos = 0f;


        if (playerFood.CountItems() > 0) {
            foreach (KeyValuePair<string,int> food in playerFood.GetInventoryAsDictionary()) {
                GameObject instantButton = Instantiate(foodButton, new Vector3(0, yPos, 0), Quaternion.identity);

                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                // instantButton.transform.position = new Vector3(20,5,0);
                instantButton.transform.SetParent(HUD.transform, false);
                // second param keeps scale etc the same


                yPos -= 30f;
            }
        } else {
            print ("You have no food");
            
            ResetTurn();
        }
    }

    public void DisplayTargets()
    {
        // print("DisplayTargets");
        RemoveAllButtons();

        if (CurrentAction.PossibleTargets.Count != 0) {
            foreach (UnitStats targetableUnit in CurrentAction.PossibleTargets) {
                GameObject canvas = targetableUnit.GetCanvasObj();
                GameObject instantButton = Instantiate(targetButton, new Vector3(0, 0, 0), Quaternion.identity);

                instantButton.transform.SetParent(canvas.transform, false);
                instantButton.transform.localScale = new Vector3(.2f,.2f,1);

                UnitStats tempTargetUnit = targetableUnit;
                instantButton.GetComponent<TargetButton>().SetParentUnit(ref tempTargetUnit);
            }
            } else {
                Debug.Log("No PossibleTargets");

                ResetTurn();
            }
    }

    public void RemoveAllButtons()
    {
        GameObject[] allButtons;
        allButtons = GameObject.FindGameObjectsWithTag("Button");

        foreach (GameObject button in allButtons) {
            Destroy(button);
        }
    }





    // GAME END

    private void CheckGameOver()
    {
        if (allyList.Count == 0)
        {
            GameOver();
        }

        if (enemyList.Count == 0) {
            GameWin();
        }
    }

    private void GameWin()
    {
        gameOver = true;
        print("Game Win");
    }

    private void GameOver()
    {
        gameOver = true;
        print("Game Over");
    }

    public void HandleDeaths()
    {
        completeList.RemoveAll(     unit => unit.isDead == true);
        nextUpList.RemoveAll(       unit => unit.isDead == true);
        enemyList.RemoveAll(        unit => unit.isDead == true);
        allyList.RemoveAll(         unit => unit.isDead == true);
        meleeAllyList.RemoveAll(    unit => unit.isDead == true);
        meleeEnemyList.RemoveAll(   unit => unit.isDead == true);

        CheckGameOver();
    }

    public void RemoveUnitFromAllLists(UnitStats unit)
    {
        completeList.Remove(unit);
        nextUpList.Remove(unit);
        enemyList.Remove(unit);
        allyList.Remove(unit);
    }
}
