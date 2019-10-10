using UnityEngine;
using UnityEngine.UI;
using System;
public class UILogic : MonoBehaviour
{
    public Text text;
    public string processedText;
    public InputField input;

    //variables and ui elements for grade calculation
    public float weight1;
    public float weight2;
    public float weight3;
    public float grade1;
    public float grade2;
    public float grade3;
    public InputField weight1Input;
    public InputField weight2Input;
    public InputField weight3Input;
    public InputField weight1Grades;
    public InputField weight2Grades;
    public InputField weight3Grades;
    public Text outputGradeField;
    public Text outputWorriedField;


    public void getInput()
    
    {
        processText(input.textComponent.text);
    }
    public void displayOutput()
    {
        Debug.Log(processedText);
        string typeOfInput = "none";
        if (InputHandler.betterIsNum(processedText))
        {
            typeOfInput = "number";
        }
        else
        {
            typeOfInput = "string";
        }
        text.text = typeOfInput;
        //text.text = InputHandler.getAverageChars(processedText).ToString();
    }
    public void processText(string inS)
    {
        Debug.Log("Before input handler: " + inS);
        //this.processedText = inS;
        this.processedText = InputHandler.processText(inS);
        displayOutput();
    }


    public void getGrade()
    {
        //clear grades
        grade1 = 0;
        grade2 = 0;
        grade3 = 0;

        //get weights for each grade field
        weight1 = float.Parse(weight1Input.text);
        weight2 = float.Parse(weight2Input.text);
        weight3 = float.Parse(weight3Input.text);
        float totalWeight = weight1 + weight2 + weight3;  //get total weight for final grade calculation

        //split the input grades into string arrays
        string[] grades1Str = weight1Grades.text.Split(' ');
        string[] grades2Str = weight2Grades.text.Split(' ');
        string[] grades3Str = weight3Grades.text.Split(' ');

        //find the total of each array
        for (int i = 0; i < grades1Str.Length; i++)
        {
            grade1 += float.Parse(grades1Str[i]);
        }
        for (int i = 0; i < grades2Str.Length; i++)
        {
            grade2 += float.Parse(grades2Str[i]);
        }
        for (int i = 0; i < grades3Str.Length; i++)
        {
            grade3 += float.Parse(grades3Str[i]);
        }

        //divide by the length of the array
        grade1 /= grades1Str.Length;
        grade2 /= grades2Str.Length;
        grade3 /= grades3Str.Length;

        //multiply by weight of each grade field
        grade1 *= weight1;
        grade2 *= weight2;
        grade3 *= weight3;

        //final grade calculation
        float finalGrade = (grade1 + grade2 + grade3) / totalWeight;
        Debug.Log(finalGrade);

        //convert to string
        outputGradeField.text = "Grade: " + finalGrade.ToString() + "%";


        //SHOULD YOU BE WORRIED?
        if (finalGrade < 70)
        {
            outputWorriedField.text = "You should be worried";
        }
        else
        {
            outputWorriedField.text = "You're fine dude";
        }
    }
}
