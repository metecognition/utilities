using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    //Returns a string that says if each word is a word or a number
    public static string ProcessText(string input)
    {
        var pieces = input.Split(" "[0]);


        string output = "";
        int i = 0;
        float s;

        while (i < pieces.Length)
        {
            if (float.TryParse(pieces[i], out s)) {
                output = output + " number";
            }
            else
            {
                output = output + " word";
            }
            i++;
        }

        return output;
    }

    //Returns a float with the average number of letters per word in the input string
    public static float AverageWordLength(string input)
    {
        var pieces = input.Split(" "[0]);
        float numWords = pieces.Length;
        float numLetters = 0;
        float averageLetters = 0;
        
        for (int i = 0; i < numWords; i++)
        {
            numLetters += pieces[i].Length;
        }
        averageLetters = numLetters / numWords;
        


        return averageLetters;
    }
}
