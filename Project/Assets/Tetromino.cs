using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {
	
	private float previousTime;
	public Vector3 rotationPoint;
	public static float fallTime = 0.8f;
	public AudioSource collisionSound;

	public static int height = 21;
	public static int width = 10;
	private static Transform[,] grid = new Transform[Mathf.Abs(width), Mathf.Abs(height)];
	private bool gameOverPosition = false;

	// Use this for initialization
	void Start () {

		collisionSound = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		//Shifts blocks left or right.
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			transform.position += new Vector3 (-1, 0, 0);
			if (!ValidMove ()) {
			transform.position -= new Vector3 (-1, 0, 0);
			}
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			transform.position += new Vector3 (1, 0, 0);
			if (!ValidMove ()) {
			transform.position -= new Vector3 (1, 0, 0);
			}
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			transform.RotateAround (transform.TransformPoint (rotationPoint), new Vector3 (0, 0, 1), 90);
			foreach (Transform child in transform) {
				RotateTile (child.gameObject, -1f);
			}
			if (!ValidMove ()) {
				transform.RotateAround (transform.TransformPoint (rotationPoint), new Vector3 (0, 0, 1), -90);
				foreach (Transform child in transform) {
					RotateTile (child.gameObject, 1f);
				}
			}
		}

		//Logic for how tetrominos should fall.
		if (Time.time - previousTime > (Input.GetKey (KeyCode.DownArrow) ? fallTime / 10 : fallTime)) {
			transform.position += new Vector3 (0, -1, 0);
			if (!ValidMove ()) {
				transform.position -= new Vector3 (0, -1, 0);
				if (transform.position.y == 20) {
					if (gameOverPosition == true) {
						print ("Game Over");
						StartCoroutine (GameOver ());
					gameOverPosition = true;
					}
				} else {
					collisionSound.Play ();
					AddToGrid ();
					CheckForLines ();
					this.enabled = false;
					FindObjectOfType<TetrominoSpawner> ().SpawnTetromino ();
					gameOverPosition = false;
				}
			}
			previousTime = Time.time;
		}
	}

	//Cylces through height and calls HasLine() to cycle through width.
	public void CheckForLines() {
		for (int i = height - 1; i >= 0; i--) {
			if (HasLine (i)) {
				DeleteLine (i);
				MoveRowDown (i);
			}
		}
	}

	bool HasLine(int i) {
		for (int j = 0; j < width; j++) {
			if (grid [j, i] == null)
				return false;
		}
		return true;
	}

	//Destroys line.  Need to add animation for this.
	void DeleteLine(int i) {
		for (int j = 0; j < width; j++) {
			Destroy (grid [j, i].gameObject);
			grid [j, i] = null;
		}
		FindObjectOfType<TetrominoSpawner> ().linesKilled += 1;
	}

	void MoveRowDown(int i)
	{
		for (int y = i; y < height; y++)
		{
			for (int j = 0; j < width; j++) 
			{
				if (grid [j, y] != null) 
				{
					grid [j, y - 1] = grid [j, y];
					grid [j, y] = null;
					grid [j, y - 1].transform.position -= new Vector3 (0, 1, 0);
				}
			}
		}
	}

	void AddToGrid() {
		foreach (Transform children in transform) {
			int roundedX = Mathf.RoundToInt (children.transform.position.x);
			int roundedY = Mathf.RoundToInt	(children.transform.position.y);
			if (grid [roundedX, roundedY] == null) {
				grid [roundedX, roundedY] = children;
			} else {
				//print ("Game Over");
				//this.enabled = false;
				//Debug.Break ();
			}
		}
	}

	bool ValidMove() {
		foreach (Transform children in transform) {
			int roundedX = Mathf.RoundToInt (children.transform.position.x);
			int roundedY = Mathf.RoundToInt	(children.transform.position.y);

			if (roundedX < 0 || roundedX >= width || roundedY < 0 ){
				return false;
			} else if ( roundedY >= height ) {
				//print ("Game over");
			}

			if (grid[roundedX, roundedY] != null) {
				return false;
			}
		}
		return true;
	}

	void RotateTile(GameObject tile, float posOrNeg) {
		tile.transform.Rotate(new Vector3 (0, 0, (90 * posOrNeg)));
	}

	IEnumerator GameOver (){
		yield return new WaitForSeconds (1);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");

	}
}
