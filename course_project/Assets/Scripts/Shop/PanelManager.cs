using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanelManager : MonoBehaviour
{
	[System.Serializable]
	public class InteractablePanelPair
	{
		public XRBaseInteractable interactable;
		public GameObject panel;
	}

	public List<InteractablePanelPair> pairs = new List<InteractablePanelPair>();

	void OnEnable()
	{
		foreach (var pair in pairs)
		{
			pair.interactable.hoverEntered.AddListener((args) => ShowPanel(pair.panel));
			pair.interactable.hoverExited.AddListener((args) => HidePanel(pair.panel));
		}
	}

	void OnDisable()
	{
		foreach (var pair in pairs)
		{
			pair.interactable.hoverEntered.RemoveAllListeners();
			pair.interactable.hoverExited.RemoveAllListeners();
		}
	}

	void ShowPanel(GameObject panel)
	{
		panel.SetActive(true);
	}

	void HidePanel(GameObject panel)
	{
		panel.SetActive(false);
	}
}
