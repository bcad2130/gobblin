﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLogManager : MonoBehaviour
{
    public Text CombatText;
    public Text CombatLog;
    public GameObject CombatLogBox;

    private string lastLog;

    public void PrintToLog(string text)
    {
        if (!CombatLogBox.activeSelf) {
            CombatLogBox.SetActive(true);
        }

        NewCombatText(text);
    }

    public void ClearLog()
    {
        SetCombatLogText("");
        SetCombatText("");
        lastLog         = null;
    }

    private void NewCombatText(string text)
    {
        CycleLog();

        SetLastLog(text);
        TypeCombatText(text);
    }

    private void CycleLog()
    {
        // Debug.Log("CycleLog");
        // Last Log keeps track of the last string given to the Combat Text, ready to be transferred to CombatLog
        if (GetLastLog() != null) {
            CombatLog.text += "\n" + GetLastLog();
        }
    }

    private string GetLastLog()
    {
        return lastLog;
    }

    private void SetLastLog(string text)
    {
        lastLog = text;
    }

    private string GetCombatLogText()
    {
        return lastLog;
    }

    private void SetCombatLogText(string text)
    {
        CombatLog.text = text;
    }

    private string GetCombatText()
    {
        return CombatText.text;
    }

    private void SetCombatText(string text)
    {
        CombatText.text = text;
    }

    private void TypeCombatText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(text));
    }
    
    private IEnumerator TypeText (string text)
    {
        CombatText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            CombatText.text += letter;
            
            // 24 frames
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }
}
