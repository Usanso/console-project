using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textRPG.Scenes
{
    public abstract class Scene
        
    {
        protected ConsoleKey input;
        public abstract void Render(); // 상황 설명
        public abstract void Choice(); // 선택지 제시
        public void Input() // 선택지 입력
        {
            input = Console.ReadKey(true).Key;
        }
        public abstract void Result(); // 선택지에 따른 결과 출력
        public abstract void Wait(); // 대기
        public abstract void Next(); // 다음상황으로 전환
    }
}
