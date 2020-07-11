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
		public bool Enabled { get => _enabled; set => _enabled = value; }

		// TODO TF: Nicify _methodInfo name
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
			}
		}
		#endregion

		#region Methods
		public void Draw()
		{
			if (_enabled == false || _methodInfo.GetParameters().Length > 0)
				return;

			// TODO TF: Draw parameters (if method has parameters)
			bool clickedOnButton = GUILayout.Button(ButtonLabel);

			if (clickedOnButton)
			{
				Apply();
			}
		}

		void Apply(params object[] parameters)
		{
			if (_methodInfo.IsStatic)
			{
				_methodInfo.Invoke(null, parameters);
			}
			else
			{
				Object[] objectsOfType = Object.FindObjectsOfType(_methodInfo.DeclaringType);

				for (int i = 0, length = objectsOfType.Length; i < length; i++)
				{
					_methodInfo.Invoke(objectsOfType[i], parameters);
				}
			}
		}
		#endregion
	}
}
