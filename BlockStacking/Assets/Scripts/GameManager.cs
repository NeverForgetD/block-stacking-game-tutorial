using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform blockHolder;

    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [SerializeField] private GameObject restartButton;

    private Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite testSprite;

    private Vector2 blockStartPosition = new Vector2(0f, 4f);

    private float blockSpeed = 8f;
    private float blockSpeedIncrement = 0.5f;
    private float blockDirection = 1;
    private float xLimit = 5f;

    private float timeBetweenSpawn = 1f;

    public bool playing = true;
    
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
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
        currentBlock = Instantiate(blockPrefab, blockHolder);
        currentBlock.position = blockStartPosition;
        //currentBlock.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentBlock.GetComponent<Rigidbody2D>();
        spriteRenderer = currentBlock.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = testSprite;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;

        // spriteRenderer.sprite = 

        blockSpeed += blockSpeedIncrement; //?
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentBlock = null;
            currentRigidbody.simulated = true;

            StartCoroutine(DelaySpawn());
        }
    }
    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        SpawnNewBlock();
    }

    private void EndGame()
    {
        // scoreText.text = "fail";
        //startButton.se
        restartButton.SetActive(true);
    }

    public void restartGame()
    {
        restartButton.SetActive(false);
        score = 0;
        playing = true;

        // 씬 다시 불러오기
        SpawnNewBlock();

    }

    // 점수 카운팅 시스템, 알고리즘 만들기
}
