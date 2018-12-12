using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;
using Pathfinding;

public class PlayerControllerScript : MonoBehaviour
{
  public int sightDistance = 1;
  IsoWorld world;
  IsoObject isoObject;
  GameObject[] floor;
  List<Vector3> path;
  Seeker seeker;
  float speed = 2.0f;
  // Use this for initialization
  void Start()
  {
    world = IsoWorld.GetWorld(0);
    floor = GameObject.FindGameObjectsWithTag("Floor");
    isoObject = GetComponent<IsoObject>();
    seeker = GetComponent<Seeker>();

    path = new List<Vector3>();

    AdjustMovableTiles();
  }

  public void OnPathComplete(Path p)
  {
    if (p.error)
    {
      print(p.error);
    }
    else
    {
      print("Calculated path: " + p.vectorPath.Count);
      foreach (Vector3 point in p.vectorPath) 
      {
          path.Add(world.ScreenToIso(new Vector2(point.x - 0.427f, point.y)));
      }
    }
  }
  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Vector3 mousePosition = world.MouseIsoTilePosition();
      BaseTile baseTile = GetTileAtPosition(mousePosition);

      Vector3 tilePosition = baseTile.GetComponent<IsoObject>().tilePositionXY;
      Vector3 playerPosition = isoObject.tilePositionXY;

      if (baseTile == null || baseTile.isCollider || !IsNextToPlayer(tilePosition, playerPosition))
      {
        print(baseTile.isCollider);
        print(Vector2.Distance(playerPosition, mousePosition));
        return;
      }

      // target = new Vector3(mousePosition.x - 0.427f, mousePosition.y, playerPosition.z);
      path.Clear();
      seeker.StartPath (transform.position, baseTile.transform.position, OnPathComplete);
    }

    if (path.Count != 0)
    {
      Vector3 target = path[0];

      float step = speed * Time.deltaTime;
      isoObject.position = Vector3.MoveTowards(isoObject.position, target, step);
      if (Vector2.Distance(isoObject.position, target) == 0)
      {
        path.RemoveAt(0);
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

        if (!baseTile.isCollider)
        {
          tempColor.a = IsNextToPlayer(isoObject.tilePositionXY, positionXY) ? 0.5f : 1.0f;
        }
        renderer.color = tempColor;
      }
    }
  }

  public BaseTile GetTileAtPosition(Vector2 position)
  {
    foreach (GameObject item in floor)
    {
      IsoObject isoObject = item.GetComponent<IsoObject>();
      // print ("Distance: " + item + (int) Vector2.Distance(isoObject.positionXY, position));
      if ((int)Vector2.Distance(isoObject.positionXY, position) == 0)
      {
        return item.GetComponent<BaseTile>();
      }

    }
    return null;
  }

  bool IsNextToPlayer(Vector2 tilePosition, Vector2 position)
  {
    return Vector2.Distance(tilePosition, position) < sightDistance; ;
  }
}
