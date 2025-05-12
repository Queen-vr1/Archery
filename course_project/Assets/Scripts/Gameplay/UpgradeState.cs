using UnityEngine;

public class UpgradeState
{
    public int Time { get; private set; } = 0;
    public int Add_Balloon { get; private set; } = 0;
    public int Init_Points { get; private set; } = 0;
    public float Power_Up { get; private set; } = 1f;
	public float Balloon_Size { get; private set; } = 0;

	public void Apply (UpgradeType type)
	{
		switch (type)
		{
			case UpgradeType.Time:
				Time += 5;
				Debug.Log("Time: " + Time);
				break;

			case UpgradeType.Add_Balloon:
				Add_Balloon += 5;
				Debug.Log("Add_Balloon: " + Add_Balloon);
				break;

			case UpgradeType.Init_Points:
				Init_Points += 3;
				Debug.Log("Init_Points: " + Init_Points);
				break;

			case UpgradeType.Power_Up:
				Power_Up += 0.5f;
				Debug.Log("Power_Up: " + Power_Up);
				break;

			case UpgradeType.Balloon_Size:
				Balloon_Size += 0.05f;
				Debug.Log("Balloon_Size: " + Balloon_Size);
				break;
		}
	}

    public void Reset() 
    {
		Time = 0;
		Add_Balloon = 0;
		Init_Points = 0;
		Power_Up = 0;
		Balloon_Size = 0;
	}
}
