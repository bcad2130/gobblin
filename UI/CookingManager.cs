using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour
{
    private BattleManager bm;

    public Canvas canvas;

    public GameObject countdownClock_0;
    public GameObject countdownClock_1;
    public GameObject countdownClock_2;

    public GameObject cornIcon;
    public GameObject greaseIcon;
    public GameObject waterIcon;

    private void Awake()
    {
        InitializeBattleManager();
    }

    private void InitializeBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
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

        GameObject clock = Instantiate(clockImage, new Vector3(-250, 300, 0), Quaternion.identity);
        clock.transform.SetParent(canvas.transform, false);
        clock.tag = "ClockIcon";
    }

    public void CreateIngredientIcons(Dictionary<string, int> requiredIngredients) {
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
                GameObject food = Instantiate(foodImage, new Vector3(-250 + j*50, 250, 0), Quaternion.identity);
                food.transform.SetParent(canvas.transform, false);
                food.tag = "IngredientIcon";
                
                j++;
            }
        }
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
}
