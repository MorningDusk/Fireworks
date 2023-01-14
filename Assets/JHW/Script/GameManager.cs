using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    // 게임오버 변수
    [SerializeField]
    private bool isGameOver = false;
    
    // getter
    public bool getGameOver() { return isGameOver; }

    // setter
    public void setGameOver(bool flag) { isGameOver = flag; }

}
