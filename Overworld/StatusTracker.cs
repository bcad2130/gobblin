using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusTracker : MonoBehaviour
{
    public int stagesCleared = 0;
    public int activeStageNumber = 0;

    public void ClearLevel(int stageClear) {
        if (stageClear > stagesCleared) {
            stagesCleared = stageClear;
        }
    }

    public int GetStageClearTotal() {
        return stagesCleared;
    }

    public void SetStageClearTotal(int stageClearParam) {
        stagesCleared = stageClearParam;
    }

    public int GetActiveStageNumber() {
        return activeStageNumber;
    }

    public void SetActiveStageNumber(int activeStageParam) {
        activeStageNumber = activeStageParam;
    }

    void Start() {
        DontDestroyOnLoad (this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
