using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

public class ChildrenPulse : MonoBehaviour, IGazeFocusable
{
    public List<float> pulses;

    public void GazeFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Pulsating>().setRate(pulses[i]);
                i++;
            }
        }
        else
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Pulsating>().setRate(0);
                i++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > pulses.Count) throw new System.Exception("Must be enough pulse rates for the children of the cluster");
    }
}
