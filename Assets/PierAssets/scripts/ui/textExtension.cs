using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using extensions;
public class textExtension : MonoBehaviour {
	public Text text;
	public InputField field;
	public Scrollbar bar;
	public string PlayerName = "Player";
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void ClearText(string str){
		
	}
	public void AppendText(string str){

		if (str != "") {
			text.Append (PlayerName + ": " + str);
			field.text = "";
			bar.value = 0;
			field.ActivateInputField ();
		}
	}
}
namespace extensions
{
	public static class FloatExtensions
	{
		public static void Double(this float i)
		{
			i= i + i;
		}
		public static void Double(this int i)
		{
			i = i + i;

		}
	}
	public static class TextExtension
	{
		public static void Append(this Text i,string str)
		{

			i.text = i.text+ str+ "\n" ;
		}

	}

}	
