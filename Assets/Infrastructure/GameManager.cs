using Assets.GameplayObjects.Balls;
using Assets.Infrastructure.Factories;
using GameCore.ScriptableObjects;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    private void SetupGameplayScene()
    {
        var assetRefs = Resources.Load<AssetRefsScriptableObject>("AssetRefs");
        var factoriesManager = new FactoriesManager();
        factoriesManager.Init(assetRefs);
        var ballSpawner = FindObjectOfType<BallSpawner>();
        ballSpawner.Init(factoriesManager);
    }
}
