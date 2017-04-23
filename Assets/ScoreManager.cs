using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text score;

    private float startingScore = 0f;
    private float hitScorePenalty = 5f;
    private float timeScorePenalty = 3f;
    private float currentScore;
    private bool weAreScoring = true;

    // Use this for initialization
    void Start () {
        currentScore = startingScore;
        InvokeRepeating("UpdateScore", 1f, 1f);
    }

    void UpdateScore()
    {
        if ( weAreScoring )
        {
            currentScore += 1;
        }
    }

    public float StopScoring()
    {
        weAreScoring = false;

        return currentScore;
    }

    public void GotHit()
    {
        //currentScore -= hitScorePenalty;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if(weAreScoring)
        {
            score.text = currentScore.ToString();
        }
	}
}
