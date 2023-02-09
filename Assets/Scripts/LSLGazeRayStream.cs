using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;
using Tobii.XR;
using LSL4Unity.Utils;

public class LSLGazeRayStream : ADoubleOutlet
{
    public string channelName1;
    public string channelName2;
    public string channelName3;

    public override List<string> ChannelNames
    {
        get
        {
            List<string> chanNames = new List<string> { channelName1, channelName2, channelName3 };
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
        // Get eye tracking data in world space
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);

        // Check if gaze ray is valid
        if (eyeTrackingData.GazeRay.IsValid)
        {
            // The origin of the gaze ray is a 3D point
            var rayOrigin = eyeTrackingData.GazeRay.Origin;

            // The direction of the gaze ray is a normalized direction vector
            var rayDirection = eyeTrackingData.GazeRay.Direction;
            Debug.Log("Direction, world " + rayDirection);

            // The EyeBlinking bool is true when the eye is closed
            var isLeftEyeBlinking = eyeTrackingData.IsLeftEyeBlinking;
            var isRightEyeBlinking = eyeTrackingData.IsRightEyeBlinking;
          
            Debug.DrawRay(rayOrigin, rayDirection * 10, Color.red);
        }

        // For social use cases, data in local space may be easier to work with
        var eyeTrackingDataLocal = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);

        if (eyeTrackingData.GazeRay.IsValid)
        {
            // Using gaze direction in local space makes it easier to apply a local rotation
            // to your virtual eye balls.
            var eyesDirection = eyeTrackingDataLocal.GazeRay.Direction;

            //Debug.Log("BlilnkL " + isLeftEyeBlinking);
            //Debug.Log("BlinkR " + isRightEyeBlinking);
            Debug.Log("Direction, local " + eyesDirection);

            sample[0] = eyesDirection.x;
            sample[1] = eyesDirection.y;
            sample[2] = eyesDirection.z;

            return true;

        }

        return false;
    }
}