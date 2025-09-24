﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHold : Note
{
    public double assignedEndTime;
    public bool missed { get; set; } = false;
    public double timeInstantiated2 { get; set; }
    public float t2 { get; set; }
    LineRenderer lineRenderer;
    public override void StartGameObject()
    {
        lineRenderer = GetComponent<LineRenderer>();
        timeInstantiated2 = SongManager.GetAudioSourceTime() + (assignedEndTime - assignedTime);
    }
    public override void UpdateGameObject()
    {
        double timeSinceInstantiated2 = SongManager.GetAudioSourceTime() - timeInstantiated2;
        t2 = (float)(timeSinceInstantiated2 / (SongManager.Instance.noteTime * 2));

        if (t > 1 && t2 > 1)
        {
            Destroy(gameObject);
        }

        if (t > 1 && !missed)
        {
            lineRenderer.SetPosition(0, GetNotePosition(1));
        } 
        else
        {
            lineRenderer.SetPosition(0, GetNotePosition(t));
        }

        lineRenderer.SetPosition(1, GetNotePosition(t2));
    }
    public override void Miss()
    {
        missed = true;
        lineRenderer.startColor = lineRenderer.endColor = Color.gray;
    }
}
