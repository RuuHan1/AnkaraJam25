using UnityEngine;

public class Car : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxfuel;

    private float curentFuel;

    private void Start()
    {
        curentFuel = maxfuel;
    }

    public void TakeDamage(float damage)
    {
        curentFuel -= damage;
        if (curentFuel <= 0) Die();
    }

    public void Die()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}
