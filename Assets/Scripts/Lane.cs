using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName headphoneNoteName;
    public Melanchall.DryWetMidi.MusicTheory.NoteName hugNoteName;
    public Melanchall.DryWetMidi.MusicTheory.NoteName coverNoteName;
    public Melanchall.DryWetMidi.MusicTheory.NoteName snoreNoteName;
    public Melanchall.DryWetMidi.MusicTheory.NoteName kickNoteName;
    public Melanchall.DryWetMidi.MusicTheory.NoteName eyesNoteName;
    private int tapNoteInt = 0;
    private int holdStartNoteInt = 1;
    private int holdMiddleNoteInt = 2;
    private int holdEndNoteInt = 3;
    public KeyCode input;
    public GameObject notePrefab;
    public GameObject noteHoldPrefab;
    public GameObject upperEye;
    public GameObject lowerEye;
    List<Note> notes = new List<Note>();
    public List<double?> timeStamps = new List<double?>();
    public List<int> noteTypes = new List<int>();
    public List<double> eyesTimeStamps = new List<double>();
    public List<int> noteSounds = new List<int>();
    int spawnIndex = 0;
    int inputIndex = 0;
    int yawnIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            //Calculate the time start and end of each note
            var metricTimeSpanStart = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
            var metricTimeSpanEnd = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time + note.Length, SongManager.midiFile.GetTempoMap());
            double timeStart = (double)metricTimeSpanStart.Minutes * 60f + metricTimeSpanStart.Seconds + (double)metricTimeSpanStart.Milliseconds / 1000f;
            double timeEnd = (double)metricTimeSpanEnd.Minutes * 60f + metricTimeSpanEnd.Seconds + (double)metricTimeSpanEnd.Milliseconds / 1000f;

            //Add time stamps to arrays based on what kind of note it is
            if (note.NoteName == hugNoteName)
            {
                timeStamps.Add(timeStart);
                noteTypes.Add(tapNoteInt);
                noteSounds.Add(2);
            }
            else if (note.NoteName == kickNoteName)
            {
                timeStamps.Add(timeStart);
                noteTypes.Add(tapNoteInt);
                noteSounds.Add(5);
            }
            else if (note.NoteName == headphoneNoteName)
            {
                int noteSoundNumber = 1;
                timeStamps.Add(timeStart);
                noteTypes.Add(holdStartNoteInt);
                noteSounds.Add(noteSoundNumber);
                timeStamps.Add(timeStart);
                noteTypes.Add(holdMiddleNoteInt);
                noteSounds.Add(noteSoundNumber);
                timeStamps.Add(timeEnd);
                noteTypes.Add(holdEndNoteInt);
                noteSounds.Add(noteSoundNumber);
            }
            else if (note.NoteName == coverNoteName)
            {
                int noteSoundNumber = 3;
                timeStamps.Add(timeStart);
                noteTypes.Add(holdStartNoteInt);
                noteSounds.Add(noteSoundNumber);
                timeStamps.Add(timeStart);
                noteTypes.Add(holdMiddleNoteInt);
                noteSounds.Add(noteSoundNumber);
                timeStamps.Add(timeEnd);
                noteTypes.Add(holdEndNoteInt);
                noteSounds.Add(noteSoundNumber);
            }
            else if (note.NoteName == snoreNoteName)
            {
                int noteSoundNumber = 4;
                timeStamps.Add(timeStart);
                noteTypes.Add(holdStartNoteInt);
                noteSounds.Add(noteSoundNumber);
                timeStamps.Add(timeStart);
                noteTypes.Add(holdMiddleNoteInt);
                noteSounds.Add(noteSoundNumber);
                timeStamps.Add(timeEnd);
                noteTypes.Add(holdEndNoteInt);
                noteSounds.Add(noteSoundNumber);
            }
            else if (note.NoteName == eyesNoteName)
            {
                eyesTimeStamps.Add(timeStart);
                eyesTimeStamps.Add(timeEnd);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            double startTimeStamp = timeStamps[spawnIndex].Value;
            if (SongManager.GetAudioSourceTime() >= startTimeStamp - SongManager.Instance.noteTime)
            {
                int type = noteTypes[spawnIndex];
                if (type == tapNoteInt || type == holdStartNoteInt || type == holdEndNoteInt)
                {
                    var note = Instantiate(notePrefab, transform);
                    notes.Add(note.GetComponent<NoteTap>());
                    note.GetComponent<NoteTap>().assignedTime = (float)timeStamps[spawnIndex];
                }
                else if (type == holdMiddleNoteInt)
                {

                    var noteHold = Instantiate(noteHoldPrefab, transform);
                    notes.Add(noteHold.GetComponent<NoteHold>());
                    noteHold.GetComponent<NoteHold>().assignedTime = (float)timeStamps[spawnIndex - 1];
                    noteHold.GetComponent<NoteHold>().assignedEndTime = (float)timeStamps[spawnIndex + 1];
                }
                spawnIndex++;
            }
                
        }
        if (yawnIndex < eyesTimeStamps.Count)
        {
            double startTimeStamp = eyesTimeStamps[yawnIndex];
            if (SongManager.GetAudioSourceTime() >= startTimeStamp - SongManager.Instance.noteTime)
            {
                ScoreManager.Yawn();
                double delay = ((double)timeStamps[spawnIndex + 1]) - ((double)timeStamps[spawnIndex]);
                upperEye.GetComponent<EyeController>().CloseEye(delay);
                lowerEye.GetComponent<EyeController>().CloseEye(delay);
                yawnIndex += 2;
            }
        }
        if (inputIndex < timeStamps.Count)
        {
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (noteTypes[inputIndex] == tapNoteInt || noteTypes[inputIndex] == holdStartNoteInt)
            {
                double timeStamp = timeStamps[inputIndex].Value;
                double marginOfError = SongManager.Instance.marginOfError;
                bool inputAction = Input.GetKeyDown(input);

                // TODO
                if(noteTypes[inputIndex] == holdEndNoteInt)
                {
                    inputAction = Input.GetKeyUp(input);
                }

                if (inputAction && !(audioTime <= timeStamp - marginOfError - 0.2))
                {
                    HitCheck(timeStamp, audioTime);
                }
                else if (timeStamp + SongManager.Instance.marginOfError <= audioTime)
                {
                    Miss(inputIndex);
                    inputIndex++;
                }
                else if (inputAction)
                {
                    ScoreManager.Miss();
                }
            }
            else if(noteTypes[inputIndex] == holdMiddleNoteInt)
            {
                double nextTimeStamp = timeStamps[inputIndex+1].Value;
                
                if (Input.GetKeyUp(input))
                {
                    inputIndex++;
                    if (!HitCheck(nextTimeStamp, audioTime))
                    {
                        Miss(inputIndex-1);
                    }

                }
                else if (!Input.GetKey(input))
                {
                    Miss(inputIndex);
                    Miss(inputIndex + 1);
                    inputIndex += 2;
                }
                else if (nextTimeStamp + SongManager.Instance.marginOfError <= audioTime)
                {
                    inputIndex += 2;
                }
            }
            else
            {
                inputIndex++;
            }
        }       
    }
    public bool HitCheck(double timeStamp, double audioTime)
    {
        bool hit = false;
        if (Math.Abs(audioTime - timeStamp) < SongManager.Instance.marginOfError)
        {
            hit = true;
            Hit(inputIndex, Math.Abs(audioTime - timeStamp));
            print($"Hit on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
        }
        else
        {
            print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
            Miss(inputIndex);
        }
        inputIndex++;
        return hit;
    }

    public void ClearNotes()
    {
        foreach (var note in notes)
        {
            if (note != null)
            {
                Destroy(note.gameObject);
            }
        }

        notes.Clear();
        timeStamps.Clear();
        noteTypes.Clear();
        eyesTimeStamps.Clear();

        spawnIndex = 0;
        inputIndex = 0;
        yawnIndex = 0;
    }

    private void Hit(int index, double timing)
    {
        ScoreManager.Hit(timing, noteSounds[index]);
        notes[index].Hit();
    }
    private void Miss(int index)
    {
        if (notes[index] != null)
        {
            notes[index].Miss();
        }
        ScoreManager.Miss();
    }
}
