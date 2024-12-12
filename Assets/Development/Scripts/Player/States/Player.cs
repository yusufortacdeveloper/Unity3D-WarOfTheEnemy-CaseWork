using UnityEngine;

public class Player : MonoBehaviour, IEventListener
{
    private void OnEnable()
    {
        EventManager.Instance?.RegisterListener(this);
    }

    private void OnDisable()
    {
        EventManager.Instance?.UnregisterListener(this);
    }
}
