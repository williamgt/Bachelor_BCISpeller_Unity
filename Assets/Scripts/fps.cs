using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Declare these in your class
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.001f;


  /*  void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //T$$anonymous$$s code will break if you set your m_refreshTime to 0, w$$anonymous$$ch makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
        Debug.Log("Framerate "+m_lastFramerate);
    }*/

    private void FixedUpdate()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //T$$anonymous$$s code will break if you set your m_refreshTime to 0, w$$anonymous$$ch makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
        Debug.Log("Framerate " + m_lastFramerate);
    }
}
