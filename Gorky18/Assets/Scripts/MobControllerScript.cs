using System.Collections;
using System.Collections.Generic;
using IsoTools;
using IsoTools.Physics;
using UnityEngine;

public class MobControllerScript : BaseIsoMovingController
{

  // Use this for initialization
  void Start()
  {
    base.Start();
  }

  // Update is called once per frame
  void Update()
  {
		if (Vector2.Distance(isoObject.positionXY, floor[5].GetComponent<IsoObject>().positionXY) <= 0.1) {
			return;
		}

    float step = speed * Time.deltaTime;
    isoObject.position = Vector3.MoveTowards(isoObject.position, floor[5].GetComponent<IsoObject>().positionXY, step);
  }
}
