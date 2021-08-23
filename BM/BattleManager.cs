﻿  
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

    // PREFABS
    public GameObject targetButton;
    public GameObject eatButton;
    public GameObject serveDrinkButton;
    public GameObject serveFoodButton;
    public GameObject serveMealButton;
    public GameObject recipeButton;
    public GameObject ingredientButton;
    public GameObject HUD;

    // TARGET LISTS
    private List<UnitStats> completeList;
    private List<UnitStats> nextUpList;
    private List<UnitStats> allyList;
    private List<UnitStats> enemyList;
    private List<UnitStats> meleeAllyList;
    private List<UnitStats> meleeEnemyList;
    private List<UnitStats> allyCookinList;
    private Dictionary<string,UnitStats> coverUnits;

    // PHASE FLAGS
    private bool myTurn = false;
    private bool canAct = false;
    // private bool movePicked = false;
    private bool recipePicked = false;
    private bool mealPicked = false;
    private bool gameOver = false;

    // ACTIVE VARS
    public UnitStats CurrentUnit;
    public Action   CurrentAction;
    public Recipe   CurrentRecipe;
    public Item     CurrentMeal;

    // INVENTORIES AND ITEMS
    public Inventory playerEquipment;
    public Inventory playerIngredients;
    public Inventory playerFood;
    public Inventory localIngredients;
    private List<Recipe> playerRecipes;
    private List<Item> playerMealItems;

    // CONSTANTS: STATS
    private const string STRENGTH   = "STRENGTH";
    private const string DEFENSE    = "DEFENSE";
    private const string SPEED      = "SPEED";
    private const string TUM        = "TUM";
    private const string TASTE      = "TASTE";
    private const string NOSE       = "NOSE";


    // CONSTANTS: STATUSES
    private const string STUN       = "STUN";
    private const string GROSS      = "GROSS";
    private const string DODGE      = "DODGE";
    private const string DEFEND     = "DEFEND";
    private const string BOIL       = "BOIL";




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
        InitializeRecipes();

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
    }

    private void InitializeItems()
    {
        playerEquipment     = new Inventory();
        playerIngredients   = new Inventory();
        playerFood          = new Inventory();
        localIngredients    = new Inventory();

        playerMealItems     = new List<Item>();

        // start with this equipment
        playerEquipment.AddItem("Cauldron");

        // start with pre-made food (for testing)
        CornCobItem test0 = new CornCobItem();
        test0.SetQuality(0);
        playerMealItems.Add(test0);

        CornCobItem test1 = new CornCobItem();
        test1.SetQuality(1);
        playerMealItems.Add(test1);

        CornCobItem test2 = new CornCobItem();
        test2.SetQuality(2);
        playerMealItems.Add(test2);

        CornCobItem test3 = new CornCobItem();
        test3.SetQuality(3);
        playerMealItems.Add(test3);
        // playerMealItems.Add(new GritsItem());
        // playerMealItems.Add(new PopCornItem());
        // playerMealItems.Add(new PopCornItem());
        // playerMealItems.Add(new BurntMessItem());

        // start with ingredients (for testing)
        playerIngredients.AddItem("Bottle");
        playerIngredients.AddItem("Bottle");
        playerIngredients.AddItem("Bottle");

        // populate local ingredients for forage (note that this system results in limited quantities of ingredients)
        localIngredients.AddItem("Bottle");
        localIngredients.AddItem("Bottle");
        localIngredients.AddItem("Bottle");
        localIngredients.AddItem("Bottle");
        localIngredients.AddItem("Water");
        localIngredients.AddItem("Water");
        localIngredients.AddItem("Grease");
        localIngredients.AddItem("Grease");
        localIngredients.AddItem("Corn");
        localIngredients.AddItem("Corn");
        localIngredients.AddItem("Corn");
        localIngredients.AddItem("Corn");
        localIngredients.AddItem("Corn");
        localIngredients.AddItem("Corn");
        localIngredients.AddItem("Corn");
        localIngredients.AddItem("Corn");
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
        coverUnits      = new Dictionary<string, UnitStats>();

        PopulateList("Player", ref completeList);
        PopulateList("Enemy",  ref completeList);

        PopulateList("Player", ref allyList);
        PopulateList("Enemy",  ref enemyList);
        PopulateList("Player", ref meleeAllyList);
        PopulateList("Enemy",  ref meleeEnemyList);
    }

    private void InitializeRecipes()
    {
        playerRecipes = new List<Recipe>();
        playerRecipes.Add(new CornCobRecipe());
        playerRecipes.Add(new GritsRecipe());
        playerRecipes.Add(new PopCornRecipe());
    }



    // LISTS

    private void PopulateList(string tag, ref List<UnitStats> list)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects) {
            list.Add(obj.GetComponent<UnitStats>());
        }
    }

    private void AddUnitToList(UnitStats unit, ref List<UnitStats> list)
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
            CheckIfDead(unit);
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

        print("Turn Start for: " + CurrentUnit.name);
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
        CheckGross(CurrentUnit);

        CurrentUnit.UpdateText();
        CheckIfDead(CurrentUnit);
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

        if (CurrentUnit.GetStatusEffectStatus("STUN")) {
            CurrentUnit.SubtractStatusEffect("STUN", 1);
        }

        // TURN MANAGEMENT
        myTurn = false;
        RemoveCurrentUnitFromNextUpList();
        SetUnitTurnNumbers();
        ResetCurrentAction();

        // UI UPDATES
        RemoveAllButtons();

        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
            CheckIfDead(unit);
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
        foreach (KeyValuePair<string,UnitStats> coverLink in coverUnits) {
            Debug.Log("Unit " + coverLink.Key + " is covering " + coverLink.Value.name);
        }

        EndRoundUpdates();
        // Debug.Log("Round Over");

        StartRound();
    }

    private void EndRoundUpdates()
    {
        CheckRecipe();

        Metabolism();

        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
            CheckIfDead(unit);
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
        nextUpList = completeList.OrderBy(w => w.GetNetSpeed()).Reverse().ToList();

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
        ResetCurrentAction();

        // RemoveAllButtons();

        PickUnitMove();
    }



    // EATING MANAGEMENT

    private void PickMealToEat()
    {
        DisplayMealsToEat();
    }

    private void PickDrinkToServe()
    {
        DisplayMealsToServe("Drink");
    }

    private void PickFoodToServe()
    {
        DisplayMealsToServe("Food");
    }

    private void PickMealToServe()
    {
        DisplayMealsToServe("Meal");
    }

    public void SetMealPicked(bool pick)
    {
        mealPicked = pick;
    }

    public bool GetMealPicked()
    {
        return mealPicked;
    }

    public void SetCurrentMeal(Item meal)
    {
        CurrentMeal = meal;
    }

    public void PrepMeal()
    {
        const int qualityMultiplier = 5;

        int netDamage   = CurrentMeal.ItemAction().Damage;
        int netHeal     = CurrentMeal.ItemAction().Heal;
        int netSate     = CurrentMeal.ItemAction().Sate;

        if (netDamage > 0) {
            netDamage   += CurrentMeal.GetQuality() * qualityMultiplier;
        }

        if (netHeal > 0) {
            netHeal     += CurrentMeal.GetQuality() * qualityMultiplier;
        }

        if (netSate > 0) {
            netSate     += CurrentMeal.GetQuality() * qualityMultiplier;
        }

        CurrentAction.Damage    = netDamage;
        CurrentAction.Heal      = netHeal;
        CurrentAction.Sate      = netSate;

        // Should Quality add Status Effect stacks?
        CurrentAction.StatusEffects = CurrentMeal.ItemAction().GetAllStatusEffects();
    }



    // COOKING MANAGEMENT
    private void PickRecipe()
    {
        DisplayRecipes();
    }

    public void SetRecipePicked(bool pick)
    {
        recipePicked = pick;
    }

    public bool GetRecipePicked()
    {
        return recipePicked;
    }

    public void SetRecipe(Recipe recipeToSet)
    {
        CurrentRecipe = recipeToSet;

        SetRecipePicked(true);

        Debug.Log("You're cooking " + CurrentRecipe.GetName() + ". It will take " + CurrentRecipe.GetCookTime() + " round(s).");
    }

    private void CheckRecipe()
    {
        if (CurrentRecipe != null) {

            if (CurrentRecipe.GetCookCount() < CurrentRecipe.GetCookTime()) {
                CurrentRecipe.AddCookTurn();
            } else if (CurrentRecipe.GetCookCount() == CurrentRecipe.GetCookTime()) {

                if (CheckIngredients() && CheckStirring() ) {
                    CreateMeal();
                } else {
                    playerMealItems.Add(new BurntMessItem());
                    Debug.Log("You made a Burnt Mess. Better luck next time!");
                }

                RecipeCleanUp();
                
            } else if (CurrentRecipe.GetCookCount() > CurrentRecipe.GetCookTime()) {
                Debug.Log("What is this, Overcooked?");
            }
        } else {
            // Debug.Log("No recipe");
        }
    }

    private void Stir()
    {
        CurrentRecipe.AddStir();

        CurrentRecipe.AddFlavor(CurrentUnit.GetNetNose());
    }

    private bool CheckStirring()
    {
        if ( CurrentRecipe.GetStirCount() == CurrentRecipe.GetStirGoal() ) {
            return true;
        } else {
            return false;
        }
    }

    private void Season()
    {
        print(CurrentUnit.GetNetTaste());

        CurrentRecipe.AddFlavor(CurrentUnit.GetNetTaste());
    }

    private void CreateMeal()
    {
        print(CurrentRecipe.GetFlavor());

        int quality = 0;

        if (CurrentRecipe.GetFlavor() >= CurrentRecipe.GetFlavorGoal()) {
            Debug.Log('A');
            quality++;
        }

        if (CurrentRecipe.GetFlavor() >= CurrentRecipe.GetFlavorGoal() + CurrentRecipe.GetFlavorGoalBonus()) {
            quality++;
        }

        if (CurrentRecipe.GetFlavor() >= CurrentRecipe.GetFlavorGoal() + CurrentRecipe.GetFlavorGoalBonus() * 2) {
            quality++;
        }

        Item meal = CurrentRecipe.GetResult();

        meal.SetQuality(quality);

        // meal.SetNameQuality(meal.Name + " +" + meal.GetQuality().ToString());

        playerMealItems.Add(meal);
        Debug.Log("You made " + meal.Name);
    }

    private void RecipeCleanUp()
    {

        // TODO handle having more than one cauldron
        //      handle having enemy cauldrons?

        CurrentUnit = allyCookinList[0];

        allyCookinList.Remove(CurrentUnit);
        allyList.Remove(CurrentUnit);
        meleeAllyList.Remove(CurrentUnit);
        completeList.Remove(CurrentUnit);

        CurrentUnit.DestroySelf();

        CurrentRecipe = null;
        SetRecipePicked(false);

        // restock cauldron item
        playerEquipment.AddItem("Cauldron");
    }



    // HUNGER MANAGEMENT

    private void Metabolism()
    {
        foreach (UnitStats unit in completeList) {
            if (!unit.isCookin) { // && !unit.isDead) {
                unit.LoseGuts(1);

                // guts should not be negative
                if (unit.GetCurrentGuts() < 0) {
                    unit.SetCurrentGuts(0);
                }

                unit.UpdateText();
            }
        }
    }

    private void GutCheck()
    {
        foreach (UnitStats unit in completeList) {
            // Debug.Log("Gut Check " + unit.name);
            if (unit.GetCurrentGuts() == 0 && !unit.isCookin) {
                Starve(unit);
            }
        }
    }

    private void Starve(UnitStats unit)
    {
        Debug.Log(unit.name + " is starving");

        unit.AddStatusEffect("STUN", 1);
        TakeTumDamage(unit, 10);
    }



    // INGREDIENT MANAGEMENT

    private void Forage()
    {
        int forageCount = CurrentUnit.GetNetNose() / 5;

        // make number inclusive 0-4
        int randomNumber = Random.Range(0, 5);

        if ((CurrentUnit.GetNetNose() % 5) > randomNumber) {
            forageCount++;
        }

        if (localIngredients.CountItems() > 0) {
            // if (localIngredients.CountItems() - forageCount < 0)
            while (forageCount > 0 && localIngredients.CountItems() > 0) {
                string foragedIngredient = localIngredients.GetRandomItem();

                playerIngredients.AddItem(foragedIngredient);
                localIngredients.RemoveItem(foragedIngredient);

                forageCount--;

                Debug.Log("You found: " + foragedIngredient);

                // TODO Kola should get a 50% chance to grab a bottle too (maybe nose multiplier?`)
            }
        } else {

            Debug.Log("No ingredients left to forage!");
            canAct = false;
        }
    }

    private void PickIngredient()
    {
        DisplayIngredients();
    }

    private bool CanAffordIngredient(string ingredient)
    {
        if (playerIngredients.CheckItem(ingredient)) {
            return true;
        }

        return false;
    }

    private void SpendIngredient(string ingredient)
    {
        playerIngredients.RemoveItem(ingredient);
    }

    private bool CheckIngredients()
    {
        Dictionary<string,int> requiredIngredients = CurrentRecipe.GetRequiredIngredients().GetInventoryAsDictionary();

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
                // print("right amount of this ingredient");
            }
        }

        return true;
    }



    // ACTION

    public void SetCurrentAction(Action action)
    {
        ResetCurrentAction();

        CurrentAction = action;
    }

    public void ResetCurrentAction()
    {
        CurrentAction = null;
    }

    public void TryAction(Action action)
    {
        if (CheckAfford(action)) {
            RouteAction(action);
        } else {
            Debug.Log("Cannot do Move");
        }
    }

    private void AutoMove()
    {
        // print("AutoMove");
        // GameObject canvas = CurrentUnit.GetCanvasObj();
        // GameObject canvas = GameObject.FindGameObjectsWithTag("HUD");


        // List<MoveButton> currentUnitMoves = CurrentUnit.GetMoves();

        if (CurrentUnit.GetMovesCount() > 0) {

            MoveButton tryMove = null;

            do {
                tryMove = CurrentUnit.GetRandomAction();

                tryMove.SetUpButtonAction();
            } while (!CheckAfford(tryMove.GetAction()));

            CurrentAction = tryMove.GetAction();

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
            
            SelectPossibleTargetsByActionType();

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

    public bool CheckAfford(Action action) {
        if (CanAffordGutsCost(action) && CanAffordResourceCost(action) && CanAffordMisc(action)) {
            return true;
        }

        return false;
    }

    public bool CanAffordGutsCost (Action action)
    {
        return (CurrentUnit.GetCurrentGuts() - action.GetGutsCost()) >= 0;
    }

    public bool CanAffordResourceCost (Action action)
    {
        if (action.GetIngredientCost() != "") {
            if (!playerIngredients.CheckItem(action.GetIngredientCost())) {
                Debug.Log("You do not have the required ingredient.");
                return false;
            }
        }

        // TODO: Make this work for requiring multiple of the same item
        if (action.ResourceCost != null) {
            // print ("resource cost is not null");

            foreach (Item resource in action.ResourceCost) {
                // print(action.TargetType);
                switch(action.TargetType) {
                    case "Cook":
                    case "Recipe":
                        if (!playerEquipment.CheckItem(resource.Name)) {
                            return false;
                        }
                        break;
                    case "Eat":
                    case "ServeDrink":
                    case "ServeFood":
                    case "ServeMeal":
                        if (!playerMealItems.Contains(resource)) {
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

    // todo redo all this with more accurate target types
    public void SpendResource (List<Item> resourceCost)
    {
        if (resourceCost != null) {
            foreach (Item resource in resourceCost) {
                switch(CurrentAction.TargetType) {
                    case "Cook":
                    case "Recipe":
                        playerEquipment.RemoveItem(resource.Name);
                        break;
                    case "Eat":
                    case "ServeDrink":
                    case "ServeFood":
                    case "ServeMeal":
                        playerMealItems.Remove(resource);
                        break;
                    default:
                        print("Invalid target type for action with resourceCost");
                        break;
                }
            }
        }

        // print ("we made it this far");
    }

    public bool CanAffordMisc(Action action)
    {
        if (action.checkIfCookin) {
            if (allyCookinList.Count == 0) {
                Debug.Log("You must be cookin when you use this move.");
                return false;
            }
        }

        return true;
    }


    // TODO maybe I should have a target type and a move type instead of having Stir alongside Ally and such
    public void RouteAction(Action action)
    {
        // Debug.Log(action.TargetType);
            // movePicked = true;
        SetCurrentAction(action);

        // TODO this is no good, find targets and set targets separately with conditionals
        SelectPossibleTargetsByActionType();

        switch (CurrentAction.TargetType) {
            case "SelfOrAlly":
            case "Ally":
            case "MeleeAlly":
            case "CoverAlly":
            case "Enemy":
            case "MeleeEnemy":
            case "MeleeEnemyPierce":
            case "CoveredEnemy": // TODO
            case "SelfOrAllyOrMeleeEnemy": // TODO THIS DOESN'T TRIGGER UNCOVER IN 
            case "Stir":
            case "Season":
            case "AddIngredient":
            case "Eat":
            case "ServeDrink":
            case "ServeFood":
            case "ServeMeal":
                // FindTargets();
                DisplayTargets();
                break;
            case "Self":
            case "AllMeleeEnemies":
            case "AllCoveredEnemies": // TODO
            case "AllEnemies":
            case "Targetless": // or "Untargeted"
            case "Recipe":
            case "Forage":
                TakeAction();
                break;
            case "Cook":
                PickRecipe();
                break;
            case "PickIngredient":
                PickIngredient();
                break;
            case "PickMealToEat":
                PickMealToEat();
                break;
            case "PickDrinkToServe":
                PickDrinkToServe();
                break;
            case "PickFoodToServe":
                PickFoodToServe();
                break;
            case "PickMealToServe":
                PickMealToServe();
                break;
            default:
                print("Invalid target");
                break;
        }
    }

    public void TakeAction()
    {
        canAct = true;
        // print(CurrentAction);

        if (CurrentAction.GutsCost != 0) {
            CurrentUnit.SpendGuts(CurrentAction.GutsCost);
        }

        // TODO: fix covering-chains, where you can cover a melee ally whose already covering someone
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
            Forage();
        }

        if (CurrentAction.TargetType == "Stir") {
            Stir();
        }

        if (CurrentAction.TargetType == "Season") {
            Season();
        }

        // new method to check ingredient cost
        if (CurrentAction.GetIngredientCost() != "") {
            if (CanAffordIngredient(CurrentAction.GetIngredientCost())) {
                SpendIngredient(CurrentAction.GetIngredientCost());
                if (CurrentAction.TargetType == "AddIngredient") {
                    CurrentRecipe.GetRequiredIngredients().RemoveItem(CurrentAction.GetIngredientCost());
                }
            } else {
                canAct = false;
            }
        }
        
        if (CurrentAction.TargetType == "Eat") {
            if (CurrentAction.ResourceCost.Count == 1) {
                SetCurrentMeal(CurrentAction.ResourceCost[0]);

                PrepMeal();

                Debug.Log("You ate: " + CurrentMeal.Name + "... Delicious!!");

                SpendResource(CurrentAction.ResourceCost);
            } else {
                Debug.Log("Error: TakeAction: You should only serve one meal at a time");
            }
        } else if (CurrentAction.TargetType == "ServeDrink" || CurrentAction.TargetType == "ServeFood" || CurrentAction.TargetType == "ServeMeal") {
            if (CurrentAction.ResourceCost.Count == 1) {
                SetCurrentMeal(CurrentAction.ResourceCost[0]);

                PrepMeal();

                Debug.Log("You served: " + CurrentMeal.Name + "... Gross!!");

                SpendResource(CurrentAction.ResourceCost);
            } else {
                Debug.Log("Error: TakeAction: You should only consume one meal at a time");
            }
        } else if (CurrentAction.ResourceCost.Count != 0) {
            SpendResource(CurrentAction.ResourceCost);
        }

        if (CurrentAction.isRecipe == true) {
            SetRecipe(CurrentAction.Recipe);
        }

        // TODO: i should reorganize the units automatically. So if there are a ton of units summoned, they get properly organized on the screen
        // i should have certain slots premade (enumerated?) and then it fills in the next available ally/enemy spot
        // then i can have a certain number of ally spots available as you level
        // and maybe a certain number of cauldron spots, upgradeable
        if (CurrentAction.isSummon == true) {
            SummonUnit();

        }

        // print(CurrentAction.GiveItem);

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
        if (CurrentAction.Damage > 0) {
            // Debug.Log(unit.name + " receives " + CurrentAction.Damage + " raw damage");

            // TODO THIS DOESN'T TRIGGER UNCOVER FOR "SelfOrAllyOrMeleeEnemy"
            switch (CurrentAction.TargetType) {
                case "MeleeEnemy":
                case "MeleeEnemyPierce":
                case "AllMeleeEnemies":
                    UncoverUnit(CurrentUnit);
                    break;
                default:
                    break;
            }

            TakeStrengthDamage(unit, CurrentAction.Damage);
        }

        if (CurrentAction.TargetType == "MeleeEnemyPierce") {
            if (coverUnits.ContainsKey(unit.name)) {
                Debug.Log(coverUnits[unit.name].name + " receives " + CurrentAction.Damage/2 + " raw pierce damage");
                
                TakeStrengthDamage(GetUnitByName(coverUnits[unit.name].name), CurrentAction.Damage/2);
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

        if (CurrentAction.Tantalize > 0) {
            unit.TantalizeHunger(CurrentAction.Tantalize);
        }

        // TODO: i should just use the status effect instead of having a local shield stat
        // unit.ApplySingleEffectStatus(CurrentAction.GetAllStatusEffects());

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



    // COVERING

    private void CoverUnit(UnitStats unit)
    {
        UncoverUnit(CurrentUnit);
        UncoverUnit(unit);

        coverUnits.Add(CurrentUnit.name, unit);

        if (myTurn) {
            RemoveUnitFromList(unit, ref meleeAllyList);
        } else {
            RemoveUnitFromList(unit, ref meleeEnemyList);
        }
    }

    private bool IsCovered(UnitStats unit)
    {
        if (coverUnits.ContainsValue(unit)){
            return true;
        }

        return false;
    }

    private bool IsCovering(UnitStats unit)
    {
        if (coverUnits.ContainsKey(unit.name)){
            return true;
        }

        return false;
    }

    private void UncoverUnit(UnitStats unit)
    {

        if (IsCovering(unit)) {
            if (!unit.isDead) {
                if (myTurn) {
                    AddUnitToList(coverUnits[unit.name], ref meleeAllyList);
                } else {
                    AddUnitToList(coverUnits[unit.name], ref meleeEnemyList);
                }
            }

            coverUnits.Remove(unit.name);
        }

        if (IsCovered(unit)) {
            if (!unit.isDead) {
                if (myTurn) {   
                    AddUnitToList(unit, ref meleeAllyList);
                } else {
                    AddUnitToList(unit, ref meleeEnemyList);
                }
            }

            var entryToRemove = coverUnits.First(kvp => kvp.Value == unit);
            coverUnits.Remove(entryToRemove.Key);
        }
    }



    // SUMMONING

    private void SummonUnit()
    {
        GameObject summon = Instantiate(CurrentAction.Summon, new Vector3(-6, -3, 0), Quaternion.identity); // hardcoded location, should make dynamic
        UnitStats summonUnit = summon.GetComponent<UnitStats>();

        completeList.Add(summonUnit);

        if (CurrentUnit.tag == "Player") {
        // if (summon.tag == "Player" || summon.tag == "NPC") {
            allyList.Add(summonUnit);
            meleeAllyList.Add(summonUnit);
        } else if (CurrentUnit.tag == "Enemy") {
            enemyList.Add(summonUnit);
            meleeEnemyList.Add(summonUnit);
        } else {
            print("who is this?");
        }

        // this is where i check if its a recipe action, but i should move this higher in the work flow
        if (summonUnit.isCookin) {
            AddUnitToList(summonUnit, ref allyCookinList);
            // will this work by ref?
            // SetRecipe();

            // print("recipe");
            // print(CurrentRecipe);
            // print("summon");
            // print(summon);

            CurrentRecipe.cauldron = summonUnit;
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

    private void RemoveSelfFromPossibleTargets()
    {
        CurrentAction.RemovePossibleTarget(ref CurrentUnit);
    }

    private void RemoveUnitFromPossibleTargets(UnitStats unit)
    {
        CurrentAction.RemovePossibleTarget(ref unit);
    }

    private void FindTargetsByList(List<UnitStats> unitList)
    {
        CurrentAction.AddPossibleTargets(ref unitList);
    }

    private void SetTargetsByList(List<UnitStats> unitList)
    {
        CurrentAction.AddTargets(ref unitList);
    }

    //TODO this should all be find targets, not set targets
    // should make another method for that and a conditional in RouteAction
    public void SelectPossibleTargetsByActionType()
    {
        // TODO use FindTargetsByList instead of FindTargetsByTag when possible
        switch (CurrentAction.TargetType) 
        {
            case "Self":
                SetTargetToSelf();
                break;
            case "SelfOrAlly":
            case "Eat":
                if (myTurn) {
                    FindTargetsByList(allyList);
                } else {
                    FindTargetsByList(enemyList);

                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "MeleeAlly":
                if (myTurn) {
                    FindTargetsByList(meleeAllyList);
                    RemoveSelfFromPossibleTargets();

                } else {
                    FindTargetsByList(meleeEnemyList);
                    RemoveSelfFromPossibleTargets();

                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "Ally":
                if (myTurn) {
                    FindTargetsByList(allyList);
                    RemoveSelfFromPossibleTargets();
                } else {
                    FindTargetsByList(enemyList);
                    RemoveSelfFromPossibleTargets();

                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "CoverAlly":
                if (myTurn) {
                    FindTargetsByList(allyList);
                    RemoveSelfFromPossibleTargets();

                    if (IsCovering(CurrentUnit)) {
                        RemoveUnitFromPossibleTargets(coverUnits[CurrentUnit.name]);
                    }
                } else {
                    FindTargetsByList(enemyList);
                    RemoveSelfFromPossibleTargets();

                    if (IsCovering(CurrentUnit)) {
                        RemoveUnitFromPossibleTargets(coverUnits[CurrentUnit.name]);
                    }

                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "MeleeEnemy":
            case "MeleeEnemyPierce":
            case "ServeFood":
            case "ServeMeal":
                if (myTurn) {
                    FindTargetsByList(meleeEnemyList);
                } else {
                    FindTargetsByList(meleeAllyList);
                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "Enemy":
            case "ServeDrink":
                if (myTurn) {
                    FindTargetsByList(enemyList);
                } else {
                    FindTargetsByList(allyList);

                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "SelfOrAllyOrMeleeEnemy":
                if (myTurn) {
                    FindTargetsByList(allyList);
                    FindTargetsByList(meleeEnemyList);
                } else {
                    FindTargetsByList(enemyList);
                    FindTargetsByList(meleeAllyList);

                    SelectRandomUnitFromPossibleTargets();
                }
                break;
            case "AllMeleeEnemies":
                if (myTurn) {
                    SetTargetsByList(meleeEnemyList);
                } else {
                    SetTargetsByList(meleeAllyList);
                }
                break;
            case "AllEnemies":
                if (myTurn) {
                    SetTargetsByList(enemyList);
                } else {
                    SetTargetsByList(allyList);
                }
                break;
            case "Stir":
            case "Season":
            case "AddIngredient":
                if (myTurn) {
                    FindTargetsByList(allyCookinList);
                } else {
                    Debug.Log("ERROR: BM.SelectPossibleTargetsByActionType : Enemies can't interact with cookin");
                }
                break;
            case "Targetless":
            case "Forage":
            case "Cook":
            case "Recipe":
            case "PickIngredient":
            case "PickMealToEat":
            case "PickDrinkToServe":
            case "PickFoodToServe":
            case "PickMealToServe":
                break;
            default:
                print("Invalid target");
                break;
        }
    }



    // ATTACK CALCULATOR

    private void TakeStrengthDamage(UnitStats targetUnit, int rawDamage)
    {
        // should i be adding the stat changes in here? should the params determine the attacking unit and def unit?

        int attackDamage = rawDamage;
        int attackSpeed = CurrentAction.Speed;

        int multiplier = 1;
        int netDamage = 0;

        // basic attacks can hit multiple times if your speed is higher than targets
        if (CurrentAction.isBasicAttack && attackSpeed > targetUnit.GetNetSpeed()) {
            int speedDiff = attackSpeed - targetUnit.GetNetSpeed();

            if (speedDiff >= 20) {
                multiplier = 3;
            } else if (speedDiff >= 10) {
                multiplier = 2;
            }
        }

        if (CurrentAction.isBasicAttack && CurrentUnit.GetStatusEffectStatus("BROKENBOTTLE")) {
            attackDamage += 5;
            CurrentUnit.SubtractStatusEffect("BROKENBOTTLE", 1);
        }

        // damage is reduced by def if target has DEFEND or by def/2 if no DEFEND
        if (targetUnit.GetStatusEffectStatus("DEFEND")) {
            netDamage = attackDamage - targetUnit.GetNetDefense();
            targetUnit.SubtractStatusEffect("DEFEND", 1); // or you could lose one at EoT
        } else  {
            netDamage = attackDamage - (targetUnit.GetNetDefense() / 2);
        }

        // do 1 damage per hit minimum
        if (netDamage <= 0) {
            netDamage = 1;
        }

        do {
            if (!targetUnit.GetStatusEffectStatus("DODGE")) {
                if (targetUnit.shield > 0) {
                    targetUnit.shield -= netDamage;

                    if (targetUnit.shield < 0) {
                        netDamage = -targetUnit.shield;
                        targetUnit.shield = 0;
                    } else {
                        netDamage = 0;
                    }
                }

                targetUnit.LoseHealth(netDamage);
            } else {
                targetUnit.SubtractStatusEffect("DODGE", 1);
            }

            multiplier--;
        } while (multiplier > 0);

        CheckIfDead(targetUnit);
    }

    private void TakeTumDamage (UnitStats targetUnit, int rawDamage)
    {
        int netDamage = rawDamage - (targetUnit.GetNetTum() / 2);

        // do 1 damage minimum
        if (netDamage <= 0) {
            netDamage = 1;
        }

        targetUnit.LoseHealth(netDamage);
        

        CheckIfDead(targetUnit);
    }

    private void TakeTrueDamage (UnitStats targetUnit, int rawDamage)
    {
        int netDamage = rawDamage;

        targetUnit.LoseHealth(netDamage);

        CheckIfDead(targetUnit);
    }

    // TODO: there are many references to this, but it should only be used when a unit takes damage/ loses hp
    private void CheckIfDead(UnitStats unit)
    {
        if (unit.GetCurrentHealth() <= 0) {
            unit.SetCurrentHealth(0);
            unit.isDead = true;
            unit.turnNumber = -1;
            unit.SetTurnText("Dead");

            UncoverUnit(unit);
        }
    }

    private void CheckGross(UnitStats unit)
    {
        if (unit.GetStatusEffectStatus("GROSS")) {
            GrossTicker(unit);
        }
    }

    private void GrossTicker(UnitStats unit)
    {
        TakeTumDamage(unit, unit.GetStatusEffectStacks("GROSS"));

        unit.SubtractStatusEffect("GROSS", 1);
    }



    // UI

    private void DisplayMoves()
    {

        // Debug.Log("fdasdf");
        RemoveAllButtons();

        // GameObject canvas = CurrentUnit.GetCanvasObj();
        // GameObject canvas = GameObject.FindGameObjectsWithTag("HUD");

        // Iterate this value to make position lower
        float yPos = 50f;


        if (CurrentUnit.GetMoves().Count > 0) {
            foreach (MoveButton button in CurrentUnit.GetMoves()) {
                MoveButton instantButton = Instantiate(button, new Vector3(0, yPos, 0), Quaternion.identity);

                // Debug.Log(button.GetGutsCost());

                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                // instantButton.transform.position = new Vector3(20,5,0);
                instantButton.transform.SetParent(HUD.transform, false);
                // second param keeps scale etc the same

                button.SetUpButtonAction();

                string moveText = button.GetName() + " : " + button.GetAction().GetGutsCost().ToString();

                instantButton.GetComponentInChildren<Text>().text = moveText;



                yPos -= 30f;
            }
        } else {
            print ("This unit has no moves");
            EndTurn();
        }
    }

    private void DisplayRecipes()
    {
        RemoveAllButtons();

        // Iterate this value to make position lower
        float yPos = 0f;


        if (playerRecipes.Count > 0) {
            foreach (Recipe recipe in playerRecipes) {
                // Debug.Log(recipe.GetName());
                // Debug.Log(recipe);

                GameObject instantButton = Instantiate(recipeButton, new Vector3(0, yPos, 0), Quaternion.identity);

                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                // instantButton.transform.position = new Vector3(20,5,0);
                instantButton.transform.SetParent(HUD.transform, false);
                // second param keeps scale etc the same
                // Debug.Log(recipe);

                instantButton.GetComponentInChildren<Text>().text = recipe.GetName();

                PickRecipe buttonMove = instantButton.GetComponent<PickRecipe>();

                // Debug.Log(recipe);
                // PickRecipe tempRecipe = new PickRecipe();
                // Debug.Log(tempRecipe);

                // tempRecipe = recipe;
                // Debug.Log(tempRecipe);

                // Debug.Log(buttonMove);

                // Debug.Log(recipe);
                buttonMove.SetRecipe(recipe);
                // Debug.Log("done");



                // PickRecipe buttonMove = instantButton.GetComponent<PickRecipe>(); //.SetRecipe(recipe);
                // tempMove.SetRecipe(recipe);

                yPos -= 30f;
            }
        } else {
            print ("You have no recipes");
            
            ResetTurn();
        }
    }

    private void DisplayIngredients()
    {
        RemoveAllButtons();

        // Iterate this value to make position lower
        float yPos = 0f;


        if (playerIngredients.CountItems() > 0) {
            foreach (KeyValuePair<string,int> ingredient in playerIngredients.GetInventoryAsDictionary()) {
                GameObject instantButton = Instantiate(ingredientButton, new Vector3(0, yPos, 0), Quaternion.identity);

                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                // instantButton.transform.position = new Vector3(20,5,0);
                instantButton.transform.SetParent(HUD.transform, false);
                // second param keeps scale etc the same

                instantButton.GetComponentInChildren<Text>().text = ingredient.Key;

                AddIngredient buttonMove = instantButton.GetComponent<AddIngredient>();

                buttonMove.SetIngredient(ingredient.Key);

                yPos -= 30f;
            }
        } else {
            print ("You have no ingredients");
            
            ResetTurn();
        }
    }

    private void DisplayMealsToEat()
    {
        RemoveAllButtons();

        // Iterate this value to make position lower
        float yPos = 0f;
        bool atLeastOneTreat = false;


        if (playerMealItems.Count > 0) {
            foreach (Item meal in playerMealItems) {
                if (meal.Meal && meal.Treat) {
                    atLeastOneTreat = true;

                    GameObject instantButton = Instantiate(eatButton, new Vector3(0, yPos, 0), Quaternion.identity);

                    instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                    // instantButton.transform.position = new Vector3(20,5,0);
                    instantButton.transform.SetParent(HUD.transform, false);
                    // second param keeps scale etc the same

                    instantButton.GetComponentInChildren<Text>().text = meal.Name;

                    Eat buttonMove = instantButton.GetComponent<Eat>();

                    buttonMove.AddResourceCost(meal);

                    yPos -= 30f;
                }
            }
        }

        if (!atLeastOneTreat) {
            print ("You have no good treats to eat.");
            
            ResetTurn();
        }
    }

    private void DisplayMealsToServe(string mealType)
    {
        RemoveAllButtons();

        // Iterate this value to make position lower
        float yPos = 0f;
        bool atLeastOneTrick = false;


        if (playerMealItems.Count > 0) {
            foreach (Item meal in playerMealItems) {
                switch(mealType) {
                    case "Drink":
                        if (meal.Meal && meal.Trick && meal.Drink) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveDrinkButton, new Vector3(0, yPos, 0), Quaternion.identity);

                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            // instantButton.transform.position = new Vector3(20,5,0);
                            instantButton.transform.SetParent(HUD.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeDrink buttonMove = instantButton.GetComponent<ServeDrink>();

                            buttonMove.AddResourceCost(meal);

                            yPos -= 30f;
                        }
                        break;
                    case "Food":
                        if (meal.Meal && meal.Trick && meal.Food) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveFoodButton, new Vector3(0, yPos, 0), Quaternion.identity);

                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            // instantButton.transform.position = new Vector3(20,5,0);
                            instantButton.transform.SetParent(HUD.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeFood buttonMove = instantButton.GetComponent<ServeFood>();

                            buttonMove.AddResourceCost(meal);

                            yPos -= 30f;
                        }
                        break;
                    case "Meal":
                        if (meal.Meal && meal.Trick) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveMealButton, new Vector3(0, yPos, 0), Quaternion.identity);

                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            // instantButton.transform.position = new Vector3(20,5,0);
                            instantButton.transform.SetParent(HUD.transform, false);
                            // second param keeps scale etc the same

                            // instantButton.GetComponentInChildren<Text>().text = meal.Name;
                            instantButton.GetComponentInChildren<Text>().text = meal.Name;


                            ServeMeal buttonMove = instantButton.GetComponent<ServeMeal>();

                            buttonMove.AddResourceCost(meal);

                            yPos -= 30f;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        if (!atLeastOneTrick) {
            print ("You have no good tricks to serve.");
            
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

                // TODO find better way than setting parents
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
}
