// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSpawner : MonoBehaviour
{
    public string sceneName;

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "OverworldPlayer") {
            // this.spawning = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}
