using UnityEngine;

public class Car : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth;

    private float curentHealth;

    private void Start()
    {
        curentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        curentHealth -= damage;
        if (curentHealth <= 0) Die();
    }

    public void Die()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}
