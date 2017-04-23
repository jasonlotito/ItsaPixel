using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ZoomController))]
public class StoryController : MonoBehaviour {

    public delegate void AfterText();

    public GameManager gameManager;
    ZoomController zoomController;
    public Text narratorText;
    public Text playerText;
    public Text npcText;
    static public float DefaultTextTiming = 4f;

    const KeyCode NULLKEY = KeyCode.ScrollLock;

    KeyCode keyToCheck = NULLKEY;

    struct TextDisplay
    {
        public string text;
        public float timing;
        public TextBoxToUse speaker;

        public TextDisplay(string _text, float _timing = -1, TextBoxToUse _speaker = TextBoxToUse.NARRATOR)
        {
            text = _text;
            timing = _timing < 0 ? StoryController.DefaultTextTiming : _timing;
            speaker = _speaker;
        }
    }

    struct Scene
    {
        public List<TextDisplay> text;
        public AfterText callback;
        public Scene(List<TextDisplay> _text)
        {
            text = _text;
            callback = delegate() { };
        }

        public Scene(List<TextDisplay> _text, AfterText _callback)
        {
            text = _text;
            callback = _callback;
        }
    }

    enum TextBoxToUse {
        NARRATOR = 1,
        NPC = 2,
        PLAYER = 3
    }

    private List<Scene> sceneList = new List<Scene>();
    private List<Scene>.Enumerator sceneEnumerator;
    private Scene completion;
    internal ScoreManager scoreManager;

    void RunScene(List<TextDisplay> scene, AfterText callback = null)
    {
        float timing = 0f;
        int i = 0, count = scene.Count;

        foreach (TextDisplay line in scene)
        {
            i++;
            timing += line.timing;

            if (i == count)
            {
                StartCoroutine(PrintText(line.text, timing, line.speaker, callback));
            } else {
                StartCoroutine(PrintText(line.text, timing, line.speaker));
            }
        }
    }

    private void EnableMoveCheck(KeyCode keyCode)
    {
        keyToCheck = keyCode;
    }

    IEnumerator PrintText(string text, float after, TextBoxToUse speaker = TextBoxToUse.NARRATOR, AfterText callback = null)
    {
        yield return new WaitForSeconds(after);
        switch(speaker)
        {
            case TextBoxToUse.PLAYER:
                playerText.text = text;
                break;
            case TextBoxToUse.NPC:
                npcText.text = text;
                break;
            case TextBoxToUse.NARRATOR:
            default:
                narratorText.text = text;
                break;
        }
        
        if (callback != null)
        {
            callback();
        }
    }

    // Use this for initialization
    void Start () {
        zoomController = GetComponent<ZoomController>();
    }

    private TextDisplay _(string _text, float _timing = -1, TextBoxToUse _speaker = TextBoxToUse.NARRATOR)
    {
        return new TextDisplay(_text, _timing, _speaker);
    }

    // Update is called once per frame
    void Update()
    {
        if (keyToCheck != NULLKEY && Input.GetKey(keyToCheck))
        {
            keyToCheck = NULLKEY;
            TriggerNextScene();
        }
    }

    public void PrepareScenes(GameManager.Scene scene)
    {
        sceneList.Clear();

        switch (scene)
        {
            case GameManager.Scene.IntroScene:
                LoadOpeningScene();
                break;
            case GameManager.Scene.SceneOne:
                LoadSceneOne();
                break;
            case GameManager.Scene.SceneTwo:
                LoadSceneTwo();
                break;
            case GameManager.Scene.SceneThree:
                LoadSceneThree();
                break;
            case GameManager.Scene.SceneFour:
                LoadSceneFour();
                break;
            case GameManager.Scene.SceneFive:
                LoadSceneFive();
                break;
            case GameManager.Scene.GameOver:
                LoadSceneGameOver();
                break;
        }
        
        sceneEnumerator = sceneList.GetEnumerator();
    }

    private void LoadSceneGameOver()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("You've done it...", 0f),
                _("The pixel, it's going home!"),
                _("It will return to its place among the vast screen of pixels."),
                _("Filled with reds, greens, and blues!"),
                _("Thanks for playing my first game ever."),
                _("From my first Ludum Dare, LD38."),
                _("It's not much, but it's mine"),
                _("")
            }
        ));
    }

    private void LoadSceneFive()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("Here you are, finally ready to fill the pixel up with everything it needs."),
                _("Reds, greens, and blue. All in moderation."),
                _("All the pixels are helping now!"),
                _("")
            }
        ));

        completion = new Scene(new List<TextDisplay>
        {
            _("You've done it!"),
            _("", 4f)
        });
    }

    private void LoadSceneFour()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("We need to mix things up some."),
                _("A pixel is more than just one color."),
                _("And a pixel is not made up of 3 colors."),
                _("A pixel can be many colors, and when it works together with other pixels..."),
                _("the colors they shine are vivid and bright!"),
                _("")
            }
        ));

        completion = new Scene(new List<TextDisplay>
        {
            _("Pixels that work together create beauty."),
            _("A single bad pixel ruins the screen."),
            _("")
        });
    }

    private void LoadSceneThree()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("Oh, that green from before wasn't good enough."),
                _("")
            }
        ));

        completion = new Scene(new List<TextDisplay>
        {
            _("Okay, I think you got the hang of this.", 0f),
            _("")
        });
    }

    private void LoadSceneTwo()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("The pixel needs more blue and green.", 0f),
                _("")
            }
        ));

        completion = new Scene(new List<TextDisplay>
            {
                _("Okay, good. Let's see what else the pixel needs.", 0f),
                _("")
            }
        );
    }

    private void LoadSceneOne()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("You are free little pixel, move around and collect those colors!", 1f),
                _("The other pixels are sharing what color they can!"),
            }
        ));

        completion = new Scene(new List<TextDisplay>
            {
                _("Good, good, but we need more color...",0f),
                _("")
            }
        );
    }

    private void LoadOpeningScene()
    {
        sceneList.Add(new Scene
        {
            text = new List<TextDisplay> {
                _("A dead pixel", 0),
                _("Look, it's missing its color.", 2f),
                _("No red."),
                _("No green.", 2f),
                _("No blue.", 2f),
                _("Almost, it's almost dead. But there is still hope."),
                _("The other pixels on the screen are going to share their colors!"),
                _("")
            },
            callback = delegate ()
            {
                gameManager.EndLevelForStory();
            }
        });
    }

    public void TriggerCompletion(float finalScore, AfterText cb)
    {
        RunScene(completion.text, cb);
    }

    private void TriggerNextScene()
    {
        sceneEnumerator.MoveNext();
        Scene scene = sceneEnumerator.Current;
        RunScene(scene.text, scene.callback);
    }

    internal void StartStory()
    {
        TriggerNextScene();
    }
}
