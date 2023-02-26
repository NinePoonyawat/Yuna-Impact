using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PlayerController character1;
    private PlayerController character2;
    private PlayerController character3;
    private PlayerController character4;

    private PlayerController takingCharacter = null;

    private List<EnemyController> enemies = new List<EnemyController>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (character1.getTaking())
            {
                TakingCharacter(0);
            }
            else
            {
                TakingCharacter(1);
            }
        }
    }

    public void AddCharacter(PlayerController character)
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

    public void AddEnemy(EnemyController enemy)
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

    public PlayerController FindNearestCharacter(Vector3 position,float maxDistance)
    {
        float a1 = (character1.transform.position - position).sqrMagnitude;
        float a2 = (character2 == null)? float.MaxValue : (character2.transform.position - position).sqrMagnitude;
        float a3 = (character3 == null)? float.MaxValue : (character3.transform.position - position).sqrMagnitude;
        float a4 = (character4 == null)? float.MaxValue : (character4.transform.position - position).sqrMagnitude;

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

        if (distance > maxDistance) return null;

        if (state == 1) return character1;
        if (state == 2) return character2;
        if (state == 3) return character3;
        return character4;
    }

    public EnemyController FindNearestEnemy(Vector3 position)
    {
        float distance = float.MaxValue;
        float temp;
        EnemyController toReturn = null;
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

    public EnemyController FindNearestEnemy(PlayerController playableCharacter)
    {
        Vector3 position = playableCharacter.transform.position;
        float distance = float.MaxValue;
        float temp;
        EnemyController toReturn = null;
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
