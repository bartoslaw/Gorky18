using System.Collections;
using System.Collections.Generic;
using IsoTools;
using IsoTools.Physics;
using UnityEngine;
using Pathfinding;
public class MobControllerScript : BaseIsoMovingController
{
  BaseTile randomizedTile;

  new void Start()
  {
    base.Start();
    StartCoroutine(RandomizeNewTile());
  }

  override public float GetPositionOffset() {
    return 0.0f;
  }

  override public void CustomLogicOnMovementComplete() {
    StartCoroutine(RandomizeNewTile());
  }

  IEnumerator RandomizeNewTile() {
    Vector2 positionXY = GetComponent<IsoObject>().tilePositionXY;
    while (true) {
      randomizedTile = floor[Random.Range(0, floor.Length - 1)].GetComponent<BaseTile>();
      if (!randomizedTile.isCollider && IsInSight(randomizedTile.GetComponent<IsoObject>().tilePositionXY, positionXY)) {
        break;
      }
    }

    path.Clear();
    yield return new WaitForSeconds(2);
    seeker.StartPath(transform.position, randomizedTile.transform.position, OnPathComplete);
  }

  void Update()
  {
    HandleMovement();
  }
}
