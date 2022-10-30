using Cysharp.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Maze {

    public class MazeInitializer : MonoBehaviour {

        public NavMeshSurface mazeNavMeshSurface;

        [Inject] private DiContainer _container;
        [Inject] private MazeSettings _mazeSettings;

        private void Awake() {
            MazeGenerator mazeGenerator = new MazeGenerator(_mazeSettings.mazeSize);
            MazeCell[,] mazeCells = mazeGenerator.GenerateMaze();
            MazeSettings.MazePreset preset = _mazeSettings.GetRandomMazePreset();
            InstantiateMaze(mazeCells, preset.prefabObject);
            BuildNavMesh();
        }

        private async void BuildNavMesh() {
            await UniTask.WaitForEndOfFrame(this);
            mazeNavMeshSurface.BuildNavMesh();
        }

        private void InstantiateMaze(MazeCell[,] maze, GameObject mazePrefab) {

            for (int i = 0; i < maze.GetLength(0); i++) {
                for (int j = 0; j < maze.GetLength(1); j++) {
                    float cellx = 6 * j - 57;
                    float celly = 2.0f;
                    float cellz = 57 - 6 * i;

                    if (maze[i, j].northWall) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellx, celly, cellz + 3f),
                            Quaternion.Euler(270, 180, 0), transform);
                    }

                    if (maze[i, j].southWall) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellx, celly, cellz - 3f),
                            Quaternion.Euler(270, 0, 0), transform);
                    }

                    if (maze[i, j].eastWall) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellx + 3f, celly, cellz),
                            Quaternion.Euler(270, 270, 0), transform);
                    }

                    if (maze[i, j].westWall) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellx - 3f, celly, cellz),
                            Quaternion.Euler(270, 90, 0), transform);
                    }
                }
            }
        }

    }
}
