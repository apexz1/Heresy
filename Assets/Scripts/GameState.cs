using UnityEngine;
using System.Collections;

public class GameState {

    private static int _turnCount;
    private static int _turn;
    private static int _phase;

    //placeholder variables, need 2 sets for both players later yo
    private static int _res1;
    private static int _res2;
    private static int _res3;
    private static int _res4;

    private static int _godState;

    void Start()
    {

    }

    //Sets player turns and phases, controlled via GameManager
    public void SetState()
    {
        if (_phase >= 3)
        {
            _turnCount++;
            _turn = _turnCount % 2;
            _phase = 0;
        }

        else
        {
            _phase++;
        }
    }

    //TENTATIVE || Adds Resources to player; modify based on mechanics
    public void AddResource(int res)
    {
        switch (res)
        {
            case 1:
                _res1++;
                break;

            case 2:
                _res2++;
                break;

            case 3:
                _res3++;
                break;

            case 4:
                _res4++;
                break;
        }
    }
    //Constructor
    public GameState()
    {
        _turnCount = 1;
        _turn = 1;
        _phase = 0;

        _res1 = 0;
        _res2 = 0;
        _res3 = 0;
        _res4 = 0;

        _godState = 0;
    }

    //Get/Set Armada
    public int GetCount()
    {
        return _turnCount;
    }

    public int GetTurn()
    {
        return _turn;
    }

    public int GetPhase()
    {
        return _phase;
    }
}
