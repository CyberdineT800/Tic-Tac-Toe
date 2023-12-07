using System;
using System.Globalization;
using System.Text;

class Program
{
    static char[,] board = new char[3, 3]; /// must always be a square

    static char point = '■';
    static int point_x, point_y;

    static int board_height = board.GetLength(0);
    static int board_width = board.GetLength(1);

    static int player;
    static char[] characters;

    static StringBuilder border;

    static void PrintHelp()
    {
        Console.WriteLine("Help: \n" +
            "1) Can move the point using the direction keys: \n" +
            "         up_arrow -> up, left_arrow -> left, right_arrow -> right, down_arrow -> down\n" +
            "2) After coming to the challenge box, the corresponding character \n" +
            "         is entered by the player from the keyboard (if another \n" +
            "         character is entered, the program does not care) and confirmed by the \"enter\" key.\n" +
            "3) It is not possible to enter another value in a cell with a value, the program ignores this attempt.\n");

        var key = Console.ReadKey().Key;

        if (key != ConsoleKey.Enter)
        {
            Console.Clear();
            PrintHelp();
        }
    }

    static void CreateEmptyBoard()
    {
        for (int i = 0; i < board_height; i++)
            for (int j = 0; j < board_width; j++)
                board[i, j] = ' ';
    }

    static void CreateBorder()
    {
        for (int i = 0; i < board_height; i++)
            border.Append("---+");
    }

    static void PrintWinner ()
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Yellow;
        
        Console.WriteLine("<---------- GAME OVER ! ---------->");
        Console.WriteLine("<---------- Player " + player + " won ! ---------->");

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void PrintDraw()
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("<---------- DRAW ! ---------->");

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void PrintBoard(int player, int point_x, int point_y)
    {
        Console.Clear();
        Console.WriteLine("\n" + border);

        for (int i = 0; i < board.GetLength(0); i++)
        {
            Console.Write("| ");

            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (i == point_x && j == point_y)
                {
                    if (board[i, j] == ' ')
                        Console.Write(point);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(board[i, j]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    if (board[i, j] == 'X')
                        Console.ForegroundColor = ConsoleColor.Red;
                    else 
                        Console.ForegroundColor = ConsoleColor.Green;

                    Console.Write(board[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(" | ");
            }

            Console.WriteLine("\n" + border);
        }

        Console.WriteLine("\nPlayer " + player + " -> " + characters[player - 1]);
    }

    static void ChangePlayer()
    {
        player = player == 2 ? 1 : 2;
    }

    static bool CheckRows ()
    {
        char player_char = characters[player - 1];
        int count;

        for (int i = 0; i < board_height; i++)
        {
            count = 0;

            for (int j = 0; j < board_width; j++)
                if (board[i, j] == player_char)
                    count++;

            if (count == board_height)
                return true;
        }

        return false;
    }

    static bool CheckColumns()
    {
        char player_char = characters[player - 1];
        int count;

        for (int i = 0; i < board_width; i++)
        {
            count = 0;

            for (int j = 0; j < board_height; j++)
                if (board[j, i] == player_char)
                    count++;

            if(count == board_width)
                return true;
        }

        return false;
    }

    static bool CheckMainDiag ()
    {
        char player_char = characters[player - 1];
        int count = 0;

        for (int i = 0; i < board_width; i++)
            if (board[i, i] == player_char)
                count++;

        return count == board_width;
    }

    static bool CheckSecondaryDiag()
    {
        char player_char = characters[player - 1];
        int count = 0;

        for (int i = 0; i < board_width; i++)
            if (board[i, board_width - i - 1] == player_char)
                count++;

        return count == board_width;
    }

    static bool IsWinner ()
    {
        bool result = CheckColumns() || CheckRows() || CheckMainDiag() || CheckSecondaryDiag();
        return result;
    }

    static bool IsFull ()
    {
        for (int i = 0; i < board_height; i++)
            for (int j = 0; j < board_width; j++)
                if (board[i, j] == ' ')
                    return false;

        return true;
    }

    static string ReadKey()
    {
        var key = Console.ReadKey();

        switch(key.Key)
        {
            case ConsoleKey.UpArrow:
                return "up";
            case ConsoleKey.RightArrow:
                return "right";
            case ConsoleKey.DownArrow:
                return "down";
            case ConsoleKey.LeftArrow:
                return "left";
            case ConsoleKey.X:
                return "x";
            case ConsoleKey.O:
                return "o";
            case ConsoleKey.Enter:
                return "enter";
        }

        return String.Empty;
    }

    static void AskCharacter ()
    {
        ConsoleKey ans;
        do
        {
            Console.Clear();
            Console.WriteLine("Player " + player + " : ");

            do
            {
                Console.Write("\nChoose O or X: ");
                ans = Console.ReadKey().Key;

                if (ans == ConsoleKey.X)
                    characters = ['X', 'O'];
                else if (ans == ConsoleKey.O)
                    characters = ['O', 'X'];
            }
            while (ans != ConsoleKey.O && ans != ConsoleKey.X);

            Thread.Sleep(300);
            Console.Clear();

            Console.WriteLine("Player 1 : " + characters[player - 1]);
            Console.WriteLine("Player 2 : " + characters[player]);

            Console.Write("Do you agree to continue? (y / n) : ");
            ans = Console.ReadKey().Key;

            if (ans == ConsoleKey.Y)
            {
                Console.WriteLine("\nOK, Get ready!");
                Thread.Sleep(1000);

                Console.Clear();
                break;
            }
        }
        while (ans != ConsoleKey.Y);
    }

    static bool MovePointOrChangePlayer(string key_result)
    {
        switch (key_result)
        {
            case "up":
                {
                    point_x = Math.Abs(point_x - 1 + board_height) % board_height;
                    PrintBoard(player, point_x, point_y);
                    
                    break;
                }
            case "right":
                {
                    point_y = (point_y + 1) % board_width;
                    PrintBoard(player, point_x, point_y);

                    break;
                }
            case "down":
                {
                    point_x = (point_x + 1) % board_height;
                    PrintBoard(player, point_x, point_y);

                    break;
                }
            case "left":
                {
                    point_y = Math.Abs(point_y - 1 + board_width) % board_width;
                    PrintBoard(player, point_x, point_y);

                    break;
                }
            case "o":
                {
                    if (characters[player - 1] == 'O' && board[point_x, point_y] == ' ')
                    {
                        board[point_x, point_y] = 'O';
                        PrintBoard(player, point_x, point_y);

                        Console.WriteLine("You clicked 'O'. Press 'enter' to confirm");
                        string key = ReadKey();
                        if (key != "enter")
                        {
                            board[point_x, point_y] = ' ';
                            PrintBoard(player, point_x, point_y);
                        }
                        else
                        {
                            Console.WriteLine("Confirmed");

                            bool is_won = IsWinner();
                            if (is_won)
                            {
                                PrintWinner();
                                return true;
                            }

                            is_won = IsFull();
                            if (is_won)
                            {
                                PrintDraw();
                                return true;
                            }

                            ChangePlayer();
                        }

                        //key_result = ReadKey();
                        //MovePointOrChangePlayer(key_result);
                    }

                    break;
                }
            case "x":
                {
                    if (characters[player - 1] == 'X' && board[point_x, point_y] == ' ')
                    {
                        board[point_x, point_y] = 'X';
                        PrintBoard(player, point_x, point_y);

                        Console.WriteLine("You clicked 'X'. Press 'enter' to confirm");
                        string key = ReadKey();
                        if (key != "enter")
                        {
                            board[point_x, point_y] = ' ';
                            PrintBoard(player, point_x, point_y);
                        }
                        else
                        {
                            Console.WriteLine("Confirmed");

                            bool is_won = IsWinner();
                            if (is_won)
                            {
                                PrintWinner();
                                return true;
                            }

                            is_won = IsFull();
                            if (is_won)
                            {
                                PrintDraw();
                                return true;
                            }

                            ChangePlayer();
                        }

                        //key_result = ReadKey();
                        //MovePointOrChangePlayer(key_result);
                    }

                    break;
                }
        }

        return false;
    }

    static void Game()
    {
        string key_result;

        while (true)
        {
            key_result = ReadKey();
            bool find_winner = MovePointOrChangePlayer(key_result);

            if (find_winner)
            {
                ConsoleKey ans;

                do
                {
                    Console.Write("Do you agree to continue? (y / n) : ");
                    ans = Console.ReadKey().Key;

                    if (ans == ConsoleKey.Y)
                    {
                        Console.WriteLine("\nOK, Get ready!");
                        Thread.Sleep(1000);

                        Run();
                        break;
                    }
                    else if (ans == ConsoleKey.N)
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("THANKS! BYE BYE!!!");
                        Console.ForegroundColor = ConsoleColor.White;
                        
                        return;
                    }
                }
                while (ans != ConsoleKey.Y && ans != ConsoleKey.N);
            }
        }
    }

    static void Run()
    {
        point_x = 0;
        point_y = 0;
        player = 1;

        border = new StringBuilder("+");
        CreateBorder();

        PrintHelp();
        AskCharacter();
        CreateEmptyBoard();
        PrintBoard(player, point_x, point_y);

        Game();
    }

    static void Main(string[] args)
    {
        Run();
    }
}