using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pulsating : MonoBehaviour
{
    public float rate = 0f; //The rate at which the object is pulsating, aka the frequency
    public string letter;
    public TextMeshProUGUI TMPLetter;

    private float freq = -1f; //TODO this is just a helper implementation for seeing the frequency, remove later. Is -1 if color is white and 1 if color is red
    private float samplingFreq = 250;

    private float elapsedTime = 0;

    private Renderer renderer;
    private Color color1;
    private Color color2;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        color2 = Color.black;
        color1 = Color.white;
        TMPLetter.text = letter;
    }
    void Update()
    {

        freq = renderer.material.color == Color.white ? -1f : 1f;

        //Don't change color if no rate is set
        if (rate <= 0)
        {
            renderer.material.color = Color.white;
            //TMPLetter.color = Color.black;
            return;
        }
        
        elapsedTime += Time.deltaTime;
     
        renderer.material.color = color1;
        //TMPLetter.color = color2;
     
        //Change color after time has reached rate
        if (elapsedTime >= 1/rate)
        {
            elapsedTime = 0;
            renderer.material.color = color2;
            //TMPLetter.color = color1;

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
    public string getLetter() => letter;

    //Calculates the proper value for the nth elements in the Y vector of SSVEP regime https://www.mdpi.com/1424-8220/20/3/891
    //In this setup, there are used three harmonics harmonics
    public (float sinh1, float cosh1, float sinh2, float cosh2, float sinh3, float cosh3) getYElement(int samplePoint)
    {
        return (
            sinh1: Mathf.Sin(1 * 2 * Mathf.PI * rate * (samplePoint / samplingFreq)),
            cosh1: Mathf.Cos(1 * 2 * Mathf.PI * rate * (samplePoint / samplingFreq)),
            sinh2: Mathf.Sin(2 * 2 * Mathf.PI * rate * (samplePoint / samplingFreq)), 
            cosh2: Mathf.Cos(2 * 2 * Mathf.PI * rate * (samplePoint / samplingFreq)),
            sinh3: Mathf.Sin(3 * 2 * Mathf.PI * rate * (samplePoint / samplingFreq)), 
            cosh3: Mathf.Cos(3 * 2 * Mathf.PI * rate * (samplePoint / samplingFreq)));
    }
}
