using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesLoader : MonoBehaviour
{
    [SerializeField] private List<string> _addressableGroupNames;

    public List<AsyncOperationHandle> LoadAllAssets()
    {
        var operations = new List<AsyncOperationHandle>();

        foreach (string labelName in _addressableGroupNames)
        {
            //load each group/label in a separate operation
            var operation = Addressables.LoadAssetsAsync<GameObject>(labelName, null);
            operations.Add(operation);
        }

        return operations;
    }
}
