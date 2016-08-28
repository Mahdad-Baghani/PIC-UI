using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class V_CameraToTeamMemberTexture : MonoBehaviour 
{
	[SerializeField] Camera targetCamera;
	[SerializeField] RenderTexture renderTexture;
	[SerializeField] RenderTexture[] RTs;
	void Awake()
	{
		// int imageWidth = (int) GetComponent<Image>().sprite.rect.width;
		// int imageHeight = (int) GetComponent<Image>().sprite.rect.height;
		// // RenderTexture renderTexture = new RenderTexture(imageWidth/20, imageHeight/20, 24);
		// // renderTexture.Create();
		// targetCamera.targetTexture = renderTexture;
		// targetCamera.Render();
		// RenderTexture.active = renderTexture;
		// // Texture2D viewport = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
		// // viewport.ReadPixels(new Rect(0,0, imageWidth, imageHeight), 0, 0);
		// // targetCamera.targetTexture = null;
		// // RenderTexture.active = null;
		// // Destroy(renderTexture);
	}
}
