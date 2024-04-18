using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockedButtonMap3 : MonoBehaviour
{
    public TMP_Text buttonText; // Use TMP_Text for TextMesh Pro
    public Button button;

    void Start()
    {
        // Check if the button text and button component are assigned
        if (buttonText == null || button == null)
        {
            Debug.LogError("Button text or button component not assigned.");
            return;
        }

        // Initially, set the button text to "LOCKED" and disable the button
        buttonText.text = "LOCKED(200)";
        button.interactable = false;
    }

    void Update()
    {
        // Check if the killScore is greater than or equal to 100
        if (control.killScore >= 200)
        {
            // If the condition is met, enable the button and change its text
            buttonText.text = "PLAY MAP 3";
            button.interactable = true;
        }
        else
        {
            // If the condition is not met, keep the button locked
            buttonText.text = "LOCKED(200)";
            button.interactable = false;
        }
    }
}
