using System.Collections;
using System.Collections.Generic;
using LSL4Unity.Utils;
using UnityEngine;

public class LSLFrequencyOutlet : ADoubleOutlet
{
    public override List<string> ChannelNames
    {
        get
        {
            List<string> chanNames = new List<string> { "freq0", "freq1", "freq2", "freq3", "freq4", "freq5"};
            return chanNames;
        }
    }

    public GameObject cluster;

    protected override bool BuildSample()
    {
        int i = 0;

        if (cluster == null)
        {
            for(; i < 6; i++)
            {
                sample[i] = 0;
            }
            return true;
        }

        
        foreach (Transform child in cluster.transform)
        {
            sample[i] = child.gameObject.GetComponent<Pulsating>().getFreq();
            i++;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public void setCluster(GameObject newCluster)
    {
        cluster = newCluster;
    }
}
