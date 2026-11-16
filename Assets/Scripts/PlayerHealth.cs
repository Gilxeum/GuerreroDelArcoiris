using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;

    [Header("Blink Settings")]
    [SerializeField] private float blinkDuration = 0.5f;
    [SerializeField] private Color blinkColor = Color.red;

    private bool isVulnerable = true;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void TakeDamage()
    {
        if (isVulnerable)
        {
            currentHealth --;
            if (currentHealth <= 0)
            {
                //TODO
            }
            else
            {
                StartCoroutine(Blink(4));
            }
        }
    }


    private IEnumerator Blink(int numberofBlinks)
    {
        isVulnerable = false;
        do
        {
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkDuration);
            numberofBlinks--;

        } while (numberofBlinks>0);
        isVulnerable = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }



}
