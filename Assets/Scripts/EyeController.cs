using UnityEngine;
using System.Collections;

public class EyeController : MonoBehaviour
{
    public Vector3 startingPos;
    public Vector3 endingPos;
    public static bool eyesClosed = false;
    public float speed;
    private bool isClosing;
    private bool isOpening;
    private bool waitingToOpen;
    private bool waitingToClose;
    private double timeInstantiated;
    private double delay;
    private float t { get; set; }
    
    // Use this for initialization
    void Start()
    {
        isClosing = false;
        waitingToOpen = false;
        isOpening = false;

        
    }

    public void CloseEye(double inputDelay)
    {
        eyesClosed = true;
        isClosing = false;
        waitingToClose = true;
        timeInstantiated = SongManager.GetAudioSourceTime();
        
        delay = inputDelay;
        print(delay);
    }
    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;

        t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime*2));
        if (t > 0.3 && waitingToClose)
        {
            isClosing = true;
            waitingToClose = false;
            timeInstantiated = SongManager.GetAudioSourceTime();
        }
        else if (isClosing)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, endingPos, speed * Time.deltaTime);
            if (transform.position == endingPos)
            {
                isClosing = false;
                waitingToOpen = true;
            }
        }
        if (waitingToOpen && timeSinceInstantiated > delay)
        {
            isOpening = true;
            waitingToOpen = false;
        }
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, speed * Time.deltaTime);
            if(transform.position == startingPos)
            {
                isOpening = false;
                eyesClosed = false;
            }
        }

    }
}
