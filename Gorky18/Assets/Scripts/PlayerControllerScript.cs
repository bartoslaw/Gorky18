using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;
using Pathfinding;

public class PlayerControllerScript : MonoBehaviour
{
  public float sightDistance = 1.0f;
  IsoWorld world;
  IsoObject isoObject;
  GameObject[] floor;
  List<Vector3> path;
  Seeker seeker;
  float speed = 2.0f;
  bool isMoving = false;
  bool isAdjustingTiles = false;
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

  // Update is called once per frame
  void Update()
  {
    HandleMovement();

    if (!isMoving) {
      HandleMouseClick();
      UpdateSelectableTile();
    }
  }

  private void UpdateSelectableTile()
  {
    if (isAdjustingTiles) {
      return;
    }

    Vector3 mousePosition = world.MouseIsoTilePosition();
    BaseTile selectedTile = GetTileAtPosition(mousePosition);

    foreach (GameObject item in floor)
    {
      BaseTile tile = item.GetComponent<BaseTile>();
      SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();
      IsoObject tileObject = item.GetComponent<IsoObject>();

      if (renderer != null)
      {
        Color tempColor = renderer.color;

        if (!tile.isCollider)
        {
          tempColor.a = selectedTile == tile ? 0.1f : IsNextToPlayer(tileObject.positionXY, isoObject.positionXY) ? 0.5f : 1.0f;
        }

        renderer.color = tempColor;
      }
    }
  }

  private void HandleMouseClick()
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

      path.Clear();
      seeker.StartPath(transform.position, baseTile.transform.position, OnPathComplete);
    }
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

  private void HandleMovement()
  {
    if (path.Count != 0)
    {
      isMoving = true;
      Vector3 target = path[0];

      float step = speed * Time.deltaTime;
      isoObject.position = Vector3.MoveTowards(isoObject.position, target, step);
      if (Vector2.Distance(isoObject.position, target) == 0)
      {
        path.RemoveAt(0);

        if (path.Count == 0 && isoObject.position == target)
        {
          AdjustMovableTiles();
          isMoving = false;
        }
      }
    }
  }

  public void AdjustMovableTiles()
  {
    isAdjustingTiles = true;
    Vector2 positionXY = GetComponent<IsoObject>().tilePositionXY;
    int counter = 0;
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
          bool isNextToPlayer = IsNextToPlayer(isoObject.tilePositionXY, positionXY);
          tempColor.a = isNextToPlayer ? 0.5f : 1.0f;

          if (isNextToPlayer) {
            counter++;
          }
        }
        renderer.color = tempColor;
      }
    }

    isAdjustingTiles = false;
  }

  public BaseTile GetTileAtPosition(Vector2 position)
  {
    BaseTile tile = null;
    float closestDistance = -1.0f;
    foreach (GameObject item in floor)
    {
      IsoObject isoObject = item.GetComponent<IsoObject>();
      float currentDistance = Vector2.Distance(isoObject.positionXY, position);
      
      if (closestDistance == -1.0f || currentDistance < closestDistance) {
        tile = isoObject.GetComponent<BaseTile>();
        closestDistance = currentDistance; 
      }
    }

    return tile;
  }

  bool IsNextToPlayer(Vector2 tilePosition, Vector2 position)
  {
    return Vector2.Distance(tilePosition, position) < sightDistance;
  }
}
