using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject goodItem;
    public GameObject badItem;

    public int gooditemCount = 30;
    public int baditemCount = 10;

    private List<GameObject> goodItemList = new List<GameObject>();
    private List<GameObject> badItemList = new List<GameObject>();

    public void SpawnItems()
    {
        foreach (GameObject obj in goodItemList)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in badItemList)
        {
            Destroy(obj);
        }

        goodItemList.Clear();
        badItemList.Clear();

        for (int i = 0; i < gooditemCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-23f, 23f), 0.05f, Random.Range(-23f, 23f));
            Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
            goodItemList.Add(Instantiate(goodItem, transform.position + position, rotation, transform));
        }

        for (int i = 0; i < baditemCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-23f, 23f), 0.05f, Random.Range(-23f, 23f));
            Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
            badItemList.Add(Instantiate(badItem, transform.position + position, rotation, transform));
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnItems();
        }
    }
}
