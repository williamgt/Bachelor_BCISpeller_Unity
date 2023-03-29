using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

public class ChildrenPulse : MonoBehaviour, IGazeFocusable
{
    public List<float> pulses;
    public GameObject clusterManager;

    //public void GazeFocusChanged(bool hasFocus) => focus = hasFocus;
    public void GazeFocusChanged(bool hasFocus)
    {
        if(hasFocus) clusterManager.GetComponent<ClusterManager>().clusterLookedAt(gameObject, pulses);
        if(!hasFocus) clusterManager.GetComponent<ClusterManager>().clusterLookedAwayFrom(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > pulses.Count) throw new System.Exception("Must be enough pulse rates for the children of the cluster");
        if (clusterManager == null) throw new System.Exception("Cluster must have a manager");
    }
}
