﻿using System.Collections;
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
    public GameObject NextLevelButton;
    public GameObject NextTurnButton;
    public GameObject CancelButton;
    public GameObject ConfirmButton;

    public MoveButton PassButton;

    private BattleManager bm;

    // CONSTANTS
    const float XPOS_SKILLS = 150f;
    const float YPOS_SKILLS = 150f;

    const float XPOS_MOVES  = -150f;
    const float YPOS_MOVES  = 150f;

    const float XPOS_PASS   = 0f;
    const float YPOS_PASS   = -165f;

    private void Awake()
    {
        InitializeBattleManager();
        // InitializeCamera();
    }

    private void InitializeBattleManager()
    {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    // private void InitializeCamera()
    // {
    //     canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
    // }

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

    public void DisplayActionButtons(UnitStats unit, bool freeMove, bool freeSkill)
    {
        if (!MoveBox.activeSelf) {
            MoveBox.SetActive(true);
        }

        CleanUpButtons();

        if (freeMove) DisplayMoves(unit);
        if (freeSkill) DisplaySkills(unit);
        if (freeSkill || freeMove) DisplayPass(unit);
    }

    public void DisplayMoves(UnitStats unit)
    {
        float xPos = XPOS_MOVES;

        // Iterate this value to make position lower
        float yPos = YPOS_MOVES;

        if (unit.GetMoves().Count > 0) {
            foreach (MoveButton button in unit.GetMoves()) {
                MoveButton instantButton = Instantiate(button, new Vector3(xPos, yPos, 0), Quaternion.identity);
                // instantButton.transform.localScale = new Vector3(2.0f,2.0f,1);
                instantButton.transform.SetParent(MoveBox.transform, false);
                // second param keeps scale etc the same

                button.SetUpButtonAction();

                string moveText = button.GetName();

                if (button.GetAction().GetGutsCost() > 0) {
                    moveText += " : " + button.GetAction().GetGutsCost().ToString();
                }

                instantButton.GetComponentInChildren<Text>().text = moveText;


                yPos -= 45f;
            }
        } else {
            print ("ERROR: This unit has no moves");
            // EndTurn();
        }
    }

    public void DisplaySkills(UnitStats unit)
    {
        float xPos = XPOS_SKILLS;

        // Iterate this value to make position lower
        float yPos = YPOS_SKILLS;

        if (unit.GetSkills().Count > 0) {
            foreach (MoveButton button in unit.GetSkills()) {
                MoveButton instantButton = Instantiate(button, new Vector3(xPos, yPos, 0), Quaternion.identity);
                // instantButton.transform.localScale = new Vector3(2.0f,2.0f,1);
                instantButton.transform.SetParent(MoveBox.transform, false);
                // second param keeps scale etc the same

                button.SetUpButtonAction();

                string moveText = button.GetName();

                if (button.GetAction().GetGutsCost() > 0) {
                    moveText += " : " + button.GetAction().GetGutsCost().ToString();
                }

                instantButton.GetComponentInChildren<Text>().text = moveText;


                yPos -= 45f;
            }
        } else {
            print ("ERROR: This unit has no SKILLS");
            // EndTurn();
        }
    }

    private void DisplayPass(UnitStats unit)
    {
        float xPos = XPOS_PASS;
        float yPos = YPOS_PASS;

        // MoveButton button = new MoveButton();
        // GameObject button;

        // MoveButton PassButton;


        MoveButton instantButton = Instantiate(PassButton, new Vector3(xPos, yPos, 0), Quaternion.identity);
        instantButton.transform.SetParent(MoveBox.transform, false);

        // PassButton.SetUpButtonAction();

        // string moveText = PassButton.GetName();

        // button.SetUpButtonAction();

        // string moveText = button.GetName();
        // instantButton.GetComponentInChildren<Text>().text = moveText;
    }

    public void DisplayRecipes(List<Recipe> recipes)
    {
        CleanUpButtons();

        float xPos = 50f;

        // Iterate this value to make position lower
        float yPos = 100f;

        if (recipes.Count > 0) {

            foreach (Recipe recipe in recipes) {
                GameObject instantButton = Instantiate(cookRecipeButton, new Vector3(xPos, yPos, 0), Quaternion.identity);
                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                instantButton.transform.SetParent(MoveBox.transform, false);
                // second param keeps scale etc the same

                instantButton.GetComponentInChildren<Text>().text = recipe.GetName();

                CookRecipe buttonMove = instantButton.GetComponent<CookRecipe>();
                buttonMove.SetRecipe(recipe);

                yPos -= 45f;

                ActivateCancelButton(true);
            }
        } else {
            FindObjectOfType<CombatLogManager>().PrintToLog("You have no recipes");

            print ("ERROR: You have no recipes");
            bm.ResetTurn();
        }
    }

    public void DisplayIngredients(Inventory ingredients)
    {
        CleanUpButtons();

        float xPos = 50f;

        // Iterate this value to make position lower
        float yPos = 100f;


        if (ingredients.CountItems() > 0) {
            foreach (KeyValuePair<string,int> ingredient in ingredients.GetInventoryAsDictionary()) {
                GameObject instantButton = Instantiate(ingredientButton, new Vector3(xPos, yPos, 0), Quaternion.identity);
                instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                instantButton.transform.SetParent(MoveBox.transform, false);
                // second param keeps scale etc the same

                instantButton.GetComponentInChildren<Text>().text = ingredient.Key;

                AddIngredient buttonMove = instantButton.GetComponent<AddIngredient>();
                buttonMove.SetIngredient(ingredient.Key);

                yPos -= 45f;

                ActivateCancelButton(true);
            }
        } else {
            FindObjectOfType<CombatLogManager>().PrintToLog("Can't use move: You have no ingredients");

            print ("You have no ingredients");
            bm.ResetTurn();
        }
    }

    public void DisplayMealsToEat(List<Item> meals)
    {
        CleanUpButtons();

        float xPos = 50f;

        // Iterate this value to make position lower
        float yPos = 100f;
        bool atLeastOneTreat = false;


        if (meals.Count > 0) {
            foreach (Item meal in meals) {
                if (meal.Meal && meal.Treat) {
                    atLeastOneTreat = true;

                    GameObject instantButton = Instantiate(eatButton, new Vector3(xPos, yPos, 0), Quaternion.identity);

                    instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                    instantButton.transform.SetParent(MoveBox.transform, false);
                    // second param keeps scale etc the same

                    instantButton.GetComponentInChildren<Text>().text = meal.Name;
                    Eat buttonMove = instantButton.GetComponent<Eat>();

                    buttonMove.AddResourceCost(meal);

                    yPos -= 45f;

                    ActivateCancelButton(true);
                }
            }
        }

        if (!atLeastOneTreat) {
            FindObjectOfType<CombatLogManager>().PrintToLog("Can't use move: You have no treat to eat");
            
            print ("You have no good treats to eat.");
            bm.ResetTurn();
        }
    }

    public void DisplayMealsToServe(List<Item> meals, string mealType)
    {
        CleanUpButtons();

        float xPos = 50f;

        // Iterate this value to make position lower
        float yPos = 150f;
        bool atLeastOneTrick = false;

        if (meals.Count > 0) {
            foreach (Item meal in meals) {
                switch(mealType) {
                    case "Drink":
                        if (meal.Meal && meal.Trick && meal.Drink) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveDrinkButton, new Vector3(xPos, yPos, 0), Quaternion.identity);
                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            instantButton.transform.SetParent(MoveBox.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeDrink buttonMove = instantButton.GetComponent<ServeDrink>();
                            buttonMove.AddResourceCost(meal);

                            yPos -= 45f;

                            ActivateCancelButton(true);
                        }
                        break;
                    case "Food":
                        if (meal.Meal && meal.Trick && meal.Food) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveFoodButton, new Vector3(xPos, yPos, 0), Quaternion.identity);
                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            instantButton.transform.SetParent(MoveBox.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeFood buttonMove = instantButton.GetComponent<ServeFood>();
                            buttonMove.AddResourceCost(meal);

                            yPos -= 45f;

                            ActivateCancelButton(true);
                        }
                        break;
                    case "Meal":
                        if (meal.Meal && meal.Trick) {

                            atLeastOneTrick = true;

                            GameObject instantButton = Instantiate(serveMealButton, new Vector3(xPos, yPos, 0), Quaternion.identity);
                            instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                            instantButton.transform.SetParent(MoveBox.transform, false);
                            // second param keeps scale etc the same

                            instantButton.GetComponentInChildren<Text>().text = meal.Name;

                            ServeMeal buttonMove = instantButton.GetComponent<ServeMeal>();
                            buttonMove.AddResourceCost(meal);

                            yPos -= 45f;

                            ActivateCancelButton(true);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        if (!atLeastOneTrick) {
            FindObjectOfType<CombatLogManager>().PrintToLog("Can't use move: You have no trick to pick");

            print ("You have no good tricks to serve.");

            bm.ResetTurn();
        }
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

                ActivateCancelButton(true);

            }
            } else {
                FindObjectOfType<CombatLogManager>().PrintToLog("Can't use move: You have no one to target");

                print ("No PossibleTargets");

                bm.ResetTurn();
            }
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

    public void NextLevel()
    {
        CleanUpButtons();

        GameObject button = Instantiate(NextLevelButton);
        button.transform.SetParent(MoveBox.transform, false);


        // NextLevelButton.SetActive(true);
    }
}