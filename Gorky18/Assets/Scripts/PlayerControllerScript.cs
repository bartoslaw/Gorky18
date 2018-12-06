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
  
  Vector3 target;
  float speed = 2.0f;
  // Use this for initialization
  void Start()
  {
    world = IsoWorld.GetWorld(0);
    floor = GameObject.FindGameObjectsWithTag("Floor");
    isoObject = GetComponent<IsoObject>();
    target = Vector3.zero;
    
		AdjustMovableTiles();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePosition = world.MouseIsoTilePosition();
      BaseTile baseTile = GetTileAtPosition(mousePosition);

      Vector3 tilePosition = baseTile.GetComponent<IsoObject>().tilePositionXY;
      Vector3 playerPosition = isoObject.tilePositionXY;

      if (baseTile == null || baseTile.isCollider || !IsNextToPlayer(tilePosition, playerPosition)) {
        print(baseTile.isCollider);
        print(Vector2.Distance(playerPosition, mousePosition));
        return;
      }

			target = new Vector3(mousePosition.x - 0.427f, mousePosition.y, playerPosition.z);
		}

    if (target != Vector3.zero) {
      float step = speed * Time.deltaTime;
      isoObject.position = Vector3.MoveTowards(isoObject.position, target, step);

      if (Vector2.Distance(isoObject.position, target) == 0) {
        target = Vector3.zero;
        AdjustMovableTiles();
      }
    }
  }

  public void AdjustMovableTiles()
  {
    Vector2 positionXY = GetComponent<IsoObject>().tilePositionXY;
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
        // print ("Distance: " + item + (int) Vector2.Distance(isoObject.positionXY, position));
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
