namespace TF.CheatsGUI
{
	using System.Reflection;
	using UnityEngine;

	public class GUI_CheatButton
	{
		private readonly MethodInfo _methodInfo = null;

		public GUI_CheatButton(MethodInfo methodInfo)
		{
			_methodInfo = methodInfo ?? throw new System.ArgumentNullException();

			if (_methodInfo.GetParameters().Length > 0)
			{
				Debug.LogWarningFormat("Attribute 'CheatMethod' doesn't support method with parameters.");
			}
		}

		public void Draw()
		{
			// TODO TF: Be able to override _methodInfo.Name
			// TODO TF: Draw parameters (if method has parameters)
			bool clickedOnButton = GUILayout.Button(_methodInfo.Name);

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
