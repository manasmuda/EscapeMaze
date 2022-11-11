using UnityEngine;

namespace EscapeMaze.Maze {

    public class MazeCell {
        public Wall northWall;
        public Wall southWall;
        public Wall eastWall;
        public Wall westWall;

        public int row, col;

        public MazeCell(int row, int col) {
            northWall = new Wall(MazeDirection.North);
            southWall = new Wall(MazeDirection.South);
            eastWall = new Wall(MazeDirection.East);
            westWall = new Wall(MazeDirection.West);
            this.row = row;
            this.col = col;
        }

        public bool IsNorthWallEnabled() {
            return northWall.IsEnabled();
        }

        public bool IsSouthWallEnabled() {
            return southWall.IsEnabled();
        }

        public bool IsEastWallEnabled() {
            return eastWall.IsEnabled();
        }

        public bool IsWestWallEnabled() {
            return westWall.IsEnabled();
        }

        public class Wall {
            private bool enabled;
            private GameObject obj;
            public MazeDirection direction;
            
            public Wall(MazeDirection direction) {
                enabled = true;
                this.direction = direction;
            }

            public bool IsEnabled() {
                return enabled;
            }

            public void Disable() {
                enabled = false;
            }

            public void Enable() {
                enabled = true;
            }

            public void SetWallObj(GameObject wall) {
                obj = wall;
            }

            public GameObject GetWallObj() {
                return obj;
            }
        }

        
    }
}