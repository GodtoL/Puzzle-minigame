using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text W;
    public Text S;
    public Text A;
    public Text D;
    public Text steps;
    private int stepsText;
    public GameObject LevelCompleteUI;
    // Start is called before the first frame update
    void Start()
    {


    }

    public void showLevelCompletedUI()
    {
        LevelCompleteUI.SetActive(true);
    }

    public void hideLevelCompletedUI()
    {
        LevelCompleteUI?.SetActive(false);
    }
    public void InitializeSteps(int currentLevelSteps)
    {
        stepsText = currentLevelSteps;
    }
    void stepsUpdateUI(int stepsCurrent)
    {
        stepsText = stepsCurrent;

    }
    // Update is called once per frame
    void Update()
    {
        steps.text = $"{stepsText}";
    }
}
