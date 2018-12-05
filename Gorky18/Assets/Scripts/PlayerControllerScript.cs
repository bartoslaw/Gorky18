using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;

public class PlayerControllerScript : MonoBehaviour
{

  public int sightDistance = 1;

  IsoWorld world;
  IsoObject isoObject;
  GameObject[] floor;
  // Use this for initialization
  void Start()
  {
    world = IsoWorld.GetWorld(0);
    floor = GameObject.FindGameObjectsWithTag("Floor");
    isoObject = GetComponent<IsoObject>();
    
		AdjustMovableTiles();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePosition = world.MouseIsoTilePosition();
      BaseTile baseTile = GetTileAtPosition(mousePosition);

      print("BaseTile: " + baseTile);
      if (baseTile == null || baseTile.isCollider) {
        return;
      }

			Vector3 playerPosition = isoObject.position;
			Vector3 newPosition = new Vector3(mousePosition.x - 0.427f, mousePosition.y, playerPosition.z);

			isoObject.position = newPosition;
      AdjustMovableTiles();
		}
  }

  public void AdjustMovableTiles()
  {
    Vector2 positionXY = GetComponent<IsoObject>().tilePositionXY;
		print ("Position: " + positionXY);
    foreach (GameObject item in floor)
    {
      IsoObject isoObject = item.GetComponent<IsoObject>();
      SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();
      BaseTile baseTile = item.GetComponent<BaseTile>();

      if (renderer != null)
      {
        Color tempColor = renderer.color;

        if (!baseTile.isCollider) {
				  tempColor.a = IsNextToPlayer(isoObject.tilePositionXY, positionXY) ? 0.5f : 1.0f;
        }
				renderer.color = tempColor;
      }
    }
  }

  public BaseTile GetTileAtPosition(Vector2 position) {
    foreach (GameObject item in floor)
    {
        IsoObject isoObject = item.GetComponent<IsoObject>();
        print ("Distance: " + Vector2.Distance(isoObject.positionXY, position));
        if ((int) Vector2.Distance(isoObject.positionXY, position) == 0) {
          return item.GetComponent<BaseTile>();
        }

    }
    return null;
  }

  bool IsNextToPlayer(Vector2 tilePosition, Vector2 position)
  {
    return Vector2.Distance(tilePosition, position) < sightDistance;;
  }
}
