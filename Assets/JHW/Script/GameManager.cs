using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    // ���ӿ��� ����
    [SerializeField]
    private bool isGameOver = false;
    
    // getter
    public bool getGameOver() { return isGameOver; }

    // setter
    public void setGameOver(bool flag) { isGameOver = flag; }

}
