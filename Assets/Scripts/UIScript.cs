using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIScript : MonoBehaviour
{
    [SerializeField] private SongManager songManager;
    [SerializeField] private TimeManager timeManager;

    private Button selectedButton;
    private bool isSelectionDefault = true;
    public int maxCompletedIndex = 4;

    public static AudioSource lobbyMusic;
    public AudioSource currLobbyMusic;

    private AudioSource timeAudio;

    public int currentSongIndex = 0;

    public List<AudioClip> audioClips;
    [SerializeField] private Button[] buttons;
    [SerializeField] private List<String> songMidiNames;
    [SerializeField] private List<float> songLobbyDelays;
    //[SerializeField] private List<int> songSpeeds;
    public void Play()
    {
        if (!songManager.isSongPlaying)
        {
            songManager.StartChart();
        }
    }

    public void ExitSong()
    {
        songManager.ExitChart();
    }

    public void UpdateButtons()
    {
        if (maxCompletedIndex == 5)
        {
            buttons[5].GetComponentInChildren<TextMeshProUGUI>().text = "CHALLENGE";
        }
        if (maxCompletedIndex == 4)
        {
            buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Level 5";
        }
        if (maxCompletedIndex >= 3)
        {
            buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Level 4";
        }
        if (maxCompletedIndex >= 2)
        {
            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Level 3";
        }
        if (maxCompletedIndex >= 1)
        {
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Level 2";
        }
        if (maxCompletedIndex >= 0)
        {
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Level 1";
        }
    }

    public void SwitchSongAndChart(Button pressedButton)
    {
        int index = Array.IndexOf(buttons, pressedButton);
        if (!songManager.isSongPlaying && (index <= maxCompletedIndex))
        {
            if (selectedButton != null)
            {
                DeselectButton(selectedButton);
            }
            currentSongIndex = index;
            SelectButton(pressedButton);
            selectedButton = pressedButton;

            isSelectionDefault = false;

            // TODO: Preview
            //currLobbyMusic.Stop();
            //currLobbyMusic = audioSources[index];
            //currLobbyMusic.time = songLobbyDelays[index];
            //StartCoroutine(FadeInSong(currLobbyMusic));
            //currLobbyMusic.Play();

            // songManager Song and Midi Assignment
            timeAudio = timeManager.GetComponent<AudioSource>();
            timeAudio.clip = audioClips[index];
            songManager.midiFileName = songMidiNames[index];
        }
    }

    void DeselectButton(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        button.colors = colors;
    }
    void SelectButton(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = colors.selectedColor;
        button.colors = colors;
    }
}
