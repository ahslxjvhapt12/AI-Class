using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class Container
{
    public float speed;
    public GameObject image;
}
public class BackgroundController : MonoBehaviour
{
    private const float Limit = 20.0f;

    [SerializeField]
    List<Container> images = new();

    private void Update()
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].image.transform.position += images[i].speed * Vector3.left * Time.deltaTime;
            if (images[i].image.transform.position.x <= -Limit)
            {
                images[i].image.transform.position = new Vector3(Limit, 0, 0);
            }
        }
    }
}
