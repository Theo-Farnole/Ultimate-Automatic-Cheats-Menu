namespace TF.CheatsGUI
{
	using UnityEngine;

	// TODO TF: pass expression to hide button if expression is true	
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class CheatMethodAttribute : System.Attribute
	{
		private readonly string _overridedButtonLabel = null;

		public string OverridedButtonLabel => _overridedButtonLabel;

		public CheatMethodAttribute() { }

		public CheatMethodAttribute(string overridedButtonLabel)
		{
			_overridedButtonLabel = overridedButtonLabel;
		}
	}
}
