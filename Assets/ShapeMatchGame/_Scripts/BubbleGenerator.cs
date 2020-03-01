using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}
public enum PosDir { up, right, left, bottom };

public class BubbleGenerator : MonoBehaviour
{
    PlayerController playerController;

    public Boundary boundary;
    public PosDir posDir;       // for tell is the bubble leave the broder (not the one it was generated)

    public GameObject circlePrefab;
    public GameObject boxPrefab;
    public float speed;
    // Use this for initialization
    void Start()
    {
        playerController = PlayerController._instance;
    }

    public void GenerateBubble()
    {
        GameObject genBubble;
        Rigidbody2D rigidbody;
        float x = Random.Range(boundary.xMin, boundary.xMax);
        float y = Random.Range(boundary.yMin, boundary.yMax);
        int ran = Random.Range(0, 100);
        // direction from bubble generated to player
        Vector2 dir = new Vector2(playerController.rigidbody.position.x - x, playerController.rigidbody.position.y - y);
        switch (playerController.currentMode)
        {
            case Mode.circle:
                genBubble = Instantiate(ran > 60 ? boxPrefab : circlePrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
            case Mode.box:
                genBubble = Instantiate(ran > 60 ? circlePrefab : boxPrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
            default:
                genBubble = Instantiate(circlePrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
        }
        rigidbody = genBubble.GetComponent<Rigidbody2D>();
        genBubble.GetComponent<BubbleController>().bubblePosDir = posDir;
        rigidbody.velocity = dir * speed * (Random.Range(0.5f, 1.2f));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "circle" || collision.tag == "box")
            if (collision.GetComponent<BubbleController>().bubblePosDir != posDir)
                Destroy(collision.gameObject, 0.2f);
    }

}
