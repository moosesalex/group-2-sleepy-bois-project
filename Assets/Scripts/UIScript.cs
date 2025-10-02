using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class UIScript : MonoBehaviour
{
    [SerializeField] private SongManager songManager;
    [SerializeField] private TimeManager timeManager;

    private Button selectedButton;
    private bool isSelectionDefault = true;

    public static AudioSource lobbyMusic;
    public AudioSource currLobbyMusic;

    private AudioSource timeAudio;

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

    public void SwitchSongAndChart(Button pressedButton)
    {
        if (!songManager.isSongPlaying)
        {
            if (selectedButton != null)
            {
                DeselectButton(selectedButton);
            }

            SelectButton(pressedButton);
            selectedButton = pressedButton;

            int index = Array.IndexOf(buttons, pressedButton);

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
