using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public List<GameObject> obstacles = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 2f, 2f);
    }

    public void SpawnObstacle()
    {
        int idx = Random.Range(0, obstacles.Count - 1);
        var obj = Instantiate(obstacles[idx], transform.position, Quaternion.identity);
        // 호박
        if (idx == 0)
        {
            obj.transform.position = transform.position + new Vector3(10, -3f, 0);
        }
        // 유령
        else
        {
            obj.transform.position = transform.position + new Vector3(10, -1.5f, 0);
        }
    }


}
