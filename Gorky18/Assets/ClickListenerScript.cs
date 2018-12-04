using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;

public class ClickListenerScript : MonoBehaviour {

	IsoWorld world;
	
	// Use this for initialization
	void Start () {
		world = IsoWorld.GetWorld(0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePosition = world.MouseIsoTilePosition();
			
			PlayerControllerScript playerController = FindObjectOfType<PlayerControllerScript>();
			Vector3 playerPosition = playerController.GetComponent<IsoObject>().position;

			Vector3 newPosition = new Vector3(mousePosition.x, mousePosition.y - 0.5f, playerPosition.z);

			playerController.GetComponent<IsoObject>().position = newPosition;
			playerController.AdjustMovableTiles();
		}
	}
}
