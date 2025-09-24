using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName tapNoteName;
    public Melanchall.DryWetMidi.MusicTheory.NoteName holdNoteName;
    public KeyCode input;
    public GameObject notePrefab;
    public GameObject noteHoldPrefab;
    List<Note> notes = new List<Note>();
    public List<double?> timeStamps = new List<double?>();

    int spawnIndex = 0;
    int inputIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == tapNoteName || note.NoteName == holdNoteName)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                
            }
            if (note.NoteName == holdNoteName)
            {
                timeStamps.Add(null);
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time + note.Length, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                print("timestamp " + timeStamps[timeStamps.Count - 3]);
                print("timestamp " + timeStamps[timeStamps.Count - 1]);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            double startTimeStamp = timeStamps[spawnIndex].HasValue ? timeStamps[spawnIndex].Value : timeStamps[spawnIndex - 1].Value;
            if (SongManager.GetAudioSourceTime() >= startTimeStamp - SongManager.Instance.noteTime)
            {
                if (timeStamps[spawnIndex].HasValue)
                {
                    var note = Instantiate(notePrefab, transform);
                    notes.Add(note.GetComponent<NoteTap>());
                    note.GetComponent<NoteTap>().assignedTime = (float)timeStamps[spawnIndex];
                }
                else
                {
                    var noteHold = Instantiate(noteHoldPrefab, transform);
                    notes.Add(noteHold.GetComponent<NoteHold>());
                    noteHold.GetComponent<NoteHold>().assignedTime = (float)timeStamps[spawnIndex - 1];
                    noteHold.GetComponent<NoteHold>().assignedEndTime = (float)timeStamps[spawnIndex + 1];
                }
                spawnIndex++;
            }
                
        } 

        if (inputIndex < timeStamps.Count)
        {
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (timeStamps[inputIndex].HasValue)
            {
                double timeStamp = timeStamps[inputIndex].Value;
                double marginOfError = SongManager.Instance.marginOfError;

                bool inputAction = false;
                if (inputIndex != 0 && !timeStamps[inputIndex].HasValue)
                {
                    // Releasing Key From Button Hold
                    inputAction = Input.GetKeyUp(input);
                }
                else
                {
                    // Tapping Key or Starting Button Hold
                    inputAction = Input.GetKeyDown(input);
                }
                    
                if (inputAction)
                {
                    HitCheck(timeStamp, audioTime);
                }
                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss(inputIndex);
                    print($"Missed {inputIndex} note");
                    inputIndex++;
                }
            }
            else
            {
                double nextTimeStamp = timeStamps[inputIndex + 1].Value;
                
                if (Input.GetKeyUp(input))
                {
                    if (HitCheck(nextTimeStamp, audioTime)) // hit note hold
                    {
                        Hit(inputIndex); // hit note tap
                        inputIndex++;
                    }
                }
                else if (!Input.GetKey(input))
                {
                    notes[inputIndex].Miss();
                    notes[inputIndex + 1].Miss();
                    inputIndex += 2;
                }
                else if (nextTimeStamp + SongManager.Instance.marginOfError <= audioTime)
                {
                    inputIndex += 2;
                }
            }
        }       
    }
    public bool HitCheck(double timeStamp, double audioTime)
    {
        bool hit = false;
        if (Math.Abs(audioTime - timeStamp) < SongManager.Instance.marginOfError)
        {
            hit = true;
            Hit(inputIndex);
            print($"Hit on {inputIndex} note");
            inputIndex++;
        }
        else
        {
            print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
        }
        return hit;
    }
    private void Hit(int index)
    {
        ScoreManager.Hit();
        notes[index].Hit();
    }
    private void Miss(int index)
    {
        MissNote(index);
        if (index - 1 >= 0 && !timeStamps[index - 1].HasValue) MissNote(index - 2);
        if (index + 1 < timeStamps.Count && !timeStamps[index + 1].HasValue) MissNote(index + 2);
        ScoreManager.Miss();
    }
    private void MissNote(int index)
    {
        if (notes[index] != null)
            notes[index].Miss();
    }
}
