using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

public class ChildrenPulse : MonoBehaviour, IGazeFocusable
{
    public List<float> pulses;
    public GameObject LSLFrequencyOutlet; //TODO this is just a helper implementation for seeing the frequency, remove later

    public void GazeFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            LSLFrequencyOutlet.GetComponent<LSLFrequencyOutlet>().setCluster(gameObject);
            int i = 0;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Pulsating>().setRate(pulses[i]);
                i++;
            }
        }
        else
        {
            LSLFrequencyOutlet.GetComponent<LSLFrequencyOutlet>().setCluster(null);
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Pulsating>().setRate(0);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > pulses.Count) throw new System.Exception("Must be enough pulse rates for the children of the cluster");
        if (LSLFrequencyOutlet == null) throw new System.Exception("LSLFrequencyOutlet must be defined");
    }
}
