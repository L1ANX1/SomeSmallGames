using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeMatchGame
{

    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax, yMin, yMax;
    }

    public class BubbleGenerator : MonoBehaviour
    {
        [Header("Private: ")]
        GameController gameController;
        PlayerController playerController;
        GameManager gameManager;

        public Boundary boundary;
        public BubbleGenDir currentGenDir;       // for tell is the bubble leave the broder (not the one it was generated)

        public GameObject circlePrefab;
        public GameObject boxPrefab;
        public float speed;

        // Use this for initialization
        void Start()
        {
            playerController = PlayerController._instance;
            gameController = GameController._instance;
            gameManager = GameManager.Instance;
        }

        /// <summary>
        /// Generate A Bubble.
        /// </summary>
        /// <param name='mode'>
        /// Current mode, generate a different type of bubble.
        /// </param>
        /// <param name='g'>Generate what</param>
        /// <param name='x'>Generate where x</param>
        /// <param name='y'>Generate where y</param>
        /// <param name='dir'>move direction</param>
        void GenerateBubble(Mode mode, GameObject g, float x, float y, Vector3 dir)
        {
            GameObject genBubble;
            switch (mode)
            {
                case Mode.circle:
                    genBubble = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    BubbelBox bubbelBox = genBubble.AddComponent<BubbelBox>();
                    bubbelBox.GenDir = currentGenDir;
                    bubbelBox.Speed = speed;
                    bubbelBox.VecDir = dir;
                    break;
                case Mode.box:
                    genBubble = Instantiate(circlePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    BubbelCircle bubbelCircle = genBubble.AddComponent<BubbelCircle>();
                    bubbelCircle.GenDir = currentGenDir;
                    bubbelCircle.Speed = speed;
                    bubbelCircle.VecDir = dir;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Start to generate Bubbles, do the calculation.
        /// </summary>
        public void GenerateBubbles()
        {
            float x = Random.Range(boundary.xMin, boundary.xMax);
            float y = Random.Range(boundary.yMin, boundary.yMax);
            int ran = Random.Range(0, 100);
            // direction from bubble generated to player
            Vector2 dir = new Vector2(playerController.PlayerPosition.x - x, playerController.PlayerPosition.y - y);
            switch (gameManager.CurrentMode)
            {
                case Mode.circle:
                    if (ran < 75)
                        GenerateBubble(Mode.circle, circlePrefab, x, y, dir);
                    else
                        GenerateBubble(Mode.box, boxPrefab, x, y, dir);
                    break;
                case Mode.box:
                    if (ran > 75)
                        GenerateBubble(Mode.circle, circlePrefab, x, y, dir);
                    else
                        GenerateBubble(Mode.box, boxPrefab, x, y, dir);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// For destory unseen Bubble
        /// </summary>
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "circle" || collision.tag == "box")
            {
                if (collision.GetComponent<Bubble>().GenDir != currentGenDir)
                    Destroy(collision.gameObject, 0.2f);
            }
        }

    }
}