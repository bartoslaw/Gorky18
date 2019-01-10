using System.Collections;
using System.Collections.Generic;
using IsoTools;
using IsoTools.Physics;
using UnityEngine;
using Pathfinding;
public class MobControllerScript : BaseIsoMovingController
{
  BaseTile randomizedTile;

  void Start()
  {
    base.Start();
    RandomizeNewTile();
  }

  override public float GetPositionOffset() {
    return 0.0f;
  }

  override public void CustomLogicOnMovementComplete() {
    RandomizeNewTile();
  }

  private void RandomizeNewTile() {
    while (true) {
      randomizedTile = floor[Random.Range(0, floor.Length - 1)].GetComponent<BaseTile>();
      if (!randomizedTile.isCollider) {
        break;
      }
    }

    path.Clear();
    seeker.StartPath(transform.position, randomizedTile.transform.position, OnPathComplete);
  }

  void Update()
  {
    HandleMovement();
  }
}
