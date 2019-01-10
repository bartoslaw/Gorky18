using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;
using Pathfinding;

abstract public class BaseIsoMovingController : MonoBehaviour
{
  public float speed = 2.0f;
  public float sightDistance = 1.0f;
  protected IsoWorld world;
  protected IsoObject isoObject;
  protected GameObject[] floor;
  protected bool isMoving = false;
  protected Seeker seeker;
  protected List<Vector3> path;

  abstract public float GetPositionOffset();
  abstract public void CustomLogicOnMovementComplete();
  public void Start()
  {
    world = IsoWorld.GetWorld(0);
    floor = GameObject.FindGameObjectsWithTag("Floor");
    isoObject = GetComponent<IsoObject>();
    seeker = GetComponent<Seeker>();
    path = new List<Vector3>();
  }

  public void HandleMovement()
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
          CustomLogicOnMovementComplete();
          isMoving = false;
        }
      }
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
        path.Add(world.ScreenToIso(new Vector2(point.x - GetPositionOffset(), point.y)));
      }
    }
  }
}
