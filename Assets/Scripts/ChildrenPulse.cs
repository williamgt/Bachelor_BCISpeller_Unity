using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

public class ChildrenPulse : MonoBehaviour, IGazeFocusable
{
    public List<float> pulses;
    public List<string> letters;
    public GameObject clusterManager;

    //public void GazeFocusChanged(bool hasFocus) => focus = hasFocus;
    public void GazeFocusChanged(bool hasFocus)
    {
        if(hasFocus) clusterManager.GetComponent<ClusterManager>().clusterLookedAt(gameObject);
        if(!hasFocus) clusterManager.GetComponent<ClusterManager>().clusterLookedAwayFrom(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > pulses.Count) throw new System.Exception("Must be enough pulse rates for the children of the cluster");
        if (transform.childCount > letters.Count) throw new System.Exception("Must be enough letters for the children of the cluster");
        if (clusterManager == null) throw new System.Exception("Cluster must have a manager");

        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<Pulsating>().setLetter(letters[i]); //TODO add error handling, should only allow one letter
            i++;
        }
    }

    public List<float> getPulses() => pulses;
}
