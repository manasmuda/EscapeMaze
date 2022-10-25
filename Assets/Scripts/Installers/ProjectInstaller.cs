using UnityEngine;
using Zenject;

namespace EscapeMaze {

   public class ProjectInstaller : MonoInstaller {

      [SerializeField] private GameObject ApplicationManager;

      public override void InstallBindings() {
         SignalBusInstaller.Install(Container);

         Container.BindInstance(ApplicationManager).AsSingle();
      }
   }
}