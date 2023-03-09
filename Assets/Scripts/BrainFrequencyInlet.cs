using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.Utils;

public class BrainFrequencyInlet : AFloatInlet
{

    //When a stream is available
    protected override void OnStreamAvailable()
    {
        //Debug.Log(ChannelNames)
        if (ChannelCount == 1)
            return;
        Debug.Log("Found a stream!");
    }

    //Used as update or fixed update or late update
    protected override void Process(float[] newSample, double timestamp)
    {
        foreach(var e in newSample)
        Debug.Log(e);
    }
}
