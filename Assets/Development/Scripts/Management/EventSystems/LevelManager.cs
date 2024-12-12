using UnityEngine;
using TMPro;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public LevelData currentData;

    [SerializeField] private float timeCounter;

    public bool isTimeCounting = false;


    [SerializeField] private TextMeshProUGUI timeCounterText,enemyCountText,hostileCountText; 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        currentData = GameStarter.Instance.currentLevelData;
        timeCounter = currentData.levelTime;
        CheckLevelStatus();
    }
    private void Update()
    {
        if(isTimeCounting)
        {
            timeCounter -= Time.deltaTime;

            timeCounterText.text = "LAST TÝME: " + Mathf.FloorToInt(timeCounter).ToString();

            if (timeCounter <= 0)
            {
                GameStarter.Instance.LoseGame();
                isTimeCounting = false; 
            }
        }
    }
    public void CheckLevelStatus()
    {
        hostileCountText.text ="Hostile: " + currentData.hostileCount.ToString();
        enemyCountText.text = "Enemy: " + currentData.enemyCount.ToString();

        if(currentData.enemyCount <= 0 && currentData.hostileCount <= 0)
        {
            GameStarter.Instance.WinGame();
            isTimeCounting = false;
            currentData.isResume = false; 
            currentData.isFinish = true; 
        }
    }
    public void DecreaseEnemyCount()
    {
        currentData.enemyCount--;
        CheckLevelStatus();
    }
    public void DecreaseHostileCount()
    {
        currentData.enemyCount--;
        CheckLevelStatus();
    }
}
