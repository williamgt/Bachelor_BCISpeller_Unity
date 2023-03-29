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
        Debug.Log(letter);                                      //Currently only allows new characters to be added
        //if(TMPWord.text[-1] != letter[0]) TMPWord.text += letter; //TODO make this better
        if(!TMPWord.text.EndsWith(letter)) TMPWord.text += letter;
    }

    public void setCluster(GameObject newCluster)
    {
        cluster = newCluster;
    }
}
