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
    public AudioSource headphoneSFX;
    public AudioSource hugSFX;
    public AudioSource coverSFX;
    public AudioSource snoreSFX;
    public AudioSource kickSFX;
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
    public static void Hit(double timing, int sound)
    {
        Instance.StopAllCoroutines();
        comboScore += 1;

        switch (sound)
        {
            case 1:
                Instance.headphoneSFX.Play();
                break;
            case 2:
                Instance.hugSFX.Play();
                break;
            case 3:
                Instance.coverSFX.Play();
                break;
            case 4:
                Instance.snoreSFX.Play();
                break;
            case 5:
                Instance.kickSFX.Play();
                break;
            case 0:
                Instance.hitSFX.Play();
                break;
        }
        

        if (timing < 0.03 || EyeController.eyesClosed)
        {
            curJudgement = "Perfect";
            Instance.judgementText.color = Color.yellow;
        }
        else if (timing < 0.05)
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

    public void ResetCombo()
    {
        comboScore = 0;
    }

    IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(0.5f); 
        judgementText.text = ""; 
    }
}