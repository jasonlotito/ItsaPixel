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
                _("The pixel..., it's going back home again!"),
                _("It will return from whence it came!"),
                _("Filled with RGB goodness"),
                _("To shine a light onto you in the darkest of nights."),
                _("Pixel, pixel, shine so bright"),
                _("Pixel there now out of sight."),
                _("Goodnight sweet pixel"),
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
                _("Finally, you've made it."),
                _("The final level.  The final point at which you can achieve what you've set out to do."),
                _("Make this bad pixel good again!"),
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
                _("Okay, that was all easy.  Now let's randomize things a bit."),
                _("You can only get one color at a time!"),
                _("")
            }
        ));

        completion = new Scene(new List<TextDisplay>
        {
            _("Oh, you finished that? Well, I think you need a greater challenge."),
            _("", 4f)
        });
    }

    private void LoadSceneThree()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("Watch out for the red and blue ones now.", 1f),
                _("I bet you already knew that."),
                _("I bet you think you are smart."),
                _("A real John Carmack."),
                _("")
            }
        ));

        completion = new Scene(new List<TextDisplay>
        {
            _("I'd congratulate you, but frankly, that had the worst completion time.  Check the leader boards!", 0f),
            _("Hahaha, you thought there were leader boards!  It's a Ludam Dare Compo entry!  You should be glad this game hasn't crashed your computer!"),
            _("", 8f)
        });
    }

    private void LoadSceneTwo()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("Watch out for the red ones!", 1f),
                _("")
            }
        ));

        completion = new Scene(new List<TextDisplay>
            {
                _("Congratulations!", 0f),
                _("")
            }
        );
    }

    private void LoadSceneOne()
    {
        sceneList.Add(new Scene(new List<TextDisplay>
            {
                _("Don't be afraid of the colors, grab them all!", 1f),
                _(""),
                _("Though, once it fills up with one color, more of that color will only hurt it.", 10f)
            }
        ));

        completion = new Scene(new List<TextDisplay>
            {
                _("Congratulations!",0f),
                _("")
            }
        );
    }

    private void LoadOpeningScene()
    {
        sceneList.Add(new Scene
        {
            text = new List<TextDisplay> {
                _("Look!", 0),
                _("It's a pixel!"),
                _("A dead pixel!"),
                _("How sad. =("),
                _("Well, almost dead.  It's got a bit of life left in it."),
                _("It just needs some help getting its color back."),
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
