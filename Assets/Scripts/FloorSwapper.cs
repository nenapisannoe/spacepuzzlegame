using UnityEngine;
using System.Collections;

public class FloorSwapper : MonoBehaviour
{
    public Transform floor1;
    public Transform floor2;
    public MoveCharacter moveCharacter;
    public RoverManager roverManager;
    public float swapSpeed = 1.0f;
    private bool playerNearby;

    private float floor1OriginalY;
    private float floor2OriginalY;
    private bool isSwapped = false;

    private void Start()
    {
        moveCharacter = FindObjectOfType<MoveCharacter>();
        roverManager = FindObjectOfType<RoverManager>();
        floor1OriginalY = floor1.position.y;
        floor2OriginalY = floor2.position.y;
    }

    public void SwapFloors()
    {
        StartCoroutine(SwapCoroutine());
    }

        void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void Update()
    {
        if(playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            SwapFloors();
        }
    }


    private IEnumerator SwapCoroutine()
    {
        float startY1 = floor1.position.y;
        float startY2 = floor2.position.y;

        float targetY1 = isSwapped ? floor1OriginalY : floor2OriginalY;
        float targetY2 = isSwapped ? floor2OriginalY : floor1OriginalY;


        float elapsedTime = 0f;

        while (elapsedTime < swapSpeed)
        {
            float t = elapsedTime / swapSpeed;

            float newY1 = Mathf.Lerp(startY1, targetY1, t);
            float newY2 = Mathf.Lerp(startY2, targetY2, t);

            float deltaY1 = newY1 - floor1.position.y;
            float deltaY2 = newY2 - floor2.position.y;

            floor1.position = new Vector3(floor1.position.x, newY1, floor1.position.z);
            floor2.position = new Vector3(floor2.position.x, newY2, floor2.position.z);

            if (moveCharacter.currentFloor == floor1)
            {
                moveCharacter.transform.position += new Vector3(0, deltaY1, 0);
            }
            else if (moveCharacter.currentFloor == floor2)
            {
                moveCharacter.transform.position += new Vector3(0, deltaY2, 0);
            }
            foreach (var rover in roverManager.ActiveRovers)
            {
                if (rover.currentFloor == floor1)
                {
                    rover.transform.position += new Vector3(0, deltaY1, 0);
                }
                if (rover.currentFloor == floor2)
                {
                    rover.transform.position += new Vector3(0, deltaY2, 0);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        floor1.position = new Vector3(floor1.position.x, targetY1, floor1.position.z);
        floor2.position = new Vector3(floor2.position.x, targetY2, floor2.position.z);

        isSwapped = !isSwapped;
    }
}
