
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
//
// ***************
// ***VARIABLES***
// ***************
//
    // LEVELS
        public string NextLevel;

        private string CurrentSceneName;

    // PREFABS
        public GameObject targetButton;
        public GameObject eatButton;
        public GameObject serveDrinkButton;
        public GameObject serveFoodButton;
        public GameObject serveMealButton;
        public GameObject cookRecipeButton;
        public GameObject ingredientButton;

        public GameObject countdownClockPrefab;

    // UI
        // public GameObject MoveBox;
        // private GameObject StazBox;

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
        private int freeMoves = 0;
        private int freeSkills = 0;


    // ACTIVE VARS
        public UnitStats    CurrentUnit;
        public Action       CurrentAction;
        public Recipe       CurrentRecipe;
        public Item         CurrentMeal;

    // INVENTORIES AND ITEMS
        public Inventory playerIngredients;
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
        private const string GIRD       = "GIRD";
        private const string GROSS      = "GROSS";
        private const string DODGE      = "DODGE";
        private const string DEFEND     = "DEFEND";
        private const string GLOW       = "GLOW";
        private const string FURY       = "FURY";


    // CONSTANTS: VALUES
        private const int METABOLISM = 1;

//
// ***************
// ***FUNCTIONS***
// ***************
//
    // INITIALIZATION

        private void Awake()
        {
            StartStage();
        }

        private void StartStage()
        {

            // InitializeStageScene();
            // InitializeIngredients();
            // InitializeItems();
            // InitializeUnitLists();
            // InitializeCurrentAction();
            // InitializeRecipes();

            // InitializeUI();
            // InitializeUnitPositions();
            // InitializeUnitPoints();
            // InitializeDialogue();

            DialogueTrigger dt = FindObjectOfType<DialogueTrigger>();

            if (dt != null) {
                dt.TriggerDialogue();
            } else {
                StartCombat();
            }
        }

        private void DisplayUnitPoints()
        {
            foreach (UnitStats unit in completeList)
            {
                unit.InitializeUnitText();
            }
        }

        private void InitializeStageScene()
        {
            CurrentSceneName = SceneManager.GetActiveScene().name;
        }

        private void InitializeIngredients()
        {
            // Initialize starting Ingredients
            playerIngredients = new Inventory();
            switch (CurrentSceneName)
            {
                case "Tutorial03":
                    // AddStartingIngredients();
                    // Debug.Log('T');
                    playerIngredients.AddItem("Bottle");
                    // playerIngredients.AddItem("Bottle");

                    break;
                case "Tutorial04":
                    // AddStartingIngredients();
                    // Debug.Log('T');
                    // playerIngredients.AddItem("Bottle");
                    // playerIngredients.AddItem("Bottle");

                    break;
                // case "Tutorial05":
                //     // AddStartingIngredients();

                //     break;
                // case "Tutorial06":
                //     // AddStartingIngredients();

                //     break;
                default:
                    break;
            }

            // Initialize forageable Ingredients
            localIngredients = new Inventory();
            switch (CurrentSceneName)
            {
                case "Tutorial04":
                //     // AddLocalIngredients();
                    localIngredients.AddItem("Bottle");
                    localIngredients.AddItem("Bottle");
                    localIngredients.AddItem("Bottle");
                    break;
                case "Tutorial05":
                    // AddLocalIngredients();
                    localIngredients.AddItem("Bottle");
                    localIngredients.AddItem("Bottle");
                    localIngredients.AddItem("Bottle");

                    // localIngredients.AddItem("Water");
                    // localIngredients.AddItem("Water");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");

                    break;
                case "Tutorial07":
                    // AddLocalIngredients();
                    localIngredients.AddItem("Bottle");
                    localIngredients.AddItem("Bottle");
                    localIngredients.AddItem("Bottle");
                    // localIngredients.AddItem("Bottle");
                    // localIngredients.AddItem("Water");
                    // localIngredients.AddItem("Water");
                    // localIngredients.AddItem("Grease");
                    // localIngredients.AddItem("Grease");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    break;
                case "Stage 1":
                    // AddLocalIngredients();
                    // localIngredients.AddItem("Bottle");
                    // localIngredients.AddItem("Bottle");
                    // localIngredients.AddItem("Bottle");
                    // localIngredients.AddItem("Bottle");
                    // localIngredients.AddItem("Water");
                    // localIngredients.AddItem("Water");
                    localIngredients.AddItem("Grease");
                    localIngredients.AddItem("Grease");
                    localIngredients.AddItem("Grease");                    
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    localIngredients.AddItem("Corn");
                    break;
                default:
                    break;
            }
        }

        private void InitializeItems()
        {
            playerMealItems     = new List<Item>();
            playerEquipmentItems= new List<Item>();

            // start with this equipment
            Pot startingPot = new Pot();
            playerEquipmentItems.Add(startingPot);

            switch (CurrentSceneName)
            {
                case "Tutorial04":
                    // AddStartingItems();

                    CornCobItem test0 = new CornCobItem();
                    test0.SetQuality(0);
                    playerMealItems.Add(test0);

                    CobFrappeItem test1 = new CobFrappeItem();
                    test1.SetQuality(0);
                    playerMealItems.Add(test1);

                    break;
                // case "Tutorial05":
                //     // AddStartingItems();

                //     break;
                case "Tutorial06":
                    // AddStartingItems();

                    CobFrappeItem test2 = new CobFrappeItem();
                    test2.SetQuality(0);
                    playerMealItems.Add(test2);

                    break;
                default:
                    break;
            }

            // // start with pre-made food (for testing)
            // CornCobItem test0 = new CornCobItem();
            // test0.SetQuality(0);
            // playerMealItems.Add(test0);

            // CornCobItem test1 = new CornCobItem();
            // test1.SetQuality(1);
            // playerMealItems.Add(test1);

            // CornCobItem test2 = new CornCobItem();
            // test2.SetQuality(2);
            // playerMealItems.Add(test2);

            // CornCobItem test3 = new CornCobItem();
            // test3.SetQuality(3);
            // playerMealItems.Add(test3);
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
            playerRecipes.Add(new BurntCornRecipe());
            // playerRecipes.Add(new CornCobRecipe());
            playerRecipes.Add(new GritsRecipe());
            playerRecipes.Add(new PopCornRecipe());
        }

        // private void InitializeUI()
        // {
        //     // ActivateMoveBox(true);
        // }

        private void InitializeUnitPositions()
        {
            UpdateUnitPositions();
            // float yPos      = 70f;
            // float xPosAlly  = -110f;
            // foreach (UnitStats allyUnit in allyList)
            // {
            //     allyUnit.SetUnitBoxPosition(xPosAlly, yPos);
            //     xPosAlly -= 110f;
            // }

            // float xPosEnemy  = 110f;
            // foreach (UnitStats enemyUnit in enemyList)
            // {
            //     // Debug.Log(xPosEnemy + " " + yPos);
            //     enemyUnit.SetUnitBoxPosition(xPosEnemy, yPos);
            //     xPosEnemy += 110f;
            // }
        }

        private void InitializeDialogue()
        {
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

        private void DisplayList(List<UnitStats> list)
        {
            // Debug.Log(list);
            // Debug.Log(list.Count);
            foreach (UnitStats unit in list) {
                if (unit == null) {
                    Debug.Log("Error : DisplayList : Can't print null unit");
                }
                print(unit.GetName());
            }
        }


    // TURN MANAGEMENT

        public void StartCombat()
        {
            InitializeUnitLists();
            // InitializeUI();
            InitializeUnitFigures();
            InitializeUnitPositions();
            InitializeUnitStazButtons();

            InitializeStageScene();
            InitializeCurrentAction();

            InitializeIngredients();
            InitializeItems();
            InitializeRecipes();

            // UI
            // NextTurnButton.GetComponentInChildren<Text>().text = "Start\nCombat";
            // DestroyDialogueOnlyUnits();

            DisplayUnitPoints();
            ActivateMoveBox(true);

            CombatLog("Combat begins!");

            StartRound();
        }

        private void StartRound()
        {
            BuildTurnList();

            StartRoundUpdates();

            ActivateNextTurnButton(true);
        }

        private void StartRoundUpdates()
        {
            GutCheck();
        }

        private void StartTurn()
        {
            // redundant cux of start turn updates
            // ClearCombatLog();

            CurrentUnit = WhoseTurn();
            if (CurrentUnit.tag == "Player") {
                myTurn = true;
            }

            StartTurnUpdates();

            StartAction();
        }

        private void StartTurnUpdates()
        {
            // UI
            ClearCombatLog();
            CombatLog(CurrentUnit.GetName() + "'s turn!");

            // TODO: what if this kills unit? they can still act
            CheckGross(CurrentUnit);

            //  would it make more sense for starving to happen at start of turn
            if (CurrentUnit.isStarving)
            {
                CombatLog(CurrentUnit.GetName() + " is still starving. They skip their turn and take TUM damage!");
                CurrentUnit.AddStatusEffect("STUN", 1);
                TakeTumDamage(CurrentUnit, 10);
            }

            freeMoves = 1;
            freeSkills = 1;

            // UI
            DisplayStaz(CurrentUnit);
        }

        private void StartAction()
        {
            if (CurrentUnit.GetStatusEffectStacks("STUN") > 0 || CurrentUnit.isNPC) {
                // Debug.Log(CurrentUnit.GetName() + " is stunned due to hunger (or is food)");
                EndTurn();
            } else {
                if (myTurn) {
                    PlayerSelectAction();
                } else {
                    AutoMove();
                }
            }
        }

        private void EndTurn()
        {
            EndTurnUpdates();

            // check for game over here? maybe add a flag for turn over, can activate in check if dead

            if (nextUpList.Count == 0) {
                NextTurn();
            } else {
                ActivateNextTurnButton(true);
            }
        }

        private void EndTurnUpdates()
        {
            // build this    
            // TickCurrentUnitStatusEffects();

            if (CurrentUnit.GetStatusEffectStatus("STUN")) {
                CurrentUnit.SubtractStatusEffect("STUN", 1);
            }

            // TURN MANAGEMENT
            myTurn = false;
            RemoveCurrentUnitFromNextUpList();
            freeMoves = 0;
            freeSkills = 0;


            CurrentUnit.SetTurnNumber(0);
            // CurrentUnit.ChangeTurnTextColor(Color.gray);

            SetUnitTurnNumbers();
            ResetCurrentAction();

            // UI UPDATES
            CleanUpButtons();
            DisplayNextTurns(nextUpList);
            DisplayStaz(CurrentUnit);
            SetNextTurnButtonText("Next\nTurn");

            CheckIfDead(CurrentUnit);
        }

        public void NextTurn()
        {
            ActivateNextTurnButton(false);

            // TODO lets not use the game over bool
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

            SetNextTurnButtonText("Next\nRound");

            StartRound();
        }

        private void EndRoundUpdates()
        {
            TurnEndRecipeCheck();

            // when does all the gut stuff happen? should it happen at the beginnings  or ends of rounds
            Metabolism();
        }

        private void RemoveCurrentUnitFromNextUpList()
        {
            if (nextUpList.Count > 0) {
                nextUpList[0].turnNumber = 0;
                nextUpList.RemoveAt(0);
                // DisplayNextTurns(nextUpList);
            }
        }

        private UnitStats WhoseTurn()
        {
            return nextUpList[0];
        }

        private void BuildTurnList()
        {
            List<UnitStats> sortedList  = new List<UnitStats>();
            List<UnitStats> npcList  = new List<UnitStats>();

            // nextUpList  = new List<UnitStats>();

            sortedList = completeList.OrderBy(unit => unit.GetNetSpeed()).Reverse().ToList();

            // all NPCs are not sorted into the nextUpList
            npcList = sortedList.FindAll(u => u.isNPC);

            for (int j = 0; j < npcList.Count; j++)
            {
                // Debug.Log(npcList[j]);
                RemoveUnitFromList(npcList[j], sortedList);
            }


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
            DisplayNextTurns(nextUpList);
        }

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

        public void ResetTurn()
        {
            ResetCurrentAction();
            
            DisplayStaz(CurrentUnit);

            StartAction();
        }

        private void CheckEndTurn()
        {
            if (freeMoves >  0 || freeSkills > 0)  {
                ResetTurn();
            } else {
                EndTurn();
            }
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
                Debug.Log("Error: Meal: You should only consume one meal at a time");
            }
        }

        private void PickMealToEat()
        {
            DisplayMealsToEat(playerMealItems);
        }

        private void PickDrinkToServe()
        {
            DisplayMealsToServe(playerMealItems, "Drink");
        }

        private void PickFoodToServe()
        {
            DisplayMealsToServe(playerMealItems, "Food");
        }

        private void PickMealToServe()
        {
            DisplayMealsToServe(playerMealItems, "Meal");
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
            DisplayRecipes(playerRecipes);
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

            // Debug.Log(recipeToSet.GetName());
            // Debug.Log(recipeToSet.GetCookCount());
            CurrentRecipe = recipeToSet;
            // Debug.Log(CurrentRecipe.GetCookCount());

            SetRecipePicked(true);

            // Debug.Log(CurrentRecipe.GetCookCount());
            // Debug.Log(CurrentRecipe.GetRequiredIngredients());
        }

        private void TurnEndRecipeCheck()
        {
            if (CurrentRecipe != null) {

                if (CurrentRecipe.GetCookCount() < CurrentRecipe.GetCookTime())
                {
                    CookTimeAddTurns(1);
                    UpdateCookingCountdown();
                } else if (CurrentRecipe.GetCookCount() == CurrentRecipe.GetCookTime())
                {
                    if (CheckIngredientsForCurrentRecipe() && CheckStirring())
                    {
                        CreateMeal();
                    } else {
                        FailMeal();
                    }

                    RecipeCleanUp();

                } else if (CurrentRecipe.GetCookCount() > CurrentRecipe.GetCookTime())
                {
                    Debug.Log("What is this, Overcooked?");
                }
            } else {
                // Debug.Log("No recipe");
            }
        }

        private void CookRecipe()
        {
            Recipe recipe = new Recipe();
            recipe = CurrentAction.Recipe;
            SetRecipe(recipe);

            // Debug.Log(CurrentAction.GetName());
            // Debug.Log(CurrentAction.Recipe.GetName());
            // Debug.Log(CurrentAction.Recipe.GetCookCount());

            // Debug.Log(CurrentRecipe.GetName());
            // Debug.Log(CurrentRecipe.GetCookCount());

            ShowPot();
            ShowRecipeIngredients();
            ShowRecipeCountdown();
            // seems redundant?
            // CombatLog(CurrentUnit.GetName() + " started cooking " + CurrentRecipe.GetName());

            // does this look better?
            CombatLog(CurrentUnit.GetName() + " started cooking " + CurrentRecipe.GetName() + ". It will take " + CurrentRecipe.GetCookTime() + " round(s).");
        }

        private void ResetRecipe() {

        }

        private void AddIngredientToRecipe(string ingredient)
        {
            playerIngredients.RemoveItem(ingredient);

            if (CurrentAction.GetSkillType() == "AddIngredient") {
                CurrentRecipe.GetRequiredIngredients().RemoveItem(CurrentAction.GetIngredientCost());
            }

            ClearIngredientIcons();
            ShowRecipeIngredients();
        }

        private void UpdateCookingCountdown() {
            ClearClockIcons();
            ShowRecipeCountdown();
        }

        private void ClearCookingIcons() {
            ClearPotIcon();
            ClearClockIcons();
            ClearIngredientIcons();
        }

        private void CookTimeAddTurns(int turns)
        {
            if (turns == 1) {
                CurrentRecipe.AddCookTurn();
            }
        }

        private void Stir()
        {
            CurrentRecipe.AddStir();

            CurrentRecipe.AddFlavor(CurrentUnit.GetNetNose());

            CombatLog(CurrentUnit.GetName() + " stirred the pot, adding " + CurrentUnit.GetNetNose() + " flavor");
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
            // print(CurrentUnit.GetNetTaste());

            CurrentRecipe.AddFlavor(CurrentUnit.GetNetTaste());

            CombatLog(CurrentUnit.GetName() + " seasoned the pot, adding " + CurrentUnit.GetNetTaste() + " flavor");
        }

        private void Lid()
        {
            if (CurrentAction.Targets.Any(t => t.isCookin)) {
                CookTimeAddTurns(1);
                UpdateCookingCountdown();
            }
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
            // Debug.Log("You made " + meal.Name);
            CombatLog("You made " + meal.Name);
        }

        private void FailMeal()
        {
            // UpdateCookingCountdown();

            // playerMealItems.Add(new BurntMessItem());
            CombatLog("Your recipe failed!");
            // Debug.Log("You made a Burnt Mess. Better luck next time!");
        }

        private void RecipeCleanUp()
        {
            ClearCookingIcons();

            // KillCauldron();

            RestockCauldron();

            InitializeRecipes();
            // TODO handle having more than one cauldron
            //      handle having enemy cauldrons?
        }

        private void KillCauldron() {
            // Debug.Log("KillCauldron");
            UnitStats unit = allyCookinList[0];
            
            UncoverUnit(unit);

            // this def isn't on any other lists right? like you can't attack the cauldron anymore?
            // RemoveUnitFromCookinList(unit);
            RemoveUnitFromAllLists(unit);
            Destroy(unit.gameObject);



            UpdateUnitPositions();

            // CheckGameOver();

            CurrentRecipe = null;
            SetRecipePicked(false);
        }

        private void RestockCauldron() {
            Pot startingPot = new Pot();
            playerEquipmentItems.Add(startingPot);
        }

        private void ShowRecipeIngredients()
        {

            // string placeholderFoodName = "corn";

            // DisplayIngredientIcons(placeholderFoodName);

            Dictionary<string, int> requiredIngredients = CurrentRecipe.GetRequiredIngredients().GetInventoryAsDictionary();

            DisplayIngredientIcons(requiredIngredients);
        }

        private void ShowRecipeCountdown()
        {
            int recipeCountdown =  CurrentRecipe.GetCookTime() - CurrentRecipe.GetCookCount();
// Debug.Log("show countdown");
// Debug.Log(CurrentRecipe.GetCookTime());
// Debug.Log('-');
// Debug.Log(CurrentRecipe.GetCookCount());
// Debug.Log(CurrentRecipe.GetCookTime() - CurrentRecipe.GetCookCount());


            DisplayRecipeCountdown(recipeCountdown);

            // GameObject countdownClock = Instantiate(countdownClockPrefab, new Vector3(0, 0, 0), Quaternion.identity); // hardcoded location, should make dynamic
        }

        private void ShowPot()
        {
            DisplayPot();
        }


    // HUNGER MANAGEMENT

        private void Metabolism()
        {
            foreach (UnitStats unit in completeList) {
                if (!unit.isCookin && !unit.isGutless) { // && !unit.isDead) {
                    if (unit.GetStatusEffectStatus(GIRD))
                    {
                        unit.SubtractStatusEffect(GIRD, 1);
                    } else {
                        unit.LoseGuts(METABOLISM);
                    }
                }
            }
        }

        private void GutCheck()
        {
            foreach (UnitStats unit in completeList) {
                // Debug.Log("Gut Check " + unit.GetName());
                if (!unit.isCookin && !unit.isGutless) {
                    if (unit.GetCurrentGuts() <= 0) {
                        Starve(unit);
                    }
                }
            }

            // CheckIfDeadAll();
        }

        private void Starve(UnitStats unit)
        {
            CombatLog(unit.GetName() + " is starving. Feed them food before they perish!");

            unit.isStarving = true;
            // unit.AddStatusEffect("STUN", 1);
            // TakeTumDamage(unit, 10);
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
                if (forageCount == 0) {
                    CombatLog(CurrentUnit.GetName() + " failed to sniff out any resources!");
                }

                // Debug.Log(forageCount);
                // if (localIngredients.CountItems() - forageCount < 0)
                while (forageCount > 0 && localIngredients.CountItems() > 0) {
                    string foragedIngredient = localIngredients.GetRandomItem();

                    playerIngredients.AddItem(foragedIngredient);
                    localIngredients.RemoveItem(foragedIngredient);

                    forageCount--;

                    CombatLog(CurrentUnit.GetName() + " found: " + foragedIngredient);

                    // TODO Kola should get a 50% chance to grab a bottle too (maybe nose multiplier?`)
                }
            } else {

                CombatLog("No ingredients left to forage!");
                canAct = false;
            }
        }

        private void PickIngredient()
        {
            DisplayIngredients(playerIngredients);
        }

        private bool CanAffordIngredient(string ingredient)
        {
            if (playerIngredients.CheckItem(ingredient)) {
                return true;
            }

            return false;
        }


        private bool CheckIngredientsForCurrentRecipe()
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

    // ACTION CHECKS


        public void TryAction(Action action)
        {
            canAct = true;
            SetCurrentAction(action);

            if (CheckAfford(action)) {
                // PickTargets();

                RouteAction();
            } else {
                ResetTurn();
                // Debug.Log("Cannot do Move");
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
                // Debug.Log(action.GetIngredientCost());

                if (!playerIngredients.CheckItem(action.GetIngredientCost())) {
                    CombatLog("Can't use move: You don't have the required ingredient");

                    Debug.Log("You do not have the required ingredient.");
                    return false;
                }
            }

            // TODO: Make this work for requiring multiple of the same item
            if (action.ResourceCost != null) {
                foreach (Item resource in action.ResourceCost) {
                    switch(action.GetSkillType()) {
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

    // PREP ACTION


        private void PlayerSelectAction()
        {
            bool freeMove = freeMoves > 0;
            bool freeSkill = freeSkills > 0;

            DisplayActionButtons(CurrentUnit, freeMove, freeSkill);
        }

        public void SetCurrentAction(Action action)
        {
            ResetCurrentAction();

            CurrentAction = action;
        }

        public void ResetCurrentAction()
        {
            CurrentAction = null;
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
                // case "AllyPot":
                // case "Stir":
                // case "Season":
                // case "AddIngredient":
                // case "Eat":
                // case "ServeDrink":
                // case "ServeFood":
                // case "ServeMeal":
                    // FindTargets();
                    DisplayTargets(CurrentAction.PossibleTargets);
                    break;
                case "Self":
                case "AllMeleeEnemies":
                case "AllCoveredEnemies": // TODO
                case "AllEnemies":
                case "Targetless": // or "Untargeted"
                    ReadyAction();
                    break;
                case "Pick":
                    RouteSkill();
                    break;
                default:
                    print("Invalid target");
                    break;
            }
        }

        public void ReadyAction()
        {
            CleanUpButtons();

            ActivateCancelButton(true);
            ActivateConfirmButton(true);

            PrepareActionPreview(CurrentAction);
        }

        private void PrepareActionPreview(Action action)
        {
            // TODO:
            // Maybe: GutsCost; ItemCost; REsourceCost; Give Item; Summon
            foreach (UnitStats target in action.Targets) {
                if (action.StrengthDamage > 0) {
                    StazMessage(PreviewStrengthDamage(target, action.StrengthDamage));
                }
                if (action.TumDamage > 0) {
                    StazMessage(PreviewTumDamage(target, action.TumDamage));
                }
                if (action.Heal > 0) {
                    StazMessage(PreviewHealDamage(target, action.Heal));
                }
                if (action.Sate > 0) {
                    StazMessage(PreviewSateHunger(target, action.Sate));
                }
                if (action.Famish > 0) {
                    StazMessage(PreviewFamishHunger(target, action.Famish));
                }
                // Do i really need to check on the count since i'm doing foreach?
                if (action.GetAllStatusEffects().Count > 0) {
                    foreach(KeyValuePair<string,int> effect in action.GetAllStatusEffects()) {
                        StazMessage("Apply " + effect.Value + " " + effect.Key + " to " + target.GetName());
                    }
                }
                if (action.GetAllStatChanges().Count > 0) {
                    foreach(KeyValuePair<string,int> statChange in action.GetAllStatChanges()) {
                        if (statChange.Value > 0) {
                            StazMessage("Add " + statChange.Value + " " + statChange.Key + " to " + target.GetName());
                        }
                        if (statChange.Value < 0) {
                            StazMessage("Lose " + statChange.Value + " " + statChange.Key + " from " + target.GetName());
                        }
                    }
                }
            }
        }

    // TAKE ACTION

        // could i use a while loop instead?
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
                    AddIngredientToRecipe(CurrentAction.GetIngredientCost());
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

            if (CurrentAction.GetIsMove()) {
                freeMoves--;
            }

            if  (CurrentAction.GetIsSkill()) {
                freeSkills--;
            }

            // should i Clear the current action at the end of the turn??

            if (!gameOver) {
                CheckEndTurn();
            }
            // EndTurn();
        }

        public void PerformTargetedAction(UnitStats unit)
        {
            if (CurrentAction.StrengthDamage > 0) {
                // Debug.Log(unit.GetName() + " receives " + CurrentAction.StrengthDamage + " raw damage");

                // TODO THIS DOESN'T TRIGGER UNCOVER FOR "SelfOrAllyOrMeleeEnemy"
                switch (CurrentAction.TargetType) {
                    case "MeleeEnemy":
                    case "MeleeEnemyPierce":
                    case "AllMeleeEnemies":
                        if (IsCovered(CurrentUnit)) {
                            UncoverUnit(CurrentUnit);
                        }
                        break;
                    default:
                        break;
                }

                TakeStrengthDamage(unit, CurrentAction.StrengthDamage);
            }

            if (CurrentAction.TargetType == "MeleeEnemyPierce") {
                if (coverUnits.ContainsKey(unit.GetName())) {
                    // Debug.Log(coverUnits[unit.GetName()].name + " receives " + CurrentAction.StrengthDamage/2 + " raw pierce damage");

                    TakeStrengthDamage(GetUnitByName(coverUnits[unit.GetName()].name), CurrentAction.StrengthDamage/2);
                } else {
                    Debug.Log ("covered unit not found");
                }
            }

            if (CurrentAction.Heal > 0) {
                HealDamage(unit, CurrentAction.Heal);
            }

            if (CurrentAction.Sate > 0) {
                SateHunger(unit, CurrentAction.Sate);
            }

            if (CurrentAction.Famish > 0) {
                FamishHunger(unit, CurrentAction.Famish);
            }

            foreach(KeyValuePair<string, int> effect in CurrentAction.GetAllStatusEffects())
            {
                ApplyEffect(unit, effect.Key, effect.Value);
                // unit.AddStatusEffect(effect.Key, effect.Value);
            }

            foreach(KeyValuePair<string, int> effect in CurrentAction.GetAllStatChanges())
            {
                ApplyStatChange(unit, effect.Key, effect.Value);
                // unit.AddStatChange(effect.Key, effect.Value);
            }

            // do this better
            unit.UpdateText();

            if (CurrentAction.DestroySelf) {
                // unit.DestroySelf();
                KillUnit(unit);

                // TODO: must clear from unit lists etc (might have solved this now, will keep testing)
                // on this note, will i have revival? should i even keep dead units around?

                // if not, i should have a more effective way of clearing units, since i might want to move them around,
                // or have them transform, or get "used up" like the cauldron
            }
        }

        // TODO redundant, i can probably run all actions with one function
        public void PerformSelfAction()
        {
            Action selfAction = CurrentAction.GetSelfAction();

            // TODO Can I damage myself?
            // i will set this to true damage, since its probably sacrificing health for attacks, not actually running through the defend actions
            if (selfAction.TrueDamage > 0) {
                TakeTrueDamage(CurrentUnit, selfAction.StrengthDamage);
            }

            if (selfAction.Heal > 0) {
                HealDamage(CurrentUnit, selfAction.Heal);
            }

            if (selfAction.Sate > 0) {
                SateHunger(CurrentUnit, selfAction.Sate);
            }

            if (selfAction.Famish > 0) {
                FamishHunger(CurrentUnit, selfAction.Famish);
            }

            foreach(KeyValuePair<string, int> effect in selfAction.GetAllStatusEffects())
            {
                ApplyEffect(CurrentUnit, effect.Key, effect.Value);
            }

            foreach(KeyValuePair<string, int> effect in selfAction.GetAllStatChanges())
            {
                ApplyStatChange(CurrentUnit, effect.Key, effect.Value);
            }

            // do this better
            CurrentUnit.UpdateText();

            if (selfAction.DestroySelf) {
                // CurrentUnit.DestroySelf();
                KillUnit(CurrentUnit);

                // TODO: must clear from unit lists etc (might have solved this now, will keep testing)
                // on this note, will i have revival? should i even keep dead units around?
                // if not, i should have a more effective way of clearing units, since i might want to move them around, or have them transform, or get "used up" like the cauldron
            }
        }

        public void RouteSkill()
        {
            switch (CurrentAction.GetSkillType()) {
                case "Cover":
                    Cover();
                    break;
                case "Forage":
                    Forage();
                    break;
                case "CookRecipe":
                    CookRecipe();
                    break;
                case "Stir":
                    Stir();
                    break;
                case "Season":
                    Season();
                    break;
                case "Lid":
                    Lid();
                    break;
                case "Serve":
                case "Eat":
                case "ServeDrink":
                case "ServeFood":
                case "ServeMeal":
                    Meal();
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

        private void ApplyEffect(UnitStats unit, string effectType, int stacks)
        {
            switch(effectType) {
                case ARMOR:
                case STUN:
                case GIRD:
                case GROSS:
                case DODGE:
                case DEFEND:
                case FURY:
                    unit.AddStatusEffect(effectType, stacks);
                    CombatLog(unit.GetName() + " got " + stacks + " stacks of " + effectType);
                    break;
                default:
                    Debug.Log("Non-existent effect type");
                    break;
            }
        }

        private void ApplyStatChange(UnitStats unit, string statType, int stacks)
        {
            switch(statType) {
                case STRENGTH:
                case DEFENSE:
                case SPEED:
                case TASTE:
                case TUM:
                case NOSE:
                    unit.AddStatChange(statType, stacks);
                    CombatLog(unit.GetName() + " got " + stacks + " stacks of " + statType);
                    break;
                default:
                    Debug.Log("Non-existent stat type");
                    break;
            }
        }

    // COVERING

        private void Cover()
        {
            // Debug.Log("Cover");
            foreach (KeyValuePair<string, UnitStats> unit in coverUnits) {
                print(unit.Key + " ->  " + unit.Value.GetName());
            }


            if (CurrentAction.Targets.Count == 1) {
                foreach (UnitStats target in CurrentAction.Targets) {
                    CoverUnit(target);
                }
            } else {
                Debug.Log("Can only cover one ally!");
            }

            foreach (KeyValuePair<string, UnitStats> unit in coverUnits) {
                print(unit.Key + " ->  " + unit.Value.GetName());
            }
        }

        private void CoverUnit(UnitStats unit)
        {
            UncoverUnit(CurrentUnit);
            UncoverUnit(unit);

            coverUnits.Add(CurrentUnit.GetName(), unit);

            if (myTurn) {
                RemoveUnitFromList(unit, meleeAllyList);
            } else {
                RemoveUnitFromList(unit, meleeEnemyList);
            }
        }

        private void UncoverUnit(UnitStats unit)
        {
            // print("Uncover Unit");

            if (IsCovering(unit)) {
                // // todo first add the covered unit to the melee list

                //instead of my turn, check the tag on the unit
                if (unit.tag == "Player") {
                    AddUnitToList(coverUnits[unit.GetName()], meleeAllyList);
                } else {
                    AddUnitToList(coverUnits[unit.GetName()], meleeEnemyList);
                }


                //  then remove the covering unit from the cover list
                coverUnits.Remove(unit.GetName());
    


                // leftovers
                // coverUnits.Remove(GetCoveringUnitName(unit));

            } else if (IsCovered(unit)) {

                // todo fist add the covered unit to the melee list

                //instead of my turn, check the tag on the unit
                if (unit.tag == "Player") {
                    AddUnitToList(unit, meleeAllyList);
                } else {
                    AddUnitToList(unit, meleeEnemyList);
                }

                // then remove the covered unit from the cover list

                coverUnits.Remove(GetCoveringUnitName(unit)); //???
            }
        }

        private string GetCoveringUnitName(UnitStats unit)
        {
            return (coverUnits.First(coverPair => coverPair.Value == unit)).Key;
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
            if (coverUnits.ContainsKey(unit.GetName())){
                Debug.Log(unit.GetName());
                return true;
            }

            return false;
        }

        public string CoverStaz(UnitStats unit)
        {
            if (coverUnits.ContainsValue(unit) && coverUnits.ContainsKey(unit.GetName())){
                Debug.Log("ERROR: unit can't cover and be covered.");
            }

            if (coverUnits.ContainsKey(unit.GetName())){
                return "Covering " + coverUnits[unit.GetName()].GetName() + "\n";
            }

            if (coverUnits.ContainsValue(unit)){
                return "Covered by " + GetCoveringUnitName(unit)  + "\n";
            }

            return "";
        }


    // SUMMONING

        private void SummonUnit()
        {
            GameObject summon = Instantiate(CurrentAction.Summon, new Vector3(-6, -3, 0), Quaternion.identity); // hardcoded location, should make dynamic
            UnitStats summonUnit = summon.GetComponent<UnitStats>();

            completeList.Add(summonUnit);

            if (CurrentUnit.tag == "Player") {
            // if (summon.tag == "Player" || summon.tag == "isNPC") {
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
            summonUnit.InitializeUnitFigure();
            summonUnit.InitializeUnitText();

            UpdateUnitPositions();
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
                case "AllAllies":
                    if (myTurn) {
                        SetTargetsByList(allyList);
                    } else {
                        SetTargetsByList(enemyList);
                    }
                    break;
                case "CoverAlly":
                    if (myTurn) {
                        FindTargetsByList(allyList);
                        RemoveSelfFromPossibleTargets();

                        if (IsCovering(CurrentUnit)) {
                            RemoveUnitFromPossibleTargets(coverUnits[CurrentUnit.GetName()]);
                        }
                    } else {
                        FindTargetsByList(enemyList);
                        RemoveSelfFromPossibleTargets();

                        if (IsCovering(CurrentUnit)) {
                            RemoveUnitFromPossibleTargets(coverUnits[CurrentUnit.GetName()]);
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


    // ACTION CALCULATOR

        private string PreviewStrengthDamage(UnitStats targetUnit, int rawDamage)
        {
            string preview = "Attacking " + targetUnit.GetName() + " for ";

            int attackDamage = rawDamage;
            int attackSpeed = CurrentAction.Speed;

            int multiplier = 1;
            int netDamage = 0;

            // basic attacks can hit multiple times if your speed is higher than targets
            if (CurrentAction.isBasicAttack && attackSpeed > targetUnit.GetNetSpeed()) {
                int speedDiff = attackSpeed - targetUnit.GetNetSpeed();

                if (speedDiff >= 15) {
                    multiplier = 3;
                } else if (speedDiff >= 5) {
                    multiplier = 2;
                }
            }

            if (CurrentAction.isBasicAttack && CurrentUnit.GetStatusEffectStatus("BROKENBOTTLE")) {
                attackDamage += 5;
                // CurrentUnit.SubtractStatusEffect("BROKENBOTTLE", 1);
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

            if (CurrentAction.isBasicAttack && targetUnit.GetStatusEffectStatus(FURY)) {
                netDamage += targetUnit.GetStatusEffectStacks(FURY);
            }

            preview += netDamage + " damage";

            if (multiplier > 1) {
                preview += " " + multiplier.ToString() + " times";
            }

            // Debug.Log(netDamage);

            return preview;
        }

        private void TakeStrengthDamage(UnitStats targetUnit, int rawDamage)
        {
            
            int attackDamage = rawDamage;
            int attackSpeed = CurrentAction.Speed;

            int multiplier = 1;
            int netDamage = 0;

            // CHANGE status to BONUS STRENGTH / POWER
            // instead of too specific broken bottle
            // bottle move should just add bonus attack
            if (CurrentAction.isBasicAttack && CurrentUnit.GetStatusEffectStatus("BROKENBOTTLE")) {
                attackDamage += 5;
                CurrentUnit.SubtractStatusEffect("BROKENBOTTLE", 1);
            }

            // MULTI-ATTACK
            if (CurrentAction.BonusHits > 0) {
                multiplier = multiplier + CurrentAction.BonusHits;
            }

            // basic attacks can hit multiple times if your speed is higher than targets
            if (CurrentAction.isBasicAttack && attackSpeed > targetUnit.GetNetSpeed()) {
                int speedDiff = attackSpeed - targetUnit.GetNetSpeed();

                if (speedDiff >= 15) {
                    multiplier = 3;
                } else if (speedDiff >= 5) {
                    multiplier = 2;
                }
            }

            // TODO SEPARATE THIS INTO HIT CALCULATOR METHOD

            // damage is reduced by def if target has DEFEND or by def/2 if no DEFEND
            if (targetUnit.GetStatusEffectStatus(DEFEND)) {
                netDamage = attackDamage - targetUnit.GetNetDefense();
                targetUnit.SubtractStatusEffect(DEFEND, 1); // or you could lose one at EoT
            } else  {
                netDamage = attackDamage - (targetUnit.GetNetDefense() / 2);

                //TODO Add chance of extra +1 damage if %2 remainder
            }

            // do 1 damage per hit minimum
            if (netDamage <= 0) {
                netDamage = 1;
            }

            if (CurrentAction.isBasicAttack && targetUnit.GetStatusEffectStatus(FURY)) {
                netDamage += targetUnit.GetStatusEffectStacks(FURY);
            }

            // Debug.Log(netDamage);


            int hits = 0;
            do {
                int hitDamage = netDamage;

                if (!targetUnit.GetStatusEffectStatus(DODGE)) {
                    hits++;
                    CombatLog(CurrentUnit.GetName() + " hits " + targetUnit.GetName() + " for " + hitDamage + " STR damage");

                    if (targetUnit.GetStatusEffectStatus(ARMOR)) {
                        // Debug.Log(targetUnit.GetStatusEffectStacks(ARMOR));
                        int shieldDamage = hitDamage - targetUnit.GetStatusEffectStacks(ARMOR);
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
                        // Debug.Log(targetUnit.GetStatusEffectStacks(ARMOR));

                    }

                    targetUnit.LoseHealth(hitDamage);
                } else {
                    targetUnit.SubtractStatusEffect(DODGE, 1);
                }

                multiplier--;
            } while (multiplier > 0);

            targetUnit.CreateDamageText(netDamage, Color.red, hits);

            PostDamageUpdates(targetUnit);
        }

        private string PreviewTumDamage(UnitStats targetUnit, int rawDamage)
        {
            string preview = "Dealing ";

            int netDamage = rawDamage - (targetUnit.GetNetTum() / 2);

            // do 1 damage minimum
            if (netDamage <= 0) {
                netDamage = 1;
            }

            preview += netDamage.ToString() + " TUM damage to " + targetUnit.GetName();
            
            return preview;
        }

        private void TakeTumDamage (UnitStats targetUnit, int rawDamage)
        {
            int netDamage = rawDamage - (targetUnit.GetNetTum() / 2);

            // do 1 damage minimum
            if (netDamage <= 0) {
                netDamage = 1;
            }

            targetUnit.LoseHealth(netDamage);

            CombatLog(targetUnit.GetName() + " takes " + netDamage + " TUM damage");

            targetUnit.CreateDamageText(netDamage, Color.red);

            PostDamageUpdates(targetUnit);
        }

        private void TakeTrueDamage (UnitStats targetUnit, int rawDamage)
        {
            int netDamage = rawDamage;

            if (netDamage <= 0) {
                targetUnit.LoseHealth(netDamage);

                CombatLog(CurrentUnit.GetName() + " hits " + targetUnit.GetName() + " for " + netDamage + " TRUE damage");

                PostDamageUpdates(targetUnit);
            } else {
                Debug.Log("Error : TakeTrueDamage : why is this zero?");
            }
        }

        private string PreviewHealDamage (UnitStats targetUnit, int rawHeal)
        {
            string preview = "";

            int netHeal = rawHeal + (rawHeal * targetUnit.GetNetTum()) / 10;

            preview += "Heal " + targetUnit.GetName() + " for " + netHeal.ToString();
            return preview;
        }

        private void HealDamage (UnitStats targetUnit, int rawHeal)
        {
            int netHeal = rawHeal + (rawHeal * targetUnit.GetNetTum()) / 10;

            targetUnit.GainHealth(netHeal);

            targetUnit.CreateDamageText(netHeal, Color.green);

            CombatLog(targetUnit.GetName() + " recovered " + netHeal + " health");
        }

        private string PreviewSateHunger (UnitStats targetUnit, int rawSatiation)
        {
            string preview = "";

            int netGuts = rawSatiation + (rawSatiation * targetUnit.GetNetTaste()) / 10;

            preview += "Sate " + targetUnit.GetName() + " for " + netGuts.ToString() + " GUTS";
            return preview;
        }

        public void SateHunger (UnitStats targetUnit, int rawSatiation)
        {
            int netGuts = rawSatiation + (rawSatiation * targetUnit.GetNetTaste()) / 10;

            targetUnit.GainGuts(netGuts);

            targetUnit.CreateDamageText(netGuts, new Color(1.0f, 0.2f, 0));

            CombatLog(targetUnit.GetName() + " recovered " + netGuts + " guts");
            targetUnit.isStarving = false;
        }

        public string PreviewFamishHunger (UnitStats targetUnit, int rawFamish)
        {
            string preview = "";

            int netGuts = rawFamish - (targetUnit.GetNetNose() / 2);

            preview += "Famished " + targetUnit.GetName() + " for " + netGuts.ToString() + " GUTS";
            return preview;

        }

        public void FamishHunger (UnitStats targetUnit, int rawFamish)
        {
            int netGuts = rawFamish- (targetUnit.GetNetNose() / 2);

            targetUnit.LoseGuts(netGuts);

            targetUnit.CreateDamageText(netGuts, new Color(0.3f, 0.1f, 0));

            CombatLog(targetUnit.GetName() + " lost " + netGuts + " guts");
        }

        private void PostDamageUpdates(UnitStats unit)
        {
            // unit.UpdateText();
            CheckIfDead(unit);
        }

        // private void CheckIfDeadAll()
        // {
        //     completeList.RemoveAll(u => u.isDead);
        // }

        // TODO: there are many references to this, but it should only be used when a unit takes damage/ loses hp
        // or it just sets the isDead attr and then i do unit cleanup at the end of the relevant phases
        private void CheckIfDead(UnitStats unit)
        {
            if (unit.GetCurrentHealth() <= 0) {
                KillUnit(unit);
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
            CombatLog(unit.GetName() + " has GROSS");
            TakeTumDamage(unit, unit.GetStatusEffectStacks("GROSS"));

            // QA this might not work if gross kills the unit and it self destructs
            unit.SubtractStatusEffect("GROSS", 1);

            // DisplayStaz(CurrentUnit);
        }


    // UI

        private void ActivateMoveBox(bool active)
        {
            FindObjectOfType<MoveManager>().ActivateMoveBox(active);
        }

        private void DisplayNextLevelButton()
        {
            FindObjectOfType<MoveManager>().NextLevel();
        }

        private void ActivateNextTurnButton(bool active)
        {
            FindObjectOfType<MoveManager>().ActivateNextTurnButton(active);
        }

        private void ActivateConfirmButton(bool active)
        {
            FindObjectOfType<MoveManager>().ActivateConfirmButton(active);
        }

        private void ActivateCancelButton(bool active)
        {
            FindObjectOfType<MoveManager>().ActivateCancelButton(active);
        }

        private void SetNextTurnButtonText(string text)
        {
            FindObjectOfType<MoveManager>().SetNextTurnButtonText(text);
        }

        private void CleanUpButtons()
        {
            FindObjectOfType<MoveManager>().CleanUpButtons();
        }

        private void DisplayActionButtons(UnitStats unit, bool freeMove, bool freeSkill)
        {
            FindObjectOfType<MoveManager>().DisplayActionButtons(unit, freeMove, freeSkill);
        }

        private void DisplayRecipes(List<Recipe> recipes)
        {
            FindObjectOfType<MoveManager>().DisplayRecipes(recipes);
        }

        private void DisplayIngredients(Inventory ingredients)
        {
            FindObjectOfType<MoveManager>().DisplayIngredients(ingredients);
        }

        private void DisplayMealsToEat(List<Item> meals)
        {
            FindObjectOfType<MoveManager>().DisplayMealsToEat(meals);
        }

        private void DisplayMealsToServe(List<Item> meals, string mealType)
        {
            FindObjectOfType<MoveManager>().DisplayMealsToServe(meals, mealType);
        }

        private void DisplayTargets(List<UnitStats> targets)
        {
            FindObjectOfType<MoveManager>().DisplayTargets(targets);
        }

        private void DisplayNextTurns(List<UnitStats> turnList)
        {
            FindObjectOfType<TurnManager>().DisplayNextTurns(turnList);
        }

        private void DisplayStaz(UnitStats unit)
        {
            FindObjectOfType<StazManager>().ShowStaz(unit);
        }

        private void StazMessage(string message)
        {
            FindObjectOfType<StazManager>().ShowMessage(message);
        }

        private void CombatLog(string text)
        {
            FindObjectOfType<CombatLogManager>().PrintToLog(text);
        }

        private void ClearCombatLog()
        {
            FindObjectOfType<CombatLogManager>().ClearLog();
        }

        private void DisplayRecipeCountdown(int countdown)
        {
            FindObjectOfType<CookingManager>().CreateClock(countdown);
        }

        private void DisplayIngredientIcons(Dictionary<string,int> ingredients)
        {
            FindObjectOfType<CookingManager>().CreateIngredientIcons(ingredients);
        }

        private void DisplayPot()
        {
            FindObjectOfType<CookingManager>().CreatePotIcon();
        }

        private void ClearIngredientIcons()
        {
            FindObjectOfType<CookingManager>().RemoveAllIngredientIcons();
        }

        private void ClearClockIcons()
        {
            FindObjectOfType<CookingManager>().RemoveAllClockIcons();
        }

        private void ClearPotIcon()
        {
            FindObjectOfType<CookingManager>().RemovePotIcon();
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

            DisplayNextLevelButton();
        }

        private void GameOver()
        {
            gameOver = true;
            // print("Game Over");
              
            // TODO this is too sudden, lets add a button like on game win or a yield return pause
            StartCoroutine(LoadGameOver());
        }

        private IEnumerator LoadGameOver()
        {
            yield return new WaitForSeconds(3);

            SceneManager.LoadScene("GameOverScreen");
        }

        // public void HandleDeath(UnitStats unit)
        // {
        //     // unit.isDead = true;
        //     // unit.turnNumber = -1;

        //     // unit.UpdateText();
        //     // unit.SetTurnText("Dead");

        //     // UncoverUnit(unit);

        //     // unit.DestroySelf();
        //     KillUnit(unit);
        //     // UpdateUnitPositions();

        //     CheckGameOver();
        // }

        // public void HandleDeaths()
        // {
        //     completeList.RemoveAll(     unit => unit.isDead == true);
        //     nextUpList.RemoveAll(       unit => unit.isDead == true);
        //     enemyList.RemoveAll(        unit => unit.isDead == true);
        //     allyList.RemoveAll(         unit => unit.isDead == true);
        //     meleeAllyList.RemoveAll(    unit => unit.isDead == true);
        //     meleeEnemyList.RemoveAll(   unit => unit.isDead == true);

        //     CheckGameOver();
        // }

        public void RemoveUnitFromAllLists(UnitStats unit)
        {
            // completeList.Remove(unit);
            // nextUpList.Remove(unit);
            // enemyList.Remove(unit);
            // allyList.Remove(unit);
            RemoveUnitFromList(unit,  completeList);
            RemoveUnitFromList(unit,  nextUpList);
            RemoveUnitFromList(unit,  allyList);
            RemoveUnitFromList(unit,  enemyList);
            RemoveUnitFromList(unit,  meleeAllyList);
            RemoveUnitFromList(unit,  meleeEnemyList);
            RemoveUnitFromList(unit,  allyCookinList);
        }

        public void RemoveUnitFromCookinList(UnitStats unit) {
            RemoveUnitFromList(unit, allyCookinList);
        }


    // AI

        private void AutoMove()
        {
            // all enemy moves are both skill and move?
            freeSkills = 0;

            if (CurrentUnit.GetMovesCount() > 0) {

                MoveButton tryMove = null;

                do {
                    tryMove = CurrentUnit.GetRandomAction();

                    tryMove.SetUpButtonAction();
                } while (!CheckAfford(tryMove.GetAction()));

                canAct = true;
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

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(NextLevel);
        }

        // public void LoadGameOver()
        // {
        //     SceneManager.LoadScene("GameOverScreen");
        // }

        private void InitializeUnitFigures()
        {
            foreach (UnitStats unit in completeList) {
                unit.InitializeUnitFigure();
            }
        }

        private void InitializeUnitStazButtons()
        {
            foreach (UnitStats unit in completeList) {
                unit.InitializeStazButton();
            }
        }

        private void UpdateUnitPositions()
        {
            // TODO: make the units show up in front of who they are blocking

            float yPos      = 300f;
            float xPosAlly  = -150f;
            foreach (UnitStats allyUnit in allyList)
            {
                allyUnit.SetUnitBoxPosition(xPosAlly, yPos);
                xPosAlly -= 200f;
            }

            float xPosEnemy  = 150f;
            foreach (UnitStats enemyUnit in enemyList)
            {
                // Debug.Log(xPosEnemy + " " + yPos);
                enemyUnit.SetUnitBoxPosition(xPosEnemy, yPos);
                xPosEnemy += 200f;
            }
        }

        private void DisplayCurrentUnit()
        {
            print(nextUpList[0]);
        }

        private UnitStats GetUnitByName(string name)
        {
            foreach (UnitStats unit in completeList) {
                if (unit.GetName() == name) {
                    return unit;
                }
            }

            return null;
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

        // private void KillUnit(UnitStats unit)
        // {
        //     unit.isDead = true;
        // }

        // TODO
        // WHEREEVER YOU CALL DESTROYSELF, CALL KILL UNIT INSTEAD
        private void KillUnit(UnitStats unit)
        {
            UncoverUnit(unit);

            RemoveUnitFromAllLists(unit);
            Destroy(unit.gameObject);

            UpdateUnitPositions();

            CheckGameOver();
        }
        // private void DestroyDialogueOnlyUnits()
        // {
        //     for (int i = 0; i < completeList.Count; i++)
        //     {
        //         if (completeList[i].isDialogueOnly)
        //             completeList[i].DestroySelf();
        //     }
        // }
}
