using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;
using Tobii.XR;
using LSL4Unity.Utils;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class LSLGazeRayAndBlinkingStream : ADoubleOutlet
            {
                public string channelName1;
                public string channelName2;
                public string channelName3;
                public string channelName4;
                public string channelName5;
                public string channelName6;
                public string channelName7;
                public string channelName8;

                private static float leftOpen, rightOpen;

                public override List<string> ChannelNames
                {
                    get
                    {
                        List<string> chanNames = new List<string> { channelName1, channelName2, channelName3, channelName4, channelName5, channelName6, channelName7, channelName8 };
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
                    //Getting data from SRAnipal about eye openness
                    SRanipal_Eye.GetEyeOpenness(EyeIndex.LEFT, out leftOpen);
                    SRanipal_Eye.GetEyeOpenness(EyeIndex.RIGHT, out rightOpen);

                    // For social use cases, data in local space may be easier to work with
                    var eyeTrackingDataLocal = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);

                    // Check if gaze ray is valid
                    //if (eyeTrackingData.GazeRay.IsValid) //NB! Using invalid gazeray may be dangerous as per https://developer.tobii.com/xr/develop/unity/documentation/api-reference/core/
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
                    sample[6] = leftOpen;
                    sample[7] = rightOpen;

                    return true;

                    //}

                    return false;
                }
            }
        }
    }
}

