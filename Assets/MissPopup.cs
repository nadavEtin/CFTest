using Assets.Infrastructure.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MissPopup : MonoBehaviour, IPooledObject
{
    [SerializeField] private TextMeshProUGUI _text;

    private RectTransform _rectTransform;
    private float _animationDuration, _moveUpDistance, _posOffset;

    public System.Action<GameObject> SendToPoolCB { get; set; }

    public void ReturnToPool()
    {
        SendToPoolCB?.Invoke(gameObject);
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animationDuration = 1.5f;
        _moveUpDistance = 35f;
        _posOffset = 50f;
    }

    public void ShowPopup(Vector2 targetPos)
    {
        gameObject.SetActive(true);
        _text.alpha = 1;
        var offSetPos = new Vector2(targetPos.x, targetPos.y + _posOffset);
        _rectTransform.position = offSetPos;
        float randomRotation = Random.Range(-20f, 20f);
        _rectTransform.rotation = Quaternion.Euler(0, 0, randomRotation);

        _rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + _moveUpDistance, _animationDuration)
                .SetEase(Ease.OutQuad);

        _text.DOFade(0, _animationDuration)
              .SetEase(Ease.Linear)
              .OnComplete(() => ReturnToPool());
    }
}
