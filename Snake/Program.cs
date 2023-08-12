using System;


class Program
{
    // Параметры змейки
    static int headX = 60;
    static int headY = 20;
    static int direction = 0;
    static int previousDirection = 2;
    static int snakeLenght = 3;
    static int[] bodyX = new int[2145];
    static int[] bodyY = new int[2145];
    static int snakeSpeed = 100;

    // Параметры еды
    static int foodX;
    static int foodY;
    static Random rnd = new Random();
    static int speedBuffX = 0;
    static int speedBuffY = 39;
    static bool buffIsSpawned = false;
    static int BuffTimer = 0;
    static bool buffActive = false;

    // Общие параметры
    static bool isLose = false;
    static int score = 0;
    static int highscore = score;

    static void SpawnFood()
    {
        foodX = rnd.Next(2, 116);
        if (foodX % 2 == 1) foodX += 1;
        foodY = rnd.Next(1, 35);

        // Проверка на появление еды в змейке
        for (int i = 1; i < snakeLenght; i++)
        {
            if (foodX == bodyX[i] && foodY == bodyY[i])
            {
                while (true)
                {
                    foodX = rnd.Next(2, 116);
                    if (foodX % 2 == 1) foodX += 1;
                    foodY = rnd.Next(1, 35);
                    if (foodX != bodyX[i] && foodY != bodyY[i]) break;
                }
            }
        }
    }

    static void spawnSpeedBuff()
    {
        if (rnd.Next(0, 100) == 0)
        {
            speedBuffX = rnd.Next(2, 116);
            if (speedBuffX % 2 == 1) speedBuffX += 1;
            speedBuffY = rnd.Next(1, 35);

            // Проверка на появление баффа в змейке
            for (int i = 1; i < snakeLenght; i++)
            {
                if (speedBuffX == bodyX[i] && speedBuffY == bodyY[i])
                {
                    while (true)
                    {
                        foodX = rnd.Next(2, 116);
                        if (speedBuffX % 2 == 1) foodX += 1;
                        foodY = rnd.Next(1, 36);
                        if (speedBuffX != bodyX[i] && speedBuffY != bodyY[i]) break;
                    }
                }
            }
            buffIsSpawned = true;
        }
    }
    static void Main(string[] args)
    {
        // Параметры программы
        Console.SetWindowSize(120, 40);
        Console.SetBufferSize(120, 40);
        Console.CursorVisible = false;
        
        /*
        // Игровое поле
        for (int x = 0; x < 120; x++)
        {
            for (int y = 0; y < 39; y++)
            {
                if (x == 0 || x == 118 || x == 1 || x == 119 || y == 0 || y == 38)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.SetCursorPosition(x, y);
                    Console.Write("█");
                }
            }
        }
        */

        // Стартовое значение змейки
        for (int i = 0; i < snakeLenght; i++)
        {
            bodyX[i] = headX - (i * 2);
            bodyY[i] = 20;
        }

        // Стартовое значение еды
        SpawnFood();

        // Игровой цикл
        while (true)
        {
            // 1. Очистка
            Console.SetCursorPosition(headX, headY);
            Console.Write("  ");

            for (int i = 0; i < snakeLenght; i++)
            {
                Console.SetCursorPosition(bodyX[i], bodyY[i]);
                Console.Write("  ");
            }

            Console.SetCursorPosition(foodX, foodY);
            Console.Write("  ");

            Console.SetCursorPosition(speedBuffX, speedBuffY);
            Console.Write("  ");

            if (buffActive == false)
            {
                Console.SetCursorPosition(55, 39);
                Console.Write("             ");
            }

            // 2. Рассчёт

            //  Движение змейки
            if (Console.KeyAvailable == true)
            {
                ConsoleKeyInfo key;
                Console.SetCursorPosition(0, 39);
                key = Console.ReadKey();
                Console.SetCursorPosition(0, 39);
                Console.Write("  ");

                if (key.Key == ConsoleKey.Spacebar && isLose == false)
                {
                    previousDirection = direction;
                    direction = 0;
                }
                if (direction != 0) previousDirection = 0;
                if (key.Key == ConsoleKey.W && direction != 3 && isLose == false && previousDirection != 3) direction = 1;
                if (key.Key == ConsoleKey.D && direction != 4 && isLose == false && previousDirection != 4) direction = 2;
                if (key.Key == ConsoleKey.S && direction != 1 && isLose == false && previousDirection != 1) direction = 3;
                if (key.Key == ConsoleKey.A && direction != 2 && isLose == false && previousDirection != 2) direction = 4;
            }

            if (direction == 1) headY -= 1;
            if (direction == 2) headX += 2;
            if (direction == 3) headY += 1;
            if (direction == 4) headX -= 2;

            // Бесконечное поле
            if (headX == 118) headX = 2;
            if (headX == 0) headX = 116;
            if (headY == 38) headY = 1;
            if (headY == 0) headY = 37;

            // Движение туловища змейки
            if (direction != 0)
            {
                for (int i = snakeLenght; i > 0; i--)
                {
                    bodyX[i] = bodyX[i - 1];
                    bodyY[i] = bodyY[i - 1];
                }
                bodyX[0] = headX;
                bodyY[0] = headY;
            }

            // Поражение
            for (int i = 1; i < snakeLenght; i++)
            {
                if (headX == bodyX[i] && headY == bodyY[i]) isLose = true;
            }

            if (isLose == true)
            {
                direction = 0;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(57, 18);
                Console.Write("DEFEAT");
                Console.SetCursorPosition(50, 20);
                Console.Write("Press \"R\" to reset");

                while (true)
                {
                    ConsoleKeyInfo key;
                    Console.SetCursorPosition(0, 39);
                    key = Console.ReadKey();
                    Console.SetCursorPosition(0, 39);
                    Console.Write(" ");

                    if (key.Key == ConsoleKey.R)
                    {
                        Console.SetCursorPosition(57, 18);
                        Console.Write("      ");
                        Console.SetCursorPosition(50, 20);
                        Console.Write("                   ");
                        Console.SetCursorPosition(2, 39);
                        Console.Write("           ");
                        headX = 60;
                        headY = 20;
                        direction = 0;
                        previousDirection = 2;
                        snakeLenght = 3;
                        score = 0;
                        isLose = false;
                        buffActive = false;
                        snakeSpeed = 100;
                        BuffTimer = 0;

                        for (int i = 0; i < snakeLenght; i++)
                        {
                            bodyX[i] = headX - (i * 2);
                            bodyY[i] = 20;
                        }
                        break;
                    }
                    System.Threading.Thread.Sleep(50);
                }
            }

            // Еда
            if (foodX == headX && foodY == headY)
            {
                snakeLenght++;
                score++;
                if (score > highscore) highscore = score;
                SpawnFood();
            }

            if (buffIsSpawned == false) spawnSpeedBuff();

            if (speedBuffX == headX && speedBuffY == headY)
            {
                snakeSpeed = 50;
                buffIsSpawned = false;
                buffActive = true;
                speedBuffX = 0;
                speedBuffY = 39;
            }
            if (buffActive == true)
            {
                BuffTimer++;
                if (BuffTimer == 50)
                {
                    snakeSpeed = 100;
                    BuffTimer = 0;
                    buffActive = false;
                }
            }

            // 3. Отрисовка
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(2, 39);
            Console.Write("Score: " + score);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(14, 39);
            Console.Write("Highscore: " + highscore);

            if (buffActive == true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(55, 39);
                Console.Write("acceleration!");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(headX, headY);
            Console.Write("██");

            for (int i = 0; i < snakeLenght; i++)
            {
                Console.SetCursorPosition(bodyX[i], bodyY[i]);
                Console.Write("██");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(foodX, foodY);
            Console.Write("██");

            if (speedBuffX != 0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(speedBuffX, speedBuffY);
                Console.Write("██");
            }

            // 4. Ожидание
            System.Threading.Thread.Sleep(snakeSpeed);
        }
    }
}