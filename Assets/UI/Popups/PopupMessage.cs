using Assets.Infrastructure.Events;
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
    private float animationDuration = 0.75f;
    private Vector3 _normalScale = new Vector3(1, 1, 1);
    private Vector3 _smallScale = new Vector3(0.01f, 0.01f, 0.01f);

    public void ShowWindow()
    {
        if (_scaleTween != null && _scaleTween.IsPlaying())
            _scaleTween.Kill();

        _rectTransform.localScale = _smallScale;

        _scaleTween = _rectTransform.DOScale(_normalScale, animationDuration)
            .SetEase(Ease.OutBack);
    }

    public void HideWindow()
    {
        if (_scaleTween != null && _scaleTween.IsPlaying())
            _scaleTween.Kill();

        _rectTransform.localScale = _normalScale;

        _scaleTween = _rectTransform.DOScale(_smallScale, animationDuration)
            .SetEase(Ease.InSine)
            .OnComplete(() => HideAnimationDone()); //animation finish callback
    }

    public void ConfirmButtonClick()
    {
        HideWindow();
    }

    private void HideAnimationDone()
    {
        gameObject.SetActive(false);
        EventManager.Instance.Publish(TypeOfEvent.GameStart, new EmptyParams());
    }
}
