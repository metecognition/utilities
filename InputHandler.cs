using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//Challenge
//Enter in some weights (.4 .3 .3 etc...)
//Enter in values (1, 3, 8, etc...)
//Output current total grade
//Output whether student should be worried when < than 70%



public static class InputHandler
{
        
    public static string processText(string inS)
    {
       return inS;
    }
    public static int getAverageChars(string inS)
    {
        string[] words = inS.Split(' ');
        int sum = 0;
        foreach (var word in words)
        {
            sum += word.Length;
        }
        return sum / words.Length;
    }
    public static bool isNum(string inS)
    {
        try
        {
            double num = Double.Parse(inS);
            return true;
        }
        catch (FormatException e)
        {
            Debug.Log(e);
            return false;
        }
    }
    public static bool betterIsNum(string inS)
    {
        if (Double.TryParse(inS, out double j))
            return true;
        else
            return false;

    }


}
