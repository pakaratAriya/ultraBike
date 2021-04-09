using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int currentLane = 2;
    Coroutine changeLaneCoroutine;
    private float accelerate = 5;
    private float maxSpeed = 10;
    public delegate void OnChangeLaneEvent(int currentLane);
    public static event OnChangeLaneEvent onChangeLaneEvent;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        rigidbody2d.AddForce(new Vector2(x * accelerate, 0));
        rigidbody2d.velocity = new Vector2(Mathf.Clamp(rigidbody2d.velocity.x, -maxSpeed, maxSpeed), rigidbody2d.velocity.y);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (changeLaneCoroutine != null) StopCoroutine(changeLaneCoroutine);
            if (rigidbody2d.velocity.y < 2)
                changeLaneCoroutine = StartCoroutine(ChangeLane(1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (changeLaneCoroutine != null) StopCoroutine(changeLaneCoroutine);
            if (rigidbody2d.velocity.y < 2)
                changeLaneCoroutine = StartCoroutine(ChangeLane(-1));
        }
    }

    IEnumerator ChangeLane(int x)
    {
        if (currentLane + x >= 1 && currentLane + x <= LaneController.LANE_NUMBER)
        {
            currentLane += x;
            Vector3 destination = transform.position;
            destination.y += LaneController.LANE_GAP * x;
            Vector3 vel = rigidbody2d.velocity;
            vel.y = 0;
            rigidbody2d.velocity = vel;
            rigidbody2d.isKinematic = true;
            if (onChangeLaneEvent != null) onChangeLaneEvent(currentLane);

            while (Mathf.Abs(transform.position.y - destination.y) > 0.1f)
            {
                Vector3 pos = transform.position + Vector3.up * x * 0.01f;
                transform.position = pos;
                yield return new WaitForEndOfFrame();
            }
            GetComponent<SpriteRenderer>().sortingOrder = currentLane - x;

            // broadcast all lanes to check their layer
            rigidbody2d.isKinematic = false;
        }
    }
}
