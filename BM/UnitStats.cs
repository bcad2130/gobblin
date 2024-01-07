using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    // ***************
    // ***VARIABLES***
    // ***************


    // STATS
    public string unitName;
    public Sprite iconSmall;
    public Sprite iconLarge;

    private int health;
    private int guts;

    public int maxHealth;
    public int maxGuts;
    public int attack;
    public int defense;
    public int speed;
    public int taste;
    public int tum;
    public int nose;

    public bool isDead;
    public bool isCookin;
    public bool isGutless;
    public bool isNPC;
    public bool isStarving;
    // public bool isDialogueOnly;
    
    public int turnNumber = 0;

    public Dictionary<string,int> StatusEffects = new Dictionary<string,int>();
    public Dictionary<string,int> StatChanges   = new Dictionary<string,int>();

    public List<MoveButton> moves;
    public List<MoveButton> skills;

    public List<Action> actions = new List<Action>();

    private BattleManager bm;
    private UIManager     um;


    // UI

    public Text healthTextPrefab;
    private Text healthText;

    public Text gutsTextPrefab;
    private Text gutsText;

    public Text nameTextPrefab;
    private Text nameText;

    public Text turnTextPrefab;
    private Text turnText;

    public GameObject DamageTextPrefab;

    public GameObject stazButtonPrefab;
    private GameObject stazButton;

    private GameObject UnitBox;
    public GameObject UnitBoxPrefab;

    public Image figurePrefab;
    private Image figure;

    // public Animation figureAnimator;

    // public Canvas canvas;


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

    // private const string SHARP      = "SHARP";




    // ***************
    // ***FUNCTIONS***
    // ***************



    // INITIALIZATION

    private void Start()
    {
        // gameObject.name = transform.name.Replace("(Clone)","").Trim();
        // gameObject

        InitializeBattleManager();
        InitializeUIManager();
        InitializeHealthGuts();
        InitializeUnitUI();
        // InitializeStazButton();
        // InitializeStatusEffects();
        // InitializeStatChanges();
        // InitializeUnitText();
        // InitializeUnitFigure();
        // InitializeCamera();
    }

    private void InitializeBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    private void InitializeUIManager()
    {
        um = GameObject.FindObjectOfType<UIManager>();
    }

    public void InitializeUnitUI()
    {
        UnitBox = Instantiate(UnitBoxPrefab, um.GetCanvas().transform);

    }
    // private void InitializeStatusEffects()
    // {
    //     if (StatusEffects == null) {
    //         StatusEffects = new Dictionary<string,int>();
    //     }
    // }

    // private void InitializeStatChanges()
    // {
    //     if (StatChanges == null) {
    //         StatChanges = new Dictionary<string,int>();
    //     }
    // }

    public void InitializeUnitText()
    {
        // print(GetName());

        // nameText = Instantiate(nameTextPrefab);
        // nameText.transform.SetParent(UnitBox.transform, false);

        // // nameText.transform.localScale = new Vector3(0.2f,0.2f,1);
        // // nameText.transform.position = new Vector3(0,100f,0);
        // nameText.text = GetName();

        // turnText = Instantiate(turnTextPrefab, new Vector3(-14, 0, 0), Quaternion.identity);
        // turnText.transform.localScale = new Vector3(0.2f,0.2f,1);
        // turnText.transform.SetParent(UnitBox.transform, false);
        // turnText.color = Color.blue;

        // TODO: make this a function since its repeated for guts
        healthText = Instantiate(healthTextPrefab);
        healthText.transform.SetParent(UnitBox.transform, false);

        // healthText.transform.localScale = new Vector3(0.2f,0.2f,1);
        // healthText.transform.position = new Vector3(-50f,50f,0);
        // set canvas as parent to the text

        if (!isGutless) {
            gutsText = Instantiate(gutsTextPrefab);
            gutsText.transform.SetParent(UnitBox.transform, false);
        }

        // gutsText.transform.localScale = new Vector3(0.2f,0.2f,1);
        // gutsText.transform.position = new Vector3(50f,50f,0);
        // set canvas as parent to the text

        UpdateText();
    }

    public void InitializeUnitFigure()
    {
        // figure.gameObject.SetActive(true);
        figure = Instantiate(figurePrefab, UnitBox.transform);

        // figureObject = Instantiate(figureObject, UnitBox);
    }

    // private void InitializeCamera()
    // {
    //     canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
    // }

    private void InitializeHealthGuts()
    {
        health  = maxHealth;
        guts    = maxGuts;
    }

    public void InitializeStazButton()
    {
        stazButton = Instantiate(stazButtonPrefab);
        stazButton.transform.SetParent(UnitBox.transform, false);
    }

    // private void Start()
    // {
    //     gameObject.name = transform.name.Replace("(Clone)","").Trim();
    // }


    // STATUS EFFECTS
    //
    // or in GOBBLIN :

    // MALADIES

    public Dictionary<string,int> GetAllStatusEffects()
    {
        if (StatusEffects == null) {
            Debug.Log("Error: GetAllStatusEffects: StatusEffects are null?");
            return null;
            // StatusEffects = new Dictionary<string,int>();
        }

        return StatusEffects;
    }

    public int GetStatusEffectStacks(string statusEffect)
    {
        if (StatusEffects.ContainsKey(statusEffect)) {
            return StatusEffects[statusEffect];
        }

        return 0;
    }

    public bool GetStatusEffectStatus(string statusEffect)
    {
        if (StatusEffects.ContainsKey(statusEffect)) {
            if (StatusEffects[statusEffect] > 0) {
                return true;
            }
        }

        return false;
    }

    public void AddStatusEffect(string statusEffect, int stacks)
    {
        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] += stacks;
        } else {
            StatusEffects.Add(statusEffect, stacks);
        }
    }

    public void SubtractStatusEffect(string statusEffect, int stacks)
    {
        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] -= stacks;

            if (StatusEffects[statusEffect] < 0) {
                StatusEffects[statusEffect] = 0;
            }
        } else {
            StatusEffects.Add(statusEffect, 0);
        }
    }



    // STAT CHANGES

    public Dictionary<string,int> GetAllStatChanges()
    {
        if (StatChanges == null) {
            Debug.Log("Error: GetAllStatChanges: StatChanges are null?");
            return null;
            // StatChanges = new Dictionary<string,int>();
        }

        return StatChanges;
    }

    public int GetStatChangeStacks(string statChange)
    {
        if (StatChanges != null && StatChanges.ContainsKey(statChange)) {
            return StatChanges[statChange];
        }

        return 0;
    }

    public bool GetStatChangeStatus(string statChange)
    {
        if (StatChanges.ContainsKey(statChange)) {
            if (StatChanges[statChange] > 0) {
                return true;
            }
        }

        return false;
    }

    public void AddStatChange(string statChange, int stacks)
    {
        if (StatChanges.ContainsKey(statChange)) {
            StatChanges[statChange] += stacks;
        } else {
            StatChanges.Add(statChange, stacks);
        }
    }

    public void SubtractStatChange(string statChange, int stacks)
    {
        if (StatChanges.ContainsKey(statChange)) {
            StatChanges[statChange] -= stacks;

            if (StatChanges[statChange] < 0) {
                StatChanges[statChange] = 0;
            }
        } else {
            StatChanges.Add(statChange, 0);
        }
    }



    // GETS AND SETS

    public GameObject GetUnitBox()
    {
        return UnitBox;
    }

    public string GetName()
    {
        // return gameObject.name;
        return unitName;
    }

    public Sprite GetSmallIcon()
    {
        return iconSmall;
    }

    public Sprite GetLargeIcon()
    {
        return iconLarge;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public void SetCurrentHealth(int healthToSet)
    {
        health = healthToSet;

        UpdateHealthText();
    }

    public void GainHealth(int healthGained)
    {
        health += healthGained;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateHealthText();
    }

    public void LoseHealth(int healthLost)
    {
        health -= healthLost;

        if (health < 0)
        {
            health = 0;
        }

        UpdateHealthText();
    }

    public int GetMaxGuts()
    {
        return maxGuts;
    }

    public int GetCurrentGuts()
    {
        return guts;
    }

    public void SetCurrentGuts(int gutsToSet)
    {
        guts = gutsToSet;

        UpdateGutsText();
    }

    public void GainGuts(int gutsGained)
    {
        guts += gutsGained;

        if (guts > maxGuts) {
            guts = maxGuts;
        }

        UpdateGutsText();
    }

    public void LoseGuts(int gutsLost)
    {
        guts -= gutsLost;

        if (guts < 0) {
            guts = 0;
        }

        UpdateGutsText();
    }

    public int GetBaseStrength()
    {
        return attack;
    }

    public int GetBonusStrength()
    {
        return GetStatChangeStacks(STRENGTH) + GetStatusEffectStacks(GLOW);
    }

    public int GetNetStrength()
    {
        return attack + GetStatChangeStacks(STRENGTH) + GetStatusEffectStacks(GLOW);
    }

    public int GetBaseDefense()
    {
        return defense;
    }

    public int GetBonusDefense()
    {
        return GetStatChangeStacks(DEFENSE) + GetStatusEffectStacks(GLOW);
    }

    public int GetNetDefense()
    {
        return defense + GetStatChangeStacks(DEFENSE) + GetStatusEffectStacks(GLOW);
    }

    public int GetBaseSpeed()
    {
        return speed;
    }

    public int GetBonusSpeed()
    {
        return GetStatChangeStacks(SPEED) + GetStatusEffectStacks(GLOW);
    }    

    public int GetNetSpeed()
    {
        return speed + GetStatChangeStacks(SPEED) + GetStatusEffectStacks(GLOW);
    }

    public int GetBaseTaste()
    {
        return taste;
    }

    public int GetBonusTaste()
    {
        return GetStatChangeStacks(TASTE) + GetStatusEffectStacks(GLOW);
    }    

    public int GetNetTaste()
    {
        return taste + GetStatChangeStacks(TASTE) + GetStatusEffectStacks(GLOW);
    }

    public int GetBaseTum()
    {
        return tum;
    }

    public int GetBonusTum()
    {
        return GetStatChangeStacks(TUM) + GetStatusEffectStacks(GLOW);
    }

    public int GetNetTum()
    {
        return tum + GetStatChangeStacks(TUM) + GetStatusEffectStacks(GLOW);
    }

    public int GetBaseNose()
    {
        return nose;
    }

    public int GetBonusNose()
    {
        return GetStatChangeStacks(NOSE) + GetStatusEffectStacks(GLOW);
    }

    public int GetNetNose()
    {
        return nose + GetStatChangeStacks(NOSE) + GetStatusEffectStacks(GLOW);
    }

    // UI

    public void UpdateText ()
    {
        UpdateHealthText();
        UpdateGutsText();
        // UpdateTurnText();
    }

    private void UpdateHealthText()
    {
        healthText.text = GetCurrentHealth().ToString();


        // print(GetCurrentHealth().ToString() + " /" + maxHealth.ToString());

        // healthText.text = GetCurrentHealth().ToString() + " / " + maxHealth.ToString();


        // TODO Show armor as status, not as part of health
        if (GetStatusEffectStatus(ARMOR)) {
            healthText.text += " + [" + GetStatusEffectStacks(ARMOR).ToString() + "]";
        }
    }

    private void UpdateGutsText()
    {
        if (!isGutless) {
            gutsText.text = GetCurrentGuts().ToString();
            // gutsText.text = GetCurrentGuts().ToString() + " / " + maxGuts.ToString();
        }
    }

    // private void UpdateTurnText()
    // {
    //     if (turnNumber == -1) {
    //         SetTurnText("Dead");
    //         ChangeTurnTextColor(Color.black);
    //     } else if (turnNumber == 0) {
    //         SetTurnText("Turn\nDone");
    //         ChangeTurnTextColor(Color.gray);
    //     } else if (turnNumber == 1) {
    //         SetTurnText("My\nTurn!");
    //         ChangeTurnTextColor(Color.white);
    //     } else if (turnNumber == 2) {
    //         SetTurnText("Next\nTurn!");
    //         ChangeTurnTextColor(Color.cyan);
    //     } else {
    //         SetTurnText("Turn\n#" + turnNumber.ToString());
    //         ChangeTurnTextColor(Color.blue);
    //     }
    // }

    // private void SetTurnText(string text)
    // {
    //     turnText.text = text;
    // }

    public void SetTurnNumber(int number)
    {
        turnNumber = number;
        // UpdateTurnText();
    }

    // public void ChangeTurnTextColor(Color newColor)
    // {
    //     turnText.color = newColor;
    // }

    public void CreateDamageText(int damage, Color color, int hits = 1, float xOffset = 0)
    {
        // Debug.Log("here");
        GameObject obj = Instantiate(DamageTextPrefab);
        obj.transform.SetParent(UnitBox.transform, false);

        obj.GetComponentInChildren<Text>().text = damage.ToString();
        obj.GetComponentInChildren<Text>().color = color;

        if (hits > 1) {
            obj.GetComponentInChildren<Text>().text += "x" + hits.ToString();
        }
        if (xOffset != 0f) {
            obj.transform.position = new Vector3(obj.transform.position.x+xOffset, obj.transform.position.y, obj.transform.position.z);
        }
    }

    public void SetUnitBoxPosition(float x, float y)
    {
        // print(new Vector3(x, y, 0));
        UnitBox.transform.localPosition = new Vector3(x, y, 0);
    }


    public void SetAttackingAnimator(bool attacking) {
        Animator figureAnimator = UnitBox.GetComponentInChildren<Animator>();


        if (figureAnimator != null) {
            // figureAnimator.SetBool("Attacking", attacking);
            figureAnimator.SetTrigger("AttackTrigger");
        } else {

            Debug.Log("UHOH");
        }
    }

    public void SetDefendingAnimator(bool defending) {
        Animator figureAnimator = UnitBox.GetComponentInChildren<Animator>();
Debug.Log('d');

        if (figureAnimator != null) {
            // figureAnimator.SetBool("Attacking", attacking);
            figureAnimator.SetTrigger("DefendTrigger");
        } else {

            Debug.Log("UHOH");
        }
    }

    // UNUSED
    // public void AdjustUnitBoxPosition(float x, float y)
    // {
    //     UnitBox.transform.position = UnitBox.transform.position + new Vector3(x, y, 0);
    // }

    // MISC

    public void DestroySelf()
    {
        bm.RemoveUnitFromAllLists(this);
        Destroy(this.gameObject);
    }

    public ref List<MoveButton> GetMoves()
    {
        return ref moves;
    }

    public ref List<MoveButton> GetSkills()
    {
        return ref skills;
    }

    public int GetMovesCount()
    {
        return moves.Count;
    }    

    public int GetSkillsCount()
    {
        return skills.Count;
    }    

    // TODO move to bm
    public MoveButton GetRandomAction()
    {
        List<MoveButton> tempList = GetMoves();

        var randomIndex = Random.Range(0, tempList.Count);

        MoveButton tempActionButton = tempList[randomIndex];

        return tempActionButton;
        // print(tempActionButton);

        // shouldn't i check this from the bm method, not the button?
        // 8/18 yes

        // if (tempActionButton.CheckAfford()) {
        //     return tempActionButton;
        // }

        // since this is recursive, make sure all units have a free action or we stack overflow
        // return GetRandomAction();
    }
}

// gonna have lots of status effects, if you have like 5 heat then catch fire effect
// if you have 5 cold and 5 wet, then freeze unit
// some effects are secret
