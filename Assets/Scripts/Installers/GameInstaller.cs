using System.Collections;
using System.Collections.Generic;
using EscapeMaze.Maze;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game {

    public class GameInstaller : MonoInstaller {

        public MazeInitializer mazeInitializer;

        public override void InstallBindings() {

            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

            Container.BindInstance(mazeInitializer).AsSingle();

            Debug.Log("Game Installed");
        }
    }
    
    
}