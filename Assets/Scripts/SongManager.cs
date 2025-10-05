using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Linq;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public UIScript UI;
    public ScoreManager scoreManager;
    public AudioSource audioSource;
    public TimeManager timeManager;
    public ResultsUIScript resultsUI;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;

    public int notesInSong;
    public static float scorePerNote;
    
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }
    public bool useNewAudioTime;
    public static MidiFile midiFile;
    public bool isSongPlaying = false;
    public string midiFileName;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void StartChart()
    {
        if (!isSongPlaying)
        {
            isSongPlaying = true;
            ReadFromFile(midiFileName);
        }
    }

    public void ExitChart(bool endedEarly = true)
    {
        isSongPlaying = false;

        if (endedEarly)
        {
            StopCoroutine(CheckIfAudioFinished());
            audioSource.Stop();
        }

        resultsUI.SetVals();
        resultsUI.ShowResults();


        foreach (var lane in lanes)
        {
            lane.ClearNotes();
        }

        scoreManager.ResetCombo();
        audioSource.time = 0;
    }

    private void ReadFromFile(string fileName)
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileName);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();

        char[] lettersToCheck = { 'G', 'D' };
        notesInSong = notes.Count;
        
        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
        {
            if (!note.NoteName.ToString().Any(c => lettersToCheck.Contains(c)))
            {
                notesInSong += 1;
                if (note.NoteName.ToString().Contains("C"))
                {
                    notesInSong -= 3;
                }
            }
        }

        scorePerNote = 1_000_000f / notesInSong;

        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        if (Instance.useNewAudioTime)
        {
            timeManager.Play();
            StartCoroutine(CheckIfAudioFinished());
        }
        else
        {
            audioSource.Play(); 
            StartCoroutine(CheckIfAudioFinished());
        }     
    }
    public static double GetAudioSourceTime()
    {
        if (Instance.useNewAudioTime)
        {
            return Instance.timeManager.GetCurrentAudioTime();
        }
        else
        {
            return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
        }
    }

    /// <summary>
    /// Continuously checks if the song is finished, then calls ExitChart(false);
    /// </summary>
    IEnumerator CheckIfAudioFinished()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }


        if (UI.currentSongIndex >= UI.maxCompletedIndex)
        {
            UI.maxCompletedIndex = UI.currentSongIndex;
        }
        UI.UpdateButtons();
        ExitChart(false);
    }
}
