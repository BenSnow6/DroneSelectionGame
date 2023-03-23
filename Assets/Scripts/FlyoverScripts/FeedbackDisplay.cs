using UnityEngine;
using UnityEngine.UI;
using System;

public class FeedbackDisplay : MonoBehaviour
{
    public GameObject panel;
    public float riskAccumulated = MainManager.Instance.accumulatedRisk;
    public int batteryLevel = MainManager.Instance.BatteryLeft;
    public Text riskText;
    public  Text batteryText;
    private void Start()
    {
        panel.SetActive(false);
    }


    public void TogglePanel()
    {
        // Set the title and message text
        // titleText.text = title;
        // messageText.text = message;

        // Show the popup
        panel.SetActive(!panel.activeSelf);
        setText();
    }

    public void setText()
    {
        riskText.text = Math.Round(1000*MainManager.Instance.accumulatedRisk, 3).ToString("G3");
        double batteryPercent = MainManager.Instance.BatteryLeft;
        batteryText.text = (batteryPercent/15*100).ToString("0.00") + "%";
        
    }
}