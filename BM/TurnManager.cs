using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    private Image OnDeck;
    private Image InTheHole;
    private Image Pos3;
    private Image Pos4;
    private Image Pos5;
    private Image Pos6;

    private List<Image> nextTurns = new List<Image>();

    public GameObject TurnBox;
    public Canvas canvas;
    public Image iconPrefab;

    private BattleManager bm;

    private void Awake()
    {
        // InitializeCamera();
        // InitializeBattleManager();
        // InitializeNextTurns();
    }

    // private void InitializeBattleManager()
    // {
    //     bm = GameObject.FindObjectOfType<BattleManager>();
    // }

    // private void InitializeNextTurns()
    // {
    //     nextTurns.Add(OnDeck);
    //     nextTurns.Add(InTheHole);
    //     nextTurns.Add(Pos3);
    //     nextTurns.Add(Pos4);
    //     nextTurns.Add(Pos5);
    //     nextTurns.Add(Pos6);
    // }

    // private void InitializeCamera()
    // {
    //     canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
    // }

    public void DisplayNextTurns(List<UnitStats> nextUnits)
    {
        if (!TurnBox.activeSelf) {
            TurnBox.SetActive(true);
        }

        RemoveAllIcons();

        // Iterate this value to make position lower
        float xPos = -875f;

        // constant
        float yPos = -0f;

        if (nextUnits.Count > 0) {
            foreach (UnitStats unit in nextUnits) {
                Image icon = Instantiate(iconPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                // instantButton.transform.localScale = new Vector3(1.0f,1.0f,1);
                icon.transform.SetParent(TurnBox.transform, false);
                // second param keeps scale etc the same

                icon.sprite = unit.GetSmallIcon();
                xPos += 150f;
            }
        }

        // JUST INSTANTIATE IMAGES LIKE YOU DO IN DISPLAY BUTTONS
        // for (int i = 0; i < nextTurns.Count; i++)
        // {
        //     // Debug.Log(nextUnits[i].GetName());
        //     // Debug.Log(nextUnits[i] != null);
        //     if (nextUnits[i] != null && nextTurns[i] != null) {
        //         Debug.Log('A');
        //         nextTurns[i] = nextUnits[i].GetSmallIcon();
        //     }
        // }
    }

    private void RemoveAllIcons()
    {
        GameObject[] allIcons;
        allIcons = GameObject.FindGameObjectsWithTag("TurnIcon");

        foreach (GameObject icon in allIcons) {
            Destroy(icon);
        }
    }
}
