using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsUIScript : MonoBehaviour
{
    public GameObject uiPanel;
    public CanvasGroup canvasGroup;
    public SongManager songManager;


    public TextMeshProUGUI rankText;
    public TextMeshProUGUI scoreText;

    private float fadeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        uiPanel.SetActive(false);
    }

    public void ShowResults()
    {
        uiPanel.SetActive(true);
        StartCoroutine(Fade(true));
        //leftUIPanel.SetActive(false);
    }

    public void SetVals()
    {
        float score = ScoreManager.comboScore;

        scoreText.text = "Score: " + Mathf.RoundToInt(score).ToString();

        rankText.fontSize = 36;
        if (score == 1000000)
        {
            rankText.text = "Rank: PERFECT";
        }
        else if (score >= 980000) rankText.text = "Rank: S+";
        else if (score >= 950000) rankText.text = "Rank: S";
        else if (score >= 930000) rankText.text = "Rank: A+";
        else if (score >= 900000) rankText.text = "Rank: A";
        else if (score >= 860000) rankText.text = "Rank: B+";
        else if (score >= 800000) rankText.text = "Rank: B";
        else if (score >= 700000) rankText.text = "Rank: C";
        else rankText.text = "Rank: FAIL";
    }

    private IEnumerator Fade(bool fadeIn)
    {
        float startTime = Time.time;
        if (fadeIn)
        {
            while (Time.time < startTime + fadeDuration)
            {
                float t = (Time.time - startTime) / fadeDuration;
                canvasGroup.alpha = t;
                yield return null;
            }

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            while (Time.time < startTime + fadeDuration)
            {
                float t = (Time.time - startTime) / fadeDuration;
                canvasGroup.alpha = 1 - t;
                yield return null;
            }

            canvasGroup.alpha = 0;
            uiPanel.SetActive(false);
        }
    }

    public void Close()
    {
        StartCoroutine(Fade(false));
    }
}
