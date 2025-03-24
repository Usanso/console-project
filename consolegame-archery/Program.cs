using System;
using System.Threading;

class RealTimeGame
{
    static bool isRunning = true;
    static int playerX = 10, playerY = 5;
    static int hp = 100, attack = 10;
    static int screenHeight = 20;
    static int infoHeight = 5;
    static int screenWidth = 50; // 논리적 가로 크기
    static int visualWidth = screenWidth * 2; // 실제 출력할 때는 2배로 확장
    static Random rand = new Random();

    static void Main()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(visualWidth, screenHeight + infoHeight);
        Console.SetBufferSize(visualWidth, screenHeight + infoHeight);

        Thread inputThread = new Thread(InputHandler);
        inputThread.Start();

        while (isRunning)
        {
            Update();
            Render();
            Thread.Sleep(100);
        }
    }

    static void InputHandler()
    {
        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.W: if (playerY > 0) playerY--; break;
                    case ConsoleKey.S: if (playerY < screenHeight - 1) playerY++; break;
                    case ConsoleKey.A: if (playerX > 0) playerX--; break;
                    case ConsoleKey.D: if (playerX < screenWidth - 1) playerX++; break;
                    case ConsoleKey.Escape: isRunning = false; break;
                }
            }
        }
    }

    static void Update()
    {
        if (rand.Next(100) < 5)
        {
            hp -= 5;
        }
    }

    static void Render()
    {
        Console.Clear();

        // ====== 동적 게임 화면 (위쪽) ======
        Console.SetCursorPosition(playerX * 2, playerY);
        Console.Write("▒▒"); // 가로를 2배로 확장하여 출력

        // ====== 정적 정보 화면 (아래쪽) ======
        Console.SetCursorPosition(0, screenHeight);
        Console.WriteLine(new string('─', visualWidth));

        Console.SetCursorPosition(0, screenHeight + 1);
        Console.WriteLine($"HP: {hp}  공격력: {attack}");

        Console.SetCursorPosition(0, screenHeight + 2);
        Console.WriteLine("[WASD] 이동  [ESC] 종료");
    }
}
