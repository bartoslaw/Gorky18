using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;
using Pathfinding;

public class PlayerControllerScript : BaseIsoMovingController
{
  bool isAdjustingTiles = false;

  // Use this for initialization
  new void Start()
  {
    base.Start();
    AdjustMovableTiles();
  }

  // Update is called once per frame
  void Update()
  {
    HandleMovement();

    if (!isMoving)
    {
      HandleMouseClick();
      UpdateSelectableTile();
    }
  }

  private void UpdateSelectableTile()
  {
    if (isAdjustingTiles)
    {
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
          tempColor.a = selectedTile == tile ? 0.1f : IsInSight(tileObject.tilePositionXY, isoObject.tilePositionXY) ? 0.5f : 1.0f;
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

      if (baseTile == null || baseTile.isCollider || !IsInSight(tilePosition, playerPosition))
      {
        print(baseTile.isCollider);
        print(Vector2.Distance(playerPosition, mousePosition));
        return;
      }

      path.Clear();
      seeker.StartPath(transform.position, baseTile.transform.position, OnPathComplete);
    }
  }
  override public float GetPositionOffset() {
    return 0.427f;
  }

  override public void CustomLogicOnMovementComplete() {
    AdjustMovableTiles();
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
          bool isNextToPlayer = IsInSight(isoObject.tilePositionXY, positionXY);
          tempColor.a = isNextToPlayer ? 0.5f : 1.0f;

          if (isNextToPlayer)
          {
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

      if (closestDistance == -1.0f || currentDistance < closestDistance)
      {
        tile = isoObject.GetComponent<BaseTile>();
        closestDistance = currentDistance;
      }
    }

    return tile;
  }
}
