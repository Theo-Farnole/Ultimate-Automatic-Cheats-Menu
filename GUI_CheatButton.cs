namespace TF.CheatsGUI
{
	using System.Reflection;
	using UnityEngine;

	// TODO TF: #region
	public class GUI_CheatButton
	{
		private readonly MethodInfo _methodInfo = null;
		private bool _enabled = true;
		private string _overridedButtonLabel = null;

		public bool Enabled { get => _enabled; set => _enabled = value; }

		// TODO TF: Nicify _methodInfo name
		public string ButtonLabel
		{
			get => _overridedButtonLabel ?? _methodInfo.Name;
			set => _overridedButtonLabel = value;
		}

		public GUI_CheatButton(MethodInfo methodInfo)
		{
			_methodInfo = methodInfo ?? throw new System.ArgumentNullException();

			if (_methodInfo.GetParameters().Length > 0)
			{
				Debug.LogWarningFormat("Button for method '{0}' will not be drawn: attribute 'CheatMethod' doesn't support method with parameters.", _methodInfo.Name);
			}
		}

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
	}
}
