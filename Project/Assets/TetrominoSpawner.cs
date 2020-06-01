using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrominoSpawner : MonoBehaviour {

	public GameObject[] Tetrominoes;
	public Sprite[] BlockTiles;
	public Sprite GameOverTile;
	private GameObject nextTetromino;

	public int currentLevel = 1;
	public int previousLinesKilled = 0;
	public int linesKilled = 0;
	public int score = 0;

	public Text scoreText;
	public Text linesText;
	public Text levelText;


	// Use this for initialization
	void Start () {
		nextTetromino = QueueTetromino ();
		SpawnTetromino ();
		scoreText.text = score.ToString();
		levelText.text = currentLevel.ToString();
		linesText.text = linesKilled.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (linesKilled > previousLinesKilled) {
			for (int i = previousLinesKilled; i < linesKilled; i++) {
				score += (10 * currentLevel);

				scoreText.text = score.ToString();
				levelText.text = currentLevel.ToString();
				linesText.text = linesKilled.ToString();

				previousLinesKilled = linesKilled;
			}

		if (linesKilled > 10 * currentLevel) {
				LevelUp();
		}

		}
	}

	private void SetTetrominoTile(GameObject tetro){
		var spriteR = tetro.GetComponent<SpriteRenderer> ();
		spriteR.sprite = BlockTiles [Random.Range (0, BlockTiles.Length)];
		//tetro.transform.position -= new Vector3(0, 1, 0);
	}

	public GameObject QueueTetromino(){
		GameObject newTetromino;
		newTetromino = Tetrominoes[Random.Range(0, Tetrominoes.Length)];

		foreach (Transform child in newTetromino.transform) {
			SetTetrominoTile (child.gameObject);
		}

		return newTetromino;
	}

	public void SpawnTetromino() {
		Instantiate(this.nextTetromino, transform.position, Quaternion.identity);
		this.nextTetromino = QueueTetromino ();
	}

	void LevelUp(){
		currentLevel++;
		Tetromino.fallTime -= 0.2f;
	}
}

