using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;
using Tobii.XR;
using LSL4Unity.Utils;

public class LSLGazeRayAndBlinkingStream : ADoubleOutlet
{
    public string channelName1;
    public string channelName2;
    public string channelName3;
    public string channelName4;
    public string channelName5;
    public string channelName6;

    public override List<string> ChannelNames
    {
        get
        {
            List<string> chanNames = new List<string> { channelName1, channelName2, channelName3, channelName4, channelName5, channelName6 };
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

        //if (eyeTrackingData.GazeRay.IsValid)
        //{
            // Using gaze direction in local space makes it easier to apply a local rotation
            // to your virtual eye balls.
            var eyesDirection = eyeTrackingDataLocal.GazeRay.Direction;

            Debug.Log("Direction, local " + eyesDirection);

            //Gathering data related to blinking with left, right and both eyes
            double isLeftBlinking = eyeTrackingDataLocal.IsLeftEyeBlinking ? 1.0 : 0.0;
            double isRightBlinking = eyeTrackingDataLocal.IsRightEyeBlinking ? 1.0 : 0.0;
            double isBlinking = eyeTrackingDataLocal.IsLeftEyeBlinking && eyeTrackingDataLocal.IsRightEyeBlinking ? 1.0 : 0.0;

            //Sending directions eyes are looking
            sample[0] = eyesDirection.x;
            sample[1] = eyesDirection.y;
            sample[2] = eyesDirection.z;
            //Sending data related to blinking, might want to take this out of the if because gazeray not valid when closing eyes for long amount of time
            sample[3] = isLeftBlinking;
            sample[4] = isRightBlinking;
            sample[5] = isBlinking;

            return true;

        //}

        return false;
    }
}