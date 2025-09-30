using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public AudioSource yawnSFX;
    public TMPro.TextMeshPro scoreText;
    public TMPro.TextMeshPro judgementText;
    public float fadeDuration = 1f;
    static int comboScore;
    static string curJudgement;
    void Start()
    {
        Instance = this;
        comboScore = 0;
        curJudgement = "";
    }
    public static void Hit(double timing)
    {
        Instance.StopAllCoroutines();
        comboScore += 1;
        Instance.hitSFX.Play();
        if (timing < 0.02)
        {
            curJudgement = "Perfect";
            Instance.judgementText.color = Color.yellow;
        }
        else if (timing < 0.04)
        {
            curJudgement = "Great";
            Instance.judgementText.color = Color.green;
        }
        else
        {
            curJudgement = "Good";
            Instance.judgementText.color = Color.blue;
        }

        Instance.judgementText.text = curJudgement;

        Instance.StartCoroutine(Instance.FadeOutText());
    }
    public static void Miss()
    {
        Instance.StopAllCoroutines();
        comboScore = 0;
        curJudgement = "Miss";
        Instance.missSFX.Play();

        Instance.judgementText.color = Color.red;
        Instance.judgementText.text = curJudgement;

        Instance.StartCoroutine(Instance.FadeOutText());
    }
    private void Update()
    {
        scoreText.text = comboScore.ToString();
    }
    public static void Yawn()
    {
        Instance.yawnSFX.Play();
    }

    IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(0.5f); 
        judgementText.text = ""; 
    }
}