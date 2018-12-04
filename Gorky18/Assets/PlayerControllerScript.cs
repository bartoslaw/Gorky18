using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;

public class PlayerControllerScript : MonoBehaviour
{

  public int sightDistance = 1;
	private float playerOffset = 0.5f;
  IsoWorld world;
  GameObject[] floor;
  // Use this for initialization
  void Start()
  {
    world = IsoWorld.GetWorld(0);
    floor = GameObject.FindGameObjectsWithTag("Floor");
    
		AdjustMovableTiles();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void AdjustMovableTiles()
  {
    Vector2 positionXY = GetComponent<IsoObject>().tilePositionXY;
		print ("Position: " + positionXY);
    foreach (GameObject item in floor)
    {
      IsoObject isoObject = item.GetComponent<IsoObject>();
      SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();

      if (renderer != null)
      {
        Color tempColor = renderer.color;
				tempColor.a = IsNextToPlayer(isoObject.tilePositionXY, positionXY) ? 0.5f : 1.0f;
				renderer.color = tempColor;
      }
    }
  }

  bool IsNextToPlayer(Vector2 tilePosition, Vector2 position)
  {
		for (int i = 0; i <= sightDistance; ++i)
		{
				if (tilePosition.x == position.x && tilePosition.y == position.y - playerOffset) {
					print ("Tile position: " + tilePosition);
					return true;
				}
			
				if (tilePosition.x == position.x + i && tilePosition.y == position.y- playerOffset) {
					print ("Tile position: " + tilePosition);
					return true;
				}

				if (tilePosition.x  == position.x - i && tilePosition.y == position.y- playerOffset) {
					print ("Tile position: " + tilePosition);
					return true;
				}
				
				if (tilePosition.x == position.x && tilePosition.y == position.y + i- playerOffset) {
					print ("Tile position: " + tilePosition);
					return true;
				}
				
				if (tilePosition.x == position.x && tilePosition.y- playerOffset == position.y - i) {
					print ("Tile position: " + tilePosition);
					return true;
				}
		}

    return false;
  }
}
