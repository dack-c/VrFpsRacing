using UnityEngine;

public class UIScreen : MonoBehaviour
{
	public bool isModal = false;
	[SerializeField] private UIScreen previousScreen = null;
	[SerializeField] private UIScreen RoomScreen = null;
	[SerializeField] private UIScreen LoadingScreen = null;

	public static UIScreen activeScreen;

	public static void Focus(UIScreen screen, bool savePrevious = true)
	{
		if (screen == activeScreen)
			return;

		if (activeScreen && !screen.isModal)
			activeScreen.Defocus();

		if (activeScreen && activeScreen.isModal)
		{
			activeScreen.Defocus();
			activeScreen.previousScreen.Defocus();
		}
		if (savePrevious)
		{
			screen.previousScreen = activeScreen;

		}
		activeScreen = screen;
		screen._Focus();
	}

	public static void BackToInitial()
	{
		activeScreen?.BackTo(null);
	}

	// Instance Methods

	public void FocusScreen(UIScreen screen)
	{
		Focus(screen);
	}

	public void FocusScreenNullPrev(UIScreen screen)
	{
		Focus(screen, false);
	}

	private void _Focus()
	{
		if (gameObject)
			gameObject.SetActive(true);
	}

	private void Defocus()
	{
		if (gameObject)
			gameObject.SetActive(false);
	}

	public void Back()
	{
		if (previousScreen)
		{
			Defocus();
			activeScreen = previousScreen;
			activeScreen._Focus();
			previousScreen = null;
		}
	}

	public void BackTo(UIScreen screen, bool showLoading = false)
	{
		while (activeScreen != null && activeScreen.previousScreen != null && activeScreen != screen)
			activeScreen.Back();

		if (showLoading)
			Focus(LoadingScreen);
	}

	public void FocusRoom()
	{
		if (RoomScreen == null) return;

		if (activeScreen.previousScreen)
		{
			activeScreen.previousScreen.Defocus();
			activeScreen.Defocus();
		}
		Focus(RoomScreen);
	}

	public string GetPreviousScreenName()
	{
		return previousScreen.name;
	}
}