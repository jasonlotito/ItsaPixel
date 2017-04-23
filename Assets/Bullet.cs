using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 1f;
    Vector3 dir;
    public LayerMask layerMask;
    GameManager gameManager;

    public enum color
    {
        Red,Green,Blue
    }

    public static List<color> availableColors = new List<color>
    {
        color.Blue,
        color.Red,
        color.Green
    };

    public color bulletColor;

	// Use this for initialization
	void Start () {
        dir = GameObject.FindGameObjectWithTag(Player.Tag).transform.position - transform.position;
        gameManager = GameManager.GetGameManager();
        gameManager.AddSceneEnemy(gameObject);
        StartCoroutine(AutoDie());
    }

    private IEnumerator AutoDie()
    {
        yield return new WaitForSeconds(30f);
        Die();
    }

    // Update is called once per frame
    void Update () {
        float distanceThisFrame = speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distanceThisFrame, layerMask);

        Debug.DrawRay(transform.position, dir * distanceThisFrame * 3, Color.red);

        if (hit && hit.collider.gameObject.tag == Player.Tag)
        {
            HitTarget(hit.collider.gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
	}

    private void HitTarget(GameObject target)
    {
        target.GetComponent<IDamageable>().Hit(bulletColor);
        Die();
    }

    private void Die()
    {
        gameManager.RemoveSceneEnemy(gameObject);
        Destroy(gameObject);
    }
}
