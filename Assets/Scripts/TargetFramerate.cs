using UnityEngine;

public class TargetFramerate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 150;   //Setting target framerate to 150 to make the application have 150fps, which is more than the sampling rate of the eye tracker
    }
}
