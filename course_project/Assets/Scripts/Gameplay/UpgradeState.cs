using UnityEngine;

public class UpgradeState
{
    public int Time { get; private set; } = 0;
    public int Add_Balloon { get; private set; } = 0;
    public int Init_Points { get; private set; } = 0;
    public float Power_Up { get; private set; } = 1f;
	public float Balloon_Size { get; private set; } = 0f;
	public float Speed_Arrow { get; private set; } = 0f;

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
				Init_Points += 5;
				Debug.Log("Init_Points: " + Init_Points);
				break;

			case UpgradeType.Power_Up:
				Power_Up += 1f;
				Debug.Log("Power_Up: " + Power_Up);
				break;

			case UpgradeType.Balloon_Size:
				Balloon_Size += 0.05f;
				Debug.Log("Balloon_Size: " + Balloon_Size);
				break;

			case UpgradeType.Speed_Arrow:
				Speed_Arrow += 5;
				Debug.Log("Speed_Arrow: " + Speed_Arrow);
				break;
		}
	}

    public void Reset() 
    {
		Time = 0;
		Add_Balloon = 0;
		Init_Points = 0;
		Power_Up = 1;
		Balloon_Size = 0;
		Speed_Arrow = 0;
	}
}
