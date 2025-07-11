using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector] public int health;
    public float acceleration;
    public float maxSpeed;
    [HideInInspector] public float shieldDuration = 5f;
    [HideInInspector] public float magnetDuration = 5f;

    public Healthbar healthbar;

    private void Awake()
    {
        health = (int)Mathf.Round(maxHealth * Global.playerUpgradesDict["health"][Global.currentPlayerLevels["health"]]);
        healthbar.SetMaxHealth(health);
    }

    private void Start()
    {
        if (Global.currentPlayerLevels.TryGetValue("shield", out int shieldLevel))    // trying to get shield upgrade level if present in currentPlayerLevels
        {
            shieldDuration += Global.playerUpgradesDict["shield"][shieldLevel];
        }
        if (Global.currentPlayerLevels.TryGetValue("magnet", out int magnetLevel))    // trying to get magnet upgrade level if present in currentPlayerLevels
        {
            magnetDuration += Global.playerUpgradesDict["magnet"][magnetLevel];
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthbar.SetHealth(health);
        print(health);
    }
}
