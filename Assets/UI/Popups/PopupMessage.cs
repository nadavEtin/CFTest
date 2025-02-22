using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;

    public TextMeshProUGUI PopupText;

    private void Start()
    {
        _confirmButton.onClick.AddListener(ConfirmButtonClick);
    }

    private void ConfirmButtonClick()
    {
        gameObject.SetActive(false);
    }
}
