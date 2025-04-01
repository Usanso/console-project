using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textRPG.Scenes
{
    internal class ShopScene: Scene
    {
        public override void Render()
        {
            Game.PrintInfo();
            Console.WriteLine("\"어서오세요~\"");
            Console.WriteLine("\"좋은 물건 많습니다~\"");
            Console.WriteLine("상점에는 다양한 물건들이 늘어져 있다.");
            Console.WriteLine("어떤 물건을 구매하시겠습니까?");
        }

        public override void Choice()
        {
            Console.WriteLine("1. 누가봐도 수상한 저주받은 것 같은 구슬을 산다.");
            Console.WriteLine("2. 상인에게서 신발을 구매한다.");
            Console.WriteLine("3. 상인을 위협하고 돈을 갈취 시도한다.");
            Console.WriteLine("4. 마을로 돌아갑니다.");
        }

        public override void Result()
        {
            switch (input)
            {
                case ConsoleKey.D2:
                    Console.WriteLine("신발을 구매합니다.");
                    Console.WriteLine("착용하니 당신의 다리가 가벼워지는 것을 느낍니다.");
                    Game.Player.Speed += 3;
                    Console.WriteLine("플레이어 스탯 상승! 속도 : {0}", Game.Player.Speed);
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("당신은 상인을 위협하고 돈을 내놓으라 소리쳤습니다...");
                    Console.WriteLine("하지만 상인이 당신보다 더 레벨이 높았습니다.");
                    Console.WriteLine("상인이 휘두른 공격에 당신은 한방에 나가 떨어졌습니다...");
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("당신은 마을로 돌아갑니다...");
                    break;
                default:
                    Console.WriteLine("잘못 입력하셨습니다. 다시 입력해주세요.");
                    break;
            }
        }

        public override void Wait()
        {
            Console.WriteLine("계속하려면 아무키나 입력하세요.");
            Console.ReadKey();
        }

        public override void Next()
        {
            switch (input)
            {
                case ConsoleKey.D3:
                    Game.GameOver("레벨이 낮으면서 함부로 행동하지 맙시다...");
                    break;
                case ConsoleKey.D4:
                    Game.ChangeScene("Town");
                    break;
            }
        }
    }
}
