using UnityEngine;

namespace EscapeMaze.Maze {

    public class MazeCell {
        public Wall northWall;
        public Wall southWall;
        public Wall eastWall;
        public Wall westWall;

        public int row, col;

        public MazeCell(int row, int col) {
            northWall = new Wall();
            southWall = new Wall();
            eastWall = new Wall();
            westWall = new Wall();
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
            
            public Wall() {
                enabled = true;
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