using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [Header("Ball References")]
    [SerializeField] private Ball ball;
    [Header("UI References")]
    [SerializeField] private TMP_Text player1HealthText;
    [SerializeField] private Image player1FillerBar;
    [SerializeField] private TMP_Text player2HealthText;
    [SerializeField] private Image player2FillerBar;
    [SerializeField] private TMP_Text livesText;
    [Header("Game Over References")]
    [SerializeField] private GameObject gameOverUI;
    [Header("Round Over References")]
    [SerializeField] private GameObject roundOverUI;
    [SerializeField] private TMP_Text roundFinished;
    [SerializeField] private TMP_Text playerScore;

    private int lives = 3;
    private int[] score = new int[] { 0, 0 };
    private string winner_name;
    private double winner_percent;

    private void OnEnable()
    {
        ball.OnScore += HandleScore;
        player1.OnLifeLost += CheckGameOver;
        player2.OnLifeLost += CheckGameOver;
    }

    private void OnDisable()
    {
        ball.OnScore -= HandleScore;
        player1.OnLifeLost -= CheckGameOver;
        player2.OnLifeLost -= CheckGameOver;
    }

    private void Start()
    {
        lives = 3;
        score[0] = 0;
        score[1] = 0;
        roundOverUI.SetActive(false);
        UpdateUI();
    }

    private void UpdateUI()
    {
        player1HealthText.text = $"P1 Health: {player1.GetHealth()}%";
        DOTweenUIManager.Instance.TweenFloatValue(player1FillerBar.fillAmount, player1.GetHealth() / 100, 2f, delegate (float x, bool y) { if (!y) { player1FillerBar.fillAmount = x; } });
        player2HealthText.text = $"P2 Health: {player2.GetHealth()}%";
        DOTweenUIManager.Instance.TweenFloatValue(player2FillerBar.fillAmount, player2.GetHealth() / 100, 2f, delegate (float x, bool y) { if (!y) { player2FillerBar.fillAmount = x; } });
        livesText.text = $"Round: {(3 - lives) + 1}";
    }

    private void RoundOverUI()
    {
        ball.m_Game_Running = false;
        ball.ResetBall();
        roundFinished.text = "Finished\n\nRound\n" + (3 - lives).ToString();
        playerScore.text = "P1 : " + player1.GetHealth() + "%" + "\n" + "P2 : " + player2.GetHealth() + "%";
        if (player1.GetHealth() > 0)
        {
            score[0]++;
        }
        else
        {
            score[1]++;
        }
        StopCoroutine(ShowRound());
        StartCoroutine(ShowRound());
    }

    private IEnumerator ShowRound()
    {
        roundOverUI.SetActive(true);
        roundOverUI.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        DOTweenUIManager.Instance.SlideInFromBottom(roundOverUI.GetComponent<RectTransform>(), 1f);
        roundOverUI.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        DOTweenUIManager.Instance.SlideInFromTop(roundOverUI.transform.GetChild(0).GetComponent<RectTransform>(), 0.5f);
        DOTweenUIManager.Instance.RotateUI(roundOverUI.transform.GetChild(0).GetComponent<RectTransform>(), 0.5f);
        yield return new WaitForSeconds(4f);
        roundOverUI.SetActive(false);
        player1.ResetHealth();
        player2.ResetHealth();
        UpdateUI();
        ball.m_Game_Running = true;
        ball.ResetBall();
    }

    private void HandleScore(string player)
    {
        Debug.Log("Handle Score Called...");
        if (player == "Player1")
        {
            player1.TakeDamage();
        }
        else
        {
            player2.TakeDamage();
        }

        UpdateUI();
    }

    private void CheckGameOver()
    {
        lives--;
        if (lives <= 0)
        {
            int index = score[0] > score[1] ? 0 : 1;
            GameOver(index == 0 ? "P1" : "P2", (score[index] / 3) * 100);
        }
        else
        {
            RoundOverUI();
        }
    }

    private void GameOver(string winnerText, double percent)
    {
        //gameOverUI.SetActive(true);
        //gameOverUI.GetComponentInChildren<Text>().text = winnerText;
        winner_name = winnerText;
        winner_percent = percent;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync("OverScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "OverScene")
        {
            var data_man = FindObjectOfType<GameDataManager>();
            if (data_man != null)
            {
                data_man.player_name = winner_name;
                data_man.percent = winner_percent;
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove listener
    }
}
