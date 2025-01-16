using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextColorInvert : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Color originalColor;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            originalColor = text.color;
        }
    }

    public void OnHoverEnter()
    {
        if (text != null)
        {
            // Invert the color
            Color invertedColor = new Color(1 - originalColor.r, 1 - originalColor.g, 1 - originalColor.b, originalColor.a);
            text.color = invertedColor;
        }
    }

    public void OnHoverExit()
    {
        if (text != null)
        {
            // Restore the original color
            text.color = originalColor;
        }
    }
}
