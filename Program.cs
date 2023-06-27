using System;

public enum Symbol
{
    Empty,
    X,
    O
}

public class ConnectFour
{
    private const int Rows = 6;
    private const int Columns = 7;

    private Symbol[,] board;
    private Symbol currentPlayer;
    private bool isGameOver;

    public ConnectFour()
    {
        board = new Symbol[Rows, Columns];
        currentPlayer = Symbol.X;
        isGameOver = false;
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                board[row, col] = Symbol.Empty;
            }
        }
    }

    public void PlayMove(int column)
    {
        if (isGameOver)
        {
            Console.WriteLine("The game is already over. Please restart.");
            return;
        }

        if (column < 1 || column > Columns)
        {
            Console.WriteLine("Invalid column number. Please choose a column between 1 and 7.");
            return;
        }

        int col = column - 1;
        if (IsColumnFull(col))
        {
            Console.WriteLine("Selected column is already full. Please choose another column.");
            return;
        }

        int row = GetLowestEmptyRow(col);
        board[row, col] = currentPlayer;

        if (IsWinningMove(row, col))
        {
            isGameOver = true;
            Console.WriteLine($"Connect Four! {currentPlayer} wins!");
        }
        else if (IsBoardFull())
        {
            isGameOver = true;
            Console.WriteLine("It's a draw. The board is full.");
        }
        else
        {
            SwitchPlayer();
        }
    }

    private bool IsColumnFull(int col)
    {
        return board[0, col] != Symbol.Empty;
    }

    private int GetLowestEmptyRow(int col)
    {
        for (int row = Rows - 1; row >= 0; row--)
        {
            if (board[row, col] == Symbol.Empty)
            {
                return row;
            }
        }
        return -1; // Column is already full, should not happen if checked with IsColumnFull() beforehand.
    }

    private bool IsWinningMove(int row, int col)
    {
        return CheckHorizontal(row) || CheckVertical(row, col) || CheckDiagonal(row, col);
    }

    private bool CheckHorizontal(int row)
    {
        int count = 0;
        for (int col = 0; col < Columns; col++)
        {
            if (board[row, col] == currentPlayer)
            {
                count++;
                if (count == 4)
                {
                    return true;
                }
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    private bool CheckVertical(int row, int col)
    {
        int count = 0;
        for (int r = row; r < Rows; r++)
        {
            if (board[r, col] == currentPlayer)
            {
                count++;
                if (count == 4)
                {
                    return true;
                }
            }
            else
            {
                break;
            }
        }
        return false;
    }

    private bool CheckDiagonal(int row, int col)
    {
        return CheckDiagonal(row, col, -1, -1) || CheckDiagonal(row, col, -1, 1);
    }

    private bool CheckDiagonal(int row, int col, int rowIncrement, int colIncrement)
    {
        int count = 1;
        int r = row + rowIncrement;
        int c = col + colIncrement;

        while (r >= 0 && r < Rows && c >= 0 && c < Columns && board[r, c] == currentPlayer)
        {
            count++;
            r += rowIncrement;
            c += colIncrement;
        }

        rowIncrement *= -1;
        colIncrement *= -1;
        r = row + rowIncrement;
        c = col + colIncrement;

        while (r >= 0 && r < Rows && c >= 0 && c < Columns && board[r, c] == currentPlayer)
        {
            count++;
            r += rowIncrement;
            c += colIncrement;
        }

        return count >= 4;
    }

    private bool IsBoardFull()
    {
        for (int col = 0; col < Columns; col++)
        {
            if (board[0, col] == Symbol.Empty)
            {
                return false;
            }
        }
        return true;
    }

    private void SwitchPlayer()
    {
        currentPlayer = currentPlayer == Symbol.X ? Symbol.O : Symbol.X;
    }

    public void PrintBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                Console.Write(board[row, col] == Symbol.Empty ? "  " : $"{board[row, col]} ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("1 2 3 4 5 6 7");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        ConnectFour game = new ConnectFour();

        while (true)
        {
            game.PrintBoard();

            Console.WriteLine("Enter the column number (1-7) to drop your symbol:");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int column))
            {
                game.PlayMove(column);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid column number.");
            }

            Console.WriteLine("Restart? Yes (1) No (0):");
            string restartInput = Console.ReadLine();

            if (restartInput != "1")
            {
                break;
            }

            game = new ConnectFour();
        }
    }
}

