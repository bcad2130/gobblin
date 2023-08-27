using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public GameObject TurnBox;
    public Image iconPrefab;

    private BattleManager bm;

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
                icon.transform.SetParent(TurnBox.transform, false);

                icon.sprite = unit.GetSmallIcon();
                xPos += 150f;
            }
        }
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
