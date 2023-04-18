using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.Utils;
using TMPro;

public class LSLCCAInlet : AFloatInlet
{
    private GameObject cluster = null;
    public TextMeshProUGUI TMPWord;

    private string currentLetter;

    // Start is called before the first frame update
    void Start()
    {
        //base.Start();
        registerAndLookUpStream();
    }
    void Reset()
    {
        StreamName = "CCA";
        StreamType = "cca";
    }

    //When a stream is available
    protected override void OnStreamAvailable()
    {
        Debug.Log("Inlet found a stream with channelnames:");
        foreach (var e in ChannelNames) Debug.Log(e);
    }

    //Used as update or fixed update or late update
    protected override void Process(float[] newSample, double timestamp)
    {
        foreach(var e in newSample) Debug.Log("Inlet: " + e);
        int pos = (int)newSample[0];
        string letter = cluster.transform.GetChild(pos).GetComponent<Pulsating>().getLetter();
        Debug.Log(letter);

        //Logicfor spelling words
        if (letter.Equals("<-") && TMPWord.text.Length > 0) //Backspace, needs text to have more than 0 letters
        {
            TMPWord.text.Remove(TMPWord.text.Length);
        }
        else if (letter.Equals("_")) //Space
        {
            TMPWord.text += " ";
        }
        else //Else, just add the letter
        {
            TMPWord.text += letter;
        }
    }

    //Setting a new cluster. Used for knowing which letter is being added to the 
    public void setCluster(GameObject newCluster)
    {
        cluster = newCluster;
    }
}
