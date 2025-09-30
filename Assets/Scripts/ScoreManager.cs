using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
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
        comboScore += 1;
        Instance.hitSFX.Play();
        if (timing < 0.01)
        {
            curJudgement = "Perfect";
            Instance.judgementText.color = Color.yellow;
        }
        else if (timing < 0.03)
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
    
    IEnumerator FadeOutText()
    {
        Color originalColor = judgementText.color;
        originalColor.a = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            judgementText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        
        judgementText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}