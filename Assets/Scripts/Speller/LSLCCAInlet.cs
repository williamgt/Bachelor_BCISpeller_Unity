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
    private string correctString = "I LIVE IN NORWAY.";
    private int[] correctValuesCount = new int[] { 0, 0, 0, 0, 0, 0 };
    private int[] correctValues = new int[] { 1, 3, 2, 1, 5, 0, 3, 1, 5, 3, 5, 0, 1, 1, 2, 1, 4 };
    private int counter = 0; //Max value is correctString.Length - 1

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
            TMPResultString.text = TMPResultString.text.Substring(0, TMPResultString.text.Length - 1);
        }
        else if (letter.Equals("_")) //Space
        {
            TMPResultString.text += " ";
        }
        else //Else, just add the letter
        {
            TMPResultString.text += letter;
        }

        //Below is result for test when spellling contents of correctString, NB very hard coded
        string corS = "";

        if (counter > correctString.Length - 1)
        {

            foreach (int corr in correctValuesCount)
            {
                corS += corr + " ";
            }
            Debug.Log("Amount of correct total: " + corS);
            return;
        }

        if (correctValues[counter] == pos) {
            correctValuesCount[pos] += 1;
        }
        
        counter++;

        foreach (int corr in correctValuesCount) {
            corS += corr + " ";
        }
        Debug.Log("Amount of correct so far: " + corS);

    }

    //Setting a new cluster. Used for knowing which letter is being added to the 
    public void setCluster(GameObject newCluster)
    {
        cluster = newCluster;
    }
}
