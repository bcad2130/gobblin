using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    // public string LevelName;

    public void NextLevel ()
    {
        FindObjectOfType<BattleManager>().LoadNextLevel();
    }
}
