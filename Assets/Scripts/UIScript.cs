using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIScript : MonoBehaviour
{
    [SerializeField] private SongManager songManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
}
