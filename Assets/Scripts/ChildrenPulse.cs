using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

public class ChildrenPulse : MonoBehaviour, IGazeFocusable
{
    public List<float> pulses;
    public List<string> letters;
    public GameObject clusterManager;
    private bool lookedAt = false;
    private Vector3 startPos;
    private Vector3 endPos;


    //public void GazeFocusChanged(bool hasFocus) => focus = hasFocus;
    public void GazeFocusChanged(bool hasFocus)
    {
        lookedAt = hasFocus;
        if (hasFocus)
        {
            clusterManager.GetComponent<ClusterManager>().clusterLookedAt(gameObject);
            //transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 5.0f);
        }
        if (!hasFocus)
        {
            clusterManager.GetComponent<ClusterManager>().clusterLookedAwayFrom(gameObject);
            //transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * 5.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > pulses.Count) throw new System.Exception("Must be enough pulse rates for the children of the cluster");
        if (transform.childCount > letters.Count) throw new System.Exception("Must be enough letters for the children of the cluster");
        if (clusterManager == null) throw new System.Exception("Cluster must have a manager");

        startPos = transform.position;
        endPos = startPos + new Vector3(0, 0, -7);

        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<Pulsating>().setLetter(letters[i]); //TODO add error handling, should only allow one letter
            i++;
        }
    }

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

    public List<float> getPulses() => pulses;
}
