using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// PC build helper. Injected automatically at runtime (no scene edits required).
// Maps keyboard/mouse to the same methods the on-screen touch buttons called,
// manages cursor lock, and forces the input system into hardware mode.
public class PCControls : MonoBehaviour
{
	private static PCControls instance;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Bootstrap()
	{
		// Make sure the cross-platform input layer reads the keyboard/mouse and
		// not the (now unused) touch joystick virtual axes.
		CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Hardware);

		if (instance == null)
		{
			GameObject go = new GameObject("PCControls");
			instance = go.AddComponent<PCControls>();
			Object.DontDestroyOnLoad(go);
		}
	}

	private void Update()
	{
		// The dev item spawner (Insert) owns the cursor and input while it is open.
		if (ExportedItemSpawner.MenuOpen)
		{
			return;
		}
		UpdateCursor();
		HandlePause();

		// Only route gameplay keys when a player controller is actually in the scene.
		if (PlayersManager.instance == null || GameUIController.instance == null)
		{
			return;
		}
		MyController ctrl = PlayersManager.instance.GetCurrentController();
		if (ctrl == null || ctrl.blocked)
		{
			return;
		}
		if (PauseController.instance != null && PauseController.instance.paused)
		{
			return;
		}

		// E = Use Item: use the doll when its prompt is showing, otherwise interact /
		// pick up / open / use the held item (mirrors the on-screen buttons).
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (GameUIController.instance.startDollButton != null && GameUIController.instance.startDollButton.activeSelf)
			{
				GameUIController.instance.OnClickUseDoll();
			}
			else if (GameUIController.instance.actionButton != null && GameUIController.instance.actionButton.activeSelf)
			{
				GameUIController.instance.OnClickAction();
			}
		}

		// H = hide / unhide (when the fast-hide prompt is available or already hidden).
		if (Input.GetKeyDown(KeyCode.H))
		{
			if (ctrl.isHidden || (GameUIController.instance.fastHiddingUI != null && GameUIController.instance.fastHiddingUI.activeSelf))
			{
				GameUIController.instance.OnClickFastHidding();
			}
		}

		// Left Ctrl or C: toggle crouch / stand up.
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
		{
			GameUIController.instance.OnClickStandUp();
		}

		// Space or G: drop the held item.
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.G))
		{
			GameUIController.instance.OnClickRelease();
		}

		// Left mouse button: use / shoot the held item (e.g. gum gun).
		if (Input.GetMouseButtonDown(0) && ctrl.hasObjectOnHand)
		{
			GameUIController.instance.OnClickShoot();
		}
	}

	private void HandlePause()
	{
		if (PauseController.instance == null)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (PauseController.instance.paused)
			{
				PauseController.instance.OnContinue();
			}
			else if (GameUIController.instance != null && !GameUIController.instance.gameOver && !GameUIController.instance.completed)
			{
				PauseController.instance.OnPaused();
			}
		}
	}

	private void UpdateCursor()
	{
		// Free the cursor while paused or when no gameplay camera is active
		// (menus); otherwise lock and hide it for mouse-look.
		bool gameplay = SimpleSmoothMouseLook.instance != null
			&& (PauseController.instance == null || !PauseController.instance.paused)
			// Free the cursor on the game-over / escaped end screens so the menu
			// button can be clicked (those keep the gameplay camera alive).
			&& (GameUIController.instance == null || (!GameUIController.instance.gameOver && !GameUIController.instance.completed));
		if (gameplay)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			UpdateCursor();
		}
	}
}
