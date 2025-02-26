using Assets.Infrastructure.ObjectPool;
using GameCore.Factories;
using UnityEngine;

namespace Assets.Infrastructure.Factories
{
    public class ParticleEffectsFactory : BaseGameObjectFactory
    {
        public ParticleEffectsFactory(GameObject prefab, GameObject[] additionalPrefabs) : base(prefab, additionalPrefabs) { }

        public override GameObject[] Create(int amount, Transform parent = null)
        {
            var objects = new GameObject[amount];
            for (int i = 0; i < amount; i++)
            {
                if (objectPool.Count > 0)
                    objects[i] = objectPool.GetObjectFromPool();
                else
                {
                    //create a random pop effect from the options
                    int randomIndex = Random.Range(0, additionalPrefabs.Length);
                    var selectedEffect = additionalPrefabs[randomIndex];

                    objects[i] = Object.Instantiate(selectedEffect);
                    objects[i].GetComponent<IPooledObject>().SendToPoolCB = objectPool.AddObjectToPool;
                    if (parent != null)
                        objects[i].transform.SetParent(parent);
                }

            }
            return objects;
        }
    }
}
