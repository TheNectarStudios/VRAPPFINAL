
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomInputField : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Image inputFieldBackground;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite selectedSprite;

    void Start()
    {
        inputField.onValueChanged.AddListener(OnTextChanged);
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void OnTextChanged(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            inputFieldBackground.sprite = selectedSprite;
        }
    }

    void OnEndEdit(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            inputFieldBackground.sprite = selectedSprite;
        }
        else
        {
            inputFieldBackground.sprite = normalSprite;
        }
    }
}

/*
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CustomInputField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Image inputFieldBackground;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite selectedSprite;

    private bool hasText = false;
    private bool isHovered = false;
    private bool isSelected = false;

    void Start()
    {
        inputField.onValueChanged.AddListener(OnTextChanged);
        inputField.onEndEdit.AddListener(OnEndEdit);
        inputFieldBackground.sprite = normalSprite;
    }

    void OnTextChanged(string text)
    {
        hasText = !string.IsNullOrEmpty(text);
        UpdateSprite();
    }

    void OnEndEdit(string text)
    {
        hasText = !string.IsNullOrEmpty(text);
        isSelected = false;
        UpdateSprite();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateSprite();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateSprite();
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        UpdateSprite();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (hasText)
        {
            inputFieldBackground.sprite = selectedSprite;
        }
        else
        {
            inputFieldBackground.sprite = isHovered || isSelected ? selectedSprite : normalSprite;
        }
    }
}
*/