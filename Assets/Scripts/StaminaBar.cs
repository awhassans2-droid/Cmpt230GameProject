using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Image fillImage;

    public float maxStamina = 100f;
    public float currentStamina = 0f;
    
    void Start()
    {
    	SetStamina(currentStamina);
    }

    public void SetStamina(float value)
    {
        currentStamina = Mathf.Clamp(value, 0f, maxStamina);
        if (fillImage != null)
        {
            fillImage.fillAmount = currentStamina / maxStamina;
        }
    }
}
