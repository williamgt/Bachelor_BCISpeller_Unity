using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pulsating : MonoBehaviour
{
    public float rate = 0f; //The rate at which the object is pulsating, aka the frequency
    //public string letter;
    public TextMeshProUGUI TMPLetter;

    private float freq = -1f; //TODO this is just a helper implementation for seeing the frequency, remove later. Is -1 if color is white and 1 if color is red
    private float samplingFreq = 250;

    private float elapsedTime = 0;

    private Renderer renderer;
    private Color color1;
    private Color color2;
    public Material material1;
    public Material material2;

    private float frequency = 2f;
    private bool isWhite = false;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        color2 = Color.black;
        color1 = Color.white;
        //TMPLetter.text = letter;
        renderer.material = material1;
        //StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        float interval = 1f / (frequency);
        while (true)
        {
            isWhite = !isWhite;
            renderer.material.color = isWhite ? Color.white : Color.black;
            yield return new WaitForSeconds(interval);
        }
    }
    void FixedUpdate()
    {

        //changeColor();
        //MakeFlicker();
        //changeMaterial();
        changeMaterial2();
    }
    private void changeMaterial()
    {
        Debug.Log("changeMaterial");

        //Let material be white if no flickering rate
        if (rate <= 0)
        {
            renderer.material = material1;
            //TMPLetter.color = Color.black;
            return;
        }

        elapsedTime += Time.deltaTime;

        renderer.material = material1;
        //TMPLetter.color = color2;

        //Change color after time has reached rate
        if (elapsedTime >= 1 / (rate * 2))
        {
            elapsedTime = 0;
            renderer.material = material2;
            //TMPLetter.color = color1;

            var temp = material1;
            material1 = material2;
            material2 = temp;
        }
    }


    private void changeMaterial2()
    {
        Debug.Log("changeMaterial2");
        float colorMix = Mathf.InverseLerp(-1f, 1f, Mathf.Sin((Time.time % 10.0f) * Mathf.PI * rate * 2.0f));

        if (colorMix > 0.5f) //Not very smart implementation, setting material for each iteration of fixedupdate
         {
             renderer.material = material2;
        }
        else
        {
            renderer.material = material1;
        }
    }

    private void changeColor()
    {
        Debug.Log("changeColor");
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
        if (elapsedTime >= 1 / rate)
        {
            elapsedTime = 0;
            renderer.material.color = color2;
            //TMPLetter.color = color1;

            var temp = color1;
            color1 = color2;
            color2 = temp;
        }
    }

    //Yoinked from https://github.com/ryanlintott/SSVEP_keyboard
    private void MakeFlicker()
    {
        Debug.Log("MakeFlicker");
        // old equation (I was making my own time.time for some reason)
        //dtime += Time.deltaTime;
        //float wave = Mathf.Sin( (dtime * 2.0f * Mathf.PI) * cycleHz);

        // Sin works on RAD not DEG.
        // Sin wave flashing is best for SSVEP
        // Time in seconds (mod to keep small values) * Pi (one sin wave per second) * cycleHz (cycleHz sin waves per second) / 2 (half a sin wave per cycleHz)
        float colorMix = Mathf.InverseLerp(-1f, 1f, Mathf.Sin((Time.time % 10.0f) * Mathf.PI * rate * 2.0f));

        renderer.material.color = Color.Lerp(color1, color2, colorMix);

        // Count cycles (also used if I want full flashing colours insetad of smooth sin wave)
        /*if (colorMix > 0.5f)
        {
            //_spriteRenderer.color = c1;
            if (swap)
            {
                //updateCounter++;
                swap = false;
            }
        }
        else
        {
            //_spriteRenderer.color = c2;
            swap = true;
        }*/

        //Debug.LogFormat("Cycle Count = {0}", updateCounter);
        //Debug.LogFormat("Accuracy = {0}", Time.time - (updateCounter / cycleHz));

        //Debug.Log("Seconds: " + Time.time.ToString() + " Flashes: " + updateCounter.ToString() + " Hz expected: " + cycleHz.ToString() + " Hz actual: " + (updateCounter / Time.time).ToString());
        //Debug.Log((1/Time.deltaTime).ToString());
    }

    public void setRate(float newRate)
    {
        rate = newRate;
    }

    public void setLetter(string letter)
    {
        TMPLetter.text = letter;
    }

    public float getFreq() => freq;
    public string getLetter() => TMPLetter.text;

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
