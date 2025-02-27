using TMPro;

namespace Assets.UI.GameScene
{
    public interface IUIGameplayPanel
    {
        TextMeshProUGUI PanelText { get; }
        void MoveIntoView();
        void MoveOutOfView();
        void PositionOutOfView(bool leftDirection);
    }
}