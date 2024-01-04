// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSpawner : MonoBehaviour
{
    public string sceneName;

    [SerializeField]
    private GameObject[] enemyEncounterPrefabs;

    [SerializeField]
    private GameObject dialogue;

    private bool spawning = false;
    private bool dialoguing = false;

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

            if (this.dialoguing && dialogue) {
                // Debug.Log("spawn dialogue");
                Instantiate (dialogue);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy (this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "OverworldPlayer") {
            this.spawning = true;
            this.dialoguing = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}
