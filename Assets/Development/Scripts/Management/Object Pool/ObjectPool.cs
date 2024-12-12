using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance => instance;
    
    private readonly Dictionary<string, object> pools = new Dictionary<string, object>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public ObjectPool<T> GetOrCreatePool<T>(string key, T prefab, Transform parent = null, int initialSize = 10) where T : Component
    {
        if (!pools.TryGetValue(key, out var pool))
        {
            var newPool = new ObjectPool<T>(prefab, parent, initialSize);
            pools[key] = newPool;
            return newPool;
        }

        return pool as ObjectPool<T>;
    }
}
public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> pool = new Queue<T>();

    public ObjectPool(T prefab, Transform parent = null, int initialSize = 10)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            AddToPool();
        }
    }
    private void AddToPool()
    {
        T obj = Object.Instantiate(prefab, ObjectPoolManager.Instance.transform.GetChild(0));
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
    public T Get()
    {
        if (pool.Count == 0)
        {
            AddToPool();
        }

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}