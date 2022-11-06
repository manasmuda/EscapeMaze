using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

public static class AssetReferenceUtils
{

    public static bool IsLoaded(this AssetReference aRef) {
        return aRef.IsValid() && aRef.Asset != null;
    }
   
    public static bool IsLoading(this AssetReference aRef) {
        return aRef.IsValid() && aRef.OperationHandle.Status == AsyncOperationStatus.None && aRef.Asset == null;
    }
    
    public static bool IsLoaded<T>(this AssetAddressReference<T> aRef) where T : Object{
        return aRef.IsValid() && aRef.Asset != null;
    }
   
    public static bool IsLoading<T>(this AssetAddressReference<T> aRef) where T : Object {
        return aRef.IsValid() && aRef.OperationHandle.Status == AsyncOperationStatus.None && aRef.Asset == null;
    }

    public static bool TryGetOrLoadObjectAsync<TObjectType>(this AssetReference aRef, out AsyncOperationHandle<TObjectType> handle) where TObjectType: Object {
        if(aRef == null || !aRef.RuntimeKeyIsValid()) {
            handle = Addressables.ResourceManager.CreateCompletedOperation(default(TObjectType), "Invalid Runtime key");
            return false;
        }
        if(IsLoaded(aRef)) {
            try {
                handle = aRef.OperationHandle.Convert<TObjectType>();
                Debug.Log("Asset Already available");
            } catch {
                handle = Addressables.ResourceManager.CreateCompletedOperation(default(TObjectType), "Loaded Asset Type and Requested asset type do not match");
            }

            return false;
        }


        if(IsLoading(aRef)) {
            try {
                handle = aRef.OperationHandle.Convert<TObjectType>();
                Debug.Log("Asset is loading");
            } catch {
                handle = handle = Addressables.ResourceManager.CreateCompletedOperation(default(TObjectType), "Loaded Asset Type and Requested asset type do not match");
            }
            return false;
        }


        handle = aRef.LoadAssetAsync<TObjectType>();

        return true;
    }
    
    public static async UniTask<TObjectType> TryGetOrLoadObjectAsync<TObjectType>(this AssetReference aRef) where TObjectType : Object {
        TObjectType asset = default(TObjectType);
        if(aRef == null || !aRef.RuntimeKeyIsValid()) {
            Debug.LogError("Invalid run time key");
            return null;
        }
        if(IsLoaded(aRef)) {
            try {
                asset = (TObjectType)aRef.Asset;
            } catch {
                Debug.LogError("Loaded Asset Type and Requested asset type do not match");
            }
        } else if(IsLoading(aRef)) {
            try {
                await aRef.OperationHandle.Convert<TObjectType>().Task;
                if(aRef.Asset != null) {
                    asset = (TObjectType)aRef.Asset;
                }
            } catch {
                Debug.LogError("Loaded Asset Type and Requested asset type do not match");
            }
        } else {
            AsyncOperationHandle<TObjectType> handle = aRef.LoadAssetAsync<TObjectType>();
            try {
                await handle;
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            if(aRef.Asset != null) {
                asset = (TObjectType)aRef.Asset;
            } else {
                Debug.LogError("Failed Loading " + aRef);
            }
        }
        return asset;
    }

    
    public static async UniTask<List<TObjectType>> TryGetOrLoadObjectsAsync<TObjectType>(this List<AssetReferenceT<TObjectType>> aRef) where TObjectType : Object {
        List<TObjectType> objectsList = new List<TObjectType>();
        for(int i = 0; i < aRef.Count; i++) {
            TObjectType loadedObject = await TryGetOrLoadObjectAsync<TObjectType>(aRef[i]);
            objectsList.Add(loadedObject);
        }
        return objectsList;
    }
    
    public static async UniTask<TObjectType> TryGetOrLoadObjectAsync<TObjectType>(this AssetAddressReference<TObjectType> aRef) where TObjectType : Object {
        TObjectType asset = default(TObjectType);
        if(aRef == null || !aRef.RuntimeKeyIsValid()) {
            Debug.LogError("Invalid run time key");
            return null;
        }
        if(IsLoaded(aRef)) {
            try {
                asset = (TObjectType)aRef.Asset;
            } catch {
                Debug.LogError("Loaded Asset Type and Requested asset type do not match");
            }
        } else if(IsLoading(aRef)) {
            try {
                await aRef.OperationHandle.Task;
                if(aRef.Asset != null) {
                    asset = (TObjectType)aRef.Asset;
                }
            } catch {
                Debug.LogError("Loaded Asset Type and Requested asset type do not match");
            }
        } else {
            var handle = aRef.LoadAssetAsync();
            try {
                await handle;
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            if(aRef.Asset != null) {
                asset = (TObjectType)aRef.Asset;
            } else {
                Debug.LogError("Failed Loading " + aRef);
            }
        }
        return asset;
    }

    
    public static async UniTask<GameObject> TryLoadAndInstantiateGameObject(this AssetReferenceGameObject assetReference, Transform parent, DiContainer container = null) {
        GameObject loadedPrefab = await TryGetOrLoadObjectAsync<GameObject>(assetReference);
        if (loadedPrefab != null) {
            GameObject instantiatedObject;
            if (container != null) {
                instantiatedObject = container.InstantiatePrefab(loadedPrefab, parent);
            } else {
                instantiatedObject = Object.Instantiate(loadedPrefab, parent);
            }
            AddNotifyOnDestroy(assetReference, instantiatedObject);
            return instantiatedObject;
        }
        return null;
    }
    
    public static async UniTask<GameObject> TryLoadAndInstantiateGameObject(this AssetAddressReference<GameObject> assetReference, Transform parent = null, DiContainer container = null) {
        GameObject loadedPrefab = await TryGetOrLoadObjectAsync(assetReference);
        if (loadedPrefab != null) {
            GameObject instantiatedObject;
            if (container != null) {
                instantiatedObject = container.InstantiatePrefab(loadedPrefab, parent);
            } else {
                if (parent != null) {
                    instantiatedObject = Object.Instantiate(loadedPrefab, parent);
                } else {
                    instantiatedObject = Object.Instantiate(loadedPrefab);
                }
            }
            AddNotifyOnDestroy(assetReference, instantiatedObject);
            return instantiatedObject;
        }
        return null;
    }

    private static void AddNotifyOnDestroy(AssetReference assetReference, GameObject gameObject) {
        NotifyOnDestroy notifyOnDestroy = gameObject.GetComponent<NotifyOnDestroy>();
        if(notifyOnDestroy == null) {
            notifyOnDestroy = (NotifyOnDestroy)gameObject.AddComponent(typeof(NotifyOnDestroy));
            notifyOnDestroy.OnDestroyAction = assetReference.Unload;
        }
    }
    
    private static void AddNotifyOnDestroy(AssetAddressReference<GameObject> assetReference, GameObject gameObject) {
        NotifyOnDestroy notifyOnDestroy = gameObject.GetComponent<NotifyOnDestroy>();
        if(notifyOnDestroy == null) {
            notifyOnDestroy = (NotifyOnDestroy)gameObject.AddComponent(typeof(NotifyOnDestroy));
            notifyOnDestroy.OnDestroyAction = assetReference.Unload;
        }
    }

    public static void Unload(this AssetReference aRef) {
        if(IsLoaded(aRef) || IsLoading(aRef) || aRef.IsValid()) {
            aRef.ReleaseAsset();
        } else {
            Debug.LogWarning("Asset is not loaded or loading");
        }
    }
    
    public static void Unload(this AssetAddressReference<GameObject> aRef) {
        if(IsLoaded(aRef) || IsLoading(aRef) || aRef.IsValid()) {
            aRef.ReleaseAsset();
        } else {
            Debug.LogWarning("Asset is not loaded or loading");
        }
    }
}
