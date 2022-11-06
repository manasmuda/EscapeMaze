using Cysharp.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace EscapeMaze.Maze {

    public class MazeInitializer : MonoBehaviour {

        public NavMeshSurface mazeNavMeshSurface;

        [Inject] private DiContainer _container;
        [Inject] private MazeSettings _mazeSettings;

        private MazeCell[,] _generatedMaze;

        public bool Initialized { get; private set; }

        public async UniTask InitializeMaze() {
            MazeGenerator mazeGenerator = new MazeGenerator(_mazeSettings.mazeSize);
            AdjustMazeScale();
            _generatedMaze = mazeGenerator.GenerateMaze();
            MazePreset preset = await _mazeSettings.GetRandomMazePreset();
            InstantiateMaze(_generatedMaze, preset.prefabObject);
            BuildNavMesh();
            Initialized = true;
            Debug.Log("Maze Initialized");
        }

        private void AdjustMazeScale() {
            float scale = _mazeSettings.mazeSize * MazeSettings.MazeWallSize;
            transform.localScale = new Vector3(scale, 1, scale);
        }

        private async void BuildNavMesh() {
            await UniTask.WaitForEndOfFrame(this);
            mazeNavMeshSurface.BuildNavMesh();
        }

        private void InstantiateMaze(MazeCell[,] maze, GameObject mazePrefab) {
            float mazeSize = MazeSettings.MazeWallSize;
            int mazeLengthX = maze.GetLength(0);
            int mazeLengthY = maze.GetLength(1);
            for (int i = 0; i < mazeLengthX; i++) {
                for (int j = 0; j < mazeLengthY; j++) {
                    float cellX = mazeSize * j - ((mazeSize * (mazeLengthY - 1))/2);
                    float cellY = 2.0f;
                    float cellZ = ((mazeSize * (mazeLengthX - 1))/2) - mazeSize * i;

                    if (maze[i, j].IsNorthWallEnabled()) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellX, cellY, cellZ + mazeSize/2),
                            Quaternion.Euler(270, 180, 0), transform);
                    }

                    if (maze[i, j].IsSouthWallEnabled()) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellX, cellY, cellZ - mazeSize/2),
                            Quaternion.Euler(270, 0, 0), transform);
                    }

                    if (maze[i, j].IsEastWallEnabled()) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellX + mazeSize/2, cellY, cellZ),
                            Quaternion.Euler(270, 270, 0), transform);
                    }

                    if (maze[i, j].IsWestWallEnabled()) {
                        _container.InstantiatePrefab(mazePrefab, new Vector3(cellX - mazeSize/2, cellY, cellZ),
                            Quaternion.Euler(270, 90, 0), transform);
                    }
                }
            }
        }

        public Vector3 GetRandomMazePosition() {
            int i = Random.Range(0, _mazeSettings.mazeSize);
            int j = Random.Range(0, _mazeSettings.mazeSize);
            return GetMazeCellCenterByIndex(i, j);
        }

        public Vector3 GetRandomMazePosition(MazeDirection direction) {
            int mazeOffset = Mathf.RoundToInt((float)_mazeSettings.mazeSize / 10f);
            mazeOffset = Mathf.Clamp(mazeOffset, 1, mazeOffset);
            int columnIndex = Random.Range(mazeOffset, _mazeSettings.mazeSize-mazeOffset);
            int rowIndex = Random.Range(0, mazeOffset); 
            int i,j;
            if (direction == MazeDirection.North) {
                i = rowIndex;
                j = columnIndex;
            } else if (direction == MazeDirection.East) {
                i = columnIndex;
                j = (_mazeSettings.mazeSize - 1) - rowIndex;
            } else if(direction == MazeDirection.South){
                i = (_mazeSettings.mazeSize - 1) - rowIndex;
                j = columnIndex;
            } else {
                i = columnIndex;
                j = rowIndex;
            }
            return GetMazeCellCenterByIndex(i, j);
        }

        private Vector3 GetMazeCellCenterByIndex(int i, int j) {
            float mazeSize = MazeSettings.MazeWallSize;
            int mazeLengthX = _generatedMaze.GetLength(0);
            int mazeLengthY = _generatedMaze.GetLength(1);
            float cellX = mazeSize * j - ((mazeSize * (mazeLengthY - 1))/2);
            float cellZ = ((mazeSize * (mazeLengthX - 1))/2) - mazeSize * i;
            return new Vector3(cellX, 0f, cellZ);
        }
        
        public MazeCell.Wall GetRandomMazeBorderWall(MazeDirection direction) {
            int mazeOffset = Mathf.RoundToInt((float)_mazeSettings.mazeSize / 2f);
            mazeOffset = Mathf.Clamp(mazeOffset, 1, mazeOffset);
            int columnIndex = 0;
            int rowIndex = Random.Range(0, mazeOffset);
            if (rowIndex == 0) {
                columnIndex = Random.Range(0, _mazeSettings.mazeSize);
            }

            MazeCell.Wall wall;
            int i,j;
            if (direction == MazeDirection.North) {
                i = rowIndex;
                j = columnIndex;
            } else if (direction == MazeDirection.East) {
                i = columnIndex;
                j = (_mazeSettings.mazeSize - 1) - rowIndex;
            } else if(direction == MazeDirection.South){
                i = (_mazeSettings.mazeSize - 1) - rowIndex;
                j = columnIndex;
            } else {
                i = columnIndex;
                j = rowIndex;
            }

            return GetBorderWallForCell(i, j);
        }
        
        private MazeCell.Wall GetBorderWallForCell(int i, int j) {
            int mazeSize = _mazeSettings.mazeSize;
            MazeCell cell = _generatedMaze[i, j];
            if (i == 0) {
                return cell.northWall;
            } else if(i == mazeSize - 1) {
                return cell.southWall;
            } else if(j == mazeSize - 1) {
                return cell.westWall;
            } else if(j == 0) {
                return cell.southWall;
            }
            Debug.LogError("Cell Does not contain any Maze Wall");
            return null;
        }

    }

    public enum MazeDirection {
        North = 1,
        South = 2,
        East = 3,
        West = 4
    }
}
