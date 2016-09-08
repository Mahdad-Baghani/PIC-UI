using UnityEngine;
using UnityEngine.EventSystems;

public class V_CustomizedTextureItem : MonoBehaviour, IPointerDownHandler
{
	V_Inventory_UI inventory; // tweak from inspector, FindObjectOfType<> is a pure bitch.
	Texture texture; // again tweak from the inspector and make a prefab;
	public void OnPointerDown(PointerEventData data)
	{
		if(data.button == PointerEventData.InputButton.Left)
		{
			// #revision: How to add texture to an already set texture?
			Texture tmpTexture = inventory.weaponToCustomize.GetComponent<Renderer>().GetComponent<Texture>();
			tmpTexture = texture;
		}
	}
}
