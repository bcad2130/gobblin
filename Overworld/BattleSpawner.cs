using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleSpawner : MonoBehaviour
{
    public string sceneName;
    public int stageClearNumber;
    public GameObject previewWindow;
    public GameObject unlockIcon;
    public GameObject lockIcon;
    public GameObject startBattleLabel;
    public GameObject cannotStartBattleLabel;
    public GameObject enemyPreviewText;

    private GameObject previewObj;
    private GameObject lockObj;
    private GameObject startBattleObj;
    private GameObject enemyTextObj;

    [SerializeField]
    private GameObject[] enemyEncounterPrefabs;

    [SerializeField]
    private GameObject dialogue;

    private StatusTracker tracker;
    private Canvas canvas;

    private bool spawning = false;
    private bool dialoguing = false;
    public bool collision = false;

    void Start() {
        DontDestroyOnLoad (this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        tracker = GameObject.FindObjectOfType<StatusTracker>();
        canvas = GameObject.FindObjectOfType<Canvas>();
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
                Instantiate (dialogue);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy (this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "OverworldPlayer") {
            collision = true;
            SpawnPreviewWindow();
        }
    }

    private void SpawnPreviewWindow() {
        previewObj = Instantiate (previewWindow, canvas.transform);
        previewObj.transform.position = this.transform.position;

        if (tracker.GetStageClearTotal() >= stageClearNumber - 1) {
            lockObj = Instantiate (unlockIcon, previewObj.transform);
            startBattleObj = Instantiate (startBattleLabel, previewObj.transform);
        } else {
            lockObj = Instantiate (lockIcon, previewObj.transform);
            startBattleObj = Instantiate (cannotStartBattleLabel, previewObj.transform);
        }

        if (enemyEncounterPrefabs.Length > 0) {
            enemyTextObj = Instantiate (enemyPreviewText, previewObj.transform);
            TMP_Text testText = enemyTextObj.GetComponent<TMP_Text>();

            foreach(GameObject enemy in enemyEncounterPrefabs) {
                UnitStats unit = enemy.GetComponent<UnitStats>();

                testText.text += unit.GetName() + "\n";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        collision = false;

        DespawnPreviewWindow();
    }

    private void DespawnPreviewWindow() {
        Destroy(previewObj);
        Destroy(lockObj);
        Destroy(startBattleObj);
        Destroy(enemyTextObj);
    }

    void LoadBattleScene() {
        SceneManager.LoadScene(sceneName);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return) && collision == true && tracker.GetStageClearTotal() >= stageClearNumber - 1)
        {
            this.spawning = true;
            this.dialoguing = true;
            LoadBattleScene();
        }
    }
}
