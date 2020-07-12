using System.Collections;
using System.Collections.Generic;
using TF.CheatsGUI;
using UnityEngine;

public class CheatActivationTest : MonoBehaviour
{
	private static bool _displayMyMethod = true;

	[CheatMethod(nameof(_displayMyMethod))]
	public static void ShowAnotherMethod()
	{
		Debug.Log(nameof(ShowAnotherMethod));
		_displayMyMethod = false;
	}

	[CheatMethod("!" + nameof(_displayMyMethod))]
	public static void HideAnotherMethod()
	{
		Debug.Log(nameof(HideAnotherMethod));
		_displayMyMethod = true;
	}

	[CheatMethod] public static void IntMethod(int integer) { }

	[CheatMethod] public static void FloatMethod(float floating) { }

	[CheatMethod] public static void StringMethod(string str) { }

	[CheatMethod] public static void NoParametersMethods() { }

	[CheatMethod] public static void CharMethod(char character) { }

	[CheatMethod] public static void BoolMethod(bool boolean) { }

	[CheatMethod] public static void LooooongNammmeeeeMethodWithAlooootsOFParaams(bool boolean, string str1, int integer1, float f1, float f2) { }

	[CheatMethod] public static void LongMethodWithLotsOfLongParameters(bool booleeeeaaaan, string thisIsAVeryLongString, int integer1, float f1, float f2) { }

	//[CheatMethod] public static void VeryVeryLoooooongMethodNaaaaaame_YesThisIsVeryLong() { }

	[CheatMethod]
	public static void MyMethod()
	{
		// [...]
	}


	[CheatMethod]
	public static void SpriteMethod(bool boolean)
	{ }
}
