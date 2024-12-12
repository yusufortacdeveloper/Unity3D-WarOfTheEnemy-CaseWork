using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePoz : MonoBehaviour
{
    [SerializeField]private List<EnemyCharacter> affectedEnemies = new List<EnemyCharacter>();

    private void OnTriggerEnter(Collider other)
    {
        EnemyCharacter enemy = other.GetComponent<EnemyCharacter>();
        if (enemy != null && !affectedEnemies.Contains(enemy))
        {
            affectedEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyCharacter enemy = other.GetComponent<EnemyCharacter>();
        if (enemy != null && affectedEnemies.Contains(enemy))
        {
            affectedEnemies.Remove(enemy);
        }
    }

    public List<EnemyCharacter> GetAffectedEnemies()
    {
        return affectedEnemies;
    }
}
