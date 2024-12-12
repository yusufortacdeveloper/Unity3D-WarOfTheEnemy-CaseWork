using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour, IEventListener
{
    public int health = 100;
    public bool isDamagable = true;
    public Image hurtPanel;

    public AudioSource hurtSoundSfx; 
    private void Start()
    {
        SetHurtPanelAlpha(0);
    }
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(this);
    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(this);
    }
    public void DecreaseHealth(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (isDamagable)
            {
                StartCoroutine(DeadPlayer());
                isDamagable = false;
            }
        }
        else
        {
            if (isDamagable)
            {
                StartCoroutine(HurtPlayer());
            }
        }
    }
    IEnumerator HurtPlayer()
    {
        hurtPanel.DOFade(0.2f, 0.2f);
        hurtSoundSfx.Play();
        yield return new WaitForSeconds(0.2f);
        hurtPanel.DOFade(0f, 1f);
        hurtSoundSfx.Stop();
    }
    IEnumerator DeadPlayer()
    {
        GameStarter.Instance.LoseGame();
        yield return new WaitForSeconds(1f);
    }
    private void SetHurtPanelAlpha(float alpha)
    {
        Color color = hurtPanel.color;
        color.a = alpha;
        hurtPanel.color = color;
    }
}
