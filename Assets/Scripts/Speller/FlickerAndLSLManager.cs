using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * FlickerAndLSLManager is responsible for making the LetterCubes of the Cluster which is looked at flicker,
 * and for communication with the LSL related scripts.
 * The implementation is not optimal, but it works.
 */
public class FlickerAndLSLManager : MonoBehaviour
{
    //public GameObject LSLFrequencyOutlet; //TODO this is just a helper implementation for seeing the frequency, remove later
    public GameObject LSLSamplePointCounterOutlet;
    public GameObject LSLCCAResultInlet;

    private GameObject currentCluster;
    private GameObject prevCluster;

    public float flickeringLimitSeconds; //How long the LetterCubes are flickering
    private float deadTime = 1.0f; //Amount of deadtime before the flickering starts
    private float timeSpent = 0.0f; //Counts how long the flickering has been ongoing

    private bool startPulsating = false;
    private bool stopPulsating = true;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        //if (LSLFrequencyOutlet == null) throw new System.Exception("LSLFrequencyOutlet must be defined");
        if (LSLSamplePointCounterOutlet == null) throw new System.Exception("LSLSamplePointCounterOutlet must be defined");
        if (LSLCCAResultInlet == null) throw new System.Exception("LSLCCAResultInlet must be defined");
    }

    // FixedUpdate is called once per physics frame
    private void FixedUpdate()
    {       
        //User is not looking at cluster, reset evreything to prepare for next time gaze is detected
        if (currentCluster == null)
        {
            //stopLSLInletAndOutlet();
            resetBoolValues();
            timeSpent = 0.0f;
            return;
        }

        timeSpent += Time.deltaTime;

        //Can start pulsating if time spent looking at cluster is larger than dead time
        startPulsating = timeSpent >= deadTime;
        if (startPulsating & !activated)
        {
            currentCluster.GetComponent<Cluster>().activateFlickering();
            notifyInletAndStartOutlet();
            activated = true;
        }

        //Must stop pulsating if time spent looking at cluster is more than the pulsating limit after the deadtime has passed
        stopPulsating = timeSpent >= deadTime + flickeringLimitSeconds;
        if (stopPulsating & activated)
        {
            currentCluster.GetComponent<Cluster>().deactivateFlickering();
            stopOutlet();
            timeSpent = 0.0f;
        }
    }

    public void clusterLookedAt(GameObject newCluster)
    {
        prevCluster = currentCluster; //If switching directly from one cluster to another, set prev as the current one
        timeSpent = 0.0f; //Reset timer, because new cluster is looked at
        
        currentCluster = newCluster; //Finally setting new cluster and getting the child frequencies
    }

    public void clusterLookedAwayFrom(GameObject oldCluster)
    {
        prevCluster = oldCluster; //Previous cluster is now old cluster
        currentCluster = null; //Currently not looking at any cluster

        //Need to deactivate and reset evreything
        prevCluster.GetComponent<Cluster>().deactivateFlickering();
        stopOutlet();
        resetBoolValues();
        timeSpent = 0.0f;
    }

    private void resetBoolValues()
    {
        activated = false;
        startPulsating = false;
        stopPulsating = true;
    }

    private void notifyInletAndStartOutlet()
    {
        LSLCCAResultInlet.GetComponent<LSLCCAInlet>().setCluster(currentCluster); //Notifying which cluster is looked at, so inlet knows which letter to spell
        LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().setIncrementSamplePoint(true); //Tell outlet to begin incrementing
    }
    private void stopOutlet()
    {
        LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().setIncrementSamplePoint(false); //Tell outlet to stop incrementing
        LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().resetSamplePoint(); //...and reset increments
    }
}
