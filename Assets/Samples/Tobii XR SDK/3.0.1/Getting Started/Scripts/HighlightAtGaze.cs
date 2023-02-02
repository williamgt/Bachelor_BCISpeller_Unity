// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using Tobii.G2OM;
using Tobii.XR;
using LSL4Unity.Utils;
using UnityEngine;

namespace Tobii.XR.Examples.GettingStarted
{
    //Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
    public class HighlightAtGaze : ADoubleOutlet, IGazeFocusable
    {
        private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
        public Color highlightColor = Color.red;
        public string channelName;
        public float animationTime = 0.1f;

        private Renderer _renderer;
        private Color _originalColor;
        private Color _targetColor;

        private bool sendSample = false;

        public override List<string> ChannelNames
        {
            get
            {
                List<string> chanNames = new List<string> { channelName, "Ch2" };
                return chanNames;
            }
        }

        //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
        public void GazeFocusChanged(bool hasFocus)
        {
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                _targetColor = highlightColor;
                sample[0] = 420.0;
                sample[1] = 69.0;

            }
            //If this object lost focus, fade the object's color to it's original color
            else
            {
                _targetColor = _originalColor;
                sample[0] = 0.0;
                sample[1] = 0.0;

            }
            sendSample = hasFocus;
        }

        protected override void Start()
        {
            base.Start();

            _renderer = GetComponent<Renderer>();
            _originalColor = _renderer.material.color;
            _targetColor = _originalColor;

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
            }

            // For social use cases, data in local space may be easier to work with
            var eyeTrackingDataLocal = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);

            // The EyeBlinking bool is true when the eye is closed
            var isLeftEyeBlinking = eyeTrackingDataLocal.IsLeftEyeBlinking;
            var isRightEyeBlinking = eyeTrackingDataLocal.IsRightEyeBlinking;

            // Using gaze direction in local space makes it easier to apply a local rotation
            // to your virtual eye balls.
            var eyesDirection = eyeTrackingDataLocal.GazeRay.Direction;

            Debug.Log("BlilnkL " + isLeftEyeBlinking);
            Debug.Log("BlinkR " + isRightEyeBlinking);
            Debug.Log("Direction " + eyesDirection);

            //This lerp will fade the color of the object
            if (_renderer.material.HasProperty(_baseColor)) // new rendering pipeline (lightweight, hd, universal...)
            {
                _renderer.material.SetColor(_baseColor, Color.Lerp(_renderer.material.GetColor(_baseColor), _targetColor, Time.deltaTime * (1 / animationTime)));
            }
            else // old standard rendering pipline
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / animationTime));
            }
            return true;
        }
    }
}
