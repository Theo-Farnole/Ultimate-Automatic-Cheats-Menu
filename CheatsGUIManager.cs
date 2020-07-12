namespace TF.CheatsGUI
{
	using UnityEngine;
	using TF.CheatsGUI.Utilities;
	using System.Collections.Generic;
	using System;
	using System.Reflection;
	using System.Linq;

	public class CheatsGUIManager : MonoBehaviour
	{
		#region Fields
		public const string DEBUG_LOG_HEADER = "<color=cyan>Cheats GUI</color> :";
		private const string MENU_TITLE = "Cheats Menu";

		[Header("INPUTS SETTINGS")]
		[SerializeField] private KeyCode _keyToToggleCheatsMenu = KeyCode.C;
		[SerializeField] private KeyCode[] _keysModifierToToggleCheatsMenu = new KeyCode[] { KeyCode.LeftShift }; // press SHIFT + C simulatenously
		[Header("GUI SETTINGS")]
		[SerializeField] private RectOffset _margin = new RectOffset();

		private bool _isCheatsMenuOpen = false;
		private GUI_CheatButton[] _cheatsButton = null;
		private Vector2 _scrollPosition = Vector2.zero;
		#endregion

		#region Properties
		private GUIStyle BackgroundStyle => new GUIStyle(GUI.skin.box)
		{
			margin = _margin
		};
		private GUIStyle MarginStyle => new GUIStyle { margin = _margin };
		private GUIStyle MenuTitleStyle => new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
		#endregion

		#region Methods
		#region MonoBehaviour Callbacks
		private void Update()
		{
			HandleToggleCheatsMenuInputs();
		}

		private void OnGUI()
		{
			if (!_isCheatsMenuOpen)
				return;

			if (_cheatsButton == null)
				SetCheatsButton();

			// This first vertical group is only used to have a margin
			GUILayout.BeginVertical(MarginStyle);
			{
				//	// This second vertical group set the background style
				GUILayout.BeginVertical(BackgroundStyle);
				{
					_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandWidth(true));
					{
						GUILayout.BeginVertical();
						GUILayout.Label(MENU_TITLE, MenuTitleStyle);

						int buttonWidth = GetLongestButtonWidth();

						for (int i = 0, length = _cheatsButton.Length; i < length; i++)
						{
							_cheatsButton[i].Draw(GUILayout.Width(buttonWidth));
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndScrollView();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
		}
		#endregion

		#region Public Methods
		public void ToggleCheatsMenu()
		{
			if (_isCheatsMenuOpen) CloseCheatsMenu();
			else OpenCheatMenu();
		}

		public void OpenCheatMenu() => _isCheatsMenuOpen = true;

		public void CloseCheatsMenu() => _isCheatsMenuOpen = false;
		#endregion

		#region Private Methods
		private void SetCheatsButton()
		{
			IEnumerable<Type> typesWithAttributes = ReflectionHelper.GetTypesWithAttribute<CheatMethodAttribute>();

			List<GUI_CheatButton> cheatsButton = new List<GUI_CheatButton>();

			foreach (Type type in typesWithAttributes)
			{
				foreach (MethodInfo method in ReflectionHelper.GetMethodsWithAttribute(type, typeof(CheatMethodAttribute)))
				{
					cheatsButton.Add(new GUI_CheatButton(method, method.GetCustomAttribute<CheatMethodAttribute>()));
				}
			}

			_cheatsButton = cheatsButton.ToArray();
		}

		private void HandleToggleCheatsMenuInputs()
		{
			if (AreKeysToToggleCheatsMenuAreDown() == true)
			{
				ToggleCheatsMenu();
			}
		}

		private bool AreKeysToToggleCheatsMenuAreDown()
		{
			if (Input.GetKeyDown(_keyToToggleCheatsMenu) == false)
				return false;

			for (int i = 0, length = _keysModifierToToggleCheatsMenu.Length; i < length; i++)
			{
				if (Input.GetKey(_keysModifierToToggleCheatsMenu[i]) == false)
					return false;
			}

			return true;
		}

		private int GetEnabledButtonsCount() => _cheatsButton.Where(x => x.Enabled).Count();

		private int GetLongestButtonWidth() => (int)GUI.skin.button.CalcSize(new GUIContent(GetLongestButtonLabel())).x;

		private string GetLongestButtonLabel() => _cheatsButton.OrderByDescending(x => x.ButtonLabel.Length).Select(x => x.ButtonLabel).First();
		#endregion
		#endregion
	}
}
