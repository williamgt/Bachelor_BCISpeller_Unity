using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.Utils;

/**
 * LSLSamplePointCounterOutlet is an LSL outlet for sending data related to sample points. 
 * A sample point is 0 if no cluster of letters is currently looked at, and will therefore always send 0 in that case.
 * If a cluster is looked at, it will start blinking after a delay. After that delay, the sample point is incremented,
 * for then being used to compute values needed for a CCA.
 */
public class LSLSamplePointCounterOutlet : ADoubleOutlet
{
    private float samplePoint = 0; //Trying to account for lag between VR headset and human visual system
    private bool incrementSamplePoint = false;
    public override List<string> ChannelNames
    {
        get
        {
            List<string> chanNames = new List<string> {"counter"};
            return chanNames;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    override
    protected void Update(){ } //Need to override Update because the BaseOutlet runs BuildSample in parent Update before checking which hook to use

    //This function is called in super class' FixedUpdate
    protected override bool BuildSample()
    {
        if (incrementSamplePoint) samplePoint++;
        sample[0] = samplePoint;
        return true;
    }
    public void setIncrementSamplePoint(bool inc)
    {
        incrementSamplePoint = inc;
    }
    public void resetSamplePoint()
    {
        samplePoint = 0;
    }
}
