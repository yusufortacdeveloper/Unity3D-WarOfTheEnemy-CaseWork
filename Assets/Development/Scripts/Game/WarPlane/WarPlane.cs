using UnityEngine;

public class WarPlane : MonoBehaviour
{
    float destroyCounter = 0,destroyTimer = 5f; 
    void Update()
    {
        destroyCounter += Time.deltaTime;

        if(destroyCounter >= destroyTimer)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.Translate(Vector3.forward * 0.1f);
        }
    }
}
