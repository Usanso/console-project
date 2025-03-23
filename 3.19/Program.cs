using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace _3._19
{
    internal class Program
    {
        struct position // 플레이어 포지션
        {
            public int x;
            public int y;
            public char view; // 바라보는 방향 = 최근 이동 방향
        }

        enum mapTile // 지역을 숫자로 표현
        {
            Empty = 0,  // 빈 공간
            Wall = 1,   // 벽
            goal = 2    // 골 승리 조건
        }


        static void Main(string[] args)
        {
            bool gameOver = false;
            position playerPos;
            position goalPos;
            int[,] map;

            Start(out playerPos, out goalPos, out map);

            while (gameOver == false)
            {
                Render(map, playerPos);
                ConsoleKey key = Input();
                Update(key, ref playerPos, map, ref goalPos, ref gameOver);
            }

            End();
        }

        static void Start(out position playerPos, out position goalPos, out int[,] map)
        {   // 초기 설정
            Console.CursorVisible = false;

            // 플레이어 위치
            playerPos = new position { x = 1, y = 1, view = 'S' };
            goalPos = new position { x = 13, y = 8 };

            // 맵설정
            map = new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1 },
                { 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1 },
                { 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1 },
                { 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1 },
                { 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1 },
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 2, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("          게임을 시작합니다.\n         아무키나 눌러주세요.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadKey();
            Console.Clear();

            //1차 랜더
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    switch (map[y, x])
                    {
                        case 0:
                            Console.Write("  ");
                            break;
                        case 1:
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("  ");
                            Console.ResetColor();
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("G ");
                            Console.ResetColor();
                            break;

                    }

                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 11);
            Console.WriteLine("@@@@@@@                                                                           @@@@@@@");
            Console.WriteLine("@      @@@@@@                                                               @@@@@@      @");
            Console.WriteLine("@            @@@@@@                                                   @@@@@@            @");
            Console.WriteLine("@                  @@@@@@                                       @@@@@@                  @");
            Console.WriteLine("@                        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                        @");
            Console.WriteLine("@                            @                             @                            @");
            Console.WriteLine("@                            @                             @                            @");
            Console.WriteLine("@                            @                             @                            @");
            Console.WriteLine("@                        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                        @");
            Console.WriteLine("@                  @@@@@@                                       @@@@@@                  @");
            Console.WriteLine("@            @@@@@@                                                   @@@@@@            @");
            Console.WriteLine("@      @@@@@@                                                               @@@@@@      @");
            Console.WriteLine("@@@@@@@                                                                           @@@@@@@");
        }

        static void Render(int[,] map, position playerPos)
        {
            PersonView(map,playerPos);

            PrintPlayer(playerPos);

        }

        static void PersonView(int[,] map, position playerPos)
        {
            // 1인칭 시점으로 좌 앞 우 확인
            int[] view = viewCheck(map, playerPos);

            // 1인칭 시점 화면 출력
            RenderFirstPersonView(view);
        }

        static int[] viewCheck(int[,] map, position playerPos)
        {
            // 동서남북
            int east = playerPos.x + 1;
            int west = playerPos.x - 1;
            int south = playerPos.y + 1;
            int north = playerPos.y - 1;

            // 동서남북을 왼 중앙 오른으로 나누어 반환
            int left = 0;
            int center= 0;
            int right = 0;


            switch (playerPos.view)
            {
                case 'S': // 아래를 바라볼 때
                    left = map[playerPos.y, east];
                    center = map[south, playerPos.x];
                    right = map[playerPos.y, west];
                    break;

                case 'A': // 왼쪽을 바라볼 때
                    left = map[south, playerPos.x];
                    center = map[playerPos.y, west];
                    right = map[north, playerPos.x];
                    break;

                case 'W': // 위를 바라볼 때
                    left = map[playerPos.y, west];
                    center = map[north, playerPos.x];
                    right = map[playerPos.y, east];
                    break;

                case 'D': // 오른쪽을 바라볼 때
                    left = map[north, playerPos.x];
                    center = map[playerPos.y, east];
                    right = map[south, playerPos.x];
                    break;
            }
            return new int[] { left, center, right };
        }

        static void RenderFirstPersonView(int[] view)
        {

            Console.SetCursorPosition(12, 17);
            Console.WriteLine(view[0] == 1 ? "벽" : " "); // 왼쪽 벽 여부

            Console.SetCursorPosition(44, 17);
            Console.WriteLine(view[1] == 1 ? "벽" : " "); // 중앙 벽 여부

            Console.SetCursorPosition(77, 17);
            Console.WriteLine(view[2] == 1 ? "벽" : " "); // 오른쪽 벽 여부
        }


        static void PrintPlayer(position playerPos)
        {
            // 플레이어 렌더
            Console.SetCursorPosition(playerPos.x * 2, playerPos.y);
            Console.ForegroundColor = ConsoleColor.Red;

            switch (playerPos.view)
            {

                case 'S':
                    Console.Write("▼▼");
                    break;
                case 'A':
                    Console.Write("◀ ");
                    break;
                case 'W':
                    Console.Write("▲▲");
                    break;
                case 'D':
                    Console.Write("▶ ");

                    break;
            }
            Console.ResetColor();
        }

        static ConsoleKey Input()
        { // 키입력
            return Console.ReadKey(true).Key;
        }

        static void Update(ConsoleKey key, ref position playerPos, int[,] map, ref position goalPos, ref bool gameOver)
        { // 업데이트 변화 (이동이나 게임클리어 조건)

            Move(key, ref playerPos, map);

            if (goalPos.x == playerPos.x && goalPos.y == playerPos.y)
            {
                gameOver = true;
            }

        }



        static void Move(ConsoleKey key, ref position playerPos, int[,] map)
        {
            position targetPos = playerPos;

            switch (key)
            {
                case ConsoleKey.LeftArrow: // 시계반대 방향 회전
                    if (playerPos.view == 'W') playerPos.view = 'A';
                    else if (playerPos.view == 'A') playerPos.view = 'S';
                    else if (playerPos.view == 'S') playerPos.view = 'D';
                    else if (playerPos.view == 'D') playerPos.view = 'W';
                    return;
                case ConsoleKey.RightArrow: // 시계 방향 회전
                    if (playerPos.view == 'W') playerPos.view = 'D';
                    else if (playerPos.view == 'D') playerPos.view = 'S';
                    else if (playerPos.view == 'S') playerPos.view = 'A';
                    else if (playerPos.view == 'A') playerPos.view = 'W';
                    return;
                case ConsoleKey.UpArrow: // 앞으로 이동
                    if (playerPos.view == 'W') targetPos.y = playerPos.y - 1;
                    else if (playerPos.view == 'S') targetPos.y = playerPos.y + 1;
                    else if (playerPos.view == 'A') targetPos.x = playerPos.x - 1;
                    else if (playerPos.view == 'D') targetPos.x = playerPos.x + 1;
                    break;
                case ConsoleKey.DownArrow: // 뒤로 이동
                    if (playerPos.view == 'W') targetPos.y = playerPos.y + 1;
                    else if (playerPos.view == 'S') targetPos.y = playerPos.y - 1;
                    else if (playerPos.view == 'A') targetPos.x = playerPos.x + 1;
                    else if (playerPos.view == 'D') targetPos.x = playerPos.x - 1;
                    break;
                default:
                    return;
            }

            int targetTile = map[targetPos.y, targetPos.x];
            if (targetTile == 1) return;

            UpdateTile(playerPos, map);
            playerPos.x = targetPos.x;
            playerPos.y = targetPos.y;
        }

        static void UpdateTile(position playerPos, int[,] map)
        {
            // 특정 좌표의 타일만 변경하는 함수  (맵리셋 없이 변경하는것만 덮어 쓰도록 
            Console.SetCursorPosition(playerPos.x*2, playerPos.y); 
            Console.Write("  ");

        }
        static void End()
        { // 게임 끝
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("          축하합니다!!!\n게임을 성공적으로 클리어했습니다!");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
