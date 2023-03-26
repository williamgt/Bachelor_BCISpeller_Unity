using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

public class ChildrenPulse : MonoBehaviour, IGazeFocusable
{
    public List<float> pulses;
    public GameObject LSLFrequencyOutlet; //TODO this is just a helper implementation for seeing the frequency, remove later
    public GameObject LSLSamplePointCounterOutlet;
    public GameObject LSLCCAResultInlet;

    public void GazeFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            LSLCCAResultInlet.GetComponent<LSLCCAInlet>().setCluster(gameObject);
            LSLFrequencyOutlet.GetComponent<LSLFrequencyOutlet>().setCluster(gameObject);
            int i = 0;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Pulsating>().setRate(pulses[i]);
                i++;
            }
            LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().setIncrementSamplePoint(true);
        }
        else
        {
            LSLCCAResultInlet.GetComponent<LSLCCAInlet>().setCluster(null);
            LSLFrequencyOutlet.GetComponent<LSLFrequencyOutlet>().setCluster(null);
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Pulsating>().setRate(0);
            }
            LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().setIncrementSamplePoint(false);
            LSLSamplePointCounterOutlet.GetComponent<LSLSamplePointCounterOutlet>().resetSamplePoint();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > pulses.Count) throw new System.Exception("Must be enough pulse rates for the children of the cluster");
        if (LSLFrequencyOutlet == null) throw new System.Exception("LSLFrequencyOutlet must be defined");
        if (LSLSamplePointCounterOutlet == null) throw new System.Exception("LSLSamplePointCounterOutlet must be defined");
        if (LSLCCAResultInlet == null) throw new System.Exception("LSLCCAResultInlet must be defined");
    }
}
