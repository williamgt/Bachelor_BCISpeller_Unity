using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

/**
 * Cluster is a class that contains functionality needed for the cluster. 
 * Is IGazeFocusable, allowing it to know when it's looked at, lerping it towards the user and alerting the FlickerAndLSLManager.
 * This class also sets its LetterCube children's letters and knows their frequencies.
 */

public class Cluster : MonoBehaviour, IGazeFocusable
{
    public List<float> frequencies;
    public List<string> letters;
    public GameObject flickerAndLSLManager; 
    
    private bool lookedAt = false;
    
    private Vector3 startPos; //Start position used for flying out when looked at
    private Vector3 endPos; //End position used for flying out when looked at
    private float flyOutDistance = -7.0f; //How far the cluster flies out when being looked at


    //Alerts Manager when it is looked at
    public void GazeFocusChanged(bool hasFocus)
    {
        lookedAt = hasFocus;
        if (hasFocus)
        {
            flickerAndLSLManager.GetComponent<FlickerAndLSLManager>().clusterLookedAt(gameObject);
        }
        if (!hasFocus)
        {
            flickerAndLSLManager.GetComponent<FlickerAndLSLManager>().clusterLookedAwayFrom(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > frequencies.Count) throw new System.Exception("Must be enough pulse rates for the children of the cluster");
        if (transform.childCount > letters.Count) throw new System.Exception("Must be enough letters for the children of the cluster");
        if (flickerAndLSLManager == null) throw new System.Exception("Cluster must have a manager");

        startPos = transform.position;
        endPos = startPos + new Vector3(0, 0, flyOutDistance);

        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<LetterCubeFlicker>().setLetter(letters[i]); //TODO add error handling, should only allow one letter
            i++;
        }
    }

    //Moves foreward when looked at and back when not looked at
    private void Update()
    {
        if(lookedAt)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 5.0f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * 5.0f);
        }
    }

    //Returns the list of frequencies given in the Inspector
    public List<float> getFrequencies() => frequencies;

    public void activateFlickering()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<LetterCubeFlicker>().setFrequency(frequencies[i]);
            i++;
        }
    }

    public void deactivateFlickering()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<LetterCubeFlicker>().setFrequency(0);
        }
    }
}
