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
    public static float comboScore;
    static string curJudgement;

    private const float PERFECT_MULT = 1.0f;
    private const float GREAT_MULT = 0.85f;
    private const float GOOD_MULT = 0.65f;
    private const float MISS_MULT = 0.0f;

    void Start()
    {
        Instance = this;
        comboScore = 0;
        curJudgement = "";
    }
    public static void Hit(double timing, int sound)
    {
        Instance.StopAllCoroutines();


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



        if (timing < 0.06 || EyeController.eyesClosed)
        {
            curJudgement = "Perfect";
            Instance.judgementText.color = Color.yellow;
            comboScore += SongManager.scorePerNote * PERFECT_MULT;
        }
        else if (timing < 0.1)
        {
            curJudgement = "Great";
            Instance.judgementText.color = Color.green;
            comboScore += SongManager.scorePerNote * GREAT_MULT;
        }
        else
        {
            curJudgement = "Good";
            Instance.judgementText.color = Color.blue;
            comboScore += SongManager.scorePerNote * GOOD_MULT;
        }

        Instance.judgementText.text = curJudgement;

        Instance.StartCoroutine(Instance.FadeOutText());
    }
    public static void Miss()
    {
        Instance.StopAllCoroutines();
        curJudgement = "Miss";
        Instance.missSFX.Play();

        Instance.judgementText.color = Color.red;
        Instance.judgementText.text = curJudgement;

        Instance.StartCoroutine(Instance.FadeOutText());
    }
    private void Update()
    {
        scoreText.text = Mathf.RoundToInt(comboScore).ToString();
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