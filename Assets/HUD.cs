using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private int maxHealth;
    private float maxSize;
    private GameObject healthBar;

    // Use this for initialization
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        player.PlayerDamaged += UpdateHealth;

        healthBar = GetHealthBar();
        maxHealth = player.maxHealth;
        if (healthBar != null)
            maxSize = healthBar.GetComponent<RectTransform>().rect.width;
    }

    private void UpdateHealth(object sender, PlayerDamagedEventArgs args)
    {
        if (healthBar == null) return;

        healthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxSize * args.Health / maxHealth);
    }

    private GameObject GetHealthBar()
    {
        foreach(Transform t in transform)
        {
            if (t.CompareTag("Health"))
                return t.gameObject;
        }

        return null;
    }

    private void OnDestroy()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        player.PlayerDamaged -= UpdateHealth;
    }
}
