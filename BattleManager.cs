  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleManager : MonoBehaviour
{

    // public GameObject atkButton;
    // public GameObject dfdButton;
    // public GameObject psnButton;
    // public GameObject healButton;
    // public GameObject chgButton;
    // public GameObject lsrButton;
    // public GameObject enmButton;
    // public GameObject nextLvlButton;
    public GameObject targetButton;
    // public Text unitNamePrefab;

    // private int defaultUnitFontSize = 24;
    // public GameObject gameOverText;
    // public GameObject gameWinText;
    // public GameObject levelUpText;


    // private string nextAction;
    // private BattleManager bm;
    public UnitStats CurrentUnit;
    // private UnitStats targetUnit;
    // private UnitStats player;
    // private UnitStats enemy;
    // private GameObject canvas;

    // private List<UnitStats> enemies;
    private List<UnitStats> completeList;
    private List<UnitStats> nextUpList;
    private List<UnitStats> allyList;
    private List<UnitStats> enemyList;

    private bool gameOver = false;

    public Action CurrentAction;

    // [SerializeField]
    public Inventory playerItems;

    // // Start is called before the first frame update
    // private void Start()
    private void Awake()
    {
        playerItems  = new Inventory();

        completeList = new List<UnitStats>();
        nextUpList   = new List<UnitStats>();
        allyList     = new List<UnitStats>();
        enemyList    = new List<UnitStats>();

        CurrentAction = new Action();

        PopulateList("Player", ref allyList);
        PopulateList("Enemy", ref enemyList);
        // SetUnitTurnNumbers();

        StartTurn();

    }

    private void PopulateList(string tag, ref List<UnitStats> list) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects) {
            list.Add(obj.GetComponent<UnitStats>());
            // also add all groups to the full list
            completeList.Add(obj.GetComponent<UnitStats>());
        }
    }

    private void StartTurn()
    {
        print("Turn Start");
        CurrentUnit = WhoseTurn();
        print(CurrentUnit.name);
        PreTurnStatusCheck();

        ResetCurrentAction();
        // CurrentAction.ParentUnit = CurrentUnit; // can i delete this?


        // TODO: is it really needed to update here?
        // what is actually being updated? just the turn order? seems like not health or mana
        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
        }

        if (CurrentUnit.tag == "Player") {
            DisplayMoves();
        } else {
            AutoMove();
        }

    }

    private void PreTurnStatusCheck() {
        CurrentUnit.CheckPoison();
    }

    public void EndTurn() {
        UpdateStatus();

        // ResetCurrentAction();

        NextTurn();
    }

    private void NextTurn() {
        if (!gameOver) {
            StartTurn();
        }
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

    // public Action GetCurrentAction() {
    //     return CurrentAction;
    // }

    // private void TickCurrentUnitStatusEffects() {
    //     CurrentUnit.TickStatusEffects();
    //     // foreach (keyvaluecurrentUnit.GetAllStatusEffects()


    //     // PoisonTicker();
    // }

    private void RemoveCurrentUnitFromNextUpList() {
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

    private void RoundEnd() {
        SortTurnList();
        SetUnitTurnNumbers();
    }

    private void SortTurnList()
    {
        nextUpList = completeList.OrderBy(w => w.speed).Reverse().ToList();

        // SetUnitTurnNumbers();

        // foreach (UnitStats unit in completeList)
        // {
        //     nextUpList.Add(unit);
        // }
        //objList.Sort((emp1, emp2) => emp1.FirstName.CompareTo(emp2.FirstName));

        // nextUpList = nextUpList.OrderBy(w => w.speed).Reverse().ToList();
        // nextUpList.Reverse();
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

    private void DisplayCurrentUnit()
    {
        print(nextUpList[0]);
    }

    public void SetAction(Action action) {
        CurrentAction = action;
        // print(action);
    }

    private void DisplayMoves() {
        GameObject canvas = CurrentUnit.GetCanvasObj();
        
        // Text tempTextBox = Instantiate(textPrefab, nextPosition, transform.rotation) as Text;
        //          //Parent to the panel
        //           tempTextBox.transform.SetParent(renderCanvas.transform, false);
        //           //Set the text box's text element font size and style:
        //            tempTextBox.fontSize = defaultFontSize;
        //            //Set the text box's text element to the current textToDisplay:
        //            tempTextBox.text = textToDisplay;

        // Iterate this value to make position lower
        float yPos = 10f;

        // print(CurrentUnit.GetMoves());

        if (CurrentUnit.GetMoves().Count > 0) {
            foreach (MoveButton button in CurrentUnit.GetMoves()) {
                // print(button);
                MoveButton instantButton = Instantiate(button, new Vector3(40, yPos, 0), Quaternion.identity);

                // instantButton.transform.localScale = new Vector3(0.3f,0.3f,1);
                // instantButton.transform.position = new Vector3(20,5,0);

                // second param keeps scale etc the same
                instantButton.transform.SetParent(canvas.transform, false);

                // instantButton.GetComponent<MoveButton>().SetParentUnit(CurrentUnit);

                yPos -= 10f;
            }
        } else {
            print ("This unit has no moves");
        }
    }

    public void ResetCurrentAction() {
        // CurrentAction = null;
        // CurrentAction = new Action();
        CurrentAction = null;

    }

    private void AutoMove() {
        // print("AutoMove");
        GameObject canvas = CurrentUnit.GetCanvasObj();

        List<MoveButton> currentUnitMoves = CurrentUnit.GetMoves();

        if (currentUnitMoves.Count > 0) {

            MoveButton currentMove = CurrentUnit.GetRandomAction();

            CurrentAction = currentMove.GetAction();

            // CurrentAction = CurrentUnit.GetRandomAction();

            // PickRandomMove();

            print(currentMove);

            // USE THESE IF YOU WANT THE MOVE AND TARGET TO BE PICKED AUTOMAGICALLY (not working yet)
            // switch (CurrentAction.TargetType)
            // {
            //     case "OneAlly": 
            //     case "OneEnemy":
            //         AutoSetTargets();
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
            
            AutoSetTargets();

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

    // private void PickRandomMove() {
    //     // print("PickRandomMove");
    //     // List<Button> currentUnitMoves = CurrentUnit.GetMoves();

    //     // var randomIndex = Random.Range(0, currentUnitMoves.Count);

    //     // Button tempActionButton = currentUnitMoves[randomIndex];

    //     // CurrentAction = tempActionButton;

    //     // CurrentAction.AISetUpMove();


    //     // if (CurrentUnit.CanAffordManaCost(tempActionButton.ManaCost) && CurrentUnit.CanAffordResourceCost(tempActionButton.ResourceCost)) {
    //     //     CurrentAction = tempAction;
    //     //     CurrentAction.AISetUpMove();
    //     // } else {
    //     //     PickRandomMove();
    //     // }
    // }

    private void AutoSetTargets() {
        switch (CurrentAction.TargetType) 
        {
            case "Self":
                SetTargetToSelf();
                break;
            case "OneAlly": 
                FindTargetsByTag(CurrentUnit.tag);
                SelectRandomUnitFromPossibleTargets();
                break;
            case "OneEnemy":
                if (CurrentUnit.tag == "Enemy") {
                    FindTargetsByTag("Player");
                } else {
                    FindTargetsByTag("Enemy");
                }
                SelectRandomUnitFromPossibleTargets();
                break;
            case "AllEnemies":
                if (CurrentUnit.tag == "Enemy") {
                    SetTargetsToAllTagged("Player");
                } else {
                    SetTargetsToAllTagged("Enemy");
                }
                break;
            case "Targetless":
                break;
            default:
                print("Invalid target");
                break;
        }




        // var randomIndex = Random.Range(0, CurrentAction.PossibleTargets.Count);

        // CurrentAction.AddTarget(ref CurrentAction.PossibleTargets[randomIndex]);
    }

    private void SelectRandomUnitFromPossibleTargets() {
        UnitStats selectedUnit = CurrentAction.PossibleTargets[Random.Range(0, CurrentAction.PossibleTargets.Count)];

        CurrentAction.AddTarget(ref selectedUnit);
    }

    public void AttemptAction(Action action) {
        // CurrentAction.SetUpMove();

        // SetUpMove();

        // JUST FOR TESTING: SET ALL TO MOVES TO BASIC ATTACK
        // CurrentAction.TargetType = "OneEnemy";
        // CurrentAction.ManaCost = 0;
        // CurrentAction.Damage = CurrentUnit.attack;

        if (CurrentUnit.CanAffordManaCost(action.ManaCost) && CurrentUnit.CanAffordResourceCost(action.ResourceCost)) {
            SetAction(action);
            SetTargets();

            switch (CurrentAction.TargetType)
                {
                    case "OneAlly": 
                    case "OneEnemy":
                        DisplayTargets();
                        break;
                    case "Self":
                    case "AllEnemies":
                    case "Targetless":
                        TakeAction();
                        break;
                    default:
                        print("Invalid target");
                        break;
                }
        } else {
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

        if (CurrentAction.ResourceCost.Count != 0) {
            CurrentUnit.SpendResource(CurrentAction.ResourceCost);
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

        // foreach (UnitStats x in completeList) {
        //     print(x.name);
        // }

        EndTurn();
    }

    public void ApplyActionToUnit(UnitStats unit) {

        if (CurrentAction.Damage > 0) {
            unit.TakeDamage(CurrentAction.Damage);
        }

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

            // TODO: must clear from unit lists etc
            // on this note, will i have revival? should i even keep dead units around?
            // if not, i should have a more effective way of clearing units, since i might want to move them around, or have them transform, or get "used up" like the cauldron
        }
    }

    // TODO: i should reorganize the units automatically. So if there are a ton of units summoned, they get properly organized on the screen
    // i should have certain slots premade (enumerated?) and then it fills in the next available ally/enemy spot
    // then i can have a certain number of ally spots available as you level
    // and maybe a certain number of cauldron spots, upgradeable
    private void SummonUnit() {
        GameObject summon = Instantiate(CurrentAction.Summon, new Vector3(-6, -3, 0), Quaternion.identity); // hardcoded location, should make dynamic

        if (summon.tag == "Player") {
            allyList.Add(summon.GetComponent<UnitStats>());
        } else if (summon.tag == "Enemy") {
            enemyList.Add(summon.GetComponent<UnitStats>());
        } else {
            print("who is this?");
        }

        completeList.Add(summon.GetComponent<UnitStats>());
    }

    public void SetTargets() {
        // print("SetTargets");
        switch (CurrentAction.TargetType) 
        {
            case "Self":
                // print('a');
                SetTargetToSelf();
                break;
            case "OneAlly": 
                FindTargetsByTag(CurrentUnit.tag);
                break;
            case "OneEnemy":
                if (CurrentUnit.tag == "Enemy") {
                    FindTargetsByTag("Player");
                } else {
                    FindTargetsByTag("Enemy");
                }
                break;
            case "AllEnemies":
                if (CurrentUnit.tag == "Enemy") {
                    SetTargetsToAllTagged("Player");
                } else {
                    SetTargetsToAllTagged("Enemy");
                }
                break;
            case "Targetless":
                break;
            default:
                print("Invalid target");
                break;
        }
    }

    private void SetTargetToSelf() {
        CurrentAction.AddTarget(ref CurrentUnit);
    }

    private void FindTargetsByTag(string tag) {
        // print("FindTargetsByTag");
        // TODO use bm to grab the enemy list or ally list as appropriate
        // maybe actions should have a "targetable group" property, like enemies, allies, dead units, etc

        // thinking that units should be organized in squads
        // basically grouped into groups of 4, if you kill enough, the groups collapse into new groups

        GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetableObject in targetableObjects) {
            UnitStats unit = targetableObject.GetComponent<UnitStats>();
            if (!unit.isDead) {
                CurrentAction.AddPossibleTarget(ref unit);
            }
        }
    }

    private void SetTargetsToAllTagged(string tag) {
        // print("SetTargetsToAllTagged");
        GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetableObject in targetableObjects) {
            UnitStats unit = targetableObject.GetComponent<UnitStats>();
            if (!unit.isDead) {
                CurrentAction.AddTarget(ref unit);
            }
        }
    }

    public void AddTargetToAction(ref UnitStats unit) {
        CurrentAction.AddTarget(ref unit);
    }

    public void DisplayTargets() {
        // print("DisplayTargets");
        RemoveAllButtons();

        // for (int i = 0; i < CurrentAction.PossibleTargets; i++) {
        //     GameObject canvas = CurrentAction.PossibleTargets[i].GetCanvasObj();
        //     GameObject instantButton = Instantiate(targetButton, new Vector3(40, 0, 0), Quaternion.identity);

        //     instantButton.transform.SetParent(canvas.transform, false);
        //     instantButton.GetComponent<TargetButton>().SetParentUnit(ref CurrentAction.PossibleTargets[i]);
        // }

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
        completeList.RemoveAll(unit => unit.isDead == true);
        nextUpList.RemoveAll(unit => unit.isDead == true);
        enemyList.RemoveAll(unit => unit.isDead == true);
        allyList.RemoveAll(unit => unit.isDead == true);
    }

    public void RemoveUnitFromAllLists(UnitStats unit) {
        completeList.Remove(unit);
        nextUpList.Remove(unit);
        enemyList.Remove(unit);
        allyList.Remove(unit);
    }
    // private void PoisonTicker() {
    //     Dictionary<string,int> currentStatusEffects = CurrentUnit.GetAllStatusEffects();
    //     // if poisoned, do damage for number of stacks, then tick down poison stacks by one
    //     if (currentStatusEffects.ContainsKey("POISON")) {
    //         print("ContainsKey");
    //         if (currentStatusEffects["POISON"] > 0) {
    //             print(currentStatusEffects["POISON"]);
    //             CurrentUnit.TakeDamage(currentStatusEffects["POISON"]);
    //             CurrentUnit.UpdateStatusEffect("POISON", currentStatusEffects["POISON"] - 1);
    //         }
    //     }
    // }
}

//TODO: make speed, randomly assign turn order
//do damage
//stop game when dead or win
// 