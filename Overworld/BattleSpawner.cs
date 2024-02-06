using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSpawner : MonoBehaviour
{
    public string sceneName;
    public int stageClearNumber;

    [SerializeField]
    private GameObject[] enemyEncounterPrefabs;

    [SerializeField]
    private GameObject dialogue;

    private StatusTracker tracker;

    private bool spawning = false;
    private bool dialoguing = false;

    void Start() {
        DontDestroyOnLoad (this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        tracker = GameObject.FindObjectOfType<StatusTracker>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        tracker = GameObject.FindObjectOfType<StatusTracker>();

        if (scene.name == "Battle") {
            if (this.spawning && enemyEncounterPrefabs.Length > 0) {

                tracker.SetActiveStageNumber(stageClearNumber);

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
            if (tracker.GetStageClearTotal() + 1 == stageClearNumber) {
                LoadBattleScene();
            }
        }
    }

    void LoadBattleScene() {
        this.spawning = true;
        this.dialoguing = true;
        SceneManager.LoadScene(sceneName);
    }
}
