using Game.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class GameState
    {
        private int BorderStartIndex {  get; set; }
        public int BorderLenght { get; set; }

        private int rows, columns;
        public bool GameOver {  get; set; }
        public GridValue[,] Grid {  get; set; }

        public Coordinates BallCoordinates { get; set; }
        public Coordinates BorderCoordinates { get; set; }
        public Direction Direction { get; set; }

        public readonly int numTargetRows = 5;

        public GameState() { }

        public GameState(int rows, int columns, int BorderLenght) 
        {
            this.rows = rows;
            this.columns = columns;
            this.BorderLenght = BorderLenght;
            Grid = new GridValue[rows, columns];
            GameOver = false;
            initGrid();
            Direction = new Direction(-1, 1);
        }

        private void initGrid()
        {
            for (int i = 0; i < numTargetRows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Grid[i, j] = GridValue.Target;
                }
            }

            for (int i = numTargetRows; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    Grid[i, j] = GridValue.Empty;
                }
            }
            int index = rows / 2;

            index -= BorderLenght / 2;
            BorderStartIndex = index;
            BorderCoordinates = new Coordinates(index, rows-1);
            BallCoordinates = new Coordinates(BorderStartIndex + BorderLenght / 2, rows - 2);
            SetBorder(index);
            Grid[BallCoordinates.Y, BallCoordinates.X] = GridValue.Ball;
        }

        private void SetBorder(int index)
        {
            for (int i = 0; i < columns; i++)
            {
                Grid[rows - 1, i] = GridValue.Empty;
            }

            for (int i = index; i < index + BorderLenght; i++)
            {
                Grid[rows - 1, i] = GridValue.Border;
            }
        }

        public void MoveLeft()
        {
            if (BorderCoordinates.X <= 0)
                return;
            BorderCoordinates.X = BorderCoordinates.X - 1 ;
            SetBorder(BorderCoordinates.X);
            for(int i = 0; i < columns; i++)
            {
                if (Grid[rows - 2, i] == GridValue.Ball)
                {
                    Direction.ChangeDirectionLeft();
                    break;
                }
            }
        }

        public void MoveRight()
        {
            if (BorderCoordinates.X >= columns - BorderLenght)
                return;
            BorderCoordinates.X = BorderCoordinates.X + 1;
            SetBorder(BorderCoordinates.X);
            for (int i = 0; i < columns; i++)
            {
                if (Grid[rows - 2, i] == GridValue.Ball)
                {
                    Direction.ChangeDirectionRight();
                    break;
                }
            }
        }

        public void Move()
        {
            
            Grid[BallCoordinates.Y, BallCoordinates.X] = GridValue.Empty;
            Coordinates newCoor = BallCoordinates.Move(Direction);
            if(newCoor.X < 0 || newCoor.X >= columns)
            {
                Direction.ChangeHorizontal();
            }
            if (newCoor.Y < 0)
            {
                Direction.ChangeVertical();
            }

            

            if(newCoor.Y == rows - 1)
            {
                if(newCoor.X >= columns || newCoor.X < 0)
                    newCoor.X = BallCoordinates.X;
                if (Grid[newCoor.Y, newCoor.X] == GridValue.Border)
                    Direction.ChangeVertical();
                else
                {
                    Grid[newCoor.Y, newCoor.X] = GridValue.Ball;
                    GameOver = true;
                }
                   
            }
            
            if ((newCoor.Y >= 0 && newCoor.X >= 0 && newCoor.Y < rows && newCoor.X < columns && Grid[newCoor.Y, newCoor.X] == GridValue.Target) ||
                (newCoor.Y >= 0 && newCoor.X - Direction.Horizontal>= 0 && newCoor.Y < rows && newCoor.X - Direction.Horizontal < columns && Grid[newCoor.Y, newCoor.X - Direction.Horizontal] == GridValue.Target))
            {
                
                if (newCoor.Y >= 0 && newCoor.X - Direction.Horizontal >= 0 && newCoor.Y < rows && newCoor.X - Direction.Horizontal < columns)
                    Grid[newCoor.Y, newCoor.X - Direction.Horizontal] = GridValue.Empty;
                

                if ((newCoor.Y >= 0 && newCoor.X >= 0 && newCoor.Y < rows && newCoor.X < columns && Grid[newCoor.Y, newCoor.X] == GridValue.Empty) &&
                (newCoor.Y >= 0 && newCoor.X - Direction.Horizontal >= 0 && newCoor.Y < rows && newCoor.X - Direction.Horizontal < columns && Grid[newCoor.Y, newCoor.X - Direction.Horizontal] == GridValue.Target)) 
                {
                    
                    Direction.ChangeHorizontal();
                    Direction.ChangeVertical();
                }
                    
                Direction.ChangeVertical();
                Grid[newCoor.Y, newCoor.X] = GridValue.Empty;
            }

            if (newCoor.X >= 0 && newCoor.X < columns && Grid[BallCoordinates.Y, newCoor.X] == GridValue.Target)
            {
                Grid[BallCoordinates.Y, newCoor.X] = GridValue.Empty;
                Direction.ChangeHorizontal();
            }
                


            if (GameOver)
                return;
            Console.WriteLine(newCoor.Y + " " + newCoor.X);
            BallCoordinates = BallCoordinates.Move(Direction);
            Grid[BallCoordinates.Y, BallCoordinates.X] = GridValue.Ball;
        }

        
    }

}
