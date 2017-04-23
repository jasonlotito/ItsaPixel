using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ZoomController))]
[RequireComponent(typeof(StoryController))]
public class GameManager : MonoBehaviour {
    public const string Tag = "GameController";

    bool startZoomingIn;
    // Use this for initialization
    private ZoomController zoomController;
    private StoryController storyController;
    private GameObject playerObject;
    private Player player;

    public GameObject sceneActivity;
    public GameObject ui;
    private List<GameObject> sceneDestructibles = new List<GameObject>();
    private ScoreManager scoreManager;

    public Image squareImage;
    public Image triangleImage;
    public Image circleImage;

    public List<Bullet.color> friendlyColors;
    public List<Bullet.color> fullColors;

    public enum Scene
    {
        Start,GameOver,IntroScene,SceneOne,SceneTwo,SceneThree,SceneFour,SceneFive,SceneSix,SceneSeven,SceneEight,SceneNine,SceneTen
    }

    public Scene sceneToRun;
    private Action sceneCompleteHandler;
    private bool triggerUpdateTransition;

    private void ShuffleColors()
    {
        ShuffleColors(Bullet.availableColors);
    }

    private void ShuffleColors(List<Bullet.color> availableColors)
    {
        StartCoroutine(ColorShuffle(availableColors));
    }

    private void EnableShuffleColorIcon(Bullet.color color)
    {
        squareImage.gameObject.SetActive(false);
        triangleImage.gameObject.SetActive(false);
        circleImage.gameObject.SetActive(false);

        switch (color)
        {
            case Bullet.color.Blue:
                circleImage.gameObject.SetActive(true);
                break;
            case Bullet.color.Green:
                triangleImage.gameObject.SetActive(true);
                break;
            case Bullet.color.Red:
                squareImage.gameObject.SetActive(true);
                break;
        }
    }

    private IEnumerator ColorShuffle(List<Bullet.color> availableColors)
    {
        friendlyColors.Clear();
        
        foreach(Bullet.color color in fullColors)
        {
            availableColors.Remove(color);
        }

        if ( availableColors.Count <= 0 )
        {
            storyController.TriggerCompletion(scoreManager.StopScoring(), delegate ()
            {
                sceneCompleteHandler();
            });
        } else {
            Bullet.color pickedColor = availableColors.RandomElement<Bullet.color>();

            EnableShuffleColorIcon(pickedColor);
            friendlyColors.Add(pickedColor);

            yield return new WaitForSeconds(15f);
            ShuffleColors(availableColors);
        }
    }

    internal void AddSceneEnemy(GameObject gameObject)
    {
        sceneDestructibles.Add(gameObject);
    }

    internal void RemoveSceneEnemy(GameObject gameObject)
    {
        sceneDestructibles.Remove(gameObject);
    }

    internal void ColorFilled(Bullet.color color)
    {
        friendlyColors.Remove(color);
        fullColors.Add(color);

        if (player.IsPlayerHealthy())
        {
            Debug.Log("TriggerNextScene()");
            DestroyEnemies();
            storyController.TriggerCompletion(scoreManager.StopScoring(), delegate () {
                sceneCompleteHandler();
            });

        }
    }

    public void EndLevelForStory()
    {
        sceneCompleteHandler();
    }

    void Start () {
        zoomController = GetComponent<ZoomController>();
        storyController = GetComponent<StoryController>();
        scoreManager = GetComponent<ScoreManager>();
        playerObject = GameObject.FindGameObjectWithTag(Player.Tag);
        player = playerObject.GetComponent<Player>();
        player.ScoreManager = scoreManager;

        storyController.gameManager = this;
        storyController.scoreManager = scoreManager;

        switch(sceneToRun)
        {
            case Scene.IntroScene:
                storyController.PrepareScenes(Scene.IntroScene);
                SceneStartIntro();
                sceneCompleteHandler = EnableLevelOne;
                break;
            case Scene.SceneOne:
                storyController.PrepareScenes(Scene.SceneOne);
                SceneStartOne();
                sceneCompleteHandler = EnableLevelTwo;
                break;
            case Scene.SceneTwo:
                storyController.PrepareScenes(Scene.SceneTwo);
                SceneStartTwo();
                sceneCompleteHandler = EnableLevelThree;
                break;
            case Scene.SceneThree:
                storyController.PrepareScenes(Scene.SceneThree);
                SceneStartThree();
                sceneCompleteHandler = EnableLevelFour;
                break;
            case Scene.SceneFour:
                storyController.PrepareScenes(Scene.SceneFour);
                SceneStartFour();
                sceneCompleteHandler = EnableLevelFive;
                break;
            case Scene.SceneFive:
                storyController.PrepareScenes(Scene.SceneFive);
                SceneStartFive();
                sceneCompleteHandler = EnableGameOver;
                break;
            case Scene.GameOver:
                storyController.PrepareScenes(Scene.GameOver);
                ScenesStartGameOver();
                sceneCompleteHandler = EnableStartMenu;
                break;
        }
	}

    private void ScenesStartGameOver()
    {
        player.SetHealth(1f, 1f, 1f);
        storyController.StartStory();
        zoomController.StartZoomingOut(delegate ()
        {
            EnableStartMenu();
        });
    }

    private void EnableGameOver()
    {
        DestroyEnemies();
        SceneManager.LoadScene("GameOver");
    }

    private void EnableLevelFive()
    {
        DestroyEnemies();
        SceneManager.LoadScene("SceneFive");
    }

    private void SceneStartFive()
    {
        player.SetHealth(0f, 0f, 0f);
        ShuffleColors();
        storyController.StartStory();
    }

    private void EnableLevelFour()
    {
        DestroyEnemies();
        SceneManager.LoadScene("SceneFour");
    }

    private void SceneStartFour()
    {
        player.SetHealth(.5f, .5f, .5f);
        ShuffleColors();
        storyController.StartStory();
    }

    private void SceneStartThree()
    {
        player.SetHealth(1f, 0f, 1f);
        storyController.StartStory();
    }

    private void SceneStartTwo()
    {
        player.SetHealth(1f, 0f, 0f);
        storyController.StartStory();
    }
    
    private void SceneStartOne()
    {
        storyController.StartStory();
    }
    

    public void SceneStartIntro(float waitTime = 4.0f)
    {
        StartCoroutine(WaitToStart(waitTime));
    }

    IEnumerator WaitToStart(float waitTime, bool zoomIn = true)
    {
        yield return new WaitForSeconds(waitTime);
        zoomController.StartZoomingIn(delegate ()
        {
            storyController.StartStory();
        });
    }
        
    // Update is called once per frame
    void Update () {
        zoomController.Zoom();
    }

    internal void EnableStartMenu()
    {
        DestroyEnemies();
        SceneManager.LoadScene("StartMenu");
    }

    internal void EnableStart()
    {
        DestroyEnemies();
        SceneManager.LoadScene("Playground");
    }

    internal void EnableLevelOne()
    {
        DestroyEnemies();
        SceneManager.LoadScene("SceneOne");
    }

    internal void EnableLevelTwo()
    {
        DestroyEnemies();
        SceneManager.LoadScene("SceneTwo");
    }

    internal void EnableLevelThree()
    {
        DestroyEnemies();
        SceneManager.LoadScene("SceneThree");
    }

    private void DestroyEnemies()
    {
        foreach (GameObject gameObject in sceneDestructibles)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager GetGameManager()
    {
        return GameObject.FindGameObjectWithTag(GameManager.Tag).GetComponent<GameManager>();
    }
}
