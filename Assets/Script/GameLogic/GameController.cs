using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<PlayerController> characters = new List<PlayerController>();

    [SerializeField] private PlayerController takingCharacter = null;

    [SerializeField] private List<EnemyController> enemies = new List<EnemyController>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakingCharacter(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TakingCharacter(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TakingCharacter(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TakingCharacter(3);
        }
    }

    public void AddCharacter(PlayerController character)
    {
        if (characters.Count >= 4) return;
        characters.Add(character);
    }

    public void AddEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);
    }

    public void TakingCharacter(int idx)
    {
        if (takingCharacter == characters[idx])
        {
            takingCharacter.SetTaking(false);
            takingCharacter = null;
            return;
        }

        if (characters[idx] == null) return;
        if (takingCharacter != null) takingCharacter.SetTaking(false);

        takingCharacter = characters[idx];
        takingCharacter.SetTaking(true);
    }

    public PlayerController FindNearestCharacter(Vector3 position, float maxDistance)
    {
        if (characters.Count == 0) return null;

        float distance = maxDistance;
        int state = -1;
        for (int idx = 0; idx < characters.Count; idx++)
        {
            float d = (characters[idx].transform.position - position).sqrMagnitude;
            if (distance > d)
            {
                distance = d;
                state = idx;
            }
        }

        if (state == -1) return null;
        return characters[state];
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
