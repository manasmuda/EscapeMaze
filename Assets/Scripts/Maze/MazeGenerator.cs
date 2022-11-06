using UnityEngine;

namespace EscapeMaze.Maze {

    public class MazeGenerator {

        private MazeCell[,] _randMaze;
        private bool[,] _visited;

        private bool _mazeComplete;

        private int _currentRow;
        private int _currentColumn;

        private int _cellSize = 20;

        public MazeGenerator(int cellSize) {
            _cellSize = cellSize;
        }

        public MazeCell[,] GenerateMaze() {
            _randMaze = new MazeCell[_cellSize, _cellSize];
            _visited = new bool[_cellSize, _cellSize];

            for (int i = 0; i < _cellSize; i++) {
                for (int j = 0; j < _cellSize; j++) {
                    _randMaze[i, j] = new MazeCell(i, j);
                }
            }

            _mazeComplete = false;

            while (!_mazeComplete) {
                CreatePath();
                SearchPath();
            }

            Debug.Log("Basic Maze Created");
            for (int i = 0; i < _cellSize; i++) {
                for (int j = 0; j < _cellSize; j++) {
                    int direction = Random.Range(1, 10);
                    if (direction > 4)
                        continue;
                    if (direction == 1 && i > 0) {
                        _randMaze[i, j].northWall.Disable();
                        _randMaze[i - 1, j].southWall.Disable();
                    } else if (direction == 2 && i < _cellSize - 1) {
                        _randMaze[i, j].southWall.Disable();
                        _randMaze[i + 1, j].northWall.Disable();
                    } else if (direction == 3 && j < _cellSize - 1) {
                        _randMaze[i, j].eastWall.Disable();
                        _randMaze[i, j + 1].westWall.Disable();
                    } else if (direction == 4 && j > 0) {
                        _randMaze[i, j].westWall.Disable();
                        _randMaze[i, j - 1].eastWall.Disable();
                    }
                }
            }

            Debug.Log("Modified Maze Created");
            return _randMaze;
        }

        private void SearchPath() {
            _mazeComplete = true;
            for (int i = 0; i < _cellSize; i++) {
                for (int j = 0; j < _cellSize; j++) {
                    if (!_visited[i, j] && HasAdjVisited(i, j)) {
                        _mazeComplete = false; // Yep, we found something so definitely do another Kill cycle.
                        _currentRow = i;
                        _currentColumn = j;
                        DestroyAdjacentWall(_currentRow, _currentColumn);
                        _visited[_currentRow, _currentColumn] = true;
                        return;
                    }
                }
            }
        }

        private void CreatePath() {
            while (RouteStillAvailable(_currentRow, _currentColumn)) {
                int direction = Random.Range(1, 5);
                if (direction == 1 && CellIsAvailable(_currentRow - 1, _currentColumn)) {
                    // North
                    _randMaze[_currentRow, _currentColumn].northWall.Disable();
                    _randMaze[_currentRow - 1, _currentColumn].southWall.Disable();
                    _currentRow--;
                } else if (direction == 2 && CellIsAvailable(_currentRow + 1, _currentColumn)) {
                    // South
                    _randMaze[_currentRow, _currentColumn].southWall.Disable();
                    _randMaze[_currentRow + 1, _currentColumn].northWall.Disable();
                    _currentRow++;
                } else if (direction == 3 && CellIsAvailable(_currentRow, _currentColumn + 1)) {
                    // east
                    _randMaze[_currentRow, _currentColumn].eastWall.Disable();
                    _randMaze[_currentRow, _currentColumn + 1].westWall.Disable();
                    _currentColumn++;
                } else if (direction == 4 && CellIsAvailable(_currentRow, _currentColumn - 1)) {
                    // west
                    _randMaze[_currentRow, _currentColumn].westWall.Disable();
                    _randMaze[_currentRow, _currentColumn - 1].eastWall.Disable();
                    _currentColumn--;
                }

                _visited[_currentRow, _currentColumn] = true;
            }
        }

        private bool HasAdjVisited(int row, int column) {
            int visitedCells = 0;

            // Look 1 row up (north) if we're on row 1 or greater
            if (row > 0 && _visited[row - 1, column]) {
                visitedCells++;
            }

            // Look one row down (south) if we're the second-to-last row (or less)
            if (row < _cellSize - 1 && _visited[row + 1, column]) {
                visitedCells++;
            }

            // Look one row left (west) if we're column 1 or greater
            if (column > 0 && _visited[row, column - 1]) {
                visitedCells++;
            }

            // Look one row right (east) if we're the second-to-last column (or less)
            if (column < _cellSize - 1 && _visited[row, column + 1]) {
                visitedCells++;
            }

            // return true if there are any adjacent visited cells to this one
            return visitedCells > 0;
        }

        private void DestroyAdjacentWall(int row, int column) {
            bool wallDestroyed = false;
            while (!wallDestroyed) {
                int direction = Random.Range(1, 5);
                if (direction == 1 && row > 0 && _visited[row - 1, column]) {
                    _randMaze[row, column].northWall.Disable();
                    _randMaze[row - 1, column].southWall.Disable();
                    wallDestroyed = true;
                } else if (direction == 2 && row < _cellSize - 1 && _visited[row + 1, column]) {
                    _randMaze[row, column].southWall.Disable();
                    _randMaze[row + 1, column].northWall.Disable();
                    wallDestroyed = true;
                } else if (direction == 3 && column > 0 && _visited[row, column - 1]) {
                    _randMaze[row, column].westWall.Disable();
                    _randMaze[row, column - 1].eastWall.Disable();
                    wallDestroyed = true;
                } else if (direction == 4 && column < _cellSize - 1 && _visited[row, column + 1]) {
                    _randMaze[row, column].eastWall.Disable();
                    _randMaze[row, column + 1].westWall.Disable();
                    wallDestroyed = true;
                }
            }

        }

        private bool RouteStillAvailable(int row, int column) {
            int availableRoutes = 0;

            if (row > 0 && !_visited[row - 1, column]) {
                availableRoutes++;
            }

            if (row < _cellSize - 1 && !_visited[row + 1, column]) {
                availableRoutes++;
            }

            if (column > 0 && !_visited[row, column - 1]) {
                availableRoutes++;
            }

            if (column < _cellSize - 1 && !_visited[row, column + 1]) {
                availableRoutes++;
            }

            return availableRoutes > 0;
        }

        private bool CellIsAvailable(int row, int column) {
            if (row >= 0 && row < _cellSize && column >= 0 && column < _cellSize && !_visited[row, column]) {
                return true;
            } else {
                return false;
            }
        }
        
    }
}
