using Assets.Infrastructure.ObjectPool;
using GameCore.Factories;
using UnityEngine;

namespace Assets.Infrastructure.Factories
{
    public class BallFactory : BaseGameObjectFactory
    {
        public BallFactory(GameObject assetPrefab) : base(assetPrefab)
        {
        }

        public override GameObject[] Create(int amount, Transform parent = null)
        {
            var objects = new GameObject[amount];
            for (int i = 0; i < amount; i++)
            {
                if (objectPool.Count > 0)
                    objects[i] = objectPool.GetObjectFromPool();
                else
                {
                    objects[i] = Object.Instantiate(prefab);
                    objects[i].GetComponent<IPooledObject>().SendToPoolCB = objectPool.AddObjectToPool;
                    if (parent != null)
                        objects[i].transform.SetParent(parent);
                }

            }
            return objects;
        }
    }
}