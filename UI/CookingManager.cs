using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour
{
    private BattleManager bm;
    private GameObject inventoryBox;

    public Canvas canvas;
    public Font font;

    public GameObject countdownClock_0;
    public GameObject countdownClock_1;
    public GameObject countdownClock_2;

    public GameObject cornIcon;
    public GameObject greaseIcon;
    public GameObject waterIcon;
    public GameObject cauldronIcon;
    public GameObject stirIcon;

    // private float cookingIconPosX = -900;

    private void Awake()
    {
        InitializeBattleManager();
        InitializeInventoryBox();
    }

    private void InitializeBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    private void InitializeInventoryBox()
    {
        inventoryBox = GameObject.FindWithTag("InventoryBox");
    }

    public void CreateClock(int countdown) {
        GameObject clockImage = null;
        switch(countdown) {
            case 0:
                clockImage = countdownClock_0;
                break;
            case 1:
                clockImage = countdownClock_1;
                break;
            case 2:
            default:
                clockImage = countdownClock_2;
                break;
        }

        GameObject clock = Instantiate(clockImage, new Vector3(-800, 490, 0), Quaternion.identity);
        clock.transform.SetParent(canvas.transform, false);
        clock.tag = "ClockIcon";
    }

    public void CreateIngredientIcons(Dictionary<string, int> requiredIngredients) {
        // float cauldronPositionX = GameObject.FindWithTag("Cauldron").transform.position.x;

        int j = 0;

        foreach (KeyValuePair<string, int> ingredient in requiredIngredients) 
        {
            GameObject foodImage = null;
            switch(ingredient.Key) {
                case "Grease":
                    foodImage = greaseIcon;
                    break;
                case "Water":
                    foodImage = waterIcon;
                    break;
                case "Corn":
                default:
                    foodImage = cornIcon;
                    break;
            }

            for(int i = 0; i < ingredient.Value; i++) {
                GameObject food = Instantiate(foodImage, new Vector3(-860 + j*60, 420, 0), Quaternion.identity);
                // GameObject food = Instantiate(foodImage, new Vector3(cauldronPositionX + j*50, 250, 0), Quaternion.identity);

                food.transform.SetParent(canvas.transform, false);
                food.tag = "IngredientIcon";
                
                j++;
            }
        }
    }

    public void CreateReqStirsIcons(int reqStirs) {
        // foreach (KeyValuePair<string, int> ingredient in req) 
        // {
        int j = 0;

            GameObject stirImage = stirIcon;
            // switch(ingredient.Key) {
            //     case "Grease":
            //         foodImage = greaseIcon;
            //         break;
            //     case "Water":
            //         foodImage = waterIcon;
            //         break;
            //     case "Corn":
            //     default:
            //         foodImage = cornIcon;
            //         break;
            // }

            for(int i = 0; i < reqStirs; i++) {
                GameObject food = Instantiate(stirImage, new Vector3(-860 + j*60, 480, 0), Quaternion.identity);
                // GameObject food = Instantiate(foodImage, new Vector3(cauldronPositionX + j*50, 250, 0), Quaternion.identity);

                food.transform.SetParent(canvas.transform, false);
                food.tag = "StirIcon";
                
                j++;
            }
        // }
    }

    public void CreatePotIcon() {
        GameObject cauldron = Instantiate(cauldronIcon);

        cauldron.transform.SetParent(canvas.transform, false);

    }

    public void RemoveAllClockIcons() {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            GameObject child = canvas.transform.GetChild(i).gameObject;
            if (child.tag == "ClockIcon") {
                Destroy(child);
            }
        }
    }

    public void RemoveAllIngredientIcons() {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            GameObject child = canvas.transform.GetChild(i).gameObject;
            if (child.tag == "IngredientIcon") {
                Destroy(child);
            }
            //Do something with child
        }
    }

    public void RemoveAllStirIcons() {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            GameObject child = canvas.transform.GetChild(i).gameObject;
            if (child.tag == "StirIcon") {
                Destroy(child);
            }
        }
    }

    public void RemovePotIcon() {
        GameObject cauldron = GameObject.FindWithTag("Cauldron");
        Destroy(cauldron);
    }

    private void RemoveAllInventoryIcons() {
        for (int i = 0; i < inventoryBox.transform.childCount; i++)
        {
            GameObject child = inventoryBox.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    public void DisplayIngredientIcons(Inventory ingredients) {

        RemoveAllInventoryIcons();

        if (ingredients.CountItems() > 0) {

            float xPos = 0f;
            float yPos = 0f;

            foreach (KeyValuePair<string,int> ingredient in ingredients.GetInventoryAsDictionary()) {

                GameObject ingredientIcon = null;
                switch(ingredient.Key) {
                    case "Grease":
                        ingredientIcon = greaseIcon;
                        break;
                    case "Water":
                        ingredientIcon = waterIcon;
                        break;
                    case "Corn":
                    default:
                        ingredientIcon = cornIcon;
                        break;
                }

                GameObject instantIcon = Instantiate(ingredientIcon, new Vector3(xPos, yPos, 0), Quaternion.identity);

                instantIcon.transform.SetParent(inventoryBox.transform, false);

                GameObject ingredientAmountLabel = new GameObject();
                ingredientAmountLabel.transform.SetParent(inventoryBox.transform, false);
                ingredientAmountLabel.transform.localPosition = new Vector3(100, yPos-25, 0);
                ingredientAmountLabel.AddComponent<Text>();
                Text text = ingredientAmountLabel.GetComponent<Text>();
                text.font = font;
                text.text = ingredient.Value.ToString();
                text.fontSize = 48;

                yPos -= 75f;
            }
        } else {
            Debug.Log("no ingredients in inventory");
        }
    }
}
