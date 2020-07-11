namespace TF.CheatsGUI
{
	using System.Reflection;
	using UnityEngine;

	public class GUI_CheatButton
	{
		#region Fields
		private readonly MethodInfo _methodInfo = null;
		private readonly CheatMethodAttribute _cheatMethodAttribute = null;

		private bool _enabled = true;
		private string _overridedButtonLabel = null;
		#endregion

		#region Properties
		public bool Enabled
		{
			get => _enabled;
			set
			{
				if (_methodInfo.GetParameters().Length > 0)
				{
					Debug.LogWarningFormat("You can force enabling of button '{0}' because drawing methods with parameters is not supported.", ButtonLabel);
					_enabled = false;

					return;
				}

				_enabled = value;
			}
		}

		public string ButtonLabel
		{
			get => _overridedButtonLabel ?? _methodInfo.Name;
			set => _overridedButtonLabel = value;
		}
		#endregion

		#region ctor
		public GUI_CheatButton(MethodInfo methodInfo, CheatMethodAttribute cheatMethodAttribute)
		{
			_methodInfo = methodInfo ?? throw new System.ArgumentNullException();
			_cheatMethodAttribute = cheatMethodAttribute ?? throw new System.ArgumentNullException();

			if (_cheatMethodAttribute.OverridedButtonLabel != null)
			{
				_overridedButtonLabel = _cheatMethodAttribute.OverridedButtonLabel;
			}

			if (_methodInfo.GetParameters().Length > 0)
			{
				Debug.LogWarningFormat("Button for method '{0}' will not be drawn: attribute 'CheatMethod' doesn't support method with parameters.", _methodInfo.Name);
				_enabled = false;
			}
		}
		#endregion

		#region Methods
		public void Draw()
		{
			UpdateEnabledState();
			
			if (_enabled == false)
				return;

			// TODO TF: Draw parameters (if method has parameters)
			bool clickedOnButton = GUILayout.Button(ButtonLabel);

			if (clickedOnButton)
			{
				Apply();
			}
		}

		private void Apply(params object[] parameters)
		{
			if (_methodInfo.IsStatic)
			{
				_methodInfo.Invoke(null, parameters);
			}
			else
			{
				Object[] objectsOfType = Object.FindObjectsOfType(_methodInfo.DeclaringType);

				if (objectsOfType.Length == 0)
				{
					Debug.LogWarningFormat("Found 0 GameObject of type {0} in the scene. Clicking on {1} button will do nothing.", _methodInfo.DeclaringType, ButtonLabel);
				}

				for (int i = 0, length = objectsOfType.Length; i < length; i++)
				{
					_methodInfo.Invoke(objectsOfType[i], parameters);
				}
			}
		}

		private void UpdateEnabledState()
		{
			if (_cheatMethodAttribute.ShowIfExpressionIsTrue == null)
				return;
			
			_enabled = AttributeExpressionHelper.IsExpressionTrue(_cheatMethodAttribute.ShowIfExpressionIsTrue, _methodInfo.DeclaringType);
		}
		#endregion
	}
}
