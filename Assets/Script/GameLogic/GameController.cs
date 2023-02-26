using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PlayableCharacter character1;
    private PlayableCharacter character2;
    private PlayableCharacter character3;
    private PlayableCharacter character4;

    private PlayableCharacter takingCharacter = null;

    private List<Enemy> enemies = new List<Enemy>();

    public void AddCharacter(PlayableCharacter character)
    {
        if (character1 == null)
        {
            character1 = character;
            return;
        }
        if (character2 == null)
        {
            character2 = character;
            return;
        }
        if (character3 == null)
        {
            character3 = character;
            return;
        }
        if (character4 == null)
        {
            character4 = character;
            return;
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void TakingCharacter(int character)
    {
        if (takingCharacter != null)
        {
            takingCharacter.SetTaking(false);
            takingCharacter = null;
        }
        switch (character)
        {
            case 1:
                takingCharacter = character1;
                break;
            case 2:
                takingCharacter = character2;
                break;
            case 3:
                takingCharacter = character3;
                break;
            case 4:
                takingCharacter = character4;
                break;
        }
        takingCharacter.SetTaking(true);
    }

    public PlayableCharacter FindNearestCharacter(Vector3 position)
    {
        float a1 = (character1.transform.position - position).sqrMagnitude;
        float a2 = (character2.transform.position - position).sqrMagnitude;
        float a3 = (character3.transform.position - position).sqrMagnitude;
        float a4 = (character4.transform.position - position).sqrMagnitude;

        float distance = a1;
        int state = 1;

        if (distance > a2)
        {
            distance = a2;
            state = 2;
        }
        if (distance > a3)
        {
            distance = a3;
            state = 3;
        }
        if (distance > a4)
        {
            distance = a4;
            state = 4;
        }

        if (state == 1) return character1;
        if (state == 2) return character2;
        if (state == 3) return character3;
        return character4;
    }

    public Enemy FindNearestEnemy(Vector3 position)
    {
        float distance = float.MaxValue;
        float temp;
        Enemy toReturn = null;
        foreach (var enemy in enemies)
        {
            if ((temp = (enemy.transform.position - position).sqrMagnitude) <= distance)
            {
                distance = temp;
                toReturn = enemy;
            }
        }
        return toReturn;
    }

    public Enemy FindNearestEnemy(PlayableCharacter playableCharacter)
    {
        Vector3 position = playableCharacter.transform.position;
        float distance = float.MaxValue;
        float temp;
        Enemy toReturn = null;
        foreach (var enemy in enemies)
        {
            if (playableCharacter.Targetable(enemy) && (temp = (enemy.transform.position - position).sqrMagnitude) <= distance)
            {
                distance = temp;
                toReturn = enemy;
            }
        }
        return toReturn;
    }
}
