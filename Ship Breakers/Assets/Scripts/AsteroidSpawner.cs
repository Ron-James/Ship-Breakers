using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : Singleton<AsteroidSpawner>
{
    [SerializeField] Transform inactives;
    [SerializeField] float rate = 3f;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float maxZ;
    bool spawning = true;
    Asteroid[] asteroids;
    float time = 0;
    float difficultyTime = 0;
    int diffcultyLevel;

    [SerializeField] float[] accLevels;
    [SerializeField] float[] rateLevels;
    [SerializeField] int[] spawnNums;

    float levelTime;
    float currentAcc;
    int spawnNum;

    // Start is called before the first frame update
    void Start()
    {

        levelTime = GameManager.instance.MaxTime / accLevels.Length;
        diffcultyLevel = 0;
        spawning = true;
        time = 0;
        difficultyTime = 0;
        rate = rateLevels[0];
        spawnNum = spawnNums[0];
        currentAcc = accLevels[diffcultyLevel];
        RefreshArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning)
        {
            time += Time.deltaTime;
            difficultyTime += Time.deltaTime;
            if (time >= rate)
            {
                Spawn();
                time = 0;
            }
            if (difficultyTime >= levelTime)
            {
                diffcultyLevel++;
                currentAcc = accLevels[diffcultyLevel];
                rate = rateLevels[diffcultyLevel];
                spawnNum = spawnNums[diffcultyLevel];
                difficultyTime = 0;
            }
        }
        else
        {
            time = 0;
        }

    }

    public void RefreshArray()
    {
        asteroids = inactives.GetComponentsInChildren<Asteroid>();
    }

    public void Spawn()
    {
        for (int loop = 0; loop < spawnNum; loop++)
        {
            Vector3 pos = spawnPoint.position;
            pos.z = Random.Range(-maxZ, maxZ);
            asteroids[0].Activate(pos, currentAcc);
            RefreshArray();
        }

    }
}
