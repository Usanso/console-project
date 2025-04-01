using textRPG.Scenes;

namespace textRPG
{
    /// <summary>
    /// 게임만들기
    /// 
    /// 1. 인트로
    /// 2. 게임 설정 및 시스템
    ///     - 불러오기? 콘솔에서 불러오기
    ///     - 저장시 코드를 주고, 불러오기시 입력하면 가능은 할듯
    /// 3. 게임
    ///     1) 게임시작
    ///     2) 상황부여
    ///     3) 선택지
    ///     4) 선택지 선택
    ///     5) 결과
    ///     6) 다음 상황
    /// 
    /// 많이 써보고 파악하기.
    /// 
    /// </summary>
    internal class Program 
    {
        static void Main(string[] args)
        {
            Game.Start();
            Game.Run();
            Game.End();
        }

    }
}
