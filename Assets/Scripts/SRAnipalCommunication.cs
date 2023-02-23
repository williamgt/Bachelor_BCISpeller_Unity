using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveSR {
    namespace anipal {
        namespace Eye {
            public class SRAnipalCommunication : MonoBehaviour
            {
                private static float blinkLimit = 0.1f;
                private static float leftOpen, rightOpen;

                public static bool IsBlinking()
                {
                    // check both eyes open
                    if (IsWinkingLeft() && IsWinkingRight())
                        return true;
                    else
                        return false;
                }

                public static bool IsWinkingLeft()
                {
                    SRanipal_Eye.GetEyeOpenness(EyeIndex.LEFT, out leftOpen);

                    if (leftOpen < blinkLimit)
                        return true;
                    else
                        return false;
                }

                public static bool IsWinkingRight()
                {
                    SRanipal_Eye.GetEyeOpenness(EyeIndex.RIGHT, out rightOpen);

                    if (rightOpen < blinkLimit)
                        return true;
                    else
                        return false;
                }
                // Start is called before the first frame update
                void Start() {
                    Debug.Log("Starting SRAnipal tracking....");
                }

                // Update is called once per frame
                void Update()
                {
                    SRanipal_Eye.GetEyeOpenness(EyeIndex.LEFT, out leftOpen);
                    float crossCheck = leftOpen;
                }
            }
            
        }
    }
}

