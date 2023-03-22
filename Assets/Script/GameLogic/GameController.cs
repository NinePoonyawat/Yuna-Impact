using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    [SerializeField] public List<PlayerController> characters = new List<PlayerController>();
    [SerializeField] private PlayerController takingCharacter = null;
    [SerializeField] private List<EnemyController> enemies = new List<EnemyController>();
    [SerializeField] public List<AreaController> areas = new List<AreaController>();

    [SerializeField] public Transform projectileRoot;

    public int enemyNum = 0;
    public int enemyKilled = 0;

    private bool isTakingAll = false;

    public string currentScene;
    public string nextScene;

    [Header ("Visualize and UI ")]
    [SerializeField] private CamFollowing cam;
    public Camera camera;
    [SerializeField] private UIController uiController;
    [SerializeField] private UITarget uiTarget;

    public GameObject WinUI;
    public GameObject LoseUI;

    public TMP_Text enemyCount;
    public TMP_Text killCount;

    private LayerMask layerClickMask;

    void Awake()
    {
        uiController = FindObjectOfType<UIController>();
        cam = FindObjectOfType<CamFollowing>();
        uiTarget = FindObjectOfType<UITarget>();
        camera = FindObjectOfType<Camera>();
        //uiController.SetProfile(characters);
        FindObjectOfType<CamFollowing>().Follow(characters[0].gameObject.transform);

        layerClickMask = LayerMask.GetMask("Entity","PlayerClickable");
    }

    void Start()
    {
        uiController.SetProfile(characters);
    }

    void Update()
    {
        if (isGameLose())
        {
            //Time.timeScale = 0f;
            LoseUI.SetActive(true);
        }
        else if (isGameWin())
        {
            //Time.timeScale = 0f;
            WinUI.SetActive(true);
        }

        if (isTakingAll)
        {
            UpdatePlayerClick();
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !isTakingAll)
        {
            isTakingAll = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isTakingAll = false;
            TakingCharacter(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isTakingAll = false;
            TakingCharacter(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isTakingAll = false;
            TakingCharacter(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isTakingAll = false;
            TakingCharacter(3);
        }
    }

    public void UpdatePlayerClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                for (int i = 0; i != characters.Count; i++)
                {
                    Vector3 destination = hit.point;
                    if (i == 1)
                    {
                        destination.x += 3;
                    }
                    else if (i == 2)
                    {
                        destination.x -= 3;
                    }
                    else if (i == 3)
                    {
                        destination.z += 3;
                    }
                    else
                    {
                        destination.z -= 3;
                    }
                    characters[i].agent.SetDestination(hit.point);
                    characters[i].moveToPos = hit.point;
                    characters[i].isPlayerMoving = true;
                    if(characters[i].animator != null) characters[i].animator.SetBool("isWalk",true);
                    characters[i].SetEntityState(EntityState.MOVE);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit,999f,layerClickMask))
            {
                IPlayerClickable playerClick = hit.transform.GetComponent<IPlayerClickable>();
                if (playerClick != null)
                {
                    for(int i = 0; i != characters.Count; i++) playerClick.click(characters[i]);
                }
                
                EnemyController enim = hit.transform.GetComponent<EnemyController>();
                if (enim != null)
                {
                    for(int i = 0; i != characters.Count; i++)
                    {
                        SetNewTarget(enim, i);
                        characters[i].focusEnemy = enim;
                        characters[i].isPlayerTarget = true;
                        if (characters[i].isPlayerMoving) characters[i].isPlayerMoving = false;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnemyController enim = FindNearestEnemy(transform.position,characters[0].currentArea);
            if (enim != null)
            {
                for(int i = 0; i != characters.Count; i++)
                {
                    SetNewTarget(enim, i);
                    characters[i].focusEnemy = enim;
                    characters[i].isPlayerTarget = true;
                    if (characters[i].isPlayerMoving) characters[i].isPlayerMoving = false;
                }
            }
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
        enemyNum++;
        //UpdateEnemyPanel();
    }

    public void DeleteEnemy(EnemyController enemyController,int idx)
    {
        areas[idx].areaEnemies.Remove(enemyController);
        enemyKilled++;
        //UpdateEnemyPanel();
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

        if (takingCharacter.focusEnemy != null) SetNewTarget(takingCharacter.focusEnemy, idx);
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

    public PlayerController FindBestCharacter(Vector3 position, float maxDistance,int area)
    {
        List<PlayerController> players = new List<PlayerController>(areas[area].areaCharacter);
        if (players.Count == 0) return null;

        players.Sort(delegate(PlayerController x, PlayerController y)
        {
            if (Vector3.Distance(position,x.transform.position) < Vector3.Distance(position,y.transform.position)) return -1;
            else return 1;
        });

        for (int idx = 0; idx < players.Count; idx++)
        {
            if (players[idx].GetEntityState() == EntityState.DEAD) continue;
            if (players[idx].GetEntityState() == EntityState.PLAYERSTART) continue;
            
            if (players[idx].IsBlockable() && Vector3.Distance(position,players[idx].transform.position) <= maxDistance)
            {
                return players[idx];
            }
        }
        return null;
    }

    public List<PlayerController> FindAllCharacter()
    {
        return FindAllCharacter(Vector3.zero, -1f);
    }

    public List<PlayerController> FindAllCharacter(Vector3 position, float distance)
    {
        if (characters.Count == 0) return null;

        List<PlayerController> toReturn = new List<PlayerController>();
        for (int idx = 0; idx < characters.Count; idx++)
        {
            if (characters[idx].GetEntityState() == EntityState.DEAD) continue;
            
            float d = Vector3.Distance(characters[idx].transform.position, position);
            if (d < distance || distance == -1)
            {
                toReturn.Add(characters[idx]);
            }
        }
        return toReturn;
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
            }
        }
        return toReturn;
    }

    public void SetNewTarget(EnemyController enemy, int idx)
    {
        uiTarget.SetTarget(enemy, idx);
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
        for(int i = 0; i != 4; i++)
        {
            if (characters[i] == character) return i;
        }
        return 0;
    }

    public bool isGameWin()
    {
        return areas[areas.Count - 1].isAreaClear();
    }

    public bool isGameLose()
    {
        for(int i = 0; i != characters.Count; i++)
        {
            if (characters[i].entityState != EntityState.DEAD) return false;
        }
        return true;
    }

    public void toNextLevel()
    {
        StartCoroutine(SwitchScene(nextScene));
    }

    public void RestartLevel()
    {
        StartCoroutine(SwitchScene(currentScene));
    }

    public void toMenu()
    {
        Debug.Log("click");
        StartCoroutine(SwitchScene("Lobby"));
    }

    public void UpdateEnemyPanel()
    {
        enemyCount.text = enemyNum + "";
        killCount.text = enemyKilled + "";
    }

    IEnumerator SwitchScene(string s)
    {
        //yield return new WaitForSeconds(2f);
        Debug.Log(s);
        SceneManager.LoadScene(s);
        yield return null;
    }
}
