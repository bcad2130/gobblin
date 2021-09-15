using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveManager : MonoBehaviour
{
    public GameObject MoveBox;
    public Canvas canvas;

    // Instant Buttons
    public GameObject targetButton;
    public GameObject eatButton;
    public GameObject serveDrinkButton;
    public GameObject serveFoodButton;
    public GameObject serveMealButton;
    public GameObject cookRecipeButton;
    public GameObject ingredientButton;

    // Active Buttons
    public GameObject NextTurnButton;
    public GameObject CancelButton;
    public GameObject ConfirmButton;

    private BattleManager bm;

    private void Awake()
    {
        InitializeBattleManager();
        InitializeCamera();
    }

    private void InitializeBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    private void InitializeCamera()
    {
        canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
    }

    public void ActivateMoveBox(bool active)
    {
        MoveBox.SetActive(active);
    }

    public void ActivateNextTurnButton(bool active)
    {
        NextTurnButton.SetActive(active);
    }

    public void ActivateCancelButton(bool active)
    {
        CancelButton.SetActive(active);
    }

    public void ActivateConfirmButton(bool active)
    {
        ConfirmButton.SetActive(active);
    }

    public void SetNextTurnButtonText(string text)
    {
        NextTurnButton.GetComponentInChildren<Text>().text = text;
    }

    public void DisplayMoves(UnitStats unit)
    {
        if (!MoveBox.activeSelf) {
            MoveBox.SetActive(true);
        }

        CleanUpButtons();

        // Iterate this value to make position lower
        float yPos = 50f;

        if (unit.GetMoves().Count > 0) {
            foreach (MoveButton button in unit.GetMoves()) {
                MoveButton instantButton = Instantiate(button, new Vector3(0, yPos, 0), Quaternion.identity);
                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                instantButton.transform.SetParent(MoveBox.transform, false);
                // second param keeps scale etc the same

                button.SetUpButtonAction();

                string moveText = button.GetName() + " : " + button.GetAction().GetGutsCost().ToString();
                instantButton.GetComponentInChildren<Text>().text = moveText;

                yPos -= 30f;
            }
        } else {
            print ("ERROR: This unit has no moves");
            // EndTurn();
        }
    }

    public void DisplayRecipes(List<Recipe> recipes)
    {
        CleanUpButtons();

        // Iterate this value to make position lower
        float yPos = 0f;

        if (recipes.Count > 0) {

            foreach (Recipe recipe in recipes) {
                GameObject instantButton = Instantiate(cookRecipeButton, new Vector3(0, yPos, 0), Quaternion.identity);
                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                instantButton.transform.SetParent(MoveBox.transform, false);
                // second param keeps scale etc the same

                instantButton.GetComponentInChildren<Text>().text = recipe.GetName();

                CookRecipe buttonMove = instantButton.GetComponent<CookRecipe>();
                buttonMove.SetRecipe(recipe);

                yPos -= 30f;
            }
        } else {
            print ("You have no recipes");
            bm.ResetTurn();
        }

        ActivateCancelButton(true);
    }

    public void DisplayIngredients(Inventory ingredients)
    {
        CleanUpButtons();

        // Iterate this value to make position lower
        float yPos = 0f;


        if (ingredients.CountItems() > 0) {
            foreach (KeyValuePair<string,int> ingredient in ingredients.GetInventoryAsDictionary()) {
                GameObject instantButton = Instantiate(ingredientButton, new Vector3(0, yPos, 0), Quaternion.identity);
                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                instantButton.transform.SetParent(MoveBox.transform, false);
                // second param keeps scale etc the same

                instantButton.GetComponentInChildren<Text>().text = ingredient.Key;

                AddIngredient buttonMove = instantButton.GetComponent<AddIngredient>();
                buttonMove.SetIngredient(ingredient.Key);

                yPos -= 30f;
            }
        } else {
            print ("You have no ingredients");
            bm.ResetTurn();
        }

        ActivateCancelButton(true);
    }

    public void DisplayMealsToEat(List<Item> meals)
    {
        CleanUpButtons();

        // Iterate this value to make position lower
        float yPos = 0f;
        bool atLeastOneTreat = false;


        if (meals.Count > 0) {
            foreach (Item meal in meals) {
                if (meal.Meal && meal.Treat) {
                    atLeastOneTreat = true;

                    GameObject instantButton = Instantiate(eatButton, new Vector3(0, yPos, 0), Quaternion.identity);

                    instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                    instantButton.transform.SetParent(MoveBox.transform, false);
                    // second param keeps scale etc the same

                    instantButton.GetComponentInChildren<Text>().text = meal.Name;
                    Eat buttonMove = instantButton.GetComponent<Eat>();

                    buttonMove.AddResourceCost(meal);

                    yPos -= 30f;
                }
            }
        }

        if (!atLeastOneTreat) {
            print ("You have no good treats to eat.");
            bm.ResetTurn();
        }

        ActivateCancelButton(true);
    }

    public void DisplayMealsToServe(List<Item> meals, string mealType)
    {
        CleanUpButtons();

        // Iterate this value to make position lower
        float yPos = 0f;
        bool atLeastOneTrick = false;

        if (meals.Count > 0) {
            foreach (Item meal in meals) {
                switch(mealType) {
                    case "Drink":
                        if (meal.Meal && meal.Trick && meal.Drink) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveDrinkButton, new Vector3(0, yPos, 0), Quaternion.identity);
                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            instantButton.transform.SetParent(MoveBox.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeDrink buttonMove = instantButton.GetComponent<ServeDrink>();
                            buttonMove.AddResourceCost(meal);

                            yPos -= 30f;
                        }
                        break;
                    case "Food":
                        if (meal.Meal && meal.Trick && meal.Food) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveFoodButton, new Vector3(0, yPos, 0), Quaternion.identity);
                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            instantButton.transform.SetParent(MoveBox.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeFood buttonMove = instantButton.GetComponent<ServeFood>();
                            buttonMove.AddResourceCost(meal);

                            yPos -= 30f;
                        }
                        break;
                    case "Meal":
                        if (meal.Meal && meal.Trick) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveMealButton, new Vector3(0, yPos, 0), Quaternion.identity);
                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            instantButton.transform.SetParent(MoveBox.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeMeal buttonMove = instantButton.GetComponent<ServeMeal>();
                            buttonMove.AddResourceCost(meal);

                            yPos -= 30f;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        if (!atLeastOneTrick) {
            print ("You have no good tricks to serve.");

            bm.ResetTurn();
        }

        ActivateCancelButton(true);
    }

    public void DisplayTargets(List<UnitStats> targets)
    {
        CleanUpButtons();

        if (targets.Count > 0) {
            foreach (UnitStats target in targets) {
                GameObject unitBox = target.GetUnitBox();

                GameObject instantButton = Instantiate(targetButton);
                instantButton.transform.SetParent(unitBox.transform, false);
                // instantButton.transform.localScale = new Vector3(.2f,.2f,1);

                UnitStats tempTargetUnit = target;

                // TODO find better way than setting parents
                instantButton.GetComponent<TargetButton>().SetParentUnit(ref tempTargetUnit);
            }
            } else {
                print ("No PossibleTargets");

                bm.ResetTurn();
            }

        ActivateCancelButton(true);
    }

    public void NextTurn()
    {
        bm.NextTurn();
    }

    public void CancelTurn()
    {
        bm.ResetTurn();
    }

    public void ConfirmAction()
    {
        bm.TakeAction();
    }

    public void CleanUpButtons()
    {
        RemoveInstantButtons();
        HideActiveButtons();
    }

    private void RemoveInstantButtons()
    {
        GameObject[] allButtons;
        allButtons = GameObject.FindGameObjectsWithTag("InstantButton");

        foreach (GameObject button in allButtons) {
            Destroy(button);
        }
    }

    private void HideActiveButtons()
    {
        ActivateConfirmButton(false);
        ActivateCancelButton(false);
        ActivateNextTurnButton(false);
    }
}
