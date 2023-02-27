using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

    private enum WaveState { WAITING, SPAWNING, PLAYING };

    [SerializeField] private WaveEnemyInfo[] waveEnemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenWaves = 5f;

    private WaveState state;
    private EnemySpawner[] enemySpawners;
    private int currentWave = 0; // TODO: Display wave (and maybe countdown)
    private float waveCountdown;
    private float waveCheckDelay = 1f;

    private void Start() {
        waveCountdown = timeBetweenWaves;
        enemySpawners = new EnemySpawner[waveEnemies.Length];
        state = WaveState.WAITING;
    }

    private void Update() {
        switch (state) {
            case WaveState.WAITING:
                waveCountdown -= Time.deltaTime;
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
                    state = WaveState.WAITING;
                    waveCountdown = timeBetweenWaves;
                }
                break;
        }
    }

    private void StartNewWave() {
        ++currentWave;
        if (currentWave == 1) SetupFirstWave();
        else SetupNextWave();

        state = WaveState.SPAWNING;
        for (int i = 0; i < enemySpawners.Length; ++i) {
            enemySpawners[i].DoneSpawning = false;
            if (waveEnemies[i].startWave <= currentWave) {
                StartCoroutine(enemySpawners[i].Spawn());
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
                scaledAmount = Mathf.FloorToInt(waveOffset * info.amountMultiplier * info.startAmount);
                scaledRate = Mathf.FloorToInt(waveOffset * info.rateMultiplier * info.startRate);
            }

            spawner.Amount = Mathf.Min(scaledAmount, info.maxAmount);
            spawner.Rate = Mathf.Min(scaledRate, info.maxRate);
        }
    }

    bool IsWaveOver()
    {
        waveCheckDelay -= Time.deltaTime;
        if (waveCheckDelay <= 0f) {
            return GameObject.FindGameObjectWithTag("Virus") == null;
        }
        return false;
    }
}

[System.Serializable] // TODO: field defaults not appearing in unity
public class WaveEnemyInfo
{
    public Transform enemy;
    [SerializeField] public int startAmount = 1;
    [SerializeField] public float startRate = 1f;
    [SerializeField] public int startWave = 1;
    [SerializeField] public float amountMultiplier = 1.1f;
    [SerializeField] public float rateMultiplier = 1.1f;
    [SerializeField] public int maxAmount = 200;
    [SerializeField] public float maxRate = 10f;
}

internal class EnemySpawner : ScriptableObject {
    private Transform enemy;
    private Transform[] spawnPoints;
    public int Amount { private get; set; }
    public float Rate { private get; set; }
    public bool DoneSpawning { get; set; }

    public void Init(Transform enemy, int amount, float rate, Transform[] spawnPoints) {
        this.enemy = enemy;
        this.Amount = amount;
        this.Rate = rate;
        this.spawnPoints = spawnPoints;
        this.DoneSpawning = false;
    }

    public static EnemySpawner CreateInstance(Transform enemy, int amount, float rate, Transform[] spawnPoints) {
        EnemySpawner spawner = ScriptableObject.CreateInstance<EnemySpawner>();
        spawner.Init(enemy, amount, rate, spawnPoints);
        return spawner;
    }

    public IEnumerator Spawn() {for (int i = 0; i < Amount; ++i) {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemy, sp.position, sp.rotation);

            yield return new WaitForSeconds( 1f / Rate );
        }

        this.DoneSpawning = true;
        yield break;
    }

}
