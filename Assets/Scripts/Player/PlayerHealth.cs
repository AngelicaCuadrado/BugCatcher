using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    public int CurrentHealth { get; private set; }

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip damageSound;

    [Header("Optional Debug")]
    public bool printDebug = true;

    private void Start()
    {
        CurrentHealth = maxHealth;

        // Auto-assign if missing
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (printDebug)
        {
            Debug.Log($"[PlayerHealth] Start with {CurrentHealth}/{maxHealth}");
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log($"[PlayerHealth] Took {amount} damage → {CurrentHealth}/{maxHealth}");
        if (amount <= 0) return;
        if (CurrentHealth <= 0) return; // already dead

        // Play Damage Audio
        if (audioSource != null && damageSound != null)
        {
            // Reset pitch in case it was stuck on high from running
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(damageSound);
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, maxHealth);

        if (printDebug)
        {
            Debug.Log($"[PlayerHealth] Took {amount} damage → {CurrentHealth}/{maxHealth}");
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (CurrentHealth <= 0) return; // can't heal dead player

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);

        if (printDebug)
        {
            Debug.Log($"[PlayerHealth] Healed {amount} → {CurrentHealth}/{maxHealth}");
        }
    }
    private void Die()
    {
        if (printDebug)
        {
            Debug.Log("[PlayerHealth] Player died");
        }

        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log($"[PlayerHealth] Loading LoseScreen from {currentScene.name}");
        SceneManager.LoadScene("LoseScreen");
    }
}