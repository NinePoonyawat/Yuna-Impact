using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<PlayerController> characters = new List<PlayerController>();
    [SerializeField] private PlayerController takingCharacter = null;
    [SerializeField] private List<EnemyController> enemies = new List<EnemyController>();
    [SerializeField] public List<AreaController> areas = new List<AreaController>();

    [SerializeField] public Transform projectileRoot;

    [Header ("Visualize and UI")]
    [SerializeField] private CamFollowing cam;
    [SerializeField] private UIController uiController;
    [SerializeField] private UITarget uiTarget;

    void Awake()
    {
        uiController = FindObjectOfType<UIController>();
        cam = FindObjectOfType<CamFollowing>();
        uiTarget = FindObjectOfType<UITarget>();
    }

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
        if (characters.Count == 1)
        {
            FindObjectOfType<CamFollowing>().Follow(character.gameObject.transform);
        }
        TakingCharacter(0);
    }

    public void AddEnemy(EnemyController enemy,int idx)
    {
        areas[idx].areaEnemies.Add(enemy);
    }

    public void DeleteEnemy(EnemyController enemyController,int idx)
    {
        areas[idx].areaEnemies.Remove(enemyController);
    }

    public void TakingCharacter(int idx)
    {
        if (idx >= characters.Count) return;
        if (takingCharacter == characters[idx])
        {
            takingCharacter.SetTaking(false);
            takingCharacter = null;

            uiController.SetProfile(characters);
            
            return;
        }

        if (characters[idx] == null) return;
        if (takingCharacter != null) takingCharacter.SetTaking(false);

        takingCharacter = characters[idx];
        takingCharacter.SetTaking(true);

        uiController.SetProfile(characters);
        cam.Follow(takingCharacter.gameObject.transform);
    }

    public PlayerController FindNearestCharacter(Vector3 position, float maxDistance,int area)
    {
        if (characters.Count == 0) return null;

        float distance = maxDistance;
        int state = -1;
        for (int idx = 0; idx < characters.Count; idx++)
        {
            if (characters[idx].currentArea != area) continue;
            if (characters[idx].GetEntityState() == EntityState.DEAD) continue;
            
            float d = Vector3.Distance(characters[idx].transform.position,position);
            if (distance > d)
            {
                distance = d;
                state = idx;
            }
        }

        if (state == -1) return null;
        return characters[state];
    }

    public List<EnemyController> FindAllNearestEnemy(Vector3 position, float distance)
    {
        List<EnemyController> toReturn = new List<EnemyController>();
        foreach (var enemy in enemies)
        {
            if ((enemy.transform.position - position).sqrMagnitude <= distance)
            {
                toReturn.Add(enemy);
            }
        }
        return toReturn;
    }

    public EnemyController FindNearestEnemy(Vector3 position,int idx)
    {
        float distance = float.MaxValue;
        float temp;
        EnemyController toReturn = null;
        foreach (var enemy in areas[idx].areaEnemies)
        {
            if ((temp = (enemy.transform.position - position).sqrMagnitude) <= distance)
            {
                distance = temp;
                toReturn = enemy;
                uiTarget.SetTarget(enemy);
            }
        }
        return toReturn;
    }

    public EnemyController FindNearestEnemy(Vector3 position,int idx,float maxDistance)
    {
        float distance = float.MaxValue;
        float temp;
        EnemyController toReturn = null;
        foreach (var enemy in areas[idx].areaEnemies)
        {
            if ((temp = Vector3.Distance(enemy.transform.position,position)) <= distance)
            {
                distance = temp;
                toReturn = enemy;
            }
        }
        return (distance <= maxDistance)? toReturn : null;
    }

    public EnemyController FindNearestEnemy(PlayerController playableCharacter)
    {
        Vector3 position = playableCharacter.transform.position;
        float distance = float.MaxValue;
        float temp;
        EnemyController toReturn = null;
        Debug.Log(playableCharacter.currentArea);
        foreach (var enemy in areas[playableCharacter.currentArea].areaEnemies)
        {
            if (playableCharacter.Targetable(enemy) && (temp = (enemy.transform.position - position).sqrMagnitude) <= distance)
            {
                distance = temp;
                toReturn = enemy;
            }
        }
        return toReturn;
    }

    public TeleportController FindTeleport(Vector3 pos,int idx)
    {
        return areas[idx].FindNearestTeleport(pos);
    }

    public int getUIIndex(PlayerController character)
    {
        if (character == takingCharacter) return 0;
        int m = 1;
        for(int i = 0; i != 4; i++)
        {
            if (characters[i] == character) return i + m;
            if (characters[i] == takingCharacter) m = 0;
        }
        return 3;
    }
}
