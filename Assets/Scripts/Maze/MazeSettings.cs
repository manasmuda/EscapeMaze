using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Maze {

    public class MazeSettings : ScriptableObjectInstaller<MazeSettings> {

        public List<MazePreset> mazePresets;
        public int mazeSize;

        public override void InstallBindings() {
            Container.BindInstance(this).AsSingle();
        }

        public MazePreset GetRandomMazePreset() {
            return mazePresets.GetRandomElement();
        }
        
        [Serializable]
        public class MazePreset {
            public GameObject prefabObject;
            public List<Material> sharedMaterials;
        }

    }
}
