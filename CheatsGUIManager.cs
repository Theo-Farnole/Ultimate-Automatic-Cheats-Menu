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

		[Header("INPUTS SETTINGS")]
		[SerializeField] private KeyCode _keyToToggleCheatsMenu = KeyCode.C;
		[SerializeField] private KeyCode[] _keysModifierToToggleCheatsMenu = new KeyCode[] { KeyCode.LeftShift }; // press SHIFT + C simulatenously
		[Header("GUI SETTINGS")]
		[SerializeField] private Vector2 _margin = new Vector2(10, 10);
		[SerializeField] private float _buttonHeight = 100;

		// TODO TF: Able to change position of cheats menu pivot point (top left, top right, etc..)

		private bool _isCheatsMenuOpen = false;
		private GUI_CheatButton[] _cheatsButton = null;
		#endregion

		#region Properties
		private Rect CheatsGUIRect
		{
			get
			{
				if (_cheatsButton == null)
				{
					Debug.LogErrorFormat("{0} Can't get menu size if _cheatsButton are null.", DEBUG_LOG_HEADER);
					return new Rect();
				}

				return new Rect(_margin.x, _margin.y, GetButtonWidth(), GetButtonHeight());
			}
		}
		#endregion

		#region Methods
		#region MonoBehaviour Callbacks
		private void Start()
		{
			SetCheatsButton();
		}

		private void Update()
		{
			HandleToggleCheatsMenuInputs();
		}

		private void OnGUI()
		{
			if (!_isCheatsMenuOpen)
				return;

			// TODO TF: add background
			GUILayout.BeginArea(CheatsGUIRect);
			{
				for (int i = 0, length = _cheatsButton.Length; i < length; i++)
				{
					_cheatsButton[i].Draw();
				}
			}
			GUILayout.EndArea();
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
		void SetCheatsButton()
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

		private float GetButtonHeight() => _cheatsButton.Length * _buttonHeight;

		private float GetButtonWidth() => GUI.skin.button.CalcSize(new GUIContent(GetLongestButtonLabel())).x;

		private string GetLongestButtonLabel()
		{
			string longestLabel = _cheatsButton
				.Select(x => x.ButtonLabel)
				.OrderByDescending(x => x)
				.First();

			return longestLabel;
		}
		#endregion
		#endregion
	}
}
