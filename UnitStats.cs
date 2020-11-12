using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    public int health;
    public int mana;
    public int attack;
    // public int magic;
    public int speed;
    public bool isDead;

    //dont need name cuz its the same as Object.name
    // public string name;
    // public string defeatMessage = "Oh No.";

    // public GameObject healthTextObj;
    // public GameObject manaTextObj;
    // public GameObject damageTextObj;
    // public GameObject shieldObj;
    // public GameObject poisonTextObj;
    // public GameObject chargeUpTextObj;

    // public List<string> randomMoves;

    // private bool randomMovesSet = false;
    // private bool isDefending = false;
    // private bool isChargedUp = false;
    // private Text damageText;
    // private Text healthText;
    // private Text manaText;
    private BattleManager bm;
    // private bool isPoisoned = false;
    // private int poisonTimer = 4;
    // private int poisonDamage = 4;
    // private float damageDisplayTime = 0.5f;

    // List<MoveDelegate> randomMoves;
    // private List<string> randomMoveStrings;
    // private Dictionary<string, int> manaCostTable;


    // delegate bool MoveDelegate(UnitStats target);

    // //mana cost table
    // int attackCost = 0;
    // int shieldCost = 3;
    // int poisonCost = 3;
    // int healCost   = 5;
    // int chargeCost = 5;
    // int laserCost  = 8;


    //new stuff
    public Text healthTextPrefab;
    private Text healthText;

    public Text manaTextPrefab;
    private Text manaText;


    public GameObject canvas;

    // int defaultFontSize = 40;

    // public Button attackButtonPrefab;
    // public Canvas canvasPrefab;

    public List<Button> moves;

    private float yPosHP = -112.98f;
    private float yPosMP = -152.98f;


    // Start is called before the first frame update
    public void Start()
    {
        // print("real start: " + name);
        bm = GameObject.FindObjectOfType<BattleManager>();

        // set moves
        // SetMoves();
        // print('t');


        // Text tempTextBox = Instantiate<Text>(healthTextPrefab);

        // find canvas
        // canvas = GameObject.Find(this.name + "Canvas");

        // print(gameObject.transform.position);

        // // Create Canvas
        // GameObject canvasGameObj;
        // Canvas canvas;

        // // Canvas
        // canvasGameObj = new GameObject();
        // canvasGameObj.name = Object.name + "Canvas";
        // canvasGameObj.AddComponent<Canvas>();

        // myCanvas = canvasGameObj.GetComponent<Canvas>();
        // myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // canvasGameObj.AddComponent<CanvasScaler>();
        // canvasGameObj.AddComponent<GraphicRaycaster>();


        // Canvas canvas = new Canvas();
        // GameObject canvas = Instantiate();

        // clone your prefab
        // Text healthText = Instantiate(healthTextPrefab);
        float xPos = 0f;
        if (gameObject.tag == "Player") {
            xPos = -171.95f;
        } else {
            // xPos = 171.95f;
            xPos = 0;
            yPosHP = 0;
            yPosMP = 0;
        }
        healthText = Instantiate(healthTextPrefab, new Vector3(xPos, yPosHP, 0), Quaternion.identity);
        healthText.transform.localScale = new Vector3(0.3f,0.3f,1);
        healthText.transform.position = new Vector3(15,5,0);

        // set canvas as parent to the text
        healthText.transform.SetParent(canvas.transform, false);
        // Set the text element to the current textToDisplay:
        healthText.text = health.ToString();


        manaText = Instantiate(manaTextPrefab, new Vector3(xPos, yPosMP, 0), Quaternion.identity);
        manaText.transform.localScale = new Vector3(0.3f,0.3f,1);
        manaText.transform.position = new Vector3(15,-5,0);

        // set canvas as parent to the text
        manaText.transform.SetParent(canvas.transform, false);
        // Set the text element to the current textToDisplay:
        manaText.text = mana.ToString();

        // healthText.transform.localScale = new Vector3(1,1,1);
        //Set the text element font size and style:
        // healthText.fontSize = defaultFontSize;

        // Text tempTextBox = Instantiate(healthTextPrefab, nextPosition, transform.rotation) as Text;
        //    //Parent to the panel
        // tempTextBox.transform.SetParent(renderCanvas.transform, false);
        //    //Set the text box's text element font size and style:
        // tempTextBox.fontSize = defaultFontSize;
        //    //Set the text box's text element to the current textToDisplay:
        // tempTextBox.text = textToDisplay;

/*
        healthText = healthTextObj.GetComponent<Text>();
        manaText = manaTextObj.GetComponent<Text>();
        damageText = damageTextObj.GetComponent<Text>();

        BuildManaCostTable();

        UpdateHealthText();
        UpdateManaText();
*/

        // Button attackButton = Instantiate(attackButtonPrefab);
        // // second param keeps scale etc the same
        // attackButton.transform.SetParent(canvas.transform, false);



        // print(this);
    }

    public GameObject GetCanvasObj() {
        return canvas;
    }

    public List<Button> GetMoves()
    {
        return moves;
    }

    // private void SetMoves()
    // {
    //     moves = new List<Button>();
    //     moves.Add(attackButtonPrefab);
    // }

    public void DoDamage(int damage) {
        health -= damage;
        UpdateHealthText();
        if (health <= 0) {
            isDead = true;
        }
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

    private void UpdateManaText()
    {
        manaText.text = mana.ToString();
    }
    // void Update() 
    // {
    // //  if( Input.GetMouseButtonDown(0) )
    // //  {
    // //      print("click");
    // //      Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
    // //      RaycastHit hit;
         
    // //      if( Physics.Raycast( ray, out hit, 100.0f ) )
    // //      {
    // //          print('t');
    // //          Debug.Log( hit.transform.gameObject.name );
    // //      }
    //     if (Input.GetMouseButtonDown(0)) {
    //         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
    //         RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
    //         if (hit.collider != null) {
    //             Debug.Log(hit.collider.gameObject.name);
    //             hit.collider.attachedRigidbody.AddForce(Vector2.up);
    //         }
    //     }

    //     // if(Input.GetMouseButtonDown(0)) {
    //     //     print(this.name);
    //     //     string currentAction = bm.GetAction();
    //     //     if(!string.IsNullOrEmpty(currentAction) ) {
    //     //         UnitStats current = bm.WhoseTurn();
    //     //         current.TakeAction(currentAction, this);
    //     //     }
    //     // }
    // }

    // public void Damage(int damage)
    // {
    //     if (!isDefending) { //Succeeded
    //         // print('s');
    //         health = health - damage;

    //     } else { //Defended
    //         // print('d');
    //         damage = 0;
    //         shieldObj.SetActive(false);
    //         isDefending = false;
    //     }

    //     ShowDamage(damage);

    //     UpdateHealthText();
    //     damageText.text = damage.ToString();

    //     if (health <= 0) {
    //         isDead = true;
    //         // HideUnit();
    //         HideGameObject();
    //         // this.SetActive(false);
    //     }
    // }

    // private void UpdateHealthText()
    // {
    //     healthText.text = health.ToString();
    // }

    // private void UpdateManaText()
    // {
    //     manaTextObj.GetComponent<Text>().color = new Color(1,1,1,1);
    //     StopAllCoroutines();

    //     manaText.text = mana.ToString();
    // }

    // public void SetDefending(bool defSetter)
    // {
    //     shieldObj.SetActive(defSetter);

    //     isDefending = defSetter;
    //     // print(isDefending);
    // }

    // private void ShowDamage(int damage)
    // {
    //     damageTextObj.SetActive(false);
    //     damageTextObj.SetActive(true);
    //     damageText.text = damage.ToString();

    //     Invoke("HideDamageText", damageDisplayTime);
    // }

    // private void ShowPoisonDamage()
    // {
    //     poisonTextObj.SetActive(true);
    //     Invoke("HidePoisonDamage", damageDisplayTime);
    // }

    // public void PoisonedUnitDamage()
    // {
    //     health = health - poisonDamage;

    //     poisonTimer--;
    //     if (poisonTimer <=0) {
    //        isPoisoned = false; 
    //     }
    //     poisonTextObj.GetComponent<Text>().text = "Psn: 4";

    //     ShowPoisonDamage();

    //     UpdateHealthText();
    // }

    // public void TurnCheck() 
    // {
    //     // print()
    //     // print("poisonTimer: " + poisonTimer);
    //     if (isPoisoned) {
    //         PoisonedUnitDamage();
    //     }
    // }

    // public void TakeAction(string action, UnitStats target)
    // {
    //     // Invoke(action)
    //     // SendMessage(action,target);
    //     // switch(action)
    //     // {
    //     //     case "Attack":
    //     //         AttackUnit(target);
    //     //         break;
    //     //     case "Defend":
    //     //         ShieldUnit(target);
    //     //         break;
    //     //     case "Poison":
    //     //         PoisonUnit(target);
    //     //         break;
    //     //     case "Heal":
    //     //         HealUnit(target);
    //     //         break;
    //     //     case "Random":
    //     //         RandomMoveToUnit(target);
    //     //         break;
    //     //     default:
    //     //         print("Error");
    //     //         break;
    //     // }
    // }

    // public bool TryMove(string action, UnitStats target) {
    //     if (action == "RandomMoveToUnit") {
    //         // print("random");
    //         action = RandomMoveToUnit();
    //         if(isChargedUp) {
    //             action = "AttackUnit";
    //         }

    //         // if(CheckManaCost(randomMove)) {
    //         //     action = randomMove;
    //         //     // print(action);
    //         // } else {
    //         //     print("not enuff mana");
    //         //     // target = GameObject.FindGameObjectWithTag()
    //         //     TryMove("RandomMoveToUnit", target);
    //         //     return false;
    //         // }

    //     }

    //     if (CheckManaCost(action)) {
    //         SendMessage(action,target);
    //         return true;
    //     } else {
    //         if(gameObject.tag == "Enemy") {
    //             SendMessage("AttackUnit",target);
    //             return true;
    //         }
    //         print("Move error");
    //         return false;
    //     }
    // }

    // private void BuildManaCostTable() {
    //     // int attackCost = 0;
    //     // int shieldCost = 2;
    //     // int poisonCost = 3;
    //     // int healCost   = 5;
    //     // int chargeCost = 5;
    //     // int laserCost  = 10;

    //     manaCostTable = new Dictionary<string, int>();
    //     manaCostTable.Add("AttackUnit",   attackCost);
    //     manaCostTable.Add("ShieldUnit",   shieldCost);
    //     manaCostTable.Add("PoisonUnit",   poisonCost);
    //     manaCostTable.Add("HealUnit",     healCost);
    //     manaCostTable.Add("ChargeUpUnit", chargeCost);
    //     manaCostTable.Add("LaserUnit",    laserCost);
    // }

    // private bool CheckManaCost(string action) {
    //     if(action == "RandomMoveToUnit") {
    //         print(action);
    //     }
    //     int manaCost = manaCostTable[action];

    //     if (mana - manaCost >= 0) {
    //         mana = mana - manaCost;
    //         UpdateManaText();
    //         return true;
    //     } else {
    //         if(gameObject.tag == "Player") {
    //            FlashManaText();
    //         }
    //         return false;
    //     }
    // }

    // public bool AttackUnit(UnitStats target)
    // {
    //     target.damageText.color = new Color(0.196f,0.196f,0.196f) ;

    //     int attackPower = attack;
    //     // target.damageText.color = Color.white;

    //     if(isChargedUp){
    //         // print('e');
    //         attackPower = attackPower*3;
    //         isChargedUp = false;
    //         // chargeUpTextObj.SetActive(true);
    //         chargeUpTextObj.SetActive(false);
    //         target.damageText.fontSize = 42;
    //         // target.damageText.fontStyle
    //     }
    //     // print(name);
    //     // print(attackPower);
    //     target.Damage(attackPower);
    //     target.damageText.fontSize = 24;
        
    //     return true;

    // }

    // public bool ShieldUnit(UnitStats target) {
    //         damageText.color = new Color(0.196f,0.196f,0.196f) ;

    //         SetDefending(true);

    //         ShowDamage(0);
    //         damageText.text = "Defending!";

    //         return true;
    // }

    // public bool PoisonUnit(UnitStats target)
    // {
    //     target.damageText.color = new Color(0.196f,0.196f,0.196f) ;

    //     // print("poison");
    //     target.isPoisoned = true;
    //     target.poisonTimer = magic;
    //     target.ShowDamage(0);
    //     // target.damageText.color = Color.white;

    //     target.damageText.text = "Poisoned!";
    //     target.poisonTextObj.GetComponent<Text>().text = "Psnd " + magic.ToString() + "turns";
    //     target.ShowPoisonDamage();

    //     poisonTextObj.GetComponent<Text>().text = "Psn: 4";


    //     return true;
    // }

    // public bool HealUnit(UnitStats target)
    // {
    //     var healPower = magic;
    //     if (isChargedUp) {
    //         healPower = healPower * 2;
    //     } 
    //     health += healPower;

    //     UpdateHealthText();
    //     damageText.color = Color.green;

    //     ShowDamage(0);
    //     damageText.text = "Healed for " + healPower.ToString() + "!";

    //     if (isPoisoned) {
    //         poisonTextObj.GetComponent<Text>().text = "Psn Healed!";
    //         ShowPoisonDamage();
    //         isPoisoned = false;
    //         poisonTimer = 0;
    //     }

    //     return true;
    // }

    // public bool ChargeUpUnit(UnitStats target)
    // {
    //         isChargedUp = true;
    //         chargeUpTextObj.SetActive(true);

    //         return true;
    // }

    // public bool LaserUnit(UnitStats target)
    // {
    //         // print(target);

    //         // print('a');
    //         // int attackPower = attack;
    //         target.damageText.color = Color.red;
    //         int attackPower = magic*3;
    //         if(isChargedUp){
    //             // print('e');
    //             attackPower = attackPower*2;
    //             isChargedUp = false;
    //             // chargeUpTextObj.SetActive(true);
    //             chargeUpTextObj.SetActive(false);
    //             target.damageText.fontSize = 42;
    //             // target.damageText.fontStyle
    //         }
    //         // print(name);
    //         // print(attackPower);
    //         target.Damage(attackPower);
            
    //         // target.damageText.fontSize = 24;


    //         return true;
    // } 

    // // public bool RandomMoveToUnit(UnitStats target) {
    // //     // randomMoves = new List<MoveDelegate>();

    // //     // List<MoveDelegate> randomMoves = new List<MoveDelegate>();

    // //     // print(AttackUnit);

    // //     if(!randomMovesSet) {
    // //         randomMoves = new List<MoveDelegate>();
    // //         if (name == "Bat") {
    // //             randomMoves.Add(AttackUnit);
    // //         }
    // //         if (name == "Snake") {
    // //             randomMoves.Add(AttackUnit);
    // //             randomMoves.Add(HealUnit);
    // //         }
    // //         if (name == "Bee") {
    // //             randomMoves.Add(AttackUnit);
    // //             randomMoves.Add(PoisonUnit);
    // //         }
    // //         if (name == "Ghost") {
    // //             randomMoves.Add(AttackUnit);
    // //             randomMoves.Add(ChargeUpUnit);
    // //         }
    // //         if (name == "Skeleton") {
    // //             randomMoves.Add(AttackUnit);
    // //             randomMoves.Add(ShieldUnit);
    // //         }
    // //         if (name == "Eye") {
    // //             randomMoves.Add(AttackUnit);
    // //             randomMoves.Add(LaserUnit);
    // //             randomMoves.Add(HealUnit);
    // //         }
    // //         randomMovesSet = true;
    // //     }

    // //     var randomIndex = Random.Range(0, randomMoves.Count);
    // //     if(randomMoves != null) {
    // //         // print(randomIndex);
    // //         randomMoves[randomIndex](target);
    // //     }
    // //     // print("random");
    // // return true;
    // //     // Random.Range(0, randomMoves.Length)]()
    // //     //  spawnRocks.Add(spawnRocks1);
    // //     //  spawnRocks.Add(spawnRocks2);
    // //     // randomMoves
    // //     // MoveDelegate randomMove;
    // //     // randomMove = AttackUnit;
    // //     // randomMove(target);

    // //     // var[] functions = new var[2];
    // //     // var[] functions = {AttackUnit, PoisonUnit};
    // // }

    // public string RandomMoveToUnit() 
    // {

    //     if (mana <= 0) {
    //         return "AttackUnit";
    //     }

    //     if(!randomMovesSet) {
    //         randomMoveStrings = new List<string>();
    //         if (name == "Bat") {
    //             randomMoveStrings.Add("AttackUnit");
    //         }
    //         if (name == "Snake") {
    //             randomMoveStrings.Add("AttackUnit");
    //             randomMoveStrings.Add("HealUnit");
    //         }
    //         if (name == "Bee") {
    //             randomMoveStrings.Add("AttackUnit");
    //             randomMoveStrings.Add("PoisonUnit");
    //         }
    //         if (name == "Ghost") {
    //             randomMoveStrings.Add("AttackUnit");
    //             randomMoveStrings.Add("ChargeUpUnit");
    //         }
    //         if (name == "Skeleton") {
    //             randomMoveStrings.Add("AttackUnit");
    //             randomMoveStrings.Add("ShieldUnit");
    //         }
    //         if (name == "Eye") {
    //             randomMoveStrings.Add("AttackUnit");
    //             randomMoveStrings.Add("AttackUnit");
    //             randomMoveStrings.Add("LaserUnit");
    //             randomMoveStrings.Add("ShieldUnit");
    //             randomMoveStrings.Add("ChargeUpUnit");
    //         }
    //         randomMovesSet = true;
    //     }

    //     var randomIndex = Random.Range(0, randomMoveStrings.Count);
    //     // print(randomIndex);
    //     if(randomMoveStrings != null) {
    //         return randomMoveStrings[randomIndex];
    //     } else
    //         return null;
    // }

    // private void HideDamageText()
    // {
    //     damageTextObj.SetActive(false);
    //     damageText.fontSize = 24;
    // }

    // private void HidePoisonDamage()
    // {
    //     poisonTextObj.SetActive(false);
    // }

    // public void HideGameObject()
    // {
    //     gameObject.SetActive(false);
    //     healthTextObj.SetActive(false);
    //     manaTextObj.SetActive(false);
    // }

    // private void FlashManaText()
    // {
    //     StopAllCoroutines();
    //     // Image image = manaTextObj.GetComponent<Image>();
    //     Text text = manaTextObj.GetComponent<Text>();
    //             // targetUnit.GetComponent<Image>().color = Color.red;
    //     print(text);
    //     IEnumerator coroutine = Blink(text);
    //     StartCoroutine(coroutine);

    //     // StartCoroutine("Blink");

    //     // yield return null;
    // }

    // private IEnumerator Blink(Text image)
    // {
    //     while (true)
    //     {
    //         switch(image.color.a.ToString())
    //         {
    //             case "0":
    //                 //image.color = Colors.red;
    //                 //  image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    //                 image.color = new Color(1, 0, 0, 1);
    //                 //Play sound
    //                 yield return new WaitForSeconds(0.4f);
    //                 break;
    //             case "1":
    //                 // image.color = new Color(1, 1, 1, 1);
    //                 image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    //                 //Play sound
    //                 yield return new WaitForSeconds(0.4f);
    //                 break;
    //         }
    //     }
    // }

    

    // public bool ReturnTrue() {
    //     return true;
    // }
}
