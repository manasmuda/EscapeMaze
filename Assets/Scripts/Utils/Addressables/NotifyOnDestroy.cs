using System;
using UnityEngine;

public class NotifyOnDestroy : MonoBehaviour {

    public Action OnDestroyAction;

    public void OnDestroy()
    {
        OnDestroyAction?.Invoke();
    }
}