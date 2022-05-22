using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] private int _startingBalance = 1000;
    [SerializeField] private int _currentBalance;
    [SerializeField] TextMeshProUGUI _displayBalance;

    public int CurrentBalance { get { return _currentBalance; } }

    private void Awake()
    {
        _currentBalance = _startingBalance;
        UpdateDisplay();
    }

    public void Deposit(int amount)
    {
        if (amount < 1)
        {
            Debug.LogError("Deposit not accepted. Amount must be a positive integer.");
            return;
        }

        _currentBalance += amount;
        UpdateDisplay();
    }

    public void Withdraw(int amount)
    {
        if (amount < 1)
        {
            Debug.LogError("Withdrawal not accepted. Amount must be a positive integer.");
            return;
        }

        _currentBalance -= amount;
        UpdateDisplay();

        if (_currentBalance < 0)
        {
            // Lose the game
            Debug.Log("You ran out of gold. Reloading.");
            ReloadScene();
        }
    }

    private void UpdateDisplay()
    {
        _displayBalance.text = "Gold: " + _currentBalance;
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}