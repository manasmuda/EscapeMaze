using UnityEngine;
using Zenject;

namespace EscapeMaze {

   public class ProjectInstaller : MonoInstaller {

      [SerializeField] private ApplicationManager applicationManager;

      public override void InstallBindings() {
         SignalBusInstaller.Install(Container);

         Container.BindInstance(applicationManager).AsSingle();
      }
   }
}