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

    public int maxHealth;
    public int maxGuts;
    public int health;
    public int guts;
    public int attack;
    public int defense;
    public int speed;
    public int taste;
    public int tum;
    public int nose;

    public bool isDead;
    public bool isCookin;
    public bool NPC;

    public int shield = 0;
    public int turnNumber = 0;

    public Dictionary<string,int> StatusEffects;
    public Dictionary<string,int> StatChanges;

    public List<MoveButton> moves;

    public BattleManager bm;


    // UI

    public Text healthTextPrefab;
    private Text healthText;

    public Text gutsTextPrefab;
    private Text gutsText;

    public Text nameTextPrefab;
    private Text nameText;

    public Text turnTextPrefab;
    private Text turnText;

    public GameObject canvas;


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
        if (StatusEffects == null) {
            StatusEffects = new Dictionary<string,int>();
        }

        InitializeBattleManager();
        InitializeStatusEffects();
        InitializeStatChanges();

        nameText = Instantiate(nameTextPrefab, new Vector3(0, 12, 0), Quaternion.identity);
        nameText.transform.SetParent(canvas.transform, false);
        nameText.text = gameObject.name;

        turnText = Instantiate(turnTextPrefab, new Vector3(-15, 0, 0), Quaternion.identity);
        turnText.transform.SetParent(canvas.transform, false);


        // TODO: make this a function since its repeated for guts
        healthText = Instantiate(healthTextPrefab);

        healthText.transform.localScale = new Vector3(0.2f,0.2f,1);
        healthText.transform.position = new Vector3(17,3,0);

        // set canvas as parent to the text
        healthText.transform.SetParent(canvas.transform, false);
        // Set the text element to the current textToDisplay:
        healthText.text = health.ToString();


        gutsText = Instantiate(gutsTextPrefab);

        gutsText.transform.localScale = new Vector3(0.2f,0.2f,1);
        gutsText.transform.position = new Vector3(17,-3,0);

        // set canvas as parent to the text
        gutsText.transform.SetParent(canvas.transform, false);
        // Set the text element to the current textToDisplay:
        gutsText.text = guts.ToString();
    }

    private void InitializeBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    private void InitializeStatusEffects()
    {
        if (StatusEffects == null) {
            StatusEffects = new Dictionary<string,int>();
        }
    }

    private void InitializeStatChanges()
    {
        if (StatChanges == null) {
            StatChanges = new Dictionary<string,int>();
        }
    }    

    public GameObject GetCanvasObj()
    {
        return canvas;
    }



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
        if (StatChanges.ContainsKey(statChange)) {
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



    // ACTION APPLICATION 
    // TODO: (move to bm)

    public void HealDamage (int rawHeal)
    {
        int netHeal = rawHeal + (rawHeal * GetNetTum()) / 10;

        health += netHeal;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateHealthText();
    }

    public void SateHunger (int rawSatiation)
    {
        int netGuts = rawSatiation + (rawSatiation * GetNetTaste()) / 10;

        guts += netGuts;

        if (guts > maxGuts) {
            guts = maxGuts;
        }

        UpdateGutsText();
    }

    // TODO should this have a multiplier? 
    public void TantalizeHunger (int rawTantalization)
    {
        int netGuts = rawTantalization;

        SpendGuts(netGuts);
    }

    public void SpendGuts (int spentGuts)
    {
        guts -= spentGuts;

        if (guts < 0) {
            guts = 0;
        }

        UpdateGutsText();
    }



    // GETS AND SETS

    public int GetCurrentHealth()
    {
        return health;
    }

    public void SetCurrentHealth(int healthToSet)
    {
        health = healthToSet;
    }

    public void LoseHealth(int lostHealth)
    {
        health -= lostHealth;
    }

    public int GetCurrentGuts()
    {
        return guts;
    }

    public void SetCurrentGuts(int gutsToSet)
    {
        guts = gutsToSet;
    }

    public void LoseGuts(int lostGuts)
    {
        guts -= lostGuts;
    }

    public int GetNetStrength()
    {
        return attack + GetStatChangeStacks(STRENGTH);
    }

    public int GetNetDefense()
    {
        return defense + GetStatChangeStacks(DEFENSE);
    }

    public int GetNetSpeed()
    {
        return speed + GetStatChangeStacks(SPEED);
    }

    public int GetNetTaste()
    {
        return taste + GetStatChangeStacks(TASTE);
    }

    public int GetNetTum()
    {
        return tum + GetStatChangeStacks(TUM);
    }

    public int GetNetNose()
    {
        return nose + GetStatChangeStacks(NOSE);
    }

    // UI

    public void UpdateText ()
    {
        UpdateHealthText();
        UpdateGutsText();
        UpdateTurnText();
    }

    private void UpdateHealthText()
    {
        // print(healthText);

        healthText.text = health.ToString();

        if (shield > 0) {
            healthText.text += "+[" + shield.ToString() + "]";
        }

        healthText.text += " / " + maxHealth.ToString();
    }

    private void UpdateGutsText()
    {
        gutsText.text = guts.ToString();

        gutsText.text += " / " + maxGuts.ToString();

    }

    private void UpdateTurnText()
    {
        if (turnNumber == 0) {
            turnText.text = "Turn\nDone";
        } else {
            turnText.text = "Turn\n#" + turnNumber.ToString();
        }
    }

    public void SetTurnText(string text)
    {
        turnText.text = text;
    }



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

    public int GetMovesCount()
    {
        return moves.Count;
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

    // public void CheckIfDead()
    // {
    //     if (health <= 0) {
    //         health = 0;
    //         isDead = true;
    //         turnNumber = -1;
    //         turnText.text = "Dead";

    //         // bm.HandleDeaths();
    //     }
    // }

    // public void CheckGross()
    // {
    //     if (StatusEffects.ContainsKey(GROSS)) {
    //         GrossTicker();
    //     }
    // }

    // // move to bm
    // private void GrossTicker()
    // {
    //     // print(StatusEffects[GROSS]);

    //     TakeTumDamage(StatusEffects[GROSS]);

    //     AddStatusEffect(GROSS, -1);
    //     // print(StatusEffects[GROSS]);

    //     if (StatusEffects[GROSS] <= 0) {
    //         StatusEffects.Remove(GROSS);
    //     }
    // }
}

// gonna have lots of status effects, if you have like 5 heat then catch fire effect
// if you have 5 cold and 5 wet, then freeze unit
// some effects are secret
