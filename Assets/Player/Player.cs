using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour, IDamageable {

    public float Speed = 2.0f;
    public const string Tag = "Player";

    Controller2D controller;
    private float inputX;
    private Vector3 velocity;

    public GameObject redHealth;
    public GameObject blueHealth;
    public GameObject greenHealth;
    public float colorHitIncrement = 0.05f;
    public float colorHitDecrement = 0.05f;

    private Dictionary<Bullet.color, GameObject> colorBars = new Dictionary<Bullet.color, GameObject>();
    private GameManager gameManager;
    private ScoreManager scoreManager;
    private AudioSource pickupColor;
    private AudioSource hitColor;

    public ScoreManager ScoreManager
    {
        set
        {
            scoreManager = value;
        }
    }

    public bool IsPlayerHealthy()
    {
        return colorBars[Bullet.color.Blue].transform.localScale.y == 1f &&
               colorBars[Bullet.color.Red].transform.localScale.y == 1f &&
               colorBars[Bullet.color.Green].transform.localScale.y == 1f;
    }

    public void SetHealth(float r, float g, float b)
    {
        colorBars[Bullet.color.Blue].transform.localScale += new Vector3(0, b, 0);
        colorBars[Bullet.color.Red].transform.localScale += new Vector3(0, r, 0);
        colorBars[Bullet.color.Green].transform.localScale += new Vector3(0, g, 0);
    }

    public void Hit(Bullet.color color)
    {
        Vector3 barLocalScale = colorBars[color].transform.localScale;
        
        if (gameManager.friendlyColors.Contains(color))
        {
            pickupColor.Play();
            Vector3 scale = colorBars[color].transform.localScale;
            barLocalScale.y = Mathf.Clamp(barLocalScale.y + colorHitIncrement, 0, 1);
            colorBars[color].transform.localScale = barLocalScale;

            if (barLocalScale.y == 1f)
            {
                gameManager.ColorFilled(color);
            }
        }
        else
        {
            hitColor.Play();
            foreach(Bullet.color col in colorBars.Keys)
            {
                if (colorBars[col].transform.localScale.y < 1f)
                {
                    scoreManager.GotHit();
                    Vector3 barScale = colorBars[col].transform.localScale;
                    barScale.y = Mathf.Clamp(barScale.y - colorHitDecrement, 0, 1);
                    colorBars[col].transform.localScale = barScale;
                }
            }
        }
    }

    private void Awake()
    {
        colorBars.Clear();
        colorBars.Add(Bullet.color.Blue, blueHealth);
        colorBars.Add(Bullet.color.Red, redHealth);
        colorBars.Add(Bullet.color.Green, greenHealth);
    }

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gameManager = GameManager.GetGameManager();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        
        foreach(AudioSource audio in audioSources)
        {
            switch (audio.clip.name)
            {
                case "pickup_color":
                    pickupColor = audio;
                    break;
                case "hit_color":
                    hitColor = audio;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        float timeSpeedMultiplier = Time.deltaTime * Speed;
        velocity.x = Input.GetAxis("Horizontal") * timeSpeedMultiplier;
        velocity.y = Input.GetAxis("Vertical") * timeSpeedMultiplier;
        controller.Move(velocity);
    }

    public static GameObject GetPlayerGameObject()
    {
        return GameObject.FindGameObjectWithTag(Player.Tag);
    }
}
