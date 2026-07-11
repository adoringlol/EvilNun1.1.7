using TMPro;
using UnityEngine;

namespace DG.Tweening
{
	public static class ShortcutExtensionsTextMeshProUGUI
	{
		public static Tweener DOColor(this TextMeshProUGUI target, Color endValue, float duration)
		{
			return DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOFaceColor(this TextMeshProUGUI target, Color32 endValue, float duration)
		{
			return DOTween.To(() => target.faceColor, delegate(Color x)
			{
				target.faceColor = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOOutlineColor(this TextMeshProUGUI target, Color32 endValue, float duration)
		{
			return DOTween.To(() => target.outlineColor, delegate(Color x)
			{
				target.outlineColor = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOGlowColor(this TextMeshProUGUI target, Color endValue, float duration, bool useSharedMaterial = false)
		{
			return (!useSharedMaterial) ? target.fontMaterial.DOColor(endValue, "_GlowColor", duration).SetTarget(target) : target.fontSharedMaterial.DOColor(endValue, "_GlowColor", duration).SetTarget(target);
		}

		public static Tweener DOFade(this TextMeshProUGUI target, float endValue, float duration)
		{
			return DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOFaceFade(this TextMeshProUGUI target, float endValue, float duration)
		{
			return DOTween.ToAlpha(() => target.faceColor, delegate(Color x)
			{
				target.faceColor = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOScale(this TextMeshProUGUI target, float endValue, float duration)
		{
			Transform t = target.transform;
			return DOTween.To(endValue: new Vector3(endValue, endValue, endValue), getter: () => t.localScale, setter: delegate(Vector3 x)
			{
				t.localScale = x;
			}, duration: duration).SetTarget(target);
		}

		public static Tweener DOFontSize(this TextMeshProUGUI target, float endValue, float duration)
		{
			return DOTween.To(() => target.fontSize, delegate(float x)
			{
				target.fontSize = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOMaxVisibleCharacters(this TextMeshProUGUI target, int endValue, float duration)
		{
			return DOTween.To(() => target.maxVisibleCharacters, delegate(int x)
			{
				target.maxVisibleCharacters = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOText(this TextMeshProUGUI target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
		{
			return DOTween.To(() => target.text, delegate(string x)
			{
				target.text = x;
			}, endValue, duration).SetOptions(richTextEnabled, scrambleMode, scrambleChars).SetTarget(target);
		}
	}
}
