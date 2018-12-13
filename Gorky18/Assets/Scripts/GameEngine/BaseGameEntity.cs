using UnityEngine;
public abstract class BaseGameEntity {
  public int health;
  public int sightDistance;
  public float speed;
  //TODO add new parameters

  public abstract void Attack(BaseGameEntity baseEntity);
  public abstract void Walk(Vector2 position);
  //todo define structure for special entities (switches, computers, quest items?)
  public abstract void SpecialAction();
}