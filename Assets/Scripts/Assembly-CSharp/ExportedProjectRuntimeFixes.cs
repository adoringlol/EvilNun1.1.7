using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Repairs references that can be lost when a Unity player is reconstructed
/// from serialized game data.
/// </summary>
public static class ExportedProjectRuntimeFixes
{
	private static TMP_FontAsset fallbackFont;

	internal static TMP_FontAsset FallbackFont
	{
		get { return fallbackFont; }
	}

	// The TMP SDF shaders were repaired, so native TextMesh Pro rendering works
	// (correct fonts, wrapping, and all languages incl. CJK). The legacy-Text mirror
	// is therefore OFF. If the shaders ever fail to compile, set this to false to
	// restore the mirror as a readable (English-only) fallback.
	internal static bool UseNativeText = true;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		// Run at a steady 60 FPS everywhere (Load screen + menus + gameplay), not just
		// in the gameplay scene where SimpleSmoothMouseLook also sets it.
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		fallbackFont = Resources.Load<TMP_FontAsset>("fonts & materials/LiberationSans SDF");
		// A single persistent host runs the text-mirror sweep, item spawner, and input fix.
		GameObject host = new GameObject("ExportedRuntimeServices");
		Object.DontDestroyOnLoad(host);
		host.AddComponent<ExportedTextFallbackDriver>();
		host.AddComponent<ExportedItemSpawner>();
		host.AddComponent<ExportedInputFieldFix>();
	}
}

/// <summary>
/// Drives the TextMesh Pro -> legacy text mirror. Sweeps on every scene load and
/// on a light repeating timer so labels that are spawned, enabled, or moved to
/// DontDestroyOnLoad after load are still caught (not just those present at load).
/// </summary>
public class ExportedTextFallbackDriver : MonoBehaviour
{
	private void Start()
	{
		ScheduleSweeps();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		ScheduleSweeps();
	}

	// Sweep now, then a few decaying follow-ups to catch labels that initialize a
	// frame or two after load (inactive ones are found too, so labels shown later
	// are already handled). This replaces a constant per-0.5s FindObjectsOfTypeAll,
	// which caused periodic lag spikes during play.
	private void ScheduleSweeps()
	{
		CancelInvoke();
		RepairAnimations();
		Invoke("RepairAnimations", 0.5f);
		Invoke("RepairAnimations", 2f);
		// Give localized labels room to shrink long translations (Vietnamese, German,
		// Russian). Runs regardless of the mirror toggle. Repeats so labels spawned or
		// enabled a frame or two after load (pop-ups, dynamically shown menus) are caught.
		FitLocalizedLabels();
		Invoke("FitLocalizedLabels", 0.5f);
		Invoke("FitLocalizedLabels", 2f);
		// With native TMP rendering restored the legacy-Text mirror is disabled by
		// default (it can't do wrapping or non-Latin fonts). Only run it as a fallback.
		if (!ExportedProjectRuntimeFixes.UseNativeText)
		{
			Sweep();
			Invoke("Sweep", 0.5f);
			Invoke("Sweep", 1.5f);
			Invoke("Sweep", 3f);
		}
	}

	// AssetRipper deduplicated colliding clip names by appending "_0" to the clip's
	// internal name (run_0, open_0, walk_0), but the game plays "run"/"open"/"walk"
	// on the legacy Animation components, so those states aren't found. Register each
	// clip under the expected name at runtime. Doing it in code (not by editing the
	// .anim m_Name) means it survives re-exporting/reverting the AnimationClip assets.
	private void RepairAnimations()
	{
		Animation[] anims = Resources.FindObjectsOfTypeAll<Animation>();
		for (int i = 0; i < anims.Length; i++)
		{
			Animation anim = anims[i];
			if (anim == null || !anim.gameObject.scene.IsValid())
			{
				continue;
			}
			AliasClip(anim, "run_0", "run");
			AliasClip(anim, "open_0", "open");
			AliasClip(anim, "walk_0", "walk");
		}
	}

	// Adds a state named `to` for the clip currently registered as `from`, unless a
	// state named `to` already exists (e.g. if the .anim was also renamed manually).
	private static void AliasClip(Animation anim, string from, string to)
	{
		if (anim.GetClip(to) != null)
		{
			return;
		}
		AnimationClip clip = anim.GetClip(from);
		if (clip != null)
		{
			anim.AddClip(clip, to);
		}
	}

	// The exported scenes bake a very tight TMP auto-size range on localized labels
	// (e.g. min 25 / max 30). Short languages fit at the ceiling, but longer ones
	// (Vietnamese especially) hit the floor while still overflowing the rect, then
	// wrap past its height / clip inside a mask and vanish. Attach a component that
	// keeps the design size as the ceiling and lowers only the floor, so overflowing
	// translations scale down to fit instead of disappearing.
	private void FitLocalizedLabels()
	{
		TMP_Text[] labels = Resources.FindObjectsOfTypeAll<TMP_Text>();
		for (int i = 0; i < labels.Length; i++)
		{
			TMP_Text label = labels[i];
			if (label == null || !label.gameObject.scene.IsValid())
			{
				continue;
			}
			if (label.GetComponent<ExportedTextAutoFit>() != null)
			{
				continue;
			}
			// Leave input fields alone: their text is user-typed, not a translation,
			// and their sizing is managed by the field.
			if (label.GetComponentInParent<TMP_InputField>() != null)
			{
				continue;
			}
			// Only labels driven by an I2 Localize component get per-language text/font
			// swaps and are where long translations overflow. The Localize component sits
			// on the label's GameObject (or a parent in a few grouped cases).
			if (label.GetComponent<Localize>() == null && label.GetComponentInParent<Localize>() == null)
			{
				continue;
			}
			label.gameObject.AddComponent<ExportedTextAutoFit>();
		}
	}

	private void Sweep()
	{
		// TMP_Text is the base of both TextMeshProUGUI (canvas) and TextMeshPro
		// (world space); the reconstructed SDF atlases render both as black boxes.
		TMP_Text[] labels = Resources.FindObjectsOfTypeAll<TMP_Text>();
		for (int i = 0; i < labels.Length; i++)
		{
			TMP_Text label = labels[i];
			if (label == null || !label.gameObject.scene.IsValid())
			{
				continue;
			}
			if (!(label is TextMeshProUGUI) && !(label is TextMeshPro))
			{
				continue;
			}
			// Never touch the text inside a TMP_InputField: disabling that TMP breaks
			// the field's caret and typed-text display (e.g. the "enter your name"
			// field on the difficulty screen). Leave those to render as-is.
			if (label.GetComponentInParent<TMP_InputField>() != null)
			{
				continue;
			}
			if (label.GetComponent<ExportedTextFallback>() != null)
			{
				continue;
			}
			if (label.font == null && ExportedProjectRuntimeFixes.FallbackFont != null)
			{
				label.font = ExportedProjectRuntimeFixes.FallbackFont;
				label.SetAllDirty();
			}
			label.gameObject.AddComponent<ExportedTextFallback>();
		}
	}
}

/// <summary>
/// Mirrors a TextMesh Pro label through Unity's built-in font renderer. Canvas
/// labels (TextMeshProUGUI) are mirrored with UnityEngine.UI.Text; world-space
/// labels (TextMeshPro) are mirrored with a 3D TextMesh. This avoids the solid
/// black boxes produced by the reconstructed TMP SDF atlases while leaving
/// localization scripts free to keep updating the original TMP source.
/// </summary>
public class ExportedTextFallback : MonoBehaviour
{
	private TMP_Text source;
	private Text uiOutput;
	private TextMesh worldOutput;
	private LayoutElement layoutProxy;

	private void Awake()
	{
		source = GetComponent<TMP_Text>();
		if (source == null)
		{
			enabled = false;
			return;
		}

		if (source is TextMeshProUGUI)
		{
			GameObject rendererObject = new GameObject("Legacy Text Renderer", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
			rendererObject.transform.SetParent(transform, false);
			RectTransform rect = (RectTransform)rendererObject.transform;
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;

			uiOutput = rendererObject.GetComponent<Text>();
			uiOutput.font = FindMatchingFont();
			// Preserve the original label's raycast behaviour. Button labels are
			// usually raycast targets that cover the button's center and bubble the
			// click up to the Button; forcing false removed that center hit area, so
			// clicking directly on the text did nothing (only the button edges worked).
			uiOutput.raycastTarget = source.raycastTarget;
			uiOutput.supportRichText = source.richText;
			uiOutput.resizeTextForBestFit = source.enableAutoSizing;
			uiOutput.resizeTextMinSize = Mathf.Max(1, Mathf.RoundToInt(source.fontSizeMin));
			uiOutput.resizeTextMaxSize = Mathf.Max(uiOutput.resizeTextMinSize, Mathf.RoundToInt(source.fontSizeMax));
			uiOutput.alignment = ConvertAlignment(source.alignment);

			// Disabling the TMP (below) removes it from Unity's layout system, which
			// collapses any label inside a VerticalLayoutGroup / ContentSizeFitter to
			// zero size (e.g. the in-game message box goes blank). Add a LayoutElement
			// that reports the TMP's preferred size so layout still gets a contribution.
			// Skip it if the label already has one (its author-set sizes win).
			if (GetComponent<LayoutElement>() == null)
			{
				layoutProxy = gameObject.AddComponent<LayoutElement>();
			}
		}
		else
		{
			GameObject rendererObject = new GameObject("Legacy Text Renderer", typeof(MeshRenderer), typeof(TextMesh));
			rendererObject.transform.SetParent(transform, false);
			rendererObject.transform.localPosition = Vector3.zero;
			rendererObject.transform.localRotation = Quaternion.identity;
			rendererObject.transform.localScale = Vector3.one;

			worldOutput = rendererObject.GetComponent<TextMesh>();
			Font font = FindMatchingFont();
			worldOutput.font = font;
			MeshRenderer meshRenderer = rendererObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null && font != null)
			{
				meshRenderer.material = font.material;
			}
			worldOutput.anchor = TextAnchor.MiddleCenter;
			worldOutput.alignment = TextAlignment.Center;
			// TextMesh has no SDF scale; approximate the TMP point size.
			worldOutput.characterSize = 0.1f;
			worldOutput.fontSize = Mathf.Max(1, Mathf.RoundToInt(source.fontSize * 3f));
		}
		SyncText();
		source.enabled = false;
	}

	private void LateUpdate()
	{
		SyncText();
	}

	private void SyncText()
	{
		if (source == null)
		{
			return;
		}
		if (uiOutput != null)
		{
			uiOutput.text = source.text;
			uiOutput.color = source.color;
			uiOutput.fontSize = Mathf.Max(1, Mathf.RoundToInt(source.fontSize));
			uiOutput.fontStyle = source.fontStyle == FontStyles.Bold ? FontStyle.Bold : FontStyle.Normal;
			uiOutput.enabled = source.gameObject.activeInHierarchy;
			if (layoutProxy != null)
			{
				// TMP_Text.preferredWidth/Height compute correctly even while disabled.
				layoutProxy.preferredWidth = source.preferredWidth;
				layoutProxy.preferredHeight = source.preferredHeight;
			}
		}
		else if (worldOutput != null)
		{
			worldOutput.text = source.text;
			worldOutput.color = source.color;
			worldOutput.gameObject.SetActive(source.gameObject.activeInHierarchy);
		}
	}

	private Font FindMatchingFont()
	{
		string wanted = (source.font == null) ? string.Empty : source.font.name.Replace(" SDF", string.Empty).Replace(" 3D", string.Empty).Trim().ToLowerInvariant();
		Font[] packagedFonts = Resources.LoadAll<Font>("Font");
		for (int i = 0; i < packagedFonts.Length; i++)
		{
			if (packagedFonts[i] != null && packagedFonts[i].name.ToLowerInvariant() == wanted)
			{
				return packagedFonts[i];
			}
		}
		Font[] fonts = Resources.FindObjectsOfTypeAll<Font>();
		for (int i = 0; i < fonts.Length; i++)
		{
			if (fonts[i] != null && fonts[i].name.ToLowerInvariant() == wanted)
			{
				return fonts[i];
			}
		}
		return Resources.GetBuiltinResource<Font>("Arial.ttf");
	}

	private static TextAnchor ConvertAlignment(TextAlignmentOptions alignment)
	{
		string value = alignment.ToString();
		bool top = value.Contains("Top");
		bool bottom = value.Contains("Bottom");
		bool left = value.Contains("Left");
		bool right = value.Contains("Right");
		if (top) return left ? TextAnchor.UpperLeft : (right ? TextAnchor.UpperRight : TextAnchor.UpperCenter);
		if (bottom) return left ? TextAnchor.LowerLeft : (right ? TextAnchor.LowerRight : TextAnchor.LowerCenter);
		return left ? TextAnchor.MiddleLeft : (right ? TextAnchor.MiddleRight : TextAnchor.MiddleCenter);
	}
}

/// <summary>
/// Widens a localized TMP label's auto-size range so long translations shrink to
/// fit instead of overflowing and clipping out. Keeps the label's design size as
/// the ceiling and lowers only the floor, so languages that already fit (English)
/// look identical and only overflowing ones (Vietnamese, German, Russian) scale
/// down. Settings are persistent, so they survive the per-language text/font swaps
/// I2 applies later; a re-apply on enable covers labels toggled off at attach time.
/// </summary>
public class ExportedTextAutoFit : MonoBehaviour
{
	// Smallest fraction of the design size we allow. 40% keeps text readable while
	// giving long strings real room to shrink (a 30pt label can drop to 12pt).
	private const float FloorFraction = 0.4f;

	private const float AbsoluteFloor = 8f;

	private TMP_Text label;

	private void Awake()
	{
		label = GetComponent<TMP_Text>();
		Apply();
	}

	private void OnEnable()
	{
		Apply();
	}

	private void Apply()
	{
		if (label == null)
		{
			return;
		}
		// Ceiling = the label's intended design size: the existing max when auto-size
		// was already on, otherwise the fixed point size it was rendering at.
		float ceiling = (label.enableAutoSizing && label.fontSizeMax > 0f) ? label.fontSizeMax : label.fontSize;
		if (ceiling <= 0f)
		{
			ceiling = 30f;
		}
		float floor = Mathf.Min(ceiling, Mathf.Max(AbsoluteFloor, ceiling * FloorFraction));
		// Only touch the label if it isn't already at least as permissive as we want,
		// so we never raise an author-set floor that is already lower.
		if (!label.enableAutoSizing || label.fontSizeMin > floor || label.fontSizeMax < ceiling)
		{
			label.fontSizeMax = ceiling;
			label.fontSizeMin = floor;
			label.enableAutoSizing = true;
		}
	}
}

/// <summary>
/// Developer helper. Press Insert to toggle a menu that lists every takeable
/// item in the current scene; clicking one drops it in front of the player so
/// it can be picked up normally. Frees the cursor and pauses mouse-look while open.
/// </summary>
public class ExportedItemSpawner : MonoBehaviour
{
	public static bool MenuOpen;

	private bool wasLookEnabled;
	private Vector2 scroll;
	private string filter = string.Empty;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Insert))
		{
			SetOpen(!MenuOpen);
		}
		else if (MenuOpen && Input.GetKeyDown(KeyCode.Escape))
		{
			SetOpen(false);
		}
	}

	private void SetOpen(bool open)
	{
		MenuOpen = open;
		Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = open;
		if (SimpleSmoothMouseLook.instance != null)
		{
			if (open)
			{
				wasLookEnabled = SimpleSmoothMouseLook.instance.canLook;
				SimpleSmoothMouseLook.instance.canLook = false;
			}
			else
			{
				SimpleSmoothMouseLook.instance.canLook = wasLookEnabled;
			}
		}
	}

	private void OnGUI()
	{
		if (!MenuOpen)
		{
			return;
		}
		float width = 340f;
		float height = Mathf.Min(Screen.height - 40f, 520f);
		GUILayout.BeginArea(new Rect(20f, 20f, width, height), GUI.skin.box);
		GUILayout.Label("Item Spawner  (Insert to close)");
		filter = GUILayout.TextField(filter);
		scroll = GUILayout.BeginScrollView(scroll);
		TakeableObject[] items = Resources.FindObjectsOfTypeAll<TakeableObject>();
		System.Array.Sort(items, (a, b) => string.Compare(a.name, b.name, System.StringComparison.OrdinalIgnoreCase));
		string needle = filter.Trim().ToLowerInvariant();
		for (int i = 0; i < items.Length; i++)
		{
			TakeableObject item = items[i];
			if (item == null || !item.gameObject.scene.IsValid())
			{
				continue;
			}
			if (needle.Length > 0 && !item.name.ToLowerInvariant().Contains(needle))
			{
				continue;
			}
			if (GUILayout.Button(item.name + "  (id " + item.id + ")"))
			{
				Give(item);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void Give(TakeableObject item)
	{
		if (PlayersManager.instance == null)
		{
			return;
		}
		MyController player = PlayersManager.instance.GetCurrentController();
		Transform anchor = (SimpleSmoothMouseLook.instance != null)
			? SimpleSmoothMouseLook.instance.transform
			: ((Camera.main != null) ? Camera.main.transform : null);
		if (player == null || anchor == null)
		{
			return;
		}
		item.gameObject.SetActive(true);
		item.transform.parent = null;
		item.transform.position = anchor.position + anchor.forward * 0.6f;
		Collider col = item.GetComponent<Collider>();
		if (col != null)
		{
			col.enabled = true;
		}
		Rigidbody rb = item.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = false;
			rb.velocity = Vector3.zero;
		}
	}
}

/// <summary>
/// Restores Backspace/Delete editing for focused TMP_InputFields. The reconstructed
/// mobile build routes text entry through a soft-keyboard path on Windows where
/// character input registers but the Backspace/Delete keys are never applied. We
/// apply those two keys directly to whichever input field currently has focus.
/// </summary>
public class ExportedInputFieldFix : MonoBehaviour
{
	private void Update()
	{
		EventSystem eventSystem = EventSystem.current;
		if (eventSystem == null || eventSystem.currentSelectedGameObject == null)
		{
			return;
		}
		TMP_InputField field = eventSystem.currentSelectedGameObject.GetComponent<TMP_InputField>();
		if (field == null || !field.isFocused || field.readOnly || string.IsNullOrEmpty(field.text))
		{
			return;
		}
		// In this build the caret index doesn't track: native per-character Backspace
		// is a no-op and only Ctrl+A then Backspace clears (via selection). A caret-
		// based delete therefore does nothing, so delete from the END of the string,
		// which is what typing/correcting a name needs.
		if (Input.GetKeyDown(KeyCode.Backspace) && field.text.Length > 0)
		{
			field.text = field.text.Substring(0, field.text.Length - 1);
			field.caretPosition = field.text.Length;
			field.stringPosition = field.text.Length;
			field.ForceLabelUpdate();
		}
	}
}

public class ExportedMovementDriver : MonoBehaviour
{
	private CharacterController controller;
	private Vector3 verticalVelocity;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		Debug.Log("Exported movement driver active. CharacterController=" + (controller != null));
	}

	private void Update()
	{
		// This driver is the sole PC mover. Some scripts (FastHide.OnExitHide,
		// WindowBehaviour) re-enable the original PlayerMovement, which then double-
		// drives the CharacterController (a speed boost) and double-pins the camera.
		// Keep it disabled every frame so no re-enable site can take effect.
		PlayerMovement legacyMovement = GetComponent<PlayerMovement>();
		if (legacyMovement != null && legacyMovement.enabled)
		{
			legacyMovement.enabled = false;
		}
		if (controller == null || !controller.enabled || ExportedItemSpawner.MenuOpen || CameraIsScripted() || (PauseController.instance != null && PauseController.instance.paused))
		{
			return;
		}
		if (Time.timeScale == 0f) Time.timeScale = 1f;
		float horizontal = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1f : 0f) - (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f);
		float vertical = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 0f) - (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1f : 0f);
		if (Mathf.Abs(horizontal) < 0.001f) horizontal = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw("Horizontal");
		if (Mathf.Abs(vertical) < 0.001f) vertical = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw("Vertical");
		Vector3 input = Vector3.ClampMagnitude(new Vector3(horizontal, 0f, vertical), 1f);
		float speed = 2.8f;
		Vector3 move = transform.TransformDirection(input) * speed;
		if (controller.isGrounded)
		{
			verticalVelocity.y = -1f;
		}
		else
		{
			verticalVelocity += Physics.gravity * Time.unscaledDeltaTime;
		}
		controller.Move((move + verticalVelocity) * Time.unscaledDeltaTime);
	}

	private void LateUpdate()
	{
		// While a script has taken over the camera (e.g. looking out the window for
		// the eagle-wing puzzle), do NOT pin it back to the player's eyes — doing so
		// every frame cancels WindowBehaviour.OnOpenWindow and the view never moves.
		if (CameraIsScripted())
		{
			return;
		}
		MyController player = GetComponent<MyController>();
		if (player != null && player.eyesTrans != null && SimpleSmoothMouseLook.instance != null)
		{
			SimpleSmoothMouseLook.instance.transform.position = player.eyesTrans.position;
		}
	}

	// True when another script owns the camera position this frame (hiding, looking
	// out a window, controlling the doll/baby, fall/game-over cinematics, etc.). In
	// those states the driver must NOT pin the camera back to the player's eyes.
	private static bool CameraIsScripted()
	{
		SimpleSmoothMouseLook look = SimpleSmoothMouseLook.instance;
		if (look != null && !look.canLook)
		{
			// SimpleSmoothMouseLook sets canLook = false whenever a script has taken
			// the camera (HideIn, OnFallInHole, StartLookingNun, game over, ...).
			return true;
		}
		if (WindowBehaviour.instance != null && WindowBehaviour.instance.isInside)
		{
			// The window view keeps canLook = true, so it needs its own check.
			return true;
		}
		if (PlayersManager.instance != null && PlayersManager.instance.isBaby)
		{
			// The doll/baby camera is driven by SimpleSmoothMouseLook.FixedUpdate.
			return true;
		}
		return false;
	}
}
