using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
	public GameObject panel;
	public GameObject content;
	private Text texto;

    // Start is called before the first frame update
    void Start()
    {
		texto = content.GetComponent<Text>();
		panel.SetActive(false);

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	internal void AddLog(string Message)
	{
		texto.text += Message + "\n";
	}

	public void ShowHidePanel()
	{
		panel.SetActive(!panel.activeSelf);
	}
}
