//Nicholas Robinson
//Keeping score for player

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static int score;		//Score that player currently has

    Text text;		//Text on screen (Top left)

    void Start()
    {

        text = GetComponent<Text>();	//Find text component

        score = 0;		//On startup, score equals 0
    }

    void Update()
    {

        if (score < 0)		//If you somehow managed to get less than 0 points (You're REALLY bad),
            score = 0;		//Keep score to 0 (Friendly game, can't have less than 0 points)

        text.text = "" + score;		//Update text on screen to show current points
    }

    public static void AddPoints(int pointsToAdd)
    {

        score += pointsToAdd;		//Function that allows other scripts to use, adding points to score
    }
}
