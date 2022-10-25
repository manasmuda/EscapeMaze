namespace Maze {

    public class MazeCell {
        public bool northWall;
        public bool southWall;
        public bool eastWall;
        public bool westWall;

        public int row, col;

        public MazeCell(int row, int col) {
            northWall = true;
            southWall = true;
            eastWall = true;
            westWall = true;
            this.row = row;
            this.col = col;
        }
    }
}