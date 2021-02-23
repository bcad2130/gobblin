  
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
    private UnitStats currentUnit;
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

    public Action CurrentAction { get; set; }

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

        PopulateList("Player", ref allyList);
        PopulateList("Enemy", ref enemyList);
        // SetUnitTurnNumbers();


        StartTurn();

    }
    
    private void StartTurn()
    {
        currentUnit = WhoseTurn();

        PreTurnStatusCheck();

        CurrentAction = new Action();
        CurrentAction.ParentUnit = currentUnit;


        // TODO: is it really needed to update here?
        // what is actually being updated? just the turn order? seems like not health or mana
        foreach (UnitStats unit in completeList) {
            unit.UpdateText();
        }

        if (currentUnit.tag == "Player") {
            DisplayMoves();
        } else {
            AutoMove();
        }

    }

    private void PreTurnStatusCheck() {
        currentUnit.CheckPoison();
    }

    private void PopulateList(string tag, ref List<UnitStats> list) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects) {
            list.Add(obj.GetComponent<UnitStats>());
            // also add all groups to the full list
            completeList.Add(obj.GetComponent<UnitStats>());
        }
    }

    public void EndTurn() {
        UpdateStatus();

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

    // private void TickCurrentUnitStatusEffects() {
    //     currentUnit.TickStatusEffects();
    //     // foreach (keyvaluecurrentUnit.GetAllStatusEffects()


    //     // PoisonTicker();
    // }

    private void RemoveCurrentUnitFromNextUpList() {
        nextUpList[0].turnNumber = 0;
        nextUpList.RemoveAt(0);
    }

    private void DisplayMoves() {
        GameObject canvas = currentUnit.GetCanvasObj();
        
        // Text tempTextBox = Instantiate(textPrefab, nextPosition, transform.rotation) as Text;
        //          //Parent to the panel
        //           tempTextBox.transform.SetParent(renderCanvas.transform, false);
        //           //Set the text box's text element font size and style:
        //            tempTextBox.fontSize = defaultFontSize;
        //            //Set the text box's text element to the current textToDisplay:
        //            tempTextBox.text = textToDisplay;

        // Iterate this value to make position lower
        float yPos = 10f;

        // print(currentUnit.GetMoves());

        if (currentUnit.GetMoves().Count > 0) {
            foreach (Button button in currentUnit.GetMoves()) {
                Button instantButton = Instantiate(button, new Vector3(40, yPos, 0), Quaternion.identity);

                // instantButton.transform.localScale = new Vector3(0.3f,0.3f,1);
                // instantButton.transform.position = new Vector3(20,5,0);

                // second param keeps scale etc the same
                instantButton.transform.SetParent(canvas.transform, false);

                instantButton.GetComponent<MoveButton>().SetParentUnit(currentUnit);

                yPos -= 10f;
            }
        } else {
            print ("This unit has no moves");
        }
    }

    private void AutoMove() {
        GameObject canvas = currentUnit.GetCanvasObj();

        List<Button> currentUnitMoves = currentUnit.GetMoves();

        if (currentUnitMoves.Count > 0) {
            var randomIndex = Random.Range(0, currentUnitMoves.Count);

            Button selectedMove = currentUnitMoves[randomIndex];

            Button instantButton = Instantiate(selectedMove, new Vector3(40, 0, 0), Quaternion.identity);
            // foreach (Button button in currentUnit.GetMoves()) {
            //     Button instantButton = Instantiate(button, new Vector3(40, yPos, 0), Quaternion.identity);

            //     // instantButton.transform.localScale = new Vector3(0.3f,0.3f,1);
            //     // instantButton.transform.position = new Vector3(20,5,0);

            //     // second param keeps scale etc the same
            instantButton.transform.SetParent(canvas.transform, false);

            instantButton.GetComponent<MoveButton>().SetParentUnit(currentUnit);

            //     yPos -= 10f;
            // }
        } else {
            print ("This unit has no moves");
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

    public void TakeAction() {
        print("t");
        CurrentAction.ParentUnit.SpendMana(CurrentAction.ManaCost);
        CurrentAction.ParentUnit.SpendResource(CurrentAction.ResourceCost);


        // TODO: i should reorganize the units automatically. So if there are a ton of units summoned, they get properly organized on the screen
        // i should have certain slots premade (enumerated?) and then it fills in the next available ally/enemy spot
        // then i can have a certain number of ally spots available as you level
        // and maybe a certain number of cauldron spots, upgradeable
        if (CurrentAction.Summon) {
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

        // print(CurrentAction.GiveItem);

        if (CurrentAction.GiveItem != null) {
            // print("succeess");
            // print (CurrentAction.GiveItem); 
            playerItems.AddItem(CurrentAction.GiveItem.Name);
            // playerItems.PrintItems();
        }

        if (CurrentAction.Targets != null) {
            foreach (UnitStats target in CurrentAction.Targets) {
                target.Effect();
            }
        }

        // foreach (UnitStats x in completeList) {
        //     print(x.name);
        // }

        EndTurn();
    }


    public void DisplayTargets() {
        RemoveAllButtons();

        foreach (UnitStats targetableUnit in CurrentAction.PossibleTargets) {
            GameObject canvas = targetableUnit.GetCanvasObj();
            GameObject instantButton = Instantiate(targetButton, new Vector3(40, 0, 0), Quaternion.identity);

            instantButton.transform.SetParent(canvas.transform, false);
            instantButton.GetComponent<TargetButton>().SetParentUnit(targetableUnit);
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

    // private void PoisonTicker() {
    //     Dictionary<string,int> currentStatusEffects = currentUnit.GetAllStatusEffects();
    //     // if poisoned, do damage for number of stacks, then tick down poison stacks by one
    //     if (currentStatusEffects.ContainsKey("POISON")) {
    //         print("ContainsKey");
    //         if (currentStatusEffects["POISON"] > 0) {
    //             print(currentStatusEffects["POISON"]);
    //             currentUnit.TakeDamage(currentStatusEffects["POISON"]);
    //             currentUnit.UpdateStatusEffect("POISON", currentStatusEffects["POISON"] - 1);
    //         }
    //     }
    // }
}

//TODO: make speed, randomly assign turn order
//do damage
//stop game when dead or win
// 