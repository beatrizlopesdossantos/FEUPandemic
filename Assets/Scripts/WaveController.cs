using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveController : MonoBehaviour {

    private enum WaveState { WAITING, SPAWNING, PLAYING };

    [SerializeField] private WaveEnemyInfo[] waveEnemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private Text waveText;
    [SerializeField] private AudioSource waveStartSound;
    [SerializeField] private AudioSource waveEndSound;

    private WaveState state;
    private EnemySpawner[] enemySpawners;
    private int currentWave = 0;
    private float waveCountdown;
    private float waveCheckDelay = 1f;
    private float ENEMY_SPAWNER_DELAY = 0.5f;

    private void Start() {
        waveCountdown = timeBetweenWaves;
        enemySpawners = new EnemySpawner[waveEnemies.Length];
        state = WaveState.WAITING;
        updateWaveText();
    }

    private void Update() {
        switch (state) {
            case WaveState.WAITING:
                waveCountdown -= Time.deltaTime;
                if (waveCountdown < waveStartSound.clip.length / 2 && !waveStartSound.isPlaying) {
                    waveStartSound.Play();
                }

                if (waveCountdown <= 0) {
                    StartNewWave();
                }
                break;
            case WaveState.SPAWNING:
                bool allDoneSpawning = true;
                for (int i = 0; i < enemySpawners.Length; ++i) {
                    if (!enemySpawners[i].DoneSpawning) {
                        allDoneSpawning = false;
                        break;
                    }
                }
                if (allDoneSpawning) state = WaveState.PLAYING;
                break;
            case WaveState.PLAYING:
                if (IsWaveOver()) {
                    waveEndSound.Play();
                    state = WaveState.WAITING;
                    waveCountdown = timeBetweenWaves;
                }
                break;
        }
        updateWaveText();
    }

    private void StartNewWave() {
        ++currentWave;
        if (currentWave == 1) SetupFirstWave();
        else SetupNextWave();

        state = WaveState.SPAWNING;
        for (int i = 0; i < enemySpawners.Length; ++i) {
            if (waveEnemies[i].startWave <= currentWave) {
                enemySpawners[i].DoneSpawning = false;
                StartCoroutine(enemySpawners[i].Spawn(i * ENEMY_SPAWNER_DELAY));
            }
        }
    }

    private void SetupFirstWave() {
        for (int i = 0; i < waveEnemies.Length; ++i) {
            WaveEnemyInfo info = waveEnemies[i];
            enemySpawners[i] = EnemySpawner.CreateInstance(info.enemy, info.startAmount, info.startRate, spawnPoints);
        }
    }

    private void SetupNextWave() {
        for (int i = 0; i < waveEnemies.Length; ++i) {
            WaveEnemyInfo info = waveEnemies[i];
            EnemySpawner spawner = enemySpawners[i];

            int waveOffset = currentWave - info.startWave;

            int scaledAmount; float scaledRate;
            if (waveOffset <= 0) {
                scaledAmount = info.startAmount;
                scaledRate = info.startRate;
            }
            else {
                scaledAmount = Mathf.FloorToInt(spawner.Amount * info.amountMultiplier);
                scaledRate = spawner.Rate * info.rateMultiplier;
            }

            spawner.Amount = Mathf.Min(scaledAmount, info.maxAmount);
            spawner.Rate = Mathf.Max(Mathf.Min(scaledRate, info.maxRate), info.minRate);
        }
    }

    private bool IsWaveOver()
    {
        waveCheckDelay -= Time.deltaTime;
        if (waveCheckDelay <= 0f) {
            return GameObject.FindGameObjectWithTag("Virus") == null;
        }
        return false;
    }

    private void updateWaveText() {
        switch (state)
        {
            case WaveState.WAITING:
                waveText.text = $"Next wave in: {waveCountdown:0.00}";
                break;
            case WaveState.SPAWNING:
            case WaveState.PLAYING:
                waveText.text = $"Wave {currentWave}";
                break;
        }
    }
}

// Default values work unexpectedly: https://issuetracker.unity3d.com/issues/serializefield-list-objects-are-not-initialized-with-class-slash-struct-default-values-when-adding-objects-in-the-inspector-window
[System.Serializable]
public class WaveEnemyInfo
{
    public Transform enemy;
    public int startAmount = 1;
    public float startRate = 1f;
    public int startWave = 1;
    public float amountMultiplier = 1.1f;
    public float rateMultiplier = 1.1f;
    public int maxAmount = 200;
    public float maxRate = 10f;
    public float minRate = 0.1f;
}

internal class EnemySpawner : ScriptableObject {
    private Transform enemy;
    private Transform[] spawnPoints;
    public int Amount { get; set; }
    public float Rate { get; set; }
    public bool DoneSpawning { get; set; }

    public void Init(Transform enemy, int amount, float rate, Transform[] spawnPoints) {
        this.enemy = enemy;
        this.Amount = amount;
        this.Rate = rate;
        this.spawnPoints = spawnPoints;
        this.DoneSpawning = true;
    }

    public static EnemySpawner CreateInstance(Transform enemy, int amount, float rate, Transform[] spawnPoints) {
        EnemySpawner spawner = ScriptableObject.CreateInstance<EnemySpawner>();
        spawner.Init(enemy, amount, rate, spawnPoints);
        return spawner;
    }

    public IEnumerator Spawn(float? delay = null) {
        // insert delay to prevent enemies from spawning on top of each other
        if (delay != null) yield return new WaitForSeconds(delay.Value);
        for (int i = 0; i < Amount; ++i) {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemy, sp.position, sp.rotation);

            yield return new WaitForSeconds( 1f / Rate );
        }

        this.DoneSpawning = true;
        yield break;
    }

}
