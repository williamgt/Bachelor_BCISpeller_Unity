using TMPro;
using UnityEngine;
using Tobii.G2OM;

/**
 * LetterCubeFlicker is a class used for flickering the LetterCube.
 * Earlier it implemented IGazeFocusable to make the TMPLetter fly, but difficulties with hitboxes did not allow for this.
 */
public class LetterCubeFlicker : MonoBehaviour//, IGazeFocusable
{
    public float frequency = 0f; //The rate at which the object is pulsating, aka the frequency
    public TextMeshProUGUI TMPLetter; //The letter associated with the flickering cube, is a reference to the TMP in LetterCube prefab

    private float samplingFreq = 250;

    private float elapsedTime = 0;

    private Renderer renderer;
    private Color color1; //Set at start, is white
    private Color color2; //Set at start, is black
    public Material material1; //Unlit material with color white, found in Materials folder
    public Material material2; //Unlit material with color black, found in Materials folder

    void Start()
    {
        renderer = GetComponent<Renderer>();
        color2 = Color.black;
        color1 = Color.white;
        renderer.material = material1;
    }

    /*public void GazeFocusChanged(bool hasFocus)
    {
        Debug.Log("Looked at box");
        if (hasFocus) TMPLetter.transform.Translate(new Vector3(0, 0, 2), Space.Self);
        if (!hasFocus) TMPLetter.transform.Translate(new Vector3(0, 0, -2), Space.Self);
    }*/

    void FixedUpdate()
    {

        //changeColor();
        //MakeFlicker();
        //changeMaterial();
        changeMaterial2();
    }

    //Old, unused implementation for flickering based on color and Unity's deltaTime
    private void changeColor()
    {
        //Don't change color if no rate is set
        if (frequency <= 0)
        {
            renderer.material.color = Color.white;
            //TMPLetter.color = Color.black;
            return;
        }

        elapsedTime += Time.deltaTime;

        renderer.material.color = color1;
        //TMPLetter.color = color2;

        //Change color after time has reached rate
        if (elapsedTime >= 1 / frequency)
        {
            elapsedTime = 0;
            renderer.material.color = color2;
            //TMPLetter.color = color1;

            var temp = color1;
            color1 = color2;
            color2 = temp;
        }
    }

    //Old, unused implementation for changing between unlit materials for better contrast, but based on Unity's deltaTime
    private void changeMaterial()
    {
        //Let material be white if no flickering rate
        if (frequency <= 0)
        {
            renderer.material = material1;
            //TMPLetter.color = Color.black;
            return;
        }

        elapsedTime += Time.deltaTime;

        renderer.material = material1;
        //TMPLetter.color = color2;

        //Change color after time has reached rate
        if (elapsedTime >= 1 / (frequency * 2))
        {
            elapsedTime = 0;
            renderer.material = material2;
            //TMPLetter.color = color1;

            var temp = material1;
            material1 = material2;
            material2 = temp;
        }
    }

    //Yoinked from https://github.com/ryanlintott/SSVEP_keyboard
    private void MakeFlicker()
    {
        // old equation (I was making my own time.time for some reason)
        //dtime += Time.deltaTime;
        //float wave = Mathf.Sin( (dtime * 2.0f * Mathf.PI) * cycleHz);

        // Sin works on RAD not DEG.
        // Sin wave flashing is best for SSVEP
        // Time in seconds (mod to keep small values) * Pi (one sin wave per second) * cycleHz (cycleHz sin waves per second) / 2 (half a sin wave per cycleHz)
        float colorMix = Mathf.InverseLerp(-1f, 1f, Mathf.Sin((Time.time % 10.0f) * Mathf.PI * frequency * 2.0f));

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

    //Implementation that is currently in use, based on Sine and lerping to change unlit materials (better flickering and contrast)
    //Is based on https://github.com/ryanlintott/SSVEP_keyboard
    private void changeMaterial2()
    {
        float colorMix = Mathf.InverseLerp(-1f, 1f, Mathf.Sin((Time.time % 10.0f) * Mathf.PI * frequency * 2.0f));

        if (colorMix > 0.5f) //Not very smart implementation, setting material for each iteration of fixedupdate
        {
            renderer.material = material2;
        }
        else
        {
            renderer.material = material1;
        }
    }

    //Called on initialisation by Cluster to set the frequency of the LetterCube
    public void setFrequency(float newFrequency)
    {
        frequency = newFrequency;
    }

    //Called on initialisation by Cluster to set the letter of the LetterCube's TMP
    public void setLetter(string letter)
    {
        TMPLetter.text = letter;
    }

    //public float getFrequency() => freq;
    public string getLetter() => TMPLetter.text;

    //Calculates the proper value for the nth elements in the Y vector of SSVEP regime https://www.mdpi.com/1424-8220/20/3/891
    //In this setup, there are used three harmonics harmonics
    public (float sinh1, float cosh1, float sinh2, float cosh2, float sinh3, float cosh3) getYElement(int samplePoint)
    {
        return (
            sinh1: Mathf.Sin(1 * 2 * Mathf.PI * frequency * (samplePoint / samplingFreq)),
            cosh1: Mathf.Cos(1 * 2 * Mathf.PI * frequency * (samplePoint / samplingFreq)),
            sinh2: Mathf.Sin(2 * 2 * Mathf.PI * frequency * (samplePoint / samplingFreq)), 
            cosh2: Mathf.Cos(2 * 2 * Mathf.PI * frequency * (samplePoint / samplingFreq)),
            sinh3: Mathf.Sin(3 * 2 * Mathf.PI * frequency * (samplePoint / samplingFreq)), 
            cosh3: Mathf.Cos(3 * 2 * Mathf.PI * frequency * (samplePoint / samplingFreq)));
    }
}
