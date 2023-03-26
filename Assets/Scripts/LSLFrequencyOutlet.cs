using System.Collections;
using System.Collections.Generic;
using LSL4Unity.Utils;
using UnityEngine;

public class LSLFrequencyOutlet : ADoubleOutlet
{
    private int samplePoint = 0;
    private float counter = 0;
    public override List<string> ChannelNames
    {
        get
        {
            List<string> chanNames = new List<string> { 
                "freq0sinh1", "freq0cosh1", "freq0sinh2", "freq0cosh2", "freq0sinh3", "freq0cosh3",
                "freq1sinh1", "freq1cosh1", "freq1sinh2", "freq1cosh2", "freq1sinh3", "freq1cosh3",
                "freq2sinh1", "freq2cosh1", "freq2sinh2", "freq2cosh2", "freq2sinh3", "freq2cosh3",
                "freq3sinh1", "freq3cosh1", "freq3sinh2", "freq3cosh2", "freq3sinh3", "freq3cosh3",
                "freq4sinh1", "freq4cosh1", "freq4sinh2", "freq4cosh2", "freq4sinh3", "freq4cosh3",
                "freq5sinh1", "freq5cosh1", "freq5sinh2", "freq5cosh2", "freq5sinh3", "freq5cosh3",
                /*, "counter"*/};
            return chanNames;
        }
    }

    public GameObject cluster = null;

    protected override bool BuildSample()
    {
        int i = 0;

        if (cluster == null)
        {
            for(; i < ChannelNames.Count; i++)
            {
                sample[i] = 0;
            }
            //sample[6] = counter; //NBNBNBNBNB
            return true;
        }

        samplePoint++;
        foreach (Transform child in cluster.transform)
        {
            //sample[i] = child.gameObject.GetComponent<Pulsating>().getFreq();
            var yValues = child.gameObject.GetComponent<Pulsating>().getYElement(samplePoint);
            sample[i] = yValues.sinh1;
            sample[i + 1] = yValues.cosh1;
            sample[i + 2] = yValues.sinh2;
            sample[i + 3] = yValues.cosh2;
            sample[i + 4] = yValues.sinh3;
            sample[i + 5] = yValues.cosh3;
            i += 6;
        }
        //sample[6] = counter; //NBNBNBNBNB
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

    /*private void FixedUpdate()
    {
        base.FixedUpdate();
        counter++;
        if (counter % 300 == 0) counter = 0;
    }*/
}
