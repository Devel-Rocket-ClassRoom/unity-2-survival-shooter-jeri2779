using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Enemy[] Prefab;

    public EnemyData[] enemyDatas;
    public Transform[] spawnPoints;

    private List<Enemy> enimies = new List<Enemy>();
    private int wave;

    //public UIManager manager;



    // Update is called once per frame
    void Update()
    {
        //    if (GameManager.instance != null && GameManager.instance.isGameOver)
        //    {
        //        return;
        //    }
        if (enimies.Count == 0)
        {
            SpawnWave();
        }
    }
    private void SpawnWave()
    {
        wave++;
        int spawnCount = Mathf.RoundToInt(wave * 5f);
        for (int i = 0; i < spawnCount; i++)
        {
            CreateEnemy();
        }
        //manager.SetWaveInfo(wave, zombies.Count);
    }
    private void CreateEnemy()
    {
        if (Prefab == null)
        {
            Debug.LogError(" Prefab이 null입니다.");
        }
        var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var enemyPrefab = Prefab[Random.Range(0, Prefab.Length)];
        var enemy = Instantiate(enemyPrefab, point.position, point.rotation);
        enemy.SetUp(enemyDatas[Random.Range(0, enemyDatas.Length)]);
        enimies.Add(enemy);
        //enemy.gameObject.SetActive(true);

        enemy.OnDead.AddListener(() => enimies.Remove(enemy));
        enemy.OnDead.AddListener(() =>
        {
            if (GameManager.instance != null)
                GameManager.instance.AddScore(enemy.score);
        });
        enemy.OnDead.AddListener(() => Destroy(enemy.gameObject, 10f));



        //enemyData zombieData = enemyDatas[Random.Range(0, enemyDatas.Length)];

        //Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        //zombie.SetUp(zombieData);
        //zombies.Add(zombie);

        //zombie.OnDead += () => zombies.Remove(zombie);
        //zombie.OnDead += () => Destroy(zombie.gameObject, 10f);
        //zombie.OnDead += () => GameManager.instance.AddScore(zombieData.score);
    }
}

