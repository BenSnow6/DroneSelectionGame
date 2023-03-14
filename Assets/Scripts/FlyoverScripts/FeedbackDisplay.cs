using UnityEngine;
using UnityEngine.UI;

public class FeedbackDisplay : MonoBehaviour
{
    public GameObject popup;
    public Text titleText;
    public Text messageText;
    public Button closeButton;
    public float riskAccumulated = MainManager.Instance.accumulatedRisk;


    public void ShowPopup(string title, string message)
    {
        // Set the title and message text
        titleText.text = title;
        messageText.text = message;

        // Show the popup
        popup.SetActive(true);
    }

    public void ClosePopup()
    {
        // Hide the popup
        popup.SetActive(false);
    }

    void GetDataFromManager()
    {
        // Get the data from the MainManager
        riskAccumulated = MainManager.Instance.accumulatedRisk; 
        Debug.Log($"Risk level is {riskAccumulated}");
    }
}