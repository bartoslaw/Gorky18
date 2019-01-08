using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsoTools;
using IsoTools.Physics;
using Pathfinding;

public class BaseIsoMovingController : MonoBehaviour
{
  public float speed = 2.0f;
  public float sightDistance = 1.0f;
  protected IsoWorld world;
  protected IsoObject isoObject;
  protected GameObject[] floor;
  protected bool isMoving = false;

  public void Start()
  {
    world = IsoWorld.GetWorld(0);
    floor = GameObject.FindGameObjectsWithTag("Floor");
    isoObject = GetComponent<IsoObject>();
  }
}
