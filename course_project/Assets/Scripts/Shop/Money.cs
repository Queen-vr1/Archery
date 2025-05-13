using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Money : MonoBehaviour
{
	public TextMeshProUGUI moneyText;

	private int lastMoney = -1;
	private float animationTime = 0.5f;
	private float scale = 1.2f;

	void Update()
    {
		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.Money != lastMoney)
			{
				moneyText.text = $"{GameManager.Instance.Money}€";
				AnimateText(moneyText);
				lastMoney = GameManager.Instance.Money;
			}
		}
	}

	private void AnimateText(TextMeshProUGUI text)
	{
		StartCoroutine(AnimateTextCoroutine(text));
	}

	private IEnumerator AnimateTextCoroutine(TextMeshProUGUI text)
	{
		Vector3 originalScale = text.transform.localScale;
		Vector3 targetScale = originalScale * scale;

		float elapsed = 0f;
		while (elapsed < animationTime)
		{
			float t = elapsed / animationTime;
			text.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
			elapsed += Time.deltaTime;
			yield return null;
		}
		text.transform.localScale = targetScale;

		elapsed = 0f;
		while (elapsed < animationTime)
		{
			float t = elapsed / animationTime;
			text.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
			elapsed += Time.deltaTime;
			yield return null;
		}
		text.transform.localScale = originalScale;
	}
}
