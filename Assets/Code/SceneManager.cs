using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {

	private List<Obstacle> obstaclesInScene = new List<Obstacle> ();
	private float timer;
	private float nextGenerationTime;
	private bool gameOver;

	[Header("Scene Settings")]
	public Transform[] lanes;

	[Header("Obstacles Generator Variables")]
	public float maxTimeBetweenObstacles;
	public float minTimeBetweenObstacles;
	public float obstacleSpeed;
	[HideInInspector] public float generationDistance;
	[HideInInspector] public float destroyDistance;

	[Header("Player Variables")]
	public int life;
	public int coins;

	public static SceneManager Instance;

	private void Awake() {
		Instance = this;
	}

	private void Start () {
		timer = 0f;
		nextGenerationTime = Random.Range (minTimeBetweenObstacles, maxTimeBetweenObstacles);
		generationDistance = 100f;
		destroyDistance = -10f;
	}

	private void Update () {
		if (!gameOver) {
			timer += Time.deltaTime;

			if (timer >= nextGenerationTime) {
				GenerateObstacle ();
				timer = 0f;
				nextGenerationTime = Random.Range (minTimeBetweenObstacles, maxTimeBetweenObstacles);
			}
		}
	}

	private void GenerateObstacle() {
		int temp = Random.Range (0, 3);
		float nextLane = lanes [temp].transform.position.x;
		temp = Random.Range (0, 2);
		float yPos = (temp == 0) ? 1 : 2.5f;

		Obstacle obs = new Obstacle (generationDistance, transform.GetChild(1).transform, nextLane, yPos);
		obstaclesInScene.Add (obs);
	}

	public void DestroyObstacle(Obstacle obs) {
		Destroy (obs.model);
		obstaclesInScene.Remove (obs);
	}

	public void GameOver() {
		gameOver = true;
	}
}
