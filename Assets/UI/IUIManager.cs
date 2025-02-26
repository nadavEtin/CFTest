using Assets.Infrastructure;
using Assets.Infrastructure.Factories;
using UnityEngine;

namespace GameCore.UI
{
    public interface IUIManager
    {
        void Init(AssetRefsScriptableObject assetRefs, Camera mainMacera, IFactoriesManager factoriesManager, int targetScore, int startingMoves, int startingTime);
        void ShowEndGamePopup(bool win);
    }
}