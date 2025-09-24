using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float assignedTime;
    public double timeInstantiated { get; set; }
    public float t { get; set; }

    public void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        StartGameObject();
    }
    public void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
        UpdateGameObject();
    }
    public virtual void StartGameObject()
    {

    }
    public virtual void UpdateGameObject()
    {

    }
    public Vector3 GetNotePosition(float _t)
    {
        return Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, _t);
    }
    public virtual void Hit()
    {

    }
    public virtual void Miss()
    {

    }
}
