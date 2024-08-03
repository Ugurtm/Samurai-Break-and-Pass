using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    // Health
    public int maxHealth = 100;
    public int currentHealth = 0;
    public HealthBar healthBar;
    public GameObject gameOverScreen;
    private CharacterController playerSmr;


    // EnemySpacing

    public bool enemyattack;

    public float enemytimer;

    public Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        enemytimer = 1.5f;

        anim = GetComponent<Animator>();

        playerSmr = FindObjectOfType<CharacterController>();
    }

    // Düþmanýn bize zarar verme aralýðý
    void EnemyAttackSpacing() 
    {
        if (enemyattack == false) 
        {
            enemytimer -= Time.deltaTime;
        }
        if (enemytimer < 0) 
        {
            enemytimer = 0f;
        }
        if (enemytimer == 0f) 
        {
            enemyattack = true;
            enemytimer = 1.5f;
        }
    }

    // Düþmaný kitlemek
    void CharacterDamage() 
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            enemyattack = false;
        }
    }


    // Karakterimizin zarar görmesi
    public void TakeDamage(int damage)
    {
        if (enemyattack) 
        {
            currentHealth -= 20;
            enemyattack = false;
            anim.SetTrigger("isHit");
        }

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            Die();
        }


    }

    void Die()
    {
        anim.SetBool("isDead", true);

        GetComponent<CharacterMove>().enabled = false;

        Destroy(gameObject, 2.0f);
    }



    // Update is called once per frame
    void Update()
    {
        EnemyAttackSpacing();
        CharacterDamage();
        GameOver();

      // if (Input.GetKeyDown(KeyCode.Z)) 
      // {
      //     TakeDamage(20);
      // }
    }

    void GameOver() 
    {
        if (currentHealth <= 0) 
        {
            gameOverScreen.SetActive(true);
            playerSmr.gameObject.SetActive(false);
        }
    }
}
