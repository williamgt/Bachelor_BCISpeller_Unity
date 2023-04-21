using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.Utils;
using TMPro;

/**
 * LSLCCAInlet is an LSL inlet used for gathreing the result of the CCA analysis run in a separate Python script. 
 * Takes an LSL stream called 'CCA' wit streamtype 'cca'.
 * The result from the CCA is an index which corresponds to the LetterCube being looked at for some cluster. This information is
 * used to spell a word.
 */
public class LSLCCAInlet : AFloatInlet
{
    private GameObject cluster = null; //The cluster which was last looked at and cointains the right letters
    public TextMeshProUGUI TMPResultString; //The result of the spelling is put here 

    // Start is called before the first frame update
    void Start()
    {
        //base.Start();
        registerAndLookUpStream(); //This is run in the base.Start(), but had some issues leaving it up to super class so doing it here
    }
    void Reset()
    {
        StreamName = "CCA";
        StreamType = "cca";
    }

    //Used as update or fixed update or late update
    protected override void Process(float[] newSample, double timestamp)
    {
        int pos = (int)newSample[0]; //Extracting the index here
        string letter = cluster.transform.GetChild(pos).GetComponent<LetterCubeFlicker>().getLetter(); //Using the index to get correct child's letter
        Debug.Log(letter);

        //Logic for spelling words
        if (letter.Contains("<") || letter.Contains("-") && TMPResultString.text.Length > 0) //Backspace, needs text to have more than 0 letters
        {
            TMPResultString.text.Remove(TMPResultString.text.Length);
        }
        else if (letter.Equals("_")) //Space
        {
            TMPResultString.text += " ";
        }
        else //Else, just add the letter
        {
            TMPResultString.text += letter;
        }
    }

    //Setting a new cluster. Used for knowing which letter is being added to the 
    public void setCluster(GameObject newCluster)
    {
        cluster = newCluster;
    }
}
