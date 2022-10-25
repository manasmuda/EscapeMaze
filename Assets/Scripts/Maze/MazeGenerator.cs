using System.Collections.Generic;
using UnityEngine;

namespace Maze {

    public class MazeGenerator {

        private MazeCell[,] _randMaze;
        private bool[,] _visited;

        private bool _mazeComplete;

        private int _currentRow;
        private int _currentColumn;

        private static int CellSize = 20;

        public MazeCell[,] GenerateMaze() {
            _randMaze = new MazeCell[CellSize, CellSize];
            _visited = new bool[CellSize, CellSize];

            for (int i = 0; i < CellSize; i++) {
                for (int j = 0; j < CellSize; j++) {
                    _randMaze[i, j] = new MazeCell(i, j);
                }
            }

            _mazeComplete = false;

            while (!_mazeComplete) {
                CreatePath();
                SearchPath();
            }

            Debug.Log("Basic Maze Created");
            for (int i = 0; i < CellSize; i++) {
                for (int j = 0; j < CellSize; j++) {
                    int direction = Random.Range(1, 10);
                    if (direction > 4)
                        continue;
                    if (direction == 1 && i > 0) {
                        _randMaze[i, j].northWall = false;
                        _randMaze[i - 1, j].southWall = false;
                    } else if (direction == 2 && i < CellSize - 1) {
                        _randMaze[i, j].southWall = false;
                        _randMaze[i + 1, j].northWall = false;
                    } else if (direction == 3 && j < CellSize - 1) {
                        _randMaze[i, j].eastWall = false;
                        _randMaze[i, j + 1].westWall = false;
                    } else if (direction == 4 && j > 0) {
                        _randMaze[i, j].westWall = false;
                        _randMaze[i, j - 1].eastWall = false;
                    }

                }
            }

            Debug.Log("Modified Maze Created");
            bool redBase = false;
            bool blueBase = false;
            for (int i = 0; i < CellSize - 1; i++) {
                if (!redBase) {
                    if (!_randMaze[0, i].eastWall && !_randMaze[0, i + 1].westWall) {
                        redBase = true;
                    }
                }

                if (!blueBase) {
                    if (!_randMaze[CellSize - 1, i].eastWall && !_randMaze[CellSize - 1, i + 1].westWall) {
                        blueBase = true;
                    }
                }

                if (redBase && blueBase) {
                    break;
                }
            }

            Debug.Log("Maze Base Checked");
            if (!redBase) {
                _randMaze[0, CellSize - 2].eastWall = false;
                _randMaze[0, CellSize - 1].westWall = false;
            }

            if (!blueBase) {
                _randMaze[CellSize - 1, 0].eastWall = false;
                _randMaze[CellSize - 1, 1].westWall = false;
            }

            Debug.Log("Maze Generation finished");

            return _randMaze;
        }

        private void SearchPath() {
            _mazeComplete = true;
            for (int i = 0; i < CellSize; i++) {
                for (int j = 0; j < CellSize; j++) {
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
            //List<int> checkedDirections = new List<int> { };
            while (RouteStillAvailable(_currentRow, _currentColumn)) {
                int direction = Random.Range(1, 5);
                //while (checkedDirections.Contains(direction))
                //{
                //  direction = Random.Range(1, 5);
                //}
                //checkedDirections.Add(direction);
                //int direction = ProceduralNumberGenerator.GetNextNumber();

                if (direction == 1 && CellIsAvailable(_currentRow - 1, _currentColumn)) {
                    // North
                    _randMaze[_currentRow, _currentColumn].northWall = false;
                    _randMaze[_currentRow - 1, _currentColumn].southWall = false;
                    //visited[currentRow, currentColumn] = true;
                    _currentRow--;
                    //checkedDirections = new List<int> { };
                } else if (direction == 2 && CellIsAvailable(_currentRow + 1, _currentColumn)) {
                    // South
                    _randMaze[_currentRow, _currentColumn].southWall = false;
                    _randMaze[_currentRow + 1, _currentColumn].northWall = false;
                    //visited[currentRow, currentColumn] = true;
                    _currentRow++;
                    //checkedDirections = new List<int> { };
                } else if (direction == 3 && CellIsAvailable(_currentRow, _currentColumn + 1)) {
                    // east
                    _randMaze[_currentRow, _currentColumn].eastWall = false;
                    _randMaze[_currentRow, _currentColumn + 1].westWall = false;
                    //visited[currentRow, currentColumn] = true;
                    _currentColumn++;
                    //checkedDirections = new List<int> { };
                } else if (direction == 4 && CellIsAvailable(_currentRow, _currentColumn - 1)) {
                    // west
                    _randMaze[_currentRow, _currentColumn].westWall = false;
                    _randMaze[_currentRow, _currentColumn - 1].eastWall = false;
                    //visited[currentRow, currentColumn] = true;
                    _currentColumn--;
                    //checkedDirections = new List<int> { };
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
            if (row < CellSize - 1 && _visited[row + 1, column]) {
                visitedCells++;
            }

            // Look one row left (west) if we're column 1 or greater
            if (column > 0 && _visited[row, column - 1]) {
                visitedCells++;
            }

            // Look one row right (east) if we're the second-to-last column (or less)
            if (column < CellSize - 1 && _visited[row, column + 1]) {
                visitedCells++;
            }

            // return true if there are any adjacent visited cells to this one
            return visitedCells > 0;
        }

        private void DestroyAdjacentWall(int row, int column) {
            bool wallDestroyed = false;

            while (!wallDestroyed) {
                int direction = Random.Range(1, 5);
                //int direction = ProceduralNumberGenerator.GetNextNumber();

                if (direction == 1 && row > 0 && _visited[row - 1, column]) {
                    _randMaze[row, column].northWall = false;
                    _randMaze[row - 1, column].southWall = false;
                    wallDestroyed = true;
                } else if (direction == 2 && row < CellSize - 1 && _visited[row + 1, column]) {
                    _randMaze[row, column].southWall = false;
                    _randMaze[row + 1, column].northWall = false;
                    wallDestroyed = true;
                } else if (direction == 3 && column > 0 && _visited[row, column - 1]) {
                    _randMaze[row, column].westWall = false;
                    _randMaze[row, column - 1].eastWall = false;
                    wallDestroyed = true;
                } else if (direction == 4 && column < CellSize - 1 && _visited[row, column + 1]) {
                    _randMaze[row, column].eastWall = false;
                    _randMaze[row, column + 1].westWall = false;
                    wallDestroyed = true;
                }
            }

        }

        private bool RouteStillAvailable(int row, int column) {
            int availableRoutes = 0;

            if (row > 0 && !_visited[row - 1, column]) {
                availableRoutes++;
            }

            if (row < CellSize - 1 && !_visited[row + 1, column]) {
                availableRoutes++;
            }

            if (column > 0 && !_visited[row, column - 1]) {
                availableRoutes++;
            }

            if (column < CellSize - 1 && !_visited[row, column + 1]) {
                availableRoutes++;
            }

            return availableRoutes > 0;
        }

        private bool CellIsAvailable(int row, int column) {
            if (row >= 0 && row < CellSize && column >= 0 && column < CellSize && !_visited[row, column]) {
                return true;
            } else {
                return false;
            }
        }

        public static List<object> ToObjectList(MazeCell[,] mazeCells) {
            List<object> list = new List<object>();
            for (int i = 0; i < CellSize; i++) {
                for (int j = 0; j < CellSize; j++) {
                    string[] temparr = {"1", "1", "1", "1"};
                    if (!mazeCells[i, j].northWall) {
                        temparr[0] = "0";
                    }

                    if (!mazeCells[i, j].southWall) {
                        temparr[1] = "0";
                    }

                    if (!mazeCells[i, j].eastWall) {
                        temparr[2] = "0";
                    }

                    if (!mazeCells[i, j].westWall) {
                        temparr[3] = "0";
                    }

                    string temp = string.Join(",", temparr);
                    list.Add(temp);
                }
            }

            return list;
        }

    }

}
