using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed;

    private void Update()
    {
        transform.position -= new Vector3(1, 0, 0) * speed * Time.deltaTime;
        if (transform.position.x < -25)
        {
            transform.parent.GetComponent<ObstacleGenerator>().obstacles.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
