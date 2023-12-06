using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private float maxShootDelay = 0.2f;
    [SerializeField]
    private float currentShootDelay = 0.2f;
    private int maxBullet = 30;
    private int currentBullet = 0;
    [SerializeField]
    private Text bulletText;

    [Header("Weapon FX")]
    [SerializeField]
    private GameObject weaponFlashFX;
    [SerializeField]
    private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;
    [SerializeField]
    private Transform weaponClipPoint;
    [SerializeField]
    private GameObject weaponClipFX;

    [Header("Enemy")]
    [SerializeField]
    private GameObject[] spawnPoint;

    [Header("BGM")]
    [SerializeField]
    private AudioClip bgmSound;
    private AudioSource BGM;

    private PlayableDirector cut;
    public bool isReady = true;

    void Start()
    {
        instance = this;

        currentShootDelay = 0;
        cut = GetComponent<PlayableDirector>();
        cut.Play();

        InitBullet();
    }

    void Update()
    {
        bulletText.text = currentBullet + " / " + maxBullet;
    }

    public void Shooting(Vector3 targetPosition, Enemy enemy, AudioSource weaponSound, AudioClip shootingSound)
    {
        currentShootDelay += Time.deltaTime;

        if (currentShootDelay < maxShootDelay || currentBullet <= 0)
            return;

        currentBullet--;
        currentShootDelay = 0;

        weaponSound.clip = shootingSound;
        weaponSound.Play();

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;

        GameObject flashFX = PoolManager.instance.ActivateObj(1); // »ç°Ý ÀÌÆåÆ® FX
        SetObjPosition(flashFX, bulletPoint);
        flashFX.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        GameObject caseFX = PoolManager.instance.ActivateObj(2); // ÅºÇÇ FX
        SetObjPosition(caseFX, bulletCasePoint);

        GameObject prefabToSpawn = PoolManager.instance.ActivateObj(0); // ÃÑ¾Ë ¿ÀºêÁ§Æ®
        SetObjPosition(prefabToSpawn, bulletPoint);
        prefabToSpawn.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        //if (enemy != null && enemy.enemyCurrentHP > 0) // Raycast
        //{
        //    enemy.enemyCurrentHP--;
        //    Debug.Log("enemy HP : " + enemy.enemyCurrentHP);
        //}
    }

    public void ReroadClip()
    {
        GameObject clipFX = PoolManager.instance.ActivateObj(3); // ÅºÃ¢ FX
        SetObjPosition(clipFX, weaponClipPoint);

        InitBullet();
    }
    
    void InitBullet()
    {
        currentBullet = maxBullet;
    }

    void SetObjPosition(GameObject obj, Transform targetTransform)
    {
        obj.transform.position = targetTransform.position;
    }

    IEnumerator EnemySpawn()
    {
        GameObject enemy = PoolManager.instance.ActivateObj(4);
        SetObjPosition(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform);

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemySpawn());
    }

    void PlayBGMSound()
    {
        BGM = GetComponent<AudioSource>();
        BGM.clip = bgmSound;
        BGM.loop = true;
        BGM.Play();
    }

    public void StartGame()
    {
        isReady = false;
        PlayBGMSound();
        StartCoroutine(EnemySpawn());
    }
}