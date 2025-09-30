using UnityEngine;
using System.Collections;

public class EyeController : MonoBehaviour
{
    public Vector3 startingPos;
    public Vector3 endingPos;
    public float speed;
    private bool isClosing;
    private bool isOpening;
    private bool waitingToOpen;
    private double timeInstantiated;
    private double delay;
    // Use this for initialization
    void Start()
    {
        isClosing = false;
        waitingToOpen = false;
        isOpening = false;

        
    }

    public void CloseEye(double inputDelay)
    {
        isClosing = true;
        timeInstantiated = SongManager.GetAudioSourceTime() + delay;
        delay = inputDelay;
    }
    // Update is called once per frame
    void Update()
    {


        if (isClosing)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, endingPos, speed * Time.deltaTime);
            if (transform.position == endingPos)
            {
                isClosing = false;
                waitingToOpen = true;
            }
        }
        if (waitingToOpen)
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
            if(t > 1)
            {
                isOpening = true;
                waitingToOpen = false;
            }
        }
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, speed * Time.deltaTime);
            if(transform.position == startingPos)
            {
                isOpening = false;
            }
        }

    }
}
