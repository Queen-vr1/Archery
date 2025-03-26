using UnityEngine;

public class AddPoint : MonoBehaviour
{
    public void AddPoints(int pointsToAdd)
    {
        Points.instance.AddPoints(pointsToAdd);
    }

}
