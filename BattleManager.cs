  
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

    // public GameObject gameOverText;
    // public GameObject gameWinText;
    // public GameObject levelUpText;


    private string nextAction;
    private BattleManager bm;
    private UnitStats unit;
    private UnitStats targetUnit;
    // private UnitStats player;
    // private UnitStats enemy;
    // private GameObject canvas;

    // private List<UnitStats> enemies;
    private List<UnitStats> completeList;
    private List<UnitStats> nextUpList;
    private List<UnitStats> allyList;
    private List<UnitStats> enemyList;

    private bool gameOver = false;


    //I SHOULD GRAB THE UNIT STATS SOONER?

    // Start is called before the first frame update
    private void Start()
    {
        if (bm == null) {
            bm = this.gameObject.GetComponent<BattleManager>();
        }

        // canvas = GameObject.Find("Canvas");

        completeList = new List<UnitStats>();
        nextUpList   = new List<UnitStats>();
        allyList     = new List<UnitStats>();
        enemyList    = new List<UnitStats>();

        // player = GameObject.FindWithTag("Player").GetComponent<UnitStats>();
        // enemy = GameObject.FindWithTag("Enemy").GetComponent<UnitStats>();

        // enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        // GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        // foreach (GameObject enemyObject in enemyObjects) {
        //     enemyList.Add(enemyObject.GetComponent<UnitStats>());
        //     completeList.Add(enemyObject.GetComponent<UnitStats>());
        // }

        PopulateList("Player", ref allyList);

        // foreach (UnitStats ally in allyList) {
        //     completeList.Add(ally);
        // }

        PopulateList("Enemy", ref enemyList);

        // foreach (UnitStats enemy in enemyList) {
        //     completeList.Add(enemy);
        // }

        // completeList.Add(player);
        // completeList.Add(enemy);

        // enemyList.Add(enemy);


        // GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");

        // foreach (GameObject enemyObj in enemyObjs) {
        //     UnitStats current = enemyObj.GetComponent<UnitStats>();
        //     enemyList.Add(current);
        //     completeList.Add(current);
        // }

        // InitialButtonDisplay();
        StartTurn();

    }
    
    private void StartTurn()
    {
        

        // unit.TurnCheck();
        CheckDeaths();
        CheckGameOver();

        if (!gameOver) {
            unit = WhoseTurn();

            DisplayMoves(unit);
        }



        // if (!gameOver) {
        //     // unit.TurnCheck();

        //     nextUpList.RemoveAt(0);

        //     ShowUnitButtons(WhoseTurn());
        // }

        //SendMessage bad?
        // if (unit.TryMove(action,targetUnit)) {
        //     unit.TurnCheck();
        //     CheckDeaths();
        //     CheckGameOver();

        //     if (!gameOver) {
        //         // unit.TurnCheck();

        //         nextUpList.RemoveAt(0);

        //         ShowUnitButtons(WhoseTurn());
        //     }
        // }
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
        RemoveCurrentUnitFromNextUpList();
        RemoveAllButtons();

        StartTurn();
    }

    private void RemoveCurrentUnitFromNextUpList() {
        nextUpList.RemoveAt(0);
    }

    private void DisplayMoves(UnitStats unit) {
        GameObject canvas = unit.GetCanvasObj();
        // GameObject canvas = GameObject.Find(unit.name + "Canvas");
        // print(canvas);
        
        // Iterate this value to make position lower
        float yPos = 10f;

        foreach (Button button in unit.GetMoves()) {
            Button instantButton = Instantiate(button, new Vector3(40, yPos, 0), Quaternion.identity);

            // instantButton.transform.localScale = new Vector3(0.3f,0.3f,1);
            // instantButton.transform.position = new Vector3(20,5,0);

            // second param keeps scale etc the same
            instantButton.transform.SetParent(canvas.transform, false);

            instantButton.GetComponent<MoveButton>().SetParentUnit(unit);

            yPos -= 10f;
        }
    }

    // private void InitialButtonDisplay()
    // {
    //     unit = WhoseTurn();

    //     ShowUnitButtons(unit);
    // }

    private UnitStats WhoseTurn()
    {
        if (nextUpList.Count == 0) {
            RefreshTurnList();
        }

        return nextUpList[0];
        // UnitStats current = nextUpList[0];

        // return current;
    }

    private void RefreshTurnList()
    {
        foreach (UnitStats unit in completeList)
        {
            nextUpList.Add(unit);
        }
        //objList.Sort((emp1, emp2) => emp1.FirstName.CompareTo(emp2.FirstName));

        nextUpList = nextUpList.OrderBy(w => w.speed).ToList();
        nextUpList.Reverse();
        //MAKE SORTING SLIGHTLY RANDOM, BUT HIGHER CHANCE IF HIGH STAT
    }

    private void DisplayCurrentUnit()
    {
        print(nextUpList[0]);
    }

    public void SelectTargets(List<UnitStats> targets, int damage, string statusEffect = null) {
        RemoveAllButtons();
        DisplayTargets(targets, damage, statusEffect);
    }

    public void TakeAction(List<UnitStats> targets, int damage, string statusEffect = null) {
        // target.DoDamage(damage);
        foreach (UnitStats target in targets) {
            // print("doing damage " + damage.ToString());
            target.DoDamage(damage);
        }

        EndTurn();
    }


    private void DisplayTargets(List<UnitStats> targets, int damage, string statusEffect = null) {
        foreach (UnitStats unit in targets) {
            GameObject canvas = unit.GetCanvasObj();

            GameObject instantButton = Instantiate(targetButton, new Vector3(40, 0, 0), Quaternion.identity);

            instantButton.transform.SetParent(canvas.transform, false);
            instantButton.GetComponent<TargetButton>().SetParentUnit(unit);
            instantButton.GetComponent<TargetButton>().SetMove(damage,statusEffect);
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
            gameOver = true;
            GameOver();
        }
        if (enemyList.Count == 0) {
            gameOver = true;
            GameWin();
        }
    }

    private void GameWin()
    {
        print("Game Win");
        // HideAllButtons();

        // gameWinText.SetActive(true);
        // levelUpText.SetActive(true);

        // Invoke("GameWinNext", 2.0f);
    }

    // private void GameWinNext()
    // {
    //     gameWinText.SetActive(false);
    //     levelUpText.SetActive(false);


    //     nextLvlButton.SetActive(true);

    // }

    private void GameOver()
    {
        print("Game Over");
        // HideAllButtons();

        // gameOverText.SetActive(true);
        // player.HideGameObject();
    }

    private void CheckDeaths()
    {
        completeList.RemoveAll(unit => unit.isDead == true);
        nextUpList.RemoveAll(unit => unit.isDead == true);
        enemyList.RemoveAll(unit => unit.isDead == true);
        allyList.RemoveAll(unit => unit.isDead == true);
    }

    // void DisplayobjList()
    // {
    //     foreach (UnitStats item in nextUpList)
    //     {
    //         print(item);
    //     }
    // }

    // public void SetAction(string action, Image buttonImage, bool targetable) {
    //     // print("start");
    //     nextAction = action;
    //     StopAllCoroutines();
    //     if (targetable) {
    //         IEnumerator coroutine = SelectTarget(buttonImage);
    //         StartCoroutine(coroutine);
    //     } else {
    //         buttonImage.color = Color.white;
    //         targetUnit = player;
    //         NextTurn(nextAction);
    //     }

    // }

    // public string GetAction() {
    //     return nextAction;
    // }

    // public void SetTargetToPlayer()
    // {
    //     targetUnit = player;
    // }

    // IEnumerator SelectTarget(Image buttonImage)
    // {
    //     bool targetSet = false;

    //     while (true) {
    //         // print("test");
    //         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
    //         RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
    //         if (hit.collider != null) {
    //             // print("test");
    //             targetUnit = hit.collider.gameObject.GetComponent<UnitStats>();
    //             targetSet = true;

    //             SpriteRenderer renderer = targetUnit.GetComponent<SpriteRenderer>();
    //             renderer.color = Color.red;
    //             if (Input.GetMouseButtonDown(0)) {
    //                 // Debug.Log(hit.collider.gameObject.name);
    //                 ResetEnemyColors();
    //                 NextTurn(nextAction);
    //                 buttonImage.color = Color.white;
    //                 renderer.color = Color.white;
    //                 yield break;
    //             }
    //         } else if (targetSet == true){
    //             SpriteRenderer renderer = targetUnit.GetComponent<SpriteRenderer>();
    //             renderer.color = Color.white;
    //             targetUnit = null;
    //             targetSet = false;
    //         }
    //         yield return null;
    //     }
    // }

    // public void NextTurn(string action)
    // {
        
    //     unit = WhoseTurn();

    //     //SendMessage bad?
    //     // if (unit.TryMove(action,targetUnit)) {
    //     //     unit.TurnCheck();
    //     //     CheckDeaths();
    //     //     CheckGameOver();

    //         if (!gameOver) {
    //             print("not gameOver");
    //             // unit.TurnCheck();

    //             nextUpList.RemoveAt(0);

    //             // ShowUnitButtons(WhoseTurn());
    //         }
    //     // }
    // }

    // private void ShowUnitButtons(UnitStats current)
    // {
    //     switch (current.tag)
    //     {
    //         case "Player":
    //             ShowPlayerButtons();
    //             HideEnemyButtons();

    //             break;
    //         case "Enemy":
    //             ShowEnemyButtons();
    //             HidePlayerButtons();

    //             break;
    //         default:
    //             print("Error");
    //             break;
    //     }
    // }

    // private void ShowEnemyButtons()
    // {
    //     enmButton.SetActive(true);
    // }

    // private void ShowPlayerButtons()
    // {
    //     if (atkButton) {
    //         atkButton.SetActive(true);
    //     }

    //     if (dfdButton) {
    //         dfdButton.SetActive(true);
    //     }

    //     if (psnButton) {
    //         psnButton.SetActive(true);
    //     }

    //     if (healButton) {
    //         healButton.SetActive(true);
    //     }

    //     if (chgButton) {
    //         chgButton.SetActive(true);
    //     }
        
    //     if (lsrButton) {
    //         lsrButton.SetActive(true);
    //     }
    // }

    // private void HideAllButtons()
    // {
    //     HidePlayerButtons();

    //     HideEnemyButtons();     
    // }

    // private void HidePlayerButtons()
    // {
    //     if (atkButton) {
    //         atkButton.SetActive(false);
    //     }

    //     if (dfdButton) {
    //         dfdButton.SetActive(false);
    //     }
        
    //     if (psnButton) {
    //         psnButton.SetActive(false);
    //     } 

    //     if (healButton) {
    //         healButton.SetActive(false);
    //     } 

    //     if (chgButton) {
    //         chgButton.SetActive(false);
    //     } 

    //     if (lsrButton) {
    //         lsrButton.SetActive(false);
    //     }
    // }

    // private void HideEnemyButtons()
    // {
    //     enmButton.SetActive(false);
    // }

    // private void ResetEnemyColors()
    // {
    //     foreach (UnitStats enemy in enemyList) {
    //         // print(renderer.color);
    //         SpriteRenderer renderer = enemy.GetComponent<SpriteRenderer>();
    //         renderer.color = Color.white;
    //     }
    // }
}

//TODO: make speed, randomly assign turn order
//do damage
//stop game when dead or win
// 