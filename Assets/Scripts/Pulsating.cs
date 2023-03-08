using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsating : MonoBehaviour
{
    public float rate = 0f;

    private float elapsedTime = 0;

    private Renderer renderer;
    private Color color1;
    private Color color2;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        color2 = Color.red;
        color1 = renderer.material.color;
    }
    void Update()
    {
        if (rate <= 0)
        {
            renderer.material.color = Color.white;
            return;
        }
        elapsedTime += Time.deltaTime;
     
        renderer.material.color = color1;
     
        //Change color after time has reached rate
        if (elapsedTime >= 1/rate)
        {
            elapsedTime = 0;
            renderer.material.color = color2;

            var temp = color1;
            color1 = color2;
            color2 = temp;
        }
    }

    public void setRate(float newRate)
    {
        rate = newRate;
    }
}
