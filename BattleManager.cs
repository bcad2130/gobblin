  
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
    public GameObject HUD;

    public UnitStats CurrentUnit;

    private List<UnitStats> completeList;
    private List<UnitStats> nextUpList;
    private List<UnitStats> allyList;
    private List<UnitStats> enemyList;
    private List<UnitStats> meleeAllyList;
    private List<UnitStats> meleeEnemyList;
    private List<UnitStats> allyCookinList;
    
    private Dictionary<string,string> coverUnits;

    private bool myTurn = false;
    private bool gameOver = false;

    public Action CurrentAction;
    public Inventory playerItems;
    public Recipe recipe;







    // ***************
    // ***FUNCTIONS***
    // ***************


    // INITIALIZATION

    private void Awake()
    {
        StartBattle();
    }

    private void StartBattle()
    {
        InitializeItems();
        InitializeUnitLists();
        InitializeCurrentAction();

        SetHUD();
        SortTurnList();
        SetUnitTurnNumbers();


        // TODO: put this at start of round, check DT's round number (must add round number to DT)
        DialogueTrigger dt = FindObjectOfType<DialogueTrigger>();
        if (dt != null) {
            dt.TriggerDialogue();
        } else {
            StartRound();
        }
        // FindObjectOfType<DialogueTrigger>().TriggerDialogue();


        // StartRound();
    }

    private void InitializeItems()
    {
        playerItems  = new Inventory();

        // start with one cauldron
        playerItems.AddItem("Cauldron");
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

    public void StartRound()
    {
        StartTurn();
    }

    private void StartTurn()
    {
        // print("Turn Start");
        CurrentUnit = WhoseTurn();
        if (CurrentUnit.tag == "Player") {
            myTurn = true;
        }

        // print("Turn Start for: " + CurrentUnit.name);
        PreTurnStatusCheck();

        // ResetCurrentAction();
        // CurrentAction.ParentUnit = CurrentUnit; // can i delete this?


        // TODO: is it really needed to update here?
        // what is actually being updated? just the turn order? seems like not health or mana
        // we're also doing this at end of turn, is there any damage that happens preturn except for poison/gross
        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
        }

        if (CurrentUnit.NPC) {
            EndTurn();
        } else {
            PickUnitMove();
        }
    }

    public void EndTurn()
    {
        UpdateStatus();

        ResetCurrentAction();

        myTurn = false;

        NextTurn();
    }

    private void NextTurn()
    {
        if (!gameOver) {
            StartTurn();
        }
    }

    private void RoundEnd()
    {
        CheckRecipe();
        // CheckCooking();

        SortTurnList();
        SetUnitTurnNumbers();
    }

    private void PickUnitMove()
    {
        if (myTurn) {
            DisplayMoves();
        } else {
            AutoMove();
        }
    }

    private void PreTurnStatusCheck()
    {
        CurrentUnit.CheckPoison();
    }

    private void UpdateStatus() {
        // TickCurrentUnitStatusEffects();

        RemoveCurrentUnitFromNextUpList();
        SetUnitTurnNumbers();

        RemoveAllButtons();

        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
            unit.CheckIfDead();
        }

        HandleDeaths();
        CheckGameOver();
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
        if (nextUpList.Count == 0) {
            RoundEnd();
        }

        return nextUpList[0];
    }

    private void SortTurnList()
    {
        nextUpList = completeList.OrderBy(w => w.speed).Reverse().ToList();

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
                Debug.Log(recipe.cookTime);
                recipe.AddCookTurn();

            } else if (recipe.cookCount == recipe.cookTime) {

                Debug.Log("READY");
                if (CheckIngredients() && CheckStirring() ) {
                    playerItems.AddItem(recipe.result.Name);
                } else {
                    playerItems.AddItem("Burnt Mess");
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

            //         playerItems.AddItem(recipe.result.Name);
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
        foreach (KeyValuePair<string, int> ingredient in recipe.ingredients) {
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
        playerItems.AddItem("Cauldron");
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
                // if (!bm.playerItems.CheckItem(resource.Name)) {
                if (!playerItems.CheckItem(resource)) {
                    // print("return false");
                    return false;
                }
            }
        }

        return true;
    }

    public void AttemptAction(Action action)
    {
        // if (CurrentUnit.CanAffordManaCost(action.ManaCost) && CurrentUnit.CanAffordResourceCost(action.ResourceCost)) {

        if (CurrentUnit.CanAffordManaCost(action.ManaCost) && CanAffordResourceCost(action.ResourceCost)) {
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
                        TakeAction();
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

    public void TakeAction() {
        // print(CurrentAction);

        if (CurrentAction.ManaCost != 0) {
            CurrentUnit.SpendMana(CurrentAction.ManaCost);
        }

        // TODO make it so you can't cover yourself in targeting
        if (CurrentAction.TargetType == "CoverAlly") {
            if (CurrentAction.Targets.Count == 1) {
                foreach (UnitStats target in CurrentAction.Targets) {
                    CoverUnit(target);
                }
            } else {
                Debug.Log("can only cover one ally");
            }
        }

        if (CurrentAction.TargetType == "Stir") {
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
                    recipe.ingredients[ingredient.Name]--;
                }

            }

            CurrentUnit.SpendResource(CurrentAction.ResourceCost);
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

        if (CurrentAction.GiveItem != null) {
            // print("succeess");
            // print (CurrentAction.GiveItem); 
            playerItems.AddItem(CurrentAction.GiveItem.Name);
            // playerItems.PrintItems();
        }


        // HERE WE ARE NOT SENDING THE REAL UNITS TO EFFECT(), JUST COPIES OF THEIR VALUES, SO THE HEALTH DOESN'T UPDATE AND THE UNITSTATS AWAKE STEPS NEVER HAPPENED
        // IF I COULD INSTEAD GRAB THESE TARGETS VIA FIND COMPONENT, IT MIGHT GRAB THE REAL ONES
        if (CurrentAction.Targets.Count != 0) {
            foreach (UnitStats target in CurrentAction.Targets) {
                // print(target);
                // print(target.bm);
                // target.Effect();
                ApplyActionToUnit(target);
            }
        }

        EndTurn();
    }

    public void ApplyActionToUnit(UnitStats unit) {

        // if (CurrentAction.Type == "Basic") {
        //     CurrentUnit.speed
        // }

        if (CurrentAction.Damage > 0) {
            Debug.Log(unit.name + " receives " + CurrentAction.Damage + " raw damage");
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

        // old pre-speed method
        // if (CurrentAction.Damage > 0) {
        //     unit.TakeDamage(CurrentAction.Damage);
        // }

        if (CurrentAction.Heal > 0) {
            unit.HealDamage(CurrentAction.Heal);
        }

        // TODO: i should just use the status effect instead of having a local shield stat
        unit.ApplySingleEffectStatus(CurrentAction.GetAllStatusEffects());

        foreach(KeyValuePair<string, int> effect in CurrentAction.GetAllStatusEffects())
        {
            // do something with effect.Value or effect.Key

            unit.UpdateStatusEffect(effect.Key, effect.Value);
        }

        if (CurrentAction.DestroySelf) {
            // print('h');
            unit.DestroySelf();

            // TODO: must clear from unit lists etc (might have solved this now, will keep testing)
            // on this note, will i have revival? should i even keep dead units around?
            // if not, i should have a more effective way of clearing units, since i might want to move them around, or have them transform, or get "used up" like the cauldron
        }
    }

    private void CoverUnit(UnitStats unitToCover) {
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

    private void SummonUnit() {
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
            case "AnyEnemy":
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

                instantButton.transform.localScale = new Vector3(1.5f,1.5f,1);
                // instantButton.transform.position = new Vector3(20,5,0);
                instantButton.transform.SetParent(HUD.transform, false);
                // second param keeps scale etc the same


                yPos -= 50f;
            }
        } else {
            print ("This unit has no moves");
        }
    }

    public void DisplayTargets() {
        // print("DisplayTargets");
        RemoveAllButtons();

        // if (CurrentAction.PossibleTargets)

        foreach (UnitStats targetableUnit in CurrentAction.PossibleTargets) {
            GameObject canvas = targetableUnit.GetCanvasObj();
            GameObject instantButton = Instantiate(targetButton, new Vector3(40, 0, 0), Quaternion.identity);

            instantButton.transform.SetParent(canvas.transform, false);
            UnitStats tempTargetUnit = targetableUnit;
            instantButton.GetComponent<TargetButton>().SetParentUnit(ref tempTargetUnit);
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

    private void HandleDeaths()
    {
        completeList.RemoveAll(     unit => unit.isDead == true);
        nextUpList.RemoveAll(       unit => unit.isDead == true);
        enemyList.RemoveAll(        unit => unit.isDead == true);
        allyList.RemoveAll(         unit => unit.isDead == true);
        meleeAllyList.RemoveAll(    unit => unit.isDead == true);
        meleeEnemyList.RemoveAll(   unit => unit.isDead == true);
    }

    public void RemoveUnitFromAllLists(UnitStats unit) {
        completeList.Remove(unit);
        nextUpList.Remove(unit);
        enemyList.Remove(unit);
        allyList.Remove(unit);
    }
}
