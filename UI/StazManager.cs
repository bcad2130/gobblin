using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StazManager : MonoBehaviour
{
    public GameObject StazBox;
    public GameObject AllStaz;
    public GameObject MidTextObj;
    public Canvas canvas;

    private BattleManager bm;

    public Text Name;
    public Text Health;
    public Text Guts;
    public Text Strength;
    public Text Defense;
    public Text Speed;
    public Text Taste;
    public Text Tum;
    public Text Nose;
    public Text Cover;
    public Text StatusEffects;


    private const string STR = "STR";
    private const string DEF = "DEF";
    private const string SPD = "SPD";
    private const string TST = "TST";
    private const string TUM = "TUM";
    private const string NOS = "NOS";

    void Awake () {
        InitializeBattleManager();
        InitializeCamera();
    }

    private void InitializeBattleManager() {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    private void InitializeCamera()
    {
        canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
    }


    // decouple this so you only update what you need to
    public void ShowStaz(UnitStats unit) {
        if (!StazBox.activeSelf) {
            StazBox.SetActive(true);
        }

        ActivateAllStazText(true);
        MidTextObj.SetActive(false);

        Name.text   = unit.GetName().ToString();
        Health.text = "HP" + "\n" + unit.GetCurrentHealth().ToString() + " / " + unit.GetMaxHealth().ToString();
        Guts.text   = "GP"   + "\n" + unit.GetCurrentGuts().ToString()   + " / " + unit.GetMaxGuts().ToString();

        Strength.text   = STR + "\n" + unit.GetBaseStrength().ToString();
        Defense.text    = DEF + "\n" + unit.GetBaseDefense().ToString();
        Speed.text      = SPD + "\n" + unit.GetBaseSpeed().ToString();
        Taste.text      = TST + "\n" + unit.GetBaseTaste().ToString();
        Tum.text        = TUM + "\n" + unit.GetBaseTum().ToString();
        Nose.text       = NOS + "\n" + unit.GetBaseNose().ToString();

        if (unit.GetBonusStrength() != 0) {
            Strength.text   += "+" + unit.GetBonusStrength().ToString();
        }
        if (unit.GetBonusDefense() != 0) {
            Defense.text    += "+" + unit.GetBonusDefense().ToString();
        }
        if (unit.GetBonusSpeed() != 0) {
            Speed.text      += "+" + unit.GetBonusSpeed().ToString();
        }
        if (unit.GetBonusTaste() != 0) {
            Taste.text      += "+" + unit.GetBonusTaste().ToString();
        }
        if (unit.GetBonusTum() != 0) {
            Tum.text        += "+" + unit.GetBonusTum().ToString();
        }
        if (unit.GetBonusNose() != 0) {
            Nose.text       += "+" + unit.GetBonusNose().ToString();
        }

        Cover.text = bm.CoverStaz(unit);

        StatusEffects.text = "";
        foreach (KeyValuePair<string,int> effect in unit.GetAllStatusEffects())
        {
            if (effect.Value > 0) {
                StatusEffects.text += effect.Value + " " + effect.Key + "\n";
            }
        }
    }

    public void ShowMessage(string message)
    {
        if (!StazBox.activeSelf) {
            StazBox.SetActive(true);
        }

        ActivateAllStazText(false);
        MidTextObj.SetActive(true);

        MidTextObj.GetComponent<Text>().text = message;
    }

    private void ActivateAllStazText(bool active)
    {
        AllStaz.SetActive(active);

        // GameObject[] allStazText;
        // allStazText = GameObject.FindGameObjectsWithTag("StazText");

        // foreach (GameObject text in allStazText) {
        //     text.SetActive(active);
        // }
    }
}
