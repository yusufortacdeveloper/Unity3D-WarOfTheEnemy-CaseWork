using UnityEngine;

[CreateAssetMenu(menuName = "LevelData", fileName = "LevelData")]
[System.Serializable]
public class LevelData : ScriptableObject
{
    public int levelCount;
    public int enemyCount ;
    public int hostileCount;
    public int levelTime;
    public bool isResume;
    public bool isFinish; 
}
