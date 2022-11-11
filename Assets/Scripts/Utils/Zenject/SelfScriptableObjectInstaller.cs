using UnityEngine;
using Zenject;

public abstract class SelfScriptableObjectInstaller<T> : ScriptableObjectInstaller where T : SelfScriptableObjectInstaller<T>
{ 
    public override void InstallBindings() {
        Container.BindInstance((T)this).AsSingle();
    }
}
