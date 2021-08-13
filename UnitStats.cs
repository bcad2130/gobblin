using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
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
    public int cookTime;
    public string cookedItem;
    public List<string> recipeIngredients;

    public int shield = 0;
    public int turnNumber = 0;

    public BattleManager bm;



    //new stuff
    public Text healthTextPrefab;
    private Text healthText;

    public Text gutsTextPrefab;
    private Text gutsText;

    public Text nameTextPrefab;
    private Text nameText;

    public Text turnTextPrefab;
    private Text turnText;

    public GameObject canvas;
    public List<MoveButton> moves;

    private Dictionary<string,int> StatusEffects;



    // CONSTANTS

    private const string STUN = "STUN";
    private const string GROSS = "GROSS";
    private const string SHIELD = "SHIELD";
    private const string DODGE  = "DODGE";
    private const string BOIL   = "BOIL";


    private void Awake()
    {
        if (StatusEffects == null) {
            StatusEffects = new Dictionary<string,int>();
        }

        InitializeBattleManager();
        InitializeStatusEffects();

        nameText = Instantiate(nameTextPrefab, new Vector3(0, 12, 0), Quaternion.identity);
        nameText.transform.SetParent(canvas.transform, false);
        nameText.text = gameObject.name;

        turnText = Instantiate(turnTextPrefab, new Vector3(-15, 0, 0), Quaternion.identity);
        turnText.transform.SetParent(canvas.transform, false);


        // TODO: make this a function since its repeated for guts
        healthText = Instantiate(healthTextPrefab);

        healthText.transform.localScale = new Vector3(0.3f,0.3f,1);
        healthText.transform.position = new Vector3(22,4,0);

        // set canvas as parent to the text
        healthText.transform.SetParent(canvas.transform, false);
        // Set the text element to the current textToDisplay:
        healthText.text = health.ToString();


        gutsText = Instantiate(gutsTextPrefab);

        gutsText.transform.localScale = new Vector3(0.3f,0.3f,1);
        gutsText.transform.position = new Vector3(22,-4,0);

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

    public GameObject GetCanvasObj()
    {
        return canvas;
    }

    public ref List<MoveButton> GetMoves()
    {
        return ref moves;
    }

    public MoveButton GetRandomAction()
    {
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

    public Dictionary<string,int> GetAllStatusEffects()
    {
        // if (StatusEffects == null) {
        //     StatusEffects = new Dictionary<string,int>();
        // }

        return StatusEffects;
    }

    public int GetStatusEffectStacks(string statusEffect)
    {
        if (StatusEffects.ContainsKey(statusEffect)) {
            return StatusEffects[statusEffect];
        }

        return 0;
    }

    public void AddStatusEffect(string statusEffect, int stacks)
    {
        // if (StatusEffects == null) {
        //     StatusEffects = new Dictionary<string,int>();
        // }

        if (StatusEffects.ContainsKey(statusEffect)) {
            StatusEffects[statusEffect] += stacks;
        } else {
            StatusEffects.Add(statusEffect, stacks);
        }
    }

    public void TakeAttack (int rawDamage, int attackSpeed)
    {
        int multiplier = 1;

        if (attackSpeed > speed) {
            int speedDiff = attackSpeed - speed;

            if (speedDiff >= 20) {
                multiplier = 3;
            } else if (speedDiff >= 10) {
                multiplier = 2;
            }
        }

        int netDamage = rawDamage - (defense / 2);

        // do 1 damage per hit minimum
        if (netDamage <= 0) {
            netDamage = 1;
        }

        do {
            if (!CheckDodge()) {
                if (shield > 0) {
                    shield -= netDamage;

                    if (shield < 0) {
                        netDamage = -shield;
                        shield = 0;
                    } else {
                        netDamage = 0;
                    }

                    StatusEffects[SHIELD] = shield;
                }

                health -= netDamage;
            }

            multiplier--;
        } while (multiplier > 0);

        if (health <= 0) {
            health = 0;
            isDead = true;
        }
    }

    // This damage is unblocked, defended, etc
    // i.e. health - rawDamage
    public void TakeTumDamage (int rawDamage)
    {
        int netDamage = rawDamage - (tum / 2);

        // do 1 damage minimum
        if (netDamage <= 0) {
            netDamage = 1;
        }

        health -= netDamage;
        
        if (health <= 0) {
            health = 0;
            isDead = true;
        }
    }

    private bool CheckDodge()
    {
        if (StatusEffects.ContainsKey(DODGE)) {
            AddStatusEffect(DODGE, -1);

            if (StatusEffects[DODGE] <= 0) {
                StatusEffects.Remove(DODGE);
            }

            return true;
        } else {
            return false;
        }
    }

    public void HealDamage (int rawHeal)
    {
        int netHeal = rawHeal + (rawHeal * taste) / 10;

        health += netHeal;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateHealthText();
    }

    public void SateHunger (int rawSatiation)
    {
        int netGuts = rawSatiation + (rawSatiation * taste) / 10;

        guts += netGuts;

        if (guts > maxGuts) {
            guts = maxGuts;
        }

        UpdateGutsText();
    }

    public bool CanAffordGutsCost (int gutsCost)
    {
        return (guts - gutsCost) >= 0;
    }

    public void SpendGuts (int spentGuts)
    {
        guts -= spentGuts;
        UpdateGutsText();
    }

    // public bool CanAffordResourceCost (List<Item> resourceCost)
    // {
    //     // TODO: Make this work for requiring multiple of the same item
    //     if (resourceCost != null) {
    //         // print ("resource cost is not null");
    //         foreach (Item resource in resourceCost) {
    //             switch(bm.CurrentAction.TargetType) {
    //                 case "Cook":
    //                     if (!bm.playerEquipment.CheckItem(resource)) {
    //                         return false;
    //                     }
    //                     break;
    //                 case "Ingredient":
    //                     if (!bm.playerIngredients.CheckItem(resource)) {
    //                         return false;
    //                     }
    //                     break;
    //                 case "Eat":
    //                     if (!bm.playerFood.CheckItem(resource)) {
    //                         return false;
    //                     }
    //                     break;
    //                 default:
    //                     print("Invalid target type for action with resourceCost");
    //                     break;
    //             }
    //         }
    //     }

    //     return true;
    // }

    // public void SpendResource (List<Item> resourceCost)
    // {
    //     if (resourceCost != null) {
    //         foreach (Item resource in resourceCost) {
    //             switch(bm.CurrentAction.TargetType) {
    //                 case "Cook":
    //                     bm.playerEquipment.RemoveItem(resource.Name);
    //                     break;
    //                 case "Ingredient":
    //                     bm.playerIngredients.RemoveItem(resource.Name);
    //                     break;
    //                 case "Eat":
    //                 case "Self":
    //                     bm.playerFood.RemoveItem(resource.Name);
    //                     break;
    //                 default:
    //                     print("Invalid target type for action with resourceCost");
    //                     break;
    //             }
    //         }
    //     }

    //     // print ("we made it this far");
    // }

    // deprecated?
    // public void Effect ()
    // {
    //     if (!bm) {
    //         InitializeBattleManager();
    //         InitializeStatusEffects();
    //         // print(bm.CurrentAction);
    //     }

    //     print("action:");
    //     // print(bm.CurrentAction.Damage);
    //     // print(bm.CurrentAction);
    //     // print(bm.GetCurrentAction().Self());
    //     // print("effect begins...");
    //     // health -= bm.CurrentAction.Damage;
    //     if (bm.CurrentAction.Damage > 0) {
    //         print("wtf");
    //         TakeAttack(bm.CurrentAction.Damage);
    //     }

    //     if (bm.CurrentAction.Heal > 0) {
    //         HealDamage(bm.CurrentAction.Heal);
    //     }

    //     // TODO: i should just use the status effect instead of having a local shield stat
    //     ApplySingleEffectStatus(bm.CurrentAction.GetAllStatusEffects());

    //     foreach(KeyValuePair<string, int> effect in bm.CurrentAction.GetAllStatusEffects())
    //     {
    //         // do something with effect.Value or effect.Key

    //         AddStatusEffect(effect.Key, effect.Value);
    //     }


    //     // print(bm.CurrentAction.DestroySelf);
    //     if (bm.CurrentAction.DestroySelf) {
    //         // print('h');
    //         Destroy(this.gameObject);

    //         // TODO: must clear from unit lists etc
    //         // on this note, will i have revival? should i even keep dead units around?
    //         // if not, i should have a more effective way of clearing units, since i might want to move them around, or have them transform, or get "used up" like the cauldron
    //     }
    // }

    public void DestroySelf()
    {
        bm.RemoveUnitFromAllLists(this);
        Destroy(this.gameObject);
    }

    public void ApplySingleEffectStatus(Dictionary<string,int> effects)
    {
        if (effects.ContainsKey(SHIELD)) {
            shield += effects[SHIELD];
        }
    }

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

    public void CheckIfDead()
    {
        if (health <= 0) {
            health = 0;
            isDead = true;
            turnNumber = -1;
            turnText.text = "Dead";

            // bm.HandleDeaths();
        }
    }

    public void CheckGross()
    {
        if (StatusEffects.ContainsKey(GROSS)) {
            GrossTicker();
        }
    }

    private void GrossTicker()
    {
        // print(StatusEffects[GROSS]);

        TakeTumDamage(StatusEffects[GROSS]);

        AddStatusEffect(GROSS, -1);
        // print(StatusEffects[GROSS]);

        if (StatusEffects[GROSS] <= 0) {
            StatusEffects.Remove(GROSS);
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
