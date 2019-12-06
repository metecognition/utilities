using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    //UI Elements for text changing
    public Text outputField;
    public InputField inputField;
    public Button runButton;

     void Start()
    {
        outputField.text = "";
    }

    public void RunButtonPressed()
    {
        outputField.text = "Average Letters Per Word : " + Utilities.AverageWordLength(inputField.text).ToString();
        //outputField.text = Utilities.ProcessText(inputField.text);
    }
}
