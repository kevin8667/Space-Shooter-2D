using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;

    [SerializeField]
    UIManager _UIManager;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    public void AddScore(int addValue)
    {
        score += addValue;
        _UIManager.UpdateScore();
    }
}
