using Keyboard;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UiKeyboardLink : MonoBehaviour
{
    [SerializeField] private KeyboardManager keyboard;
    [SerializeField] private List<Button> buttons;

    private TextMeshProUGUI selectedButtonText;

    private void Start()
    {
        keyboard.OnKeyPressed.AddListener(UpdateUiText);
        keyboard.OnEnterPressed.AddListener(delegate { SetKeyboardVisibility(false); });
        SetKeyboardVisibility(false);
        foreach (var button in buttons)
        {
            button.onClick.AddListener(delegate { LinkKeyBoard(button); });
        }
    }

    private void SetKeyboardVisibility(bool visible)
    {
        keyboard.gameObject.SetActive(visible);
    }

    private void LinkKeyBoard(Button button)
    {
        selectedButtonText = getButtonText(button.transform);
        SetKeyboardVisibility(true);
        keyboard.ResetText();
        if (selectedButtonText.text.Length > 2)
        {
            keyboard.SetText(selectedButtonText.text);
        }
    }

    private void UpdateUiText()
    {
        if (selectedButtonText)
        {
            selectedButtonText.text = keyboard.GetText();
        }
    }

    private TextMeshProUGUI getButtonText(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<TextMeshProUGUI>())
            {
                return child.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                TextMeshProUGUI result = getButtonText(child);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return null;
    }
}
