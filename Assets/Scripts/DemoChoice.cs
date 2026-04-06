using UnityEngine;
using TMPro;

public class DemoChoice : MonoBehaviour
{
    public TMP_Text resultText;
    int morality = 0;

    void Start()
    {
        resultText.text = "Press Q to do good or R to cause some trouble";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            morality += 1;
            resultText.text = "You did good. Morality: " + morality;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            morality -= 1;
            resultText.text = "The owner won't like that. Morality: " + morality;
        }
    }
}