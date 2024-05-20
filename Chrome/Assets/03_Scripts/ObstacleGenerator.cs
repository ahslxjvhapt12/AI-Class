using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> obstacles;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 2f, 2f);
        obstacles = new List<GameObject>();
    }

    public void SpawnObstacle()
    {

        int idx = Random.Range(0, prefabs.Count);
        GameObject obj = Instantiate(prefabs[idx], transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        obstacles.Add(obj);

    }

}


