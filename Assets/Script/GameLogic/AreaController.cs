using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    public int areaIdx;
    public List<PlayerController> areaCharacter;
    public List<EnemyController> areaEnemies;
    public List<TeleportController> teleports;

    public void AddCharacter(PlayerController character)
    {
        areaCharacter.Add(character);
    }

    public void RemoveCharacter(PlayerController character)
    {
        areaCharacter.Remove(character);
    }

    public void AddEnemy(EnemyController enemy)
    {
        areaEnemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        areaEnemies.Remove(enemy);
    }

    public List<EnemyController> FindAllNearestEnemy(Vector3 position, float distance)
    {
        List<EnemyController> toReturn = new List<EnemyController>();
        foreach (var enemy in areaEnemies)
        {
            if ((enemy.transform.position - position).sqrMagnitude <= distance)
            {
                toReturn.Add(enemy);
            }
        }
        return toReturn;
    }

    public EnemyController FindNearestEnemy(Vector3 position)
    {
        float distance = float.MaxValue;
        float temp;
        EnemyController toReturn = null;
        foreach (var enemy in areaEnemies)
        {
            if ((temp = (enemy.transform.position - position).sqrMagnitude) <= distance)
            {
                distance = temp;
                toReturn = enemy;
            }
        }
        return toReturn;
    }

    public TeleportController FindNearestTeleport(Vector3 position)
    {
        TeleportController toReturn = null;
        float distance = float.MaxValue;
        float tmp;

        foreach (var member in teleports)
        {
            if ((tmp = Mathf.Abs(Vector3.Distance(position,member.teleportTransform.position))) < distance)
            {
                distance = tmp;
                toReturn = member;
            }
        }
        return toReturn;
    }

    public List<TeleportController> FindTeleport(int destinate)
    {
        List<TeleportController> toReturn = new List<TeleportController>();
        foreach (var member in teleports)
        {
            if (member.destinateAreaIdx == destinate) toReturn.Add(member);
        }
        return toReturn;
    }

    public TeleportController FindHighGroundTeleport()
    {
        foreach (var member in teleports)
        {
            if (member.isHighGround) return member;
        }
        return null;
    }

    public TeleportController FindHighGroundTeleport(int destinate)
    {
        foreach (var member in teleports)
        {
            if (member.isHighGround && member.destinateAreaIdx == destinate) return member;
        }
        return null;
    }
}