using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    public TextMeshProUGUI PopupText;

    [SerializeField] private Button _confirmButton;
    [SerializeField] private RectTransform _rectTransform;

    private Tween _scaleTween;
    private float animationDuration = 1.5f;
    private Vector3 _normalScale = new Vector3(1, 1, 1);
    private Vector3 _smallScale = new Vector3(0.01f, 0.01f, 0.01f);

    public void ShowWindow()
    {
        // Kill any existing animation
        if (_scaleTween != null && _scaleTween.IsPlaying())
            _scaleTween.Kill();

        // Set initial scale
        _rectTransform.localScale = _smallScale;

        // Animate to full scale
        _scaleTween = _rectTransform.DOScale(_normalScale, animationDuration)
            .SetEase(Ease.OutBack); // You can change the ease type for different effects
    }

    public void HideWindow()
    {
        // Kill any existing animation
        if (_scaleTween != null && _scaleTween.IsPlaying())
            _scaleTween.Kill();

        // Set initial scale
        _rectTransform.localScale = _normalScale;

        // Animate back to small scale
        _scaleTween = _rectTransform.DOScale(_smallScale, animationDuration)
            .SetEase(Ease.InBack) // You can change the ease type for different effects
            .OnComplete(() => gameObject.SetActive(false)); // Optionally hide the window when animation completes
    }

    public void ConfirmButtonClick()
    {
        HideWindow();
    }
}
