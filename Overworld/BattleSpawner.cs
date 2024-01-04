// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSpawner : MonoBehaviour
{
    public string sceneName;

    [SerializeField]
    private GameObject[] enemyEncounterPrefabs;

    private bool spawning = false;

    void Start() {
        DontDestroyOnLoad (this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Battle") {
            if (this.spawning && enemyEncounterPrefabs.Length > 0) {

                foreach(GameObject enemy in enemyEncounterPrefabs)
                {
                    Instantiate (enemy);
                }
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy (this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "OverworldPlayer") {
            this.spawning = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}
