using System.Collections;
using System.Collections.Generic;
using LSL4Unity.Utils;
using UnityEngine;

/**
 * Class LSLFrequencyOutlet is an LSL outlet for sending values needed for the Y vector used to perform CCA. 
 * This class takes care of putting correct harmonics in the LSL stream. 
 * The number of harmonics is 3 for each letter-cube in a cluster.
 * NB! This class is no longer in use due to the existence of LSLSamplePointCounter
 */
public class LSLFrequencyOutlet : ADoubleOutlet
{
    public GameObject cluster = null;
    private int samplePoint = 0;

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
                "freq5sinh1", "freq5cosh1", "freq5sinh2", "freq5cosh2", "freq5sinh3", "freq5cosh3",};
            return chanNames;
        }
    }


    //This function is called in super class' FixedUpdate
    protected override bool BuildSample()
    {
        int i = 0;

        if (cluster == null) //Only push 0 if no cluster is currently looked at
        {
            for(; i < ChannelNames.Count; i++)
            {
                sample[i] = 0;
            }
            return true;
        }

        samplePoint++;
        foreach (Transform child in cluster.transform) //A cluster is looked at, get values related to Y vector for all letter-cubes in cluster
        {
            var yValues = child.gameObject.GetComponent<LetterCubeFlicker>().getYElement(samplePoint);
            sample[i] = yValues.sinh1;
            sample[i + 1] = yValues.cosh1;
            sample[i + 2] = yValues.sinh2;
            sample[i + 3] = yValues.cosh2;
            sample[i + 4] = yValues.sinh3;
            sample[i + 5] = yValues.cosh3;
            i += 6;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    override
    protected void Update(){ } //Need to override Update because the BaseOutlet runs BuildSample in parent Update before checking which hook to use


    public void setCluster(GameObject newCluster)
    {
        cluster = newCluster;
    }
}
