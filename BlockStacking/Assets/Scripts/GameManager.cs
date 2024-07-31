using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform blockHolder;

    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [SerializeField] private GameObject restartButton;
    [SerializeField] private Transform blockStack;
    [SerializeField] private Camera camera;


    private Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;
    //private SpriteRenderer spriteRenderer;

    private Vector2 blockStartPosition = new Vector2(0f, 4f);

    private float blockSpeed = 8f;
    private float blockSpeedIncrement = 0.4f;
    private float blockDirection = 1;
    private float xLimit = 5f;

    private float lastTimeSpawned;
    private float timeBetweenSpawn = 1f;

    public bool playing = true;
    
    private int score = 0;

    public Transform[] prefabs;
    public AudioClip[] musicClips;
    private AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SpawnNewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            MoveBlock();
            DropBlock();
        }
        if (!playing)
        {
            // endGame;
            EndGame();
        }

    }

    private void SpawnNewBlock()
    {
        //currentBlock = Instantiate(blockPrefab, blockHolder);
        //Transform block = GetRandomPrefab();
        float randomRotation = UnityEngine.Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, randomRotation);

        currentBlock = Instantiate(GetRandomPrefab(), blockHolder.position, rotation, blockStack);
        currentBlock.position = blockStartPosition;
        //currentBlock.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentBlock.GetComponent<Rigidbody2D>();
        //spriteRenderer = currentBlock.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = testSprite;
        //spriteRenderer.drawMode = SpriteDrawMode.Sliced;

        // spriteRenderer.sprite = 

        blockSpeed += blockSpeedIncrement; //?
    }

    
    private Transform GetRandomPrefab()
    {
        // 랜덤으로 프리팹 선택
        int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);
        Transform selectedPrefab = prefabs[randomIndex];

        // 프리팹 인스턴스화
        //Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        return selectedPrefab;
    }

    private void PlayRandomMusic()
    {
        // 랜덤 인덱스 선택
        int randomIndex = UnityEngine.Random.Range(0, musicClips.Length);

        // 선택된 오디오 클립 설정 및 재생
        audioSource.clip = musicClips[randomIndex];
        audioSource.Play();
    }

    private void MoveBlock()
    {
        if (currentBlock)
        {
            float moveAmount = Time.deltaTime * blockSpeed * blockDirection;
            currentBlock.position += new Vector3(moveAmount, 0f, 0f);
            
            if(Math.Abs(currentBlock.position.x) > xLimit)
            {
                currentBlock.position = new Vector3(blockDirection * xLimit, currentBlock.position.y, 0f);
                blockDirection = -blockDirection;
            }
        }
    }

    private void DropBlock()
    {
        if (Time.time >= lastTimeSpawned + timeBetweenSpawn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayRandomMusic();
                currentBlock = null;
                currentRigidbody.simulated = true;
                score += 1;
                updateScoreUI();

                lastTimeSpawned = Time.time;
                StartCoroutine(DelaySpawn());
            }
        }
    }
    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        SpawnNewBlock();
    }

    private void updateScoreUI()
    {
        scoreText.text = "Score : " + score;
    }


    private void EndGame()
    {
        // scoreText.text = "fail";
        //startButton.se
        restartButton.SetActive(true);
        camera.orthographicSize += 0.2f * Time.deltaTime;
    }

    public void restartGame()
    {
        restartButton.SetActive(false);
        camera.orthographicSize = 5;
        score = 0;
        updateScoreUI();
        playing = true;
        blockSpeed = 8f;
        DeleteAllChildren(blockStack);
        // 씬 다시 불러오기
        SpawnNewBlock();

    }

    private void DeleteAllChildren(Transform parent)
    {
        // 부모 오브젝트가 없으면 리턴
        if (parent == null) return;

        // 자식 오브젝트들의 배열을 가져옵니다.
        foreach (Transform child in parent.transform)
        {
            // 자식 오브젝트를 삭제합니다.
            Destroy(child.gameObject);
        }
    }
}
