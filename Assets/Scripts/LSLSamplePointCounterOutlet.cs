using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.Utils;

public class LSLSamplePointCounterOutlet : ADoubleOutlet
{
    private float samplePoint = 0;
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
