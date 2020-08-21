using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilitySlot : MonoBehaviour {

	public bool occupied;
	public GameObject element;
	public Color color;
	public string elementName;
	public bool active;

	public GameObject Air;
	public GameObject Fire;
	public GameObject Earth;

	public Sprite Sprite
	{
		get
		{
			return Element.sprite;
		}
	}

	public Element Element
	{
		get
		{
			return element.GetComponent<Element>();
		}
	}

	public bool Occupied
	{
		get
		{
			return occupied;
		}
	}

	public void Awake()
	{
		active = true;
		Air = GameObject.Find("Player/Air");
		Fire = GameObject.Find("Player/Fire");
		Earth = GameObject.Find("Player/Earth");
	}

	void Update() {

		if (element == null && elementName != "")
		{
			SetElement(elementName);
		}

		//Change the color of the slot depending on what element is occupying it
		if (!occupied) {
			color = Color.white;
		} else {
			color = Element.Color;
		}
		transform.GetComponent<SpriteRenderer>().color = color;
	}


	//Activates the element in the slot, then clears slot
	public bool Activate(GameObject player, Vector2 dir)
	{
		if (occupied)
		{
			Element.Activate(player, dir);
			Clear();
			return true;
		}
		else
		{
			return false;
		}
	}

	public void Clear()
	{
		element = null;
		occupied = false;
		elementName = "";
	}

	//Places element into slot
	public void SetElement(string s)
	{
		Debug.Log("setting element");
		GameObject e = null;
		switch (s)
		{
			case "Air":
				e = Instantiate(Air);
				break;
			case "Fire":
				e = Instantiate(Fire);
				break;
			case "Earth":
				e = Instantiate(Earth);
				break;
		}
		if (e != null)
		{
			elementName = s;
			element = e;
			occupied = true;
		}
	}
}
