using System;
using System.Threading;

namespace Snake
{
    class Program
    {
        /// <summary>
        /// Funzione per ascoltare i tasti inseriti da tastiera
        /// </summary>
        /// <param name="snake"></param>
        static void KeyListener(Snake snake)
        {
            while (true)
            {
                // Se un tasto è stato premuto
                if (Console.KeyAvailable)
                {
                    // Controllo se è una delle 4 frecce
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow: snake.currentDirection = direction.UP;
                            break;
                        case ConsoleKey.DownArrow: snake.currentDirection = direction.DOWN;
                            break;
                        case ConsoleKey.LeftArrow: snake.currentDirection = direction.LEFT;
                            break;
                        case ConsoleKey.RightArrow: snake.currentDirection = direction.RIGHT;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Funzione per spostare la posizione del frutto in una non occupata da qualcos'altro
        /// </summary>
        /// <param name="fruit"></param>
        /// <param name="board"></param>
        /// <param name="L"></param>
        static void MoveFruit(Fruit fruit, string[,] board, int L)
        {
            Random rd = new Random();
            int x = rd.Next(1, L - 1);
            int y = rd.Next(1, L - 1);
            if(board[x, y] != " ")
            {
                // Ricursione :o
                MoveFruit(fruit, board, L);
                return;
            }
            fruit.SetXY(x, y);
        }
        /// <summary>
        /// Funzione per eseguire i calcoli delle posizioni successive e renderizzarle
        /// </summary>
        /// <param name="board"></param>
        /// <param name="snake"></param>
        /// <param name="fruit"></param>
        static void Render(string[,] board, Snake snake, Fruit fruit)
        {
            // Pulizia della console
            Console.Clear();
            // lato della matrice
            int side = (int)Math.Sqrt(board.Length);
            // Impostazioni barriere di gioco
            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    if (i == 0 || i == side - 1)
                    {
                        board[i, j] = "═";
                    }
                    else if(j == 0 || j == side - 1)
                    {
                        board[i, j] = "║";
                    }
                    else
                    {
                        board[i, j] = " ";
                    }
                }
            }
            // Impostazione angoli
            board[0, 0] = "╔";
            board[0, side - 1] = "╗";
            board[side - 1, 0] = "╚";
            board[side - 1, side -1] = "╝";


            // Cambio posizione segmenti dello snake tranne per la testa
            for(int i = snake.segments.Count - 1; i > 0; i--)
            {
                snake.segments[i].SetXY(snake.segments[i - 1]);
            }

            // Aggiornamento posizione testa snake, è un po' più complicata degli altri segmenti
            // perchè deve anche cambiare direzione
            switch (snake.currentDirection)
            {
                case direction.UP: snake.segments[0].y += 1;
                    snake.oldDirection = direction.UP;
                    break;
                case direction.DOWN: snake.segments[0].y -= 1;
                    snake.oldDirection = direction.DOWN;
                                    break;
                case direction.LEFT: snake.segments[0].x -= 1;
                    snake.oldDirection = direction.LEFT;
                                    break;
                case direction.RIGHT: snake.segments[0].x += 1;
                    snake.oldDirection = direction.RIGHT;
                                    break;
            }
            // Se il frutto non è già stato mangiato lo renderizzo già
            if (!fruit.isEaten)
            {
                board[fruit.x, fruit.y] = Fruit.fruitChar;
            }
            // Scrittura posizione snake
            for(int i = 0; i < snake.segments.Count; i++)
            {
                // Controllo se tocca robe
                if(board[side - snake.segments[i].y, snake.segments[i].x] != " " )
                {
                    // Se tocca il frutto il serpente si allunga di un segmento
                    if(board[side - snake.segments[i].y, snake.segments[i].x] == Fruit.fruitChar)
                    {
                        snake.segments.Add(new Segment(snake.segments[snake.segments.Count - 1]));
                        fruit.isEaten = true;
                    }
                    // Controllo perché quando si allunga si tocca da solo
                    else if(!fruit.isEaten)
                    {
                        snake.isAlive = false;
                    }
                }
                // Impostazione della matrice della testa o corpod dello snake
                board[side - snake.segments[i].y, snake.segments[i].x] = i == 0 ? Snake.head : Snake.body;

            }

            // Se il frutto è stato mangiato dobbiamo rilocarlo
            if (fruit.isEaten)
            {
                MoveFruit(fruit, board, side);
                fruit.isEaten = false;
                board[fruit.x, fruit.y] = Fruit.fruitChar;
            }
            // Stampa della matrice
            for(int i = 0; i < side; i++)
            {
                for(int j = 0; j < side; j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            // L = lato della matrice
            const int L = 25;
            Snake snake = new Snake();
            Fruit fruit = new Fruit(0, 0);
            fruit.isEaten = true;
            string[,] board = new string[L, L];
            // Thread per il rendering
            new Thread(() =>
            {
                do
                {
                    Render(board, snake, fruit);
                    if (snake.isAlive)
                    {
                        Thread.Sleep(500);
                    }
                } while (snake.isAlive);
                Console.WriteLine("GAME OVER!");
            }).Start();
            //Thread per gestire l'input
            new Thread(() =>
            {
                KeyListener(snake);
            }).Start();
            // Impostazioni console
            Console.Title = "Snake";
            Console.SetWindowSize(30, 26);
        }
    }
}
