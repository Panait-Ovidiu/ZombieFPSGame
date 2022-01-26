using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public PlayerScript playerScript;
    [SerializeField] public Transform player;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemies;
    public int numberOfEnemies = 10;
    public int waveNumber = 1;
    public int maxEnemies;
    public int enemiesRemain;

    [SerializeField] private float waveInfoShowTime = 3f;
    [SerializeField] private float waveEndInfoShowTime = 3f;

    private bool waveFinished;

    // Start is called before the first frame update
    void Start()
    {
       // PlayerPrefs.SetInt("waveNumber", 1);
        waveFinished = false;
        waveNumber = PlayerPrefs.GetInt("waveNumber");
        maxEnemies = waveNumber * numberOfEnemies;
        enemiesRemain = maxEnemies;
        playerScript.guiManager.setEnemies(maxEnemies, enemiesRemain);
        playerScript.guiManager.waveInfo.SetActive(true);
        playerScript.guiManager.waveText.text = "Wave " + waveNumber;
        Invoke("HideWaveInfo", waveInfoShowTime);
        Vector3 pos = new Vector3(58, -11, 6);
        for (int i = 0; i < enemiesRemain; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            newEnemy.GetComponent<AiScript>().playerScript = playerScript;
            newEnemy.GetComponent<AiScript>().player = player;
            newEnemy.GetComponent<AiScript>().gameManager = this;
            newEnemy.transform.SetParent(enemies);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!waveFinished)
        {
            if (enemiesRemain == 0)
            {
                waveFinished = true;

                playerScript.guiManager.waveInfo.SetActive(true);
                playerScript.guiManager.waveText.text = "Congrats Next Wave";

                Invoke("NextLevel", waveEndInfoShowTime);
            }

            if (playerScript.health == 0)
            {
                waveFinished = true;

                playerScript.guiManager.waveInfo.SetActive(true);
                playerScript.guiManager.waveText.text = "You lost !";
                Invoke("RestartLevel", waveEndInfoShowTime);
            }
        }

    }

    public void DecreseEnemies()
    {
        enemiesRemain--;
        playerScript.guiManager.setEnemies(maxEnemies, enemiesRemain);
    }

    public void HideWaveInfo()
    {
        playerScript.guiManager.waveInfo.SetActive(false);
    }

    public void NextLevel()
    {
        waveNumber++;
        PlayerPrefs.SetInt("waveNumber", waveNumber);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
