using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    public int maxHP;
    public int maxMP;
    public int health;
    public int mana;
    public int attack;
    public int defense;
    // public int magic;
    public int speed;
    public bool isDead;

    public bool isCookin;
    public bool NPC;
    public int cookTime;
    public string cookedItem;
    // public Item cookedItem;
    public List<string> recipeIngredients;

    public int shield = 0;
    public int turnNumber = 0;

    // private BattleManager bm;
    public BattleManager bm;



    //new stuff
    public Text healthTextPrefab;
    private Text healthText;

    public Text manaTextPrefab;
    private Text manaText;

    public Text nameTextPrefab;
    private Text nameText;

    public Text turnTextPrefab;
    private Text turnText;

    public GameObject canvas;
    public List<MoveButton> moves;

    // private float yPosHP = -112.98f;
    // private float yPosMP = -152.98f;

    private Dictionary<string,int> StatusEffects;

    // public List<Action> myActions;


    // CONSTANTS

    private const string POISON = "POISON";
    private const string SHIELD = "SHIELD";
    private const string DODGE  = "DODGE";
    private const string BOIL   = "BOIL";


    private void Awake() {
        if (StatusEffects == null) {
            StatusEffects = new Dictionary<string,int>();
        }

        SetBattleManager();
        SetStatusEffects();

        nameText = Instantiate(nameTextPrefab, new Vector3(0, 15, 0), Quaternion.identity);
        nameText.transform.SetParent(canvas.transform, false);
        nameText.text = gameObject.name;

        turnText = Instantiate(turnTextPrefab, new Vector3(-20, 0, 0), Quaternion.identity);
        turnText.transform.SetParent(canvas.transform, false);


        // TODO: make this a function since its repeated for mana
        healthText = Instantiate(healthTextPrefab);

        healthText.transform.localScale = new Vector3(0.3f,0.3f,1);
        healthText.transform.position = new Vector3(15,5,0);

        // set canvas as parent to the text
        healthText.transform.SetParent(canvas.transform, false);
        // Set the text element to the current textToDisplay:
        healthText.text = health.ToString();


        manaText = Instantiate(manaTextPrefab);

        manaText.transform.localScale = new Vector3(0.3f,0.3f,1);
        manaText.transform.position = new Vector3(15,-5,0);

        // set canvas as parent to the text
        manaText.transform.SetParent(canvas.transform, false);
        // Set the text element to the current textToDisplay:
        manaText.text = mana.ToString();
    }

    private void SetBattleManager() {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    private void SetStatusEffects() {
        if (StatusEffects == null) {
            StatusEffects = new Dictionary<string,int>();
        }
    }

    public GameObject GetCanvasObj() {
        return canvas;
    }

    public ref List<MoveButton> GetMoves()
    {
        return ref moves;
    }

    public MoveButton GetRandomAction() {
        List<MoveButton> tempList = GetMoves();

        var randomIndex = Random.Range(0, tempList.Count);

        MoveButton tempActionButton = tempList[randomIndex];
        // print(tempActionButton);

        if (tempActionButton.CheckAfford()) {
            return tempActionButton;
        }

        // since this is recursive, make sure all units have a free action or we stack overflow
        return GetRandomAction();
    }

    public Dictionary<string,int> GetAllStatusEffects() {
        // if (StatusEffects == null) {
        //     StatusEffects = new Dictionary<string,int>();
        // }

        return StatusEffects;
    }

    public void UpdateStatusEffect(string statusEffect, int stacks) {
        // if (StatusEffects == null) {
        //     StatusEffects = new Dictionary<string,int>();
        // }

        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] += stacks;
        } else {
            StatusEffects.Add(statusEffect, stacks);
        }
    }

    // private void SetMoves()
    // {
    //     moves = new List<Button>();
    //     moves.Add(attackButtonPrefab);
    // }

    public void TakeAttack (int rawDamage, int attackSpeed) {
        int multiplier = 1;

        if (attackSpeed > speed) {
            int speedDiff = attackSpeed - speed;

            if (speedDiff >= 20) {
                multiplier = 3;
            } else if (speedDiff >= 10) {
                multiplier = 2;
            }
        }

        int damage = rawDamage - (defense / 2);

        if (damage < 0) {
            damage = 0;
        }

        do {
            if (!CheckDodge()) {
                if (shield > 0) {
                    shield -= damage;

                    if (shield < 0) {
                        damage = -shield;
                        shield = 0;
                    } else {
                        damage = 0;
                    }

                    StatusEffects[SHIELD] = shield;
                }

                health -= damage;
            }

            multiplier--;
        } while (multiplier > 0);

        if (health <= 0) {
            health = 0;
            isDead = true;
        }
    }

    public void TakeDamage (int rawDamage) {
        // print("TakeDamage");
        // if you heal when you have a shield, it adds it to the shield (NOT ANYMORE!)
        // we need to make a heal function
        // we also need max health and mana stats

        int damage = rawDamage - (defense / 2);

        if (damage < 0) {
            damage = 0;
        }

        if (!CheckDodge()) {
            if (shield > 0) {
                shield -= damage;

                if (shield < 0) {
                    damage = -shield;
                    shield = 0;
                } else {
                    damage = 0;
                }

                StatusEffects[SHIELD] = shield;
            }

            health -= damage;

            // UpdateHealthText();
            
            if (health <= 0) {
                health = 0;
                isDead = true;
            }
        }
    }

    private bool CheckDodge() {
        if (StatusEffects.ContainsKey(DODGE)) {
            UpdateStatusEffect(DODGE, -1);

            if (StatusEffects[DODGE] <= 0) {
                StatusEffects.Remove(DODGE);
            }

            return true;
        } else {
            return false;
        }
    }

    public void HealDamage (int damage) {
        // if you heal when you have a shield, it adds it to the shield
        // we need to make a heal function
        // we also need max health and mana stats

        health += damage;
        UpdateHealthText();
    }

    public bool CanAffordManaCost (int manaCost) {
        return (mana - manaCost) >= 0;
    }

    public void SpendMana (int spentMana) {
        mana -= spentMana;
        UpdateManaText();
    }

    public bool CanAffordResourceCost (List<Item> resourceCost) {
        // TODO: Make this work for requiring multiple of the same item
        if (resourceCost != null) {
            // print ("resource cost is not null");
            foreach (Item resource in resourceCost) {
                // if (!bm.playerItems.CheckItem(resource.Name)) {
                if (!bm.playerItems.CheckItem(resource)) {
                    // print("return false");
                    return false;
                }
            }
        }

        return true;
    }

    public void SpendResource (List<Item> resourceCost) {
        if (resourceCost != null) {
            foreach (Item resource in resourceCost) {
                bm.playerItems.RemoveItem(resource);
            }
        }

        // print ("we made it this far");
    }

    public void Effect () {
        if (!bm) {
            SetBattleManager();
            SetStatusEffects();
            // print(bm.CurrentAction);
        }

        print("action:");
        // print(bm.CurrentAction.Damage);
        // print(bm.CurrentAction);
        // print(bm.GetCurrentAction().Self());
        // print("effect begins...");
        // health -= bm.CurrentAction.Damage;
        if (bm.CurrentAction.Damage > 0) {
            print("wtf");
            TakeDamage(bm.CurrentAction.Damage);
        }

        if (bm.CurrentAction.Heal > 0) {
            HealDamage(bm.CurrentAction.Heal);
        }

        // TODO: i should just use the status effect instead of having a local shield stat
        ApplySingleEffectStatus(bm.CurrentAction.GetAllStatusEffects());

        foreach(KeyValuePair<string, int> effect in bm.CurrentAction.GetAllStatusEffects())
        {
            // do something with effect.Value or effect.Key

            UpdateStatusEffect(effect.Key, effect.Value);
        }


        // print(bm.CurrentAction.DestroySelf);
        if (bm.CurrentAction.DestroySelf) {
            // print('h');
            Destroy(this.gameObject);

            // TODO: must clear from unit lists etc
            // on this note, will i have revival? should i even keep dead units around?
            // if not, i should have a more effective way of clearing units, since i might want to move them around, or have them transform, or get "used up" like the cauldron
        }
    }

    public void DestroySelf() {
        bm.RemoveUnitFromAllLists(this);
        Destroy(this.gameObject);
    }

    public void ApplySingleEffectStatus(Dictionary<string,int> effects) {
        if (effects.ContainsKey(SHIELD)) {
            shield += effects[SHIELD];
        }
    }

    // public void TickStatusEffects () {
    //     List<string> effects = new List<string>();

    //     foreach (string statusEffect in StatusEffects.Keys) {
    //         effects.Add(statusEffect);
    //     }
    //     // foreach (KeyValuePair<string, int> effect in StatusEffects.ToList())
    //     foreach (string effect in effects)
    //     {
    //         // do something with effect.Value or effect.Key
    //         switch (effect) 
    //         {
    //           case "POISON": 
    //             PoisonTicker();
    //             break;
    //           case "SHIELD":
    //             break;
    //           default:
    //             print("Invalid Status Effect");
    //             break;
    //         }
    //     }
    // }

    public void UpdateText () {
        UpdateHealthText();
        UpdateManaText();
        UpdateTurnText();
    }

    private void UpdateHealthText()
    {
        // print(healthText);

        healthText.text = health.ToString();

        if (shield > 0) {
            healthText.text += "+[" + shield.ToString() + "]";
        }
    }

    private void UpdateManaText()
    {
        manaText.text = mana.ToString();
    }

    private void UpdateTurnText()
    {
        if (turnNumber == 0) {
            turnText.text = "Turn\nDone";
        } else {
            turnText.text = "Turn\n#" + turnNumber.ToString();
        }
    }

    public void CheckIfDead() {
        if (health <= 0) {
            health = 0;
            isDead = true;
            turnNumber = -1;
            turnText.text = "Dead";
        }
    }

    public void CheckPoison() {
        if (StatusEffects.ContainsKey(POISON)) {
            PoisonTicker();
        }
    }

    private void PoisonTicker() {
                // print(StatusEffects[POISON]);

                TakeDamage(StatusEffects[POISON]);

                UpdateStatusEffect(POISON, -1);
                // print(StatusEffects[POISON]);

                if (StatusEffects[POISON] <= 0) {
                    StatusEffects.Remove(POISON);
                }
    }

    // public bool CheckRecipe() {
    //     // print('T');
    //     if (StatusEffects.ContainsKey(BOIL)) {
    //         if (StatusEffects[BOIL] >= cookTime) {
    //             if (recipeIngredients.Count == 0) {
    //                 return true;
    //             }
    //         }
    //     }

    //     return false;
    // }

    // public void AddIngredient(List<Item> ingredients)
    // {
    //     print("AddIngredient");
    //     if (ingredients != null) {
    //         print("ingredients exist");
    //         foreach (Item ingredient in ingredients) {
    //             print(this.name);
    //             print(ingredient.Name);
    //             print(recipeIngredients.Count);
    //             if (recipeIngredients.Exists(i => i == ingredient.Name)) {
    //                 print(ingredient.Name + " removed");
    //                 recipeIngredients.Remove(ingredient.Name);
    //             }
    //         }
    //     }
    // }
}

// gonna have lots of status effects, if you have like 5 heat then catch fire effect
// if you have 5 cold and 5 wet, then freeze unit
// some effects are secret
