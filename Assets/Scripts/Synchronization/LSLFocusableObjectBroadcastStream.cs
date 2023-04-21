using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.Utils;

/**
 * LSLFocusableObjectBroadcastStream is an LSL outlet for streaming a focusable object's position when it's looked at.
 * The objects which have this script in the scene LSLBlinkingStream is deactivated, meaning this script is unused.
 */

public class LSLFocusableObjectBroadcastStream : ADoubleOutlet
{
    public string channelName1; //x coordinate
    public string channelName2; //y coordinate
    public string channelName3; //z coordinate

    private Vector3 focusedObjectPosition = Vector3.zero;
    private bool stream = false;
    private bool justStarted = true;

    public override List<string> ChannelNames
    {
        get
        {
            List<string> chanNames = new List<string> { channelName1, channelName2, channelName3 };
            return chanNames;
        }
    }

    public void setFocusedObjectPosition(Vector3 newFocusedObjectPos, bool stream)
    {
        Debug.Log("Can i stream? " + stream);
        focusedObjectPosition = newFocusedObjectPos;
        this.stream = stream;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        IrregularRate = true;
    }

    protected override bool BuildSample()
    {
        if(justStarted)
        {
            sample[0] = 9999999999.0;
            sample[1] = 9999999999.0;
            sample[2] = 9999999999.0;
            justStarted = false;
            return true;
        }
        if (!stream)
        {
            //sample[0] = 9999999999.0;
            //sample[1] = 9999999999.0;
            //sample[2] = 9999999999.0;
            return false;
        }

        sample[0] = focusedObjectPosition.x;
        sample[1] = focusedObjectPosition.y;
        sample[2] = focusedObjectPosition.z;

        return true;
    }
}