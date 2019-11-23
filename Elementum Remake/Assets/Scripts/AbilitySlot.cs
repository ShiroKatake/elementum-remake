using UnityEngine;

public class AbilitySlot : MonoBehaviour {
	public bool occupied;
	public Element element;
	public Color airColor;
	public Color fireColor;
	public Color earthColor;

	void Update() {
		if (!occupied) {
			transform.GetComponent<SpriteRenderer>().color = Color.white;
		} else {
			switch (element.name) {
				case "Air":
					transform.GetComponent<SpriteRenderer>().color = airColor;
					break;
				case "Fire":
					transform.GetComponent<SpriteRenderer>().color = fireColor;
					break;
				case "Earth":
					transform.GetComponent<SpriteRenderer>().color = earthColor;
					break;
			}
		}
	}
}
