using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : Entity
{
    // Start is called before the first frame update
    [Header ("Character")]
    [SerializeField] private CharacterProfile characterProfile;
    public int blockCount = 3;
    
    public bool Targetable(Enemy enemy)
    {
        return true;
    }

    public override bool isInAttackRange(Vector3 position)
    {
        return Vector3.Distance(position,transform.position) <= attackRange;
    }

    public int getHp()
    {
        return healthController.getValue();
    }

    public int getMaxHp()
    {
        return healthController.getMaxValue();
    }
    
    public CharacterProfile GetProfile()
    {
        return characterProfile;
    }
}
