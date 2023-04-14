using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterManager : MonoBehaviour
{
    public GameObject LSLFrequencyOutlet; //TODO this is just a helper implementation for seeing the frequency, remove later
    public GameObject LSLSamplePointCounterOutlet;
    public GameObject LSLCCAResultInlet;

    public GameObject currentCluster;
    private List<float> currentPulses;
    private bool blinkContiniously = false;

    private float pulsatingLimitSeconds = 6.0f;
    private float deadTime = 1.0f;
    private float timeSpent = 0.0f;

    private bool startPulsating = false;
    private bool stopPulsating = true;
    private bool activated = false;
    private bool alwaysRun = false;

    // Start is called before the first frame update
    void Start()
    {
        if (LSLFrequencyOutlet == null) throw new System.Exception("LSLFrequencyOutlet must be defined");
        if (LSLSamplePointCounterOutlet == null) throw new System.Exception("LSLSamplePointCounterOutlet must be defined");
        if (LSLCCAResultInlet == null) throw new System.Exception("LSLCCAResultInlet must be defined");
        if (currentCluster != null)
        {
            currentPulses = currentCluster.GetComponent<ChildrenPulse>().getPulses(); //current cluster is already set, need to get their pulses
            blinkContiniously = true;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(blinkContiniously)
        {
            startLSLInletAndOutlet();
            alwaysRun = true;
            return;
        }
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
            startLSLInletAndOutlet();
            activated = true;
        }

        //Must stop pulsating if time spent looking at cluster is more than the pulsating limit after the deadtime has passed
        stopPulsating = timeSpent >= deadTime + pulsatingLimitSeconds;
        if (stopPulsating & activated)
        {
            stopLSLInletAndOutlet();
            timeSpent = 0.0f;
        }
    }

    public void clusterLookedAt(GameObject newCluster)
    {
        if (currentCluster != null) return; //Don't set new cluster if already set
        currentCluster = newCluster;
        currentPulses = currentCluster.GetComponent<ChildrenPulse>().getPulses();
    }

    public void clusterLookedAwayFrom(GameObject oldCluster)
    {
        if(GameObject.ReferenceEquals(currentCluster, oldCluster))
        {
            stopLSLInletAndOutlet();
            currentCluster = null;
        }
    }

    private void resetBoolValues()
    {
        activated = false;
        startPulsating = false;
        stopPulsating = true;
    }

    private void startLSLInletAndOutlet()
    {
        LSLCCAResultInlet.GetComponent<LSLCCAInlet>().setCluster(currentCluster);
        LSLFrequencyOutlet.GetComponent<LSLFrequencyOutlet>().setCluster(currentCluster);
        int i = 0;
        foreach (Transform child in currentCluster.transform)
        {
            child.gameObject.GetComponent<Pulsating>().setRate(currentPulses[i]);
            i++;
        }
        LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().setIncrementSamplePoint(true);
    }
    private void stopLSLInletAndOutlet()
    {
        //LSLCCAResultInlet.GetComponent<LSLCCAInlet>().setCluster(null);
        LSLFrequencyOutlet.GetComponent<LSLFrequencyOutlet>().setCluster(null);
        foreach (Transform child in currentCluster.transform)
        {
            child.gameObject.GetComponent<Pulsating>().setRate(0);
        }
        LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().setIncrementSamplePoint(false);
        LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().resetSamplePoint();
    }
}
