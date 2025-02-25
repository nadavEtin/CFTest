using UnityEngine;

namespace Assets.UI.GameScene
{
    public class UIMovesPanel : MonoBehaviour
    {

        [SerializeField] private UISlideAnimation _slideAnimation;
        [SerializeField] private RectTransform _rectTransform;

        private void Awake()
        {
            //_rectTransform = GetComponent<RectTransform>();
            
            Vector2 panelSize = _rectTransform.rect.size;
            var inViewPos = _rectTransform.anchoredPosition;
            var outOfViewXPos = inViewPos.x + panelSize.x * 2;
            var outOfViewPos = new Vector2(outOfViewXPos, inViewPos.y);
            //_slideAnimation.Init(outOfViewPos, inViewPos);
        }
    }
}