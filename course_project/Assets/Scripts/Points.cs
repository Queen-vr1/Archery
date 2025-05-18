using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI successText;

    public int maxPoints = 10;
    private int points = 0;

    public static Points instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        successText.gameObject.SetActive(false);
        UpdatePointsText();
    }

    void UpdatePointsText()
    {
        pointsText.text = string.Format("{0}", points);
    }

    public void AddPoints(int pointsToAdd)
    {
        if (!Timer.isRunning) return;

        if (points + pointsToAdd > maxPoints) {
            points = maxPoints;

        } else {
            points += pointsToAdd;
        }

        UpdatePointsText();

        if (points >= maxPoints) {
            Timer.isRunning = false;
            successText.text = "Congratulations!\nYou have passed the round";
            successText.gameObject.SetActive(true);
        }
    }
}
