using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This script copies a value into a text
public class SetSliderValue : MonoBehaviour 
{
	Text textThis;

	void Start()
	{
		textThis = GetComponent<Text> ();
	}

	public void ShowValue(float f)
	{
		textThis.text = f.ToString ("F0");
	}
}
