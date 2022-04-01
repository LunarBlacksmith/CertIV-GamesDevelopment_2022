using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : BaseManager
{
    public enum State
    {
        FullHP,
        LowHP,
        Dead
    }

    public State currentState;
    protected PlayerManager _playerManager;
    [SerializeField] protected Animator _anim;

    protected override void Start()
    {
        base.Start();
        _playerManager = GetComponent<PlayerManager>();
        if (_playerManager == null)
        {
            Debug.LogError("Wtf?? PlayerManager not found!?");
        }
    }

    public override void TakeTurn()
    {
        if (_health <= 0)
        {
            currentState = State.Dead;
        }
        else if (_health > 60f)
        {
            currentState = State.FullHP;
        }
        else if (_health < 40f)
        {
            currentState = State.LowHP;
        }

        switch (currentState)
        {
            case State.FullHP:
                FullHPState();
                StartCoroutine(EndTurn());
                break;
            case State.LowHP:
                LowHPState();
                StartCoroutine(EndTurn());
                break;
            case State.Dead:
                Dead();
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
    IEnumerator EndTurn()
    {
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("The AI has ended their turn!");
        _playerManager.TakeTurn();
    }

    #region State Methods
    void FullHPState()
    {
        int randomAttack = Random.Range(1, 10);

        switch (randomAttack)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                BerryBomb();
                break;
            case 6:
            case 7:
            case 8:
                EatBerries();
                break;
            case 9:
                DestructoBerry();
                break;
            default:
                break;
        }
    }
    void LowHPState()
    {
        int randomAttack = Random.Range(1, 10);

        switch (randomAttack)
        {
            case 1:
            case 2:
            case 3:
                BerryBomb();
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                EatBerries();
                break;
            case 8:
            case 9:
                DestructoBerry();
                break;
            default:
                break;
        }
    }
    void Dead()
    {
        currentState = State.Dead;
        _playerManager.TakeTurn();
    }
    #endregion

    #region Moves
    public void EatBerries()
    {
        Debug.Log("AI used EatBerries");
        Heal(20f);
    }
    public void DestructoBerry()
    {
        _anim.SetTrigger("BerryBomb");
        Debug.Log("AI used DestructoBerry");
        DealDamage(_maxHealth);
        _playerManager.DealDamage(80f);
        Dead();
    }
    public void BerryBomb()
    {
        _anim.SetTrigger("BerryBomb");
        Debug.Log("AI used BerryBomb");
        _playerManager.DealDamage(30f);
    }
    public void PoisonBerry()
    {
        StartCoroutine(_DamageOverTime(4f));
        Debug.Log("AI used PoisonBerry");
    }
    private IEnumerator _DamageOverTime(float waitTime)
    {
        for (int i = 0; i < waitTime; i++)
        {
            Debug.Log(i);
            _playerManager.DealDamage(10f);
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion
}
