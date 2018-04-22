using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour {

    public float spawnRate = 4f;
    public int columnPoolSize = 5;
    public float columnYMin =  -2f;//-2
    public float columnYMax = 2f;//2

    private float timeSinceLastSpawn = 5;
    private float spawnXPos = 3f;
    private int currentColumn = 0;

    public GameObject columnsPrefab;
    private GameObject[] columns;
    private Vector2 objectPoolPosition = new Vector2(-15, -25f);//-15 -25

    public GameObject castlePrefab;
    private GameObject castle;
    private int counterLimit = 2;

    private int counter = 0;



	// Use this for initialization
	void Start () {
        columns = new GameObject[columnPoolSize];
        for (int i = 0; i < columnPoolSize; i++)
        {
            columns[i] = (GameObject)Instantiate(columnsPrefab, objectPoolPosition, Quaternion.identity);
            
        }

		
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= spawnRate && counter < counterLimit)
        {
            timeSinceLastSpawn = 0;

            float spawnYPos = Random.Range(columnYMin, columnYMax);
            columns[currentColumn].transform.position = new Vector2(spawnXPos, spawnYPos);
            currentColumn++;
            
            if(currentColumn >= columnPoolSize)
            {
                currentColumn = 0;
                counter++;
            }
        }

        if (counter == counterLimit)
        {
            castle = (GameObject)Instantiate(castlePrefab, objectPoolPosition, Quaternion.identity);
            castle.transform.position = new Vector2(15f, 0);
            counter++;
        }
		
	}
}
