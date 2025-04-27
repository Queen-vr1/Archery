using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStarter : MonoBehaviour
{
	private void Start()
	{
		if (GameManager.Instance.CurrentState == GameState.Playing)
		{
			RoundManager roundManager = GetComponent<RoundManager>();
			if (roundManager != null)
			{
				roundManager.StartRound();
			}
			else
			{
				Debug.LogWarning("RoundManager don't exist");
			}
		}
	}
}
