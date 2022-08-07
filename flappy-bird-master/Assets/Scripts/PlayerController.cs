using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float thrust,
        minTiltSmooth,
        maxTiltSmooth,
        hoverDistance,
        hoverSpeed;
    private bool start;
    private float timer,
        tiltSmooth,
        y;
    private Rigidbody2D playerRigid;
    private Quaternion downRotation,
        upRotation;
    private ChangePlayerController changePlayer;
    private float speed = 3f;
    private float speed_down = 4f;
    private bool isDead = false;

    void Start()
    {
        tiltSmooth = maxTiltSmooth;
        playerRigid = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        upRotation = Quaternion.Euler(0, 0, 35);
        changePlayer = GetComponent<ChangePlayerController>();
    }

    void Update()
    {
        if (!start)
        {
            // Hover the player before starting the game
            timer += Time.deltaTime;
            y = hoverDistance * Mathf.Sin(hoverSpeed * timer);
            transform.localPosition = new Vector3(0, y, 0);
        }
        else
        {
            // Rotate downward while falling
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                downRotation,
                tiltSmooth * Time.deltaTime
            );
            // player rơi xuống theo vận tốc là speed_down;
            if (transform.position.y > -2.9f)
            {
                transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * speed_down;
            }
            // end game khi player rơi xuống sàn
            else if (transform.position.y < -2.9f && !isDead)
            {
                KillPlayer();
				transform.rotation = downRotation;
                isDead = true;
            }
        }
        // Limit the rotation that can occur to the player
        transform.rotation = new Quaternion(
            transform.rotation.x,
            transform.rotation.y,
            Mathf.Clamp(transform.rotation.z, downRotation.z, upRotation.z),
            transform.rotation.w
        );
    }

    void LateUpdate()
    {
        if (GameManager.Instance.GameState())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!start)
                {
                    // This code checks the first tap. After first tap the tutorial image is removed and game starts
                    start = true;
                    GameManager.Instance.GetReady();
                    GetComponent<Animator>().speed = 2;
                }
				//di chuyển player lên trên
                Bird_move_up();
                SoundManager.Instance.PlayTheAudio("Flap");
            }
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Score"))
        {
            Destroy(col.gameObject);
            GameManager.Instance.UpdateScore();
        }
        else if (col.transform.CompareTag("Obstacle"))
        {
            // Destroy the Obstacles after they reach a certain area on the screen
            foreach (Transform child in col.transform.parent.transform)
            {
                child.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            KillPlayer();
        }
    }


    public void KillPlayer()
    {
        GameManager.Instance.EndGame();
        GetComponent<Animator>().enabled = false;
        // doi mau player
        changePlayer.ChangeColorPlayerDead();
    }

    // tạo 1 Coroutine để di chuyển player lên trên dần dần trong khoảng thời gian nhất định
    public void Bird_move_up()
    {
        StartCoroutine(move_up());
    }

    public IEnumerator move_up()
    {
        float time = 0;
        float duration_time = 0.5f;
        while (duration_time > time)
        {
            float duration_frame = Time.deltaTime;
            transform.position += new Vector3(0, 2, 0) * Time.deltaTime * speed;
            transform.rotation = upRotation;
            time += duration_frame;
            yield return null;
        }
    }
}
