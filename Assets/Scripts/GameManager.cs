using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, Subject
{
    // 1. Singleton Pattern: Instance() method
    private static GameManager _instance;

    public static GameManager Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // 초기화 설정 바꾸지 말 것
    private int _gameRound = 0;
    private string _whoseTurn = "Enemy";
    private bool _isEnd = false;

    // delegate: TurnHandler, FinishHandler 선언
    private delegate void TurnHandler(int round, string turn);
    private event TurnHandler _turnHandler;

    private delegate void FinishHandler(bool isFinish);
    private event FinishHandler _finishHandler; 


    /// <summary>
    /// 2. RoundNotify:
    /// 1) 현재 턴이 Enemy이면 다음 gameRound로
    ///  + Debug.Log($"GameManager: Round {gameRound}.");
    /// 2) TurnNotify() 호출
    /// </summary>
    public void RoundNotify()
    {
        if (_whoseTurn == "Enemy")
        {
            _gameRound += 1;
        }

        TurnNotify();

        Debug.Log($"GameManager: Round {_gameRound}.");
    }

    /// <summary>
    /// 3. TurnNotify:
    /// 1) whoseTurn update
    ///  + Debug.Log($"GameManager: {_whoseTurn} turn.");
    /// 2) _turnHandler 호출
    /// </summary>
    public void TurnNotify()
    {
        if (_whoseTurn == "Enemy")
        {
            _whoseTurn = "Player";
        }
        else
        {
            _whoseTurn = "Enemy";
        }

        _turnHandler(_gameRound, _whoseTurn);

        Debug.Log($"GameManager: {_whoseTurn} turn.");
    }

    /// <summary>
    /// 4. EndNotify: 
    /// 1) isEnd update
    ///  + Debug.Log("GameManager: The End");
    ///  + Debug.Log($"GameManager: {_whoseTurn} is Win!");
    /// 2) _finishHandler 호출
    /// </summary>
    public void EndNotify()
    {
        _isEnd = true;
        _finishHandler(true);

        Debug.Log("GameManager: The End");
        Debug.Log($"GameManager: {_whoseTurn} is Win!");
    }

    // 5. AddCharacter: _turnHandler, _finishHandler 각각에 메소드 추가
    public void AddCharacter(Character character)
    {
        _turnHandler += character.TurnUpdate;
        _finishHandler += character.FinishUpdate;
    }
}
