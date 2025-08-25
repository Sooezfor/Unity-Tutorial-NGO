using TMPro;
using UnityEngine;

public class ScoreManager1 : MonoBehaviour
{
    int score;
    [SerializeField] TextMeshProUGUI scoreUi;

    private void Awake()
    {
        scoreUi.text = $"add Minerals count : {score}";
    }
    public void AddScore()
    {
        score++;
        scoreUi.text = $"add Minerals count : {score}";
    }
}
