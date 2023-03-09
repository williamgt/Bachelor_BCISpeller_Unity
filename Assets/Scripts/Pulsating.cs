using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsating : MonoBehaviour
{
    public float rate = 0f;

    private float freq = -1f; //TODO this is just a helper implementation for seeing the frequency, remove later. Is -1 if color is white and 1 if color is red

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

        freq = renderer.material.color == Color.white ? -1f : 1f;

        //Don't change color if no rate is set
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

    public float getFreq() => freq;
}
