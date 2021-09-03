
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
        public GameObject cookRecipeButton;
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
        public UnitStats    CurrentUnit;
        public Action       CurrentAction;
        public Recipe       CurrentRecipe;
        public Item         CurrentMeal;

    // INVENTORIES AND ITEMS
        // public Inventory playerEquipment;
        public Inventory playerIngredients;
        public Inventory playerFood;
        public Inventory localIngredients;
        private List<Recipe> playerRecipes;
        private List<Item> playerMealItems;
        private List<Item> playerEquipmentItems;


    // CONSTANTS: STATS
        private const string STRENGTH   = "STRENGTH";
        private const string DEFENSE    = "DEFENSE";
        private const string SPEED      = "SPEED";
        private const string TUM        = "TUM";
        private const string TASTE      = "TASTE";
        private const string NOSE       = "NOSE";


    // CONSTANTS: STATUSES
        private const string ARMOR      = "ARMOR";
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
            // playerEquipment     = new Inventory();
            playerIngredients   = new Inventory();
            playerFood          = new Inventory();
            localIngredients    = new Inventory();

            playerMealItems     = new List<Item>();
            playerEquipmentItems= new List<Item>();

            // start with this equipment
            // playerEquipment.AddItem("Cauldron");

            // start with this equipment (refactored equip)
            Pot startingPot = new Pot();
            playerEquipmentItems.Add(startingPot);

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

            PopulateListByTag("Player", completeList);
            PopulateListByTag("Enemy",  completeList);

            PopulateListByTag("Player", allyList);
            PopulateListByTag("Enemy",  enemyList);
            PopulateListByTag("Player", meleeAllyList);
            PopulateListByTag("Enemy",  meleeEnemyList);
        }

        private void InitializeRecipes()
        {
            playerRecipes = new List<Recipe>();
            playerRecipes.Add(new CornCobRecipe());
            playerRecipes.Add(new GritsRecipe());
            playerRecipes.Add(new PopCornRecipe());
        }


    // LISTS

        private void PopulateListByTag(string tag, List<UnitStats> list)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objects) {
                list.Add(obj.GetComponent<UnitStats>());
            }
        }

        private void AddUnitToList(UnitStats unit, List<UnitStats> list)
        {
            if (unit == null) {
                Debug.Log("Error : AddUnitToList : Can't add null unit");
            }
            list.Add(unit);
        }

        private void RemoveUnitFromList(UnitStats unit, List<UnitStats> list)
        {
            list.Remove(unit);
        }

        private void AddUnitListToList(List<UnitStats> listToAdd, List<UnitStats> list)
        {
            // if (listToAdd != null) {
            //     Debug.Log("Error : AddUnitListToList : Can't add null list");
            // }

            foreach (UnitStats unit in listToAdd) {
                AddUnitToList(unit, list);
                // list.Add(unit);
            }
        }

        private void PrintList(List<UnitStats> list)
        {
            // Debug.Log(list);
            // Debug.Log(list.Count);
            foreach (UnitStats unit in list) {
                if (unit == null) {
                    Debug.Log("Error : PrintList : Can't print null unit");
                }
                print(unit.GetName());
            }
        }


    // TURN MANAGEMENT

        public void StartCombat()
        {
            StartRound();
        }

        private void StartRound()
        {
            Debug.Log("Start Round");

            BuildTurnList();

            StartRoundUpdates();

            NextTurn();
        }

        private void StartRoundUpdates()
        {
            // Debug.Log("StartRoundUpdates");
            GutCheck();

            // foreach (UnitStats unit in completeList) {
            //     unit.UpdateText();
            //     CheckIfDead(unit);
            // }

            // HandleDeaths();
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

            // CurrentUnit.UpdateText();
            // CheckIfDead(CurrentUnit);
            // HandleDeaths();
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


            CurrentUnit.SetTurnNumber(0);
            CurrentUnit.ChangeTurnTextColor(Color.gray);

            SetUnitTurnNumbers();
            ResetCurrentAction();

            // UI UPDATES
            RemoveAllButtons();

            // foreach (UnitStats unit in completeList) {
            //     unit.UpdateText();
            //     CheckIfDead(unit);
            // }

            // HandleDeaths();
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
            // foreach (KeyValuePair<string,UnitStats> coverLink in coverUnits) {
            //     Debug.Log("Unit " + coverLink.Key + " is covering " + coverLink.Value.name);
            // }

            EndRoundUpdates();
            // Debug.Log("Round Over");

            StartRound();
        }

        private void EndRoundUpdates()
        {
            CheckRecipe();

            Metabolism();

            // foreach (UnitStats unit in completeList) {
            //     unit.UpdateText();
            //     CheckIfDead(unit);
            // }

            // HandleDeaths();
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

        /*
            private void BuildTurnList()
            {
                // List<UnitStats> tempUnitList = new List<UnitStats>();
                nextUpList = completeList.OrderBy(w => w.GetNetSpeed()).Reverse().ToList();
                // tempUnitList = nextUpList;

                // PrintList(nextUpList);

                // int a = 0;
                // int b = 0;

                    // for (int i = 0; i < nextUpList.Count - 1; i++)
                    // for (int i = 0; i < 3; i++)
                    // int i = 0;
                    // while (i < nextUpList.Count - 1)
                    // {
                        // Debug.Log(i);
                        // if (nextUpList[i].GetNetSpeed() == nextUpList[i+1].GetNetSpeed()) {
                        //     // Debug.Log("Ding");

                        //     List<UnitStats> listToRandomize = new List<UnitStats>();
                        //     // AddUnitToList(nextUpList[i],  listToRandomize);

                        //     int j = 0;
                        //     int k = i;
                        //     bool breakOut = false;
                        //     // do {
                        //     //         breakOut = true;
                        //     //         AddUnitToList(nextUpList[i],  tempUnitList);

                        //     //     // AddUnitToList(nextUpList[i + j],  listToRandomize);
                        //     //     // j++;
                        //     //     // k++;
                        //     //     // if (nextUpList[i + j].GetNetSpeed() != nextUpList[i + j + 1].GetNetSpeed()) {
                        //     //     //     breakOut = true;
                        //     //     // }
                        //     // // }   while (j < 3);

                        //     // }   while (k < nextUpList.Count - 1 && !breakOut);
                        //     // while (nextUpList[i + j].GetNetSpeed() == nextUpList[i + j + 1].GetNetSpeed()) {
                        //     //     AddUnitToList(nextUpList[i + j + 1],  listToRandomize);
                        //     //     j++;
                        //     //     k++;
                        //     // }
                        //     i += j;
                        //     Shuffle(listToRandomize);
                        //     PrintList(listToRandomize);

                        //     // int randomNumber = Random.Range(0, 2);

                        //     // if (randomNumber == 1) {
                        //     //     // Debug.Log(a++);
                        //     //     a++;

                        //     //     UnitStats tempUnit = tempUnitList[i];
                        //     //     tempUnitList[i] = tempUnitList[i+1];
                        //     //     tempUnitList[i+1] = tempUnit;
                        //     // }
                        //     // b++;

                        //     // Debug.Log(b++);
                        // } else {
                        //     // AddUnitToList(nextUpList[i],  tempUnitList);
                        //     i++;
                        // }
                    // }
                // Debug.Log(a + " : " + b);
                // PrintList(tempUnitList);

                SetUnitTurnNumbers();

                // TODO: MAKE SORTING SLIGHTLY RANDOM, BUT HIGHER CHANCE IF HIGH STAT?
            }
        */

        // private void BuildTurnList()
        // {
        //     nextUpList = completeList.OrderBy(w => w.GetNetSpeed()).Reverse().ToList();

        //     SetUnitTurnNumbers();

        //     // if (nextUpList[2] != null) {
        //     //     Debug.Log(nextUpList[2]);
        //     // }

        //     List<UnitStats> listToRandomize = new List<UnitStats>();
        //     bool breakOut = false;
        //                 // AddUnitToList(nextUpList[i],  listToRandomize);
        //     int i = 0;
        //     Debug.Log(nextUpList.Count - 1);
        //     while (i < nextUpList.Count - 1) {
        //         // while (!breakOut) {
        //             if (i+1 < nextUpList.Count) {
        //                 Debug.Log("Ding");
        //                 if (nextUpList[i].GetNetSpeed() == nextUpList[i+1].GetNetSpeed()) {
        //                     AddUnitToList(nextUpList[i],  listToRandomize);
        //                     AddUnitToList(nextUpList[i+1],  listToRandomize);
        //                 } else {
        //                     breakOut = true;
        //                 }
        //             }
        //             breakOut = true;
        //             i++;


        //         // if (nextUpList[i].GetNetSpeed() == nextUpList[i+1].GetNetSpeed()) {
        //         //         AddUnitToList(nextUpList[i],  listToRandomize);
        //         //         AddUnitToList(nextUpList[i+1],  listToRandomize);
        //         // }
        //         print(i);
        //     }

        //     Shuffle(listToRandomize);
        //     PrintList(listToRandomize);


        //     // TODO: MAKE SORTING SLIGHTLY RANDOM, BUT HIGHER CHANCE IF HIGH STAT?
        // }

        private void BuildTurnList()
        {
            List<UnitStats> sortedList  = new List<UnitStats>();

            sortedList = completeList.OrderBy(w => w.GetNetSpeed()).Reverse().ToList();

            // handling for units with same speed, grab them and randomize their order
            int i = 0;
            UnitStats tempUnit = null;
            List<UnitStats> listToRandomizeAndAdd = new List<UnitStats>();

            while ( i < sortedList.Count) {
                if (tempUnit != null && tempUnit.GetNetSpeed() == sortedList[i].GetNetSpeed()) {
                    AddUnitToList(sortedList[i], listToRandomizeAndAdd);
                } else {

                    // if you dont match and the list isn't null, you must shuffle and add
                    if (listToRandomizeAndAdd.Count > 0) {
                        Shuffle(listToRandomizeAndAdd);
                        AddUnitListToList(listToRandomizeAndAdd, nextUpList);
                    }

                    listToRandomizeAndAdd = new List<UnitStats>();

                    tempUnit = sortedList[i];
                    AddUnitToList(tempUnit, listToRandomizeAndAdd);
                }
                i++;
            }

            Shuffle(listToRandomizeAndAdd);
            AddUnitListToList(listToRandomizeAndAdd, nextUpList);

            SetUnitTurnNumbers();
        }

        // private void BuildTurnList()
        // {
        //     nextUpList = completeList.OrderBy(w => w.GetNetSpeed()).Reverse().ToList();

        //     SetUnitTurnNumbers();
        // }

        private void SetUnitTurnNumbers()
        {
            int iter = 1;

            foreach (UnitStats unit in nextUpList)
            {
                // Debug.Log(unit);
                unit.SetTurnNumber(iter);
                // unit.ChangeTurnTextColor(Color.blue);
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

        private void Meal()
        {
            if (CurrentAction.ResourceCost.Count == 1) {
                SetCurrentMeal(CurrentAction.ResourceCost[0]);

                PrepMeal();

                // Debug.Log("You served: " + CurrentMeal.Name + "... Gross!!");

                // SpendResource(CurrentAction.ResourceCost);
            } else {
                Debug.Log("Error: TakeAction: You should only consume one meal at a time");
            }
        }

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

            int netDamage   = CurrentMeal.ItemAction().TumDamage;
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

            CurrentAction.TumDamage = netDamage;
            CurrentAction.Heal      = netHeal;
            CurrentAction.Sate      = netSate;

            // Should Quality add Status Effect stacks?
            CurrentAction.StatusEffects = CurrentMeal.ItemAction().GetAllStatusEffects();
        }


    // COOKING MANAGEMENT
        private void PickRecipe()
        {
            print('a');
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

        private void CookRecipe()
        {
            SetRecipe(CurrentAction.Recipe);
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
            // print(CurrentRecipe.GetFlavor());

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
            // playerEquipment.AddItem("Cauldron");

            Pot startingPot = new Pot();
            playerEquipmentItems.Add(startingPot);
        }


    // HUNGER MANAGEMENT

        private void Metabolism()
        {
            foreach (UnitStats unit in completeList) {
                if (!unit.isCookin) { // && !unit.isDead) {
                    unit.LoseGuts(1);
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
            canAct = true;
            SetCurrentAction(action);

            if (CheckAfford(action)) {
                // PickTargets();

                RouteAction();
            } else {
                ResetTurn();
                Debug.Log("Cannot do Move");
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
                    switch(action.GetSkillType()) {
                        // case "PickRecipe":
                        case "CookRecipe":
                            if (!playerEquipmentItems.Exists(x => x.Name == resource.Name)) {
                                return false;
                            }
                            break;
                        case "Serve":
                        case "Eat":
                        case "ServeDrink":
                        case "ServeFood":
                        case "ServeMeal":
                            // if (!playerMealItems.Contains(resource)) {
                            if (!playerMealItems.Exists(x => x.Name == resource.Name)) {
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
                    switch(CurrentAction.GetSkillType()) {
                        case "CookRecipe":
                            // playerEquipment.RemoveItem(resource.Name);
                            playerEquipmentItems.Remove(new Pot());
                            break;
                        case "Serve":
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

        public void SpendGuts ()
        {
            if (CurrentAction.GutsCost > 0) {
                CurrentUnit.LoseGuts(CurrentAction.GutsCost);
            }
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

        public void RouteAction()
        {
            SelectPossibleTargetsByActionType();

            switch (CurrentAction.TargetType) {
                case "SelfOrAlly":
                case "Ally":
                case "MeleeAlly":
                case "CoverAlly":
                case "Enemy":
                case "MeleeEnemy":
                case "MeleeEnemyPierce":
                // case "CoveredEnemy": // TODO
                case "SelfOrAllyOrMeleeEnemy": // TODO THIS DOESN'T TRIGGER UNCOVER IN
                case "AllyPot":
                // case "Stir":
                // case "Season":
                // case "AddIngredient":
                // case "Eat":
                // case "ServeDrink":
                // case "ServeFood":
                // case "ServeMeal":
                    // FindTargets();
                    DisplayTargets();
                    break;
                case "Self":
                case "AllMeleeEnemies":
                case "AllCoveredEnemies": // TODO
                case "AllEnemies":
                case "Targetless": // or "Untargeted"
                    TakeAction();
                    break;
                case "Pick":
                    RouteSkill();
                    break;
                default:
                    print("Invalid target");
                    break;
            }
        }

        // // TODO maybe I should have a target type and a move type instead of having Stir alongside Ally and such
        // public void RouteAction(Action action)
        // {
        //     // Debug.Log(action.TargetType);
        //         // movePicked = true;
        //     SetCurrentAction(action);

        //     // TODO this is no good, find targets and set targets separately with conditionals
        //     SelectPossibleTargetsByActionType();

        //     switch (CurrentAction.TargetType) {
        //         case "SelfOrAlly":
        //         case "Ally":
        //         case "MeleeAlly":
        //         case "CoverAlly":
        //         case "Enemy":
        //         case "MeleeEnemy":
        //         case "MeleeEnemyPierce":
        //         case "CoveredEnemy": // TODO
        //         case "SelfOrAllyOrMeleeEnemy": // TODO THIS DOESN'T TRIGGER UNCOVER IN
        //         case "Stir":
        //         case "Season":
        //         case "AddIngredient":
        //         case "Eat":
        //         case "ServeDrink":
        //         case "ServeFood":
        //         case "ServeMeal":
        //             // FindTargets();
        //             DisplayTargets();
        //             break;
        //         case "Self":
        //         case "AllMeleeEnemies":
        //         case "AllCoveredEnemies": // TODO
        //         case "AllEnemies":
        //         case "Targetless": // or "Untargeted"
        //         case "Recipe":
        //         case "Forage":
        //             TakeAction();
        //             break;
        //         case "Cook":
        //             PickRecipe();
        //             break;
        //         case "PickIngredient":
        //             PickIngredient();
        //             break;
        //         case "PickMealToEat":
        //             PickMealToEat();
        //             break;
        //         case "PickDrinkToServe":
        //             PickDrinkToServe();
        //             break;
        //         case "PickFoodToServe":
        //             PickFoodToServe();
        //             break;
        //         case "PickMealToServe":
        //             PickMealToServe();
        //             break;
        //         default:
        //             print("Invalid target");
        //             break;
        //     }
        // }

        public void TakeAction()
        {
            if (CurrentAction.GetSkillType() != null) {
                RouteSkill();
            }

            // canAct = true;
            // print(CurrentAction);

            // SPEND COSTS
            SpendGuts();

            // new method to check ingredient cost, cuz we should check the cost earlier
            if (CurrentAction.GetIngredientCost() != "") {
                if (CanAffordIngredient(CurrentAction.GetIngredientCost())) {
                    SpendIngredient(CurrentAction.GetIngredientCost());
                    if (CurrentAction.GetSkillType() == "AddIngredient") {
                        CurrentRecipe.GetRequiredIngredients().RemoveItem(CurrentAction.GetIngredientCost());
                    }
                } else {
                    canAct = false;
                }
            }

            if (CurrentAction.ResourceCost.Count != 0) {
                SpendResource(CurrentAction.ResourceCost);
            }

            // PERFORM SKILLS

            // TODO: fix covering-chains, where you can cover a melee ally whose already covering someone
            // if (CurrentAction.GetSkillType() == "Cover") {
            //     // if (CurrentAction.Targets.Count == 1) {
            //     //     foreach (UnitStats target in CurrentAction.Targets) {
            //     //         CoverUnit(target);
            //     //     }
            //     // } else {
            //     //     Debug.Log("Can only cover one ally!");
            //     // }
            // }

            // if (CurrentAction.GetSkillType() == "Forage") {
            //     Forage();
            // }

            // if (CurrentAction.TargetType == "Stir") {
            //     Stir();
            // }

            // if (CurrentAction.TargetType == "Season") {
            //     Season();
            // }

            // if (CurrentAction.isRecipe == true) {
            //     SetRecipe(CurrentAction.Recipe);
            // }

            // TODO: i should reorganize the units automatically. So if there are a ton of units summoned, they get properly organized on the screen
            // i should have certain slots premade (enumerated?) and then it fills in the next available ally/enemy spot
            // then i can have a certain number of ally spots available as you level
            // and maybe a certain number of cauldron spots, upgradeable
            if (CurrentAction.isSummon == true) {
                SummonUnit();
            }

            if (canAct) {
                PerformActions();

                // if ( CurrentAction.Targets.Count > 0) {
                //     foreach (UnitStats target in CurrentAction.Targets) {
                //         PerformTargetedAction(target);
                //     }
                // }

                // if (CurrentAction.GetSelfAction() != null) {
                //     PerformSelfAction();
                // }

                // EndTurn();
            } else {
                canAct = true;
                ResetTurn();
            }
        }

        private void PerformActions()
        {
            if ( CurrentAction.Targets.Count > 0) {
                foreach (UnitStats target in CurrentAction.Targets) {
                    PerformTargetedAction(target);
                }
            }

            if (CurrentAction.GetSelfAction() != null) {
                PerformSelfAction();
            }

            EndTurn();
        }

        public void PerformTargetedAction(UnitStats unit)
        {
            if (CurrentAction.StrengthDamage > 0) {
                // Debug.Log(unit.name + " receives " + CurrentAction.StrengthDamage + " raw damage");

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

                TakeStrengthDamage(unit, CurrentAction.StrengthDamage);
            }

            if (CurrentAction.TargetType == "MeleeEnemyPierce") {
                if (coverUnits.ContainsKey(unit.name)) {
                    Debug.Log(coverUnits[unit.name].name + " receives " + CurrentAction.StrengthDamage/2 + " raw pierce damage");

                    TakeStrengthDamage(GetUnitByName(coverUnits[unit.name].name), CurrentAction.StrengthDamage/2);
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

            foreach(KeyValuePair<string, int> effect in CurrentAction.GetAllStatusEffects())
            {
                unit.AddStatusEffect(effect.Key, effect.Value);
            }

            foreach(KeyValuePair<string, int> effect in CurrentAction.GetAllStatChanges())
            {
                unit.AddStatChange(effect.Key, effect.Value);
            }

            // do this better
            unit.UpdateText();

            if (CurrentAction.DestroySelf) {
                unit.DestroySelf();

                // TODO: must clear from unit lists etc (might have solved this now, will keep testing)
                // on this note, will i have revival? should i even keep dead units around?

                // if not, i should have a more effective way of clearing units, since i might want to move them around,
                // or have them transform, or get "used up" like the cauldron
            }
        }

        public void PerformSelfAction()
        {
            Action selfAction = CurrentAction.GetSelfAction();

            // TODO Can I damage myself?
            // i will set this to true damage, since its probably sacrificing health for attacks, not actually running through the defend actions
            if (selfAction.TrueDamage > 0) {
                TakeTrueDamage(CurrentUnit, selfAction.StrengthDamage);
            }

            if (selfAction.Heal > 0) {
                CurrentUnit.HealDamage(selfAction.Heal);
            }

            if (selfAction.Sate > 0) {
                CurrentUnit.SateHunger(selfAction.Sate);
            }

            if (selfAction.Tantalize > 0) {
                CurrentUnit.TantalizeHunger(selfAction.Tantalize);
            }

            foreach(KeyValuePair<string, int> effect in selfAction.GetAllStatusEffects())
            {
                CurrentUnit.AddStatusEffect(effect.Key, effect.Value);
            }

            foreach(KeyValuePair<string, int> effect in selfAction.GetAllStatChanges())
            {
                CurrentUnit.AddStatChange(effect.Key, effect.Value);
            }

            // do this better
            CurrentUnit.UpdateText();

            if (selfAction.DestroySelf) {
                CurrentUnit.DestroySelf();

                // TODO: must clear from unit lists etc (might have solved this now, will keep testing)
                // on this note, will i have revival? should i even keep dead units around?
                // if not, i should have a more effective way of clearing units, since i might want to move them around, or have them transform, or get "used up" like the cauldron
            }
        }

        public void RouteSkill()
        {
            switch (CurrentAction.GetSkillType()) {
                // case "SelfOrAlly":
                // case "Ally":
                // case "MeleeAlly":
                // case "CoverAlly":
                // case "Enemy":
                // case "MeleeEnemy":
                // case "MeleeEnemyPierce":
                // case "CoveredEnemy": // TODO
                // case "SelfOrAllyOrMeleeEnemy": // TODO THIS DOESN'T TRIGGER UNCOVER IN
                case "CookRecipe":
                    CookRecipe();
                    break;
                case "Stir":
                    Stir();
                    break;
                case "Season":
                    Season();
                    break;
                // case "AddIngredient":
                case "Serve":
                case "Eat":
                case "ServeDrink":
                case "ServeFood":
                case "ServeMeal":
                    Meal();
                    break;
                //     // FindTargets();
                //     DisplayTargets();
                //     break;
                // case "Self":
                // case "AllMeleeEnemies":
                // case "AllCoveredEnemies": // TODO
                // case "AllEnemies":
                // case "Targetless": // or "Untargeted"
                // case "Recipe":
                case "Forage":
                    Forage();
                    break;
                case "PickRecipe":
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
            }
        }


    // COVERING

        private void Cover()
        {
            if (CurrentAction.Targets.Count == 1) {
                foreach (UnitStats target in CurrentAction.Targets) {
                    CoverUnit(target);
                }
            } else {
                Debug.Log("Can only cover one ally!");
            }
        }

        private void CoverUnit(UnitStats unit)
        {
            UncoverUnit(CurrentUnit);
            UncoverUnit(unit);

            coverUnits.Add(CurrentUnit.name, unit);

            if (myTurn) {
                RemoveUnitFromList(unit, meleeAllyList);
            } else {
                RemoveUnitFromList(unit, meleeEnemyList);
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
                        AddUnitToList(coverUnits[unit.name], meleeAllyList);
                    } else {
                        AddUnitToList(coverUnits[unit.name], meleeEnemyList);
                    }
                }

                coverUnits.Remove(unit.name);
            }

            if (IsCovered(unit)) {
                if (!unit.isDead) {
                    if (myTurn) {
                        AddUnitToList(unit, meleeAllyList);
                    } else {
                        AddUnitToList(unit, meleeEnemyList);
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
                AddUnitToList(summonUnit, allyCookinList);
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

        public void SelectPossibleTargetsByActionType()
        {
            // TODO this should all be find targets, not set targets
            // should make another method for that and a conditional in RouteAction

            // TODO use FindTargetsByList instead of FindTargetsByTag when possible
            switch (CurrentAction.TargetType)
            {
                // TODO just use self action instead of self target type
                case "Self":
                    SetTargetToSelf();
                    break;
                case "SelfOrAlly":
                // case "Eat":
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
                // case "MeleeEnemyPierce":
                // case "ServeFood":
                // case "ServeMeal":
                    if (myTurn) {
                        FindTargetsByList(meleeEnemyList);
                    } else {
                        FindTargetsByList(meleeAllyList);
                        SelectRandomUnitFromPossibleTargets();
                    }
                    break;
                case "Enemy":
                // case "ServeDrink":
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
                case "AllyPot":
                // case "Stir":
                // case "Season":
                // case "AddIngredient":
                    if (myTurn) {
                        FindTargetsByList(allyCookinList);
                    } else {
                        Debug.Log("ERROR: BM.SelectPossibleTargetsByActionType : Enemies can't interact with cookin");
                    }
                    break;
                // case "Targetless":
                // case "Forage":
                // case "Cook":
                // case "Recipe":
                // case "PickIngredient":
                // case "PickMealToEat":
                // case "PickDrinkToServe":
                // case "PickFoodToServe":
                // case "PickMealToServe":
                    // break;
                default:
                    // print("Untargeted action");
                    break;
            }
        }


    // ATTACK CALCULATOR

        private void TakeStrengthDamage(UnitStats targetUnit, int rawDamage)
        {
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
            if (targetUnit.GetStatusEffectStatus(DEFEND)) {
                netDamage = attackDamage - targetUnit.GetNetDefense();
                targetUnit.SubtractStatusEffect(DEFEND, 1); // or you could lose one at EoT
            } else  {
                netDamage = attackDamage - (targetUnit.GetNetDefense() / 2);
            }

            // do 1 damage per hit minimum
            if (netDamage <= 0) {
                netDamage = 1;
            }

            do {
                int hitDamage = netDamage;

                if (!targetUnit.GetStatusEffectStatus(DODGE)) {
                    Debug.Log(CurrentUnit + " attacks for " + hitDamage);
                    if (targetUnit.GetStatusEffectStatus(ARMOR)) {
                        Debug.Log(targetUnit.GetStatusEffectStacks(ARMOR));
                        // int shieldDamage = hitDamage - targetUnit.GetStatusEffectStacks(ARMOR);
                        // targetUnit
                        // targetUnit.shield -= hitDamage;

                        if (targetUnit.GetStatusEffectStacks(ARMOR) >= hitDamage) {
                            targetUnit.SubtractStatusEffect(ARMOR, hitDamage);
                            hitDamage = 0;
                            // hitDamage = -targetUnit.GetStatusEffectStacks(ARMOR);
                            // targetUnit.shield = 0;
                        } else /*if (targetUnit.GetStatusEffectStacks(ARMOR) < hitDamage)*/ {
                            int tempArmorDamage = targetUnit.GetStatusEffectStacks(ARMOR);
                            hitDamage -= tempArmorDamage;

                            targetUnit.SubtractStatusEffect(ARMOR, targetUnit.GetStatusEffectStacks(ARMOR));
                        }
                        Debug.Log(targetUnit.GetStatusEffectStacks(ARMOR));

                    }

                    targetUnit.LoseHealth(hitDamage);
                } else {
                    targetUnit.SubtractStatusEffect(DODGE, 1);
                }

                multiplier--;
            } while (multiplier > 0);

            // CheckIfDead(targetUnit);
            PostDamageUpdates(targetUnit);
        }

        private void TakeTumDamage (UnitStats targetUnit, int rawDamage)
        {
            int netDamage = rawDamage - (targetUnit.GetNetTum() / 2);

            // do 1 damage minimum
            if (netDamage <= 0) {
                netDamage = 1;
            }

            targetUnit.LoseHealth(netDamage);

            // CheckIfDead(targetUnit);
            PostDamageUpdates(targetUnit);

        }

        private void TakeTrueDamage (UnitStats targetUnit, int rawDamage)
        {
            int netDamage = rawDamage;

            if (netDamage <= 0) {
                targetUnit.LoseHealth(netDamage);

                // CheckIfDead(targetUnit);
                PostDamageUpdates(targetUnit);
            } else {
                Debug.Log("Error : TakeTrueDamage : why is this zero?");
            }
        }

        // TODO: there are many references to this, but it should only be used when a unit takes damage/ loses hp
        private void CheckIfDead(UnitStats unit)
        {
            if (unit.GetCurrentHealth() <= 0) {
                HandleDeath(unit);
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

            // QA this might not work if gross kills the unit and it self destructs
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
                print('s');
                foreach (Recipe recipe in playerRecipes) {
                    Debug.Log(recipe.GetName());
                    Debug.Log(recipe);

                    GameObject instantButton = Instantiate(cookRecipeButton, new Vector3(0, yPos, 0), Quaternion.identity);

                    instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                    // instantButton.transform.position = new Vector3(20,5,0);
                    instantButton.transform.SetParent(HUD.transform, false);
                    // second param keeps scale etc the same
                    // Debug.Log(recipe);

                    instantButton.GetComponentInChildren<Text>().text = recipe.GetName();

                    CookRecipe buttonMove = instantButton.GetComponent<CookRecipe>();

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

        public void HandleDeath(UnitStats unit)
        {
            unit.isDead = true;
            unit.turnNumber = -1;

            unit.UpdateText();
            // unit.SetTurnText("Dead");

            UncoverUnit(unit);

            RemoveUnitFromList(unit, completeList);
            RemoveUnitFromList(unit, nextUpList);
            RemoveUnitFromList(unit,  allyList);
            RemoveUnitFromList(unit,  enemyList);
            RemoveUnitFromList(unit,  meleeAllyList);
            RemoveUnitFromList(unit,  meleeEnemyList);

            CheckGameOver();
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


    // AI

        private void AutoMove()
        {
            if (CurrentUnit.GetMovesCount() > 0) {

                MoveButton tryMove = null;

                do {
                    tryMove = CurrentUnit.GetRandomAction();

                    tryMove.SetUpButtonAction();
                } while (!CheckAfford(tryMove.GetAction()));

                CurrentAction = tryMove.GetAction();

                SelectPossibleTargetsByActionType();

                TakeAction();
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

        private void PostDamageUpdates(UnitStats unit)
        {
            // unit.UpdateText();
            CheckIfDead(unit);
        }

        private void Shuffle (List<UnitStats> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = Random.Range(0, n + 1);
                UnitStats unit = list[k];
                list[k] = list[n];
                list[n] = unit;
            }
        }
}
