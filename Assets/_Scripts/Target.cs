using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private Text _scoreText;

    public void Hit()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}
