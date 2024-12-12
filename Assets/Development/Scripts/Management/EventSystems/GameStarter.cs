using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
public class GameStarter : MonoBehaviour
{
    public List<Transform> spawnPozs;

    public GameObject enemyPrefab;

    public LevelData currentLevelData;

    public GameObject mainMenuPanel, policeCar, actionUi, statsUi, playerCamera,winLosePanel, gamePanel;

    [SerializeField] private TextMeshProUGUI levelCountText, enemyCountText, hostileCountText;
    [SerializeField] private TextMeshProUGUI levelStatusText,levelStatusDescriptionText;

    public static GameStarter Instance { get; private set; }

    public LevelData currentData;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
#if UNITY_EDITOR
        Application.targetFrameRate = 400;
#endif 
    }
    public void InitGame(bool isNewGame)
    {
        if(isNewGame)
        {
            foreach (LevelData levelData in SaveLoadManager.Instance.levelDataList)
            {
                if (!levelData.isFinish && !levelData.isResume)
                {
                    currentLevelData = levelData;
                    break;
                }
            }
        }
        else
        {
            foreach (LevelData levelData in SaveLoadManager.Instance.levelDataList)
            {
                if ((!levelData.isFinish && levelData.isResume ) || (!levelData.isFinish && !levelData.isResume))
                {
                    currentLevelData = levelData;
                    break;
                }
            }
        }

        mainMenuPanel.SetActive(true);
        policeCar.SetActive(true);
        actionUi.SetActive(false);
        statsUi.SetActive(false);
        playerCamera.SetActive(false);
        winLosePanel.SetActive(false);
        CheckStatsLevel();
    }
    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        policeCar.SetActive(false);
        actionUi.SetActive(true);
        statsUi.SetActive(true);
        playerCamera.SetActive(true);
        LevelManager.Instance.isTimeCounting = true;
        currentLevelData.isResume = true;

        SpawnEnemies();

        SaveLoadManager.Instance.SaveGame();
    }

    public void SpawnEnemies()
    {
        if (spawnPozs.Count > 0 && currentLevelData.enemyCount > 0)
        {
            for (int i = 0; i < currentLevelData.enemyCount; i++)
            {
                Transform spawnPos = spawnPozs[Random.Range(0, spawnPozs.Count)];
                Instantiate(enemyPrefab, spawnPos.position, spawnPos.rotation);
            }
        }
    }
    public void CloseGame()
    {
        SaveLoadManager.Instance.SaveGame();
        Application.Quit();
    }
    public void NextLevel()
    {
        SaveLoadManager.Instance.SaveGame();
        SceneManager.LoadScene(0);
    }
    public void WinGame()
    {
        mainMenuPanel.SetActive(false);
        actionUi.SetActive(false);
        statsUi.SetActive(false);
        playerCamera.SetActive(false);
        winLosePanel.SetActive(true);

        levelStatusText.text = "Win";
        levelStatusDescriptionText.text = "You are the rescue all hostile and killed the enemys. Go next level!";
    }
    public void LoseGame()
    {
        mainMenuPanel.SetActive(false);
        actionUi.SetActive(false);
        statsUi.SetActive(false);
        playerCamera.SetActive(false);
        winLosePanel.SetActive(true);

        levelStatusText.text = "LOSE";
        levelStatusDescriptionText.text = "You are lose this level. Try again";
    }
    public void CheckStatsLevel()
    {
        levelCountText.text = "LEVEL " + currentLevelData.levelCount.ToString();
        enemyCountText.text = currentLevelData.enemyCount.ToString() + " Enemy";
        hostileCountText.text = currentLevelData.hostileCount.ToString() + " Hostile";
    }
}
