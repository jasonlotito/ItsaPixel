using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorSpawner : MonoBehaviour {

    public Transform redBulletPrefab;
    public Transform blueBulletPrefab;
    public Transform greenBulletPrefab;
    public List<Transform> bulletPrefabs = new List<Transform>();
    public float timeBetweenBullets = 1f;
    private GameManager gameManager;

    private void Start()
    {
        bulletPrefabs.Add(redBulletPrefab);
        bulletPrefabs.Add(blueBulletPrefab);
        bulletPrefabs.Add(greenBulletPrefab);
        StartCoroutine(ShootBullets());
        gameManager = GameManager.GetGameManager();
        gameManager.AddSceneEnemy(gameObject);
    }

    private IEnumerator ShootBullets()
    {
        while (true)
        {
            SpawnBullet();
            yield return new WaitForSeconds(timeBetweenBullets);
        }
    }

    private void SpawnBullet()
    {
        
        Transform bulletPrefab = bulletPrefabs.RandomElement<Transform>();
        Instantiate(bulletPrefab, transform.position,transform.rotation);
    }
}

public static class ColectionExtension
{
    private static System.Random rng = new System.Random();

    public static T RandomElement<T>(this IList<T> list)
    {
        return list[rng.Next(list.Count)];
    }

    public static T RandomElement<T>(this T[] array)
    {
        return array[rng.Next(array.Length)];
    }
}