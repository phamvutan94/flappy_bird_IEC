using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private float waitTime;

    //[SerializeField] private GameObject[] obstaclePrefabs;

    [SerializeField]
    private GameObject obstaclePrefab;
    private float tempTime;
    private int countTime = -1;
    private float maxRange = 1.4f;
    private float minRange = -2.8f;

    void Start()
    {
        tempTime = waitTime - Time.deltaTime;
    }

    void LateUpdate()
    {
        if (GameManager.Instance.GameState())
        {
            tempTime += Time.deltaTime;
            if (tempTime > waitTime)
            {
                // Wait for some time, create an obstacle, then set wait time to 0 and start again
                tempTime = 0;
                /* GameObject pipeClone = Instantiate(
                    obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)],
                    transform.position,
                    transform.rotation
                ); */
                GameObject pipeClone = Instantiate(
                    obstaclePrefab,
                    transform.position,
                    transform.rotation
                );
                transform.position = new Vector3(
                    transform.position.x,
                    PatternPos(transform.position.y),
                    transform.position.z
                );
                countTime++;
                if (countTime > 4)
                {
                    countTime = 0;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.parent != null)
        {
            Destroy(col.gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(col.gameObject);
        }
    }
// hàm tạo pattern E-E-E-H-H (với E là đoạn dễ, H là đoạn khó)
    private float PatternPos(float current_pos)
    {
        float pos = 0;
        float e = EasyPos();
        float h = HardPos();
        if (countTime >= 0 && countTime < 3)
        {
            pos = current_pos + e;
            if (pos > 2f || pos < -2f)
            {
                pos = current_pos - e;
            }
            Debug.Log(" de ");
        }
        else if (countTime >= 3 && countTime <= 4)
        {
            pos = current_pos + h;
            if (pos > 2f || pos < -2f)
            {
                pos = current_pos - h;
            }
            Debug.Log(" kho ");
        }
        return pos;
    }

//hàm tạo khoảng cách ngẫu nhiên từ -0.5 -> 0.5
    private float EasyPos()
    {
        float pos;
        pos = Random.Range(-0.5f, 0.5f);
        return pos;
    }

//hàm tạo khoảng cách ngẫu nhiên từ -2 -> -1.5 và 1.5 -> 2
    private float HardPos()
    {
        int Rad = Random.Range(0, 2);
        float pos;
        if (Rad == 0)
        {
            pos = Random.Range(1.5f, 2f);
        }
        else
        {
            pos = -Random.Range(1.5f, 2f);
        }
        return pos;
    }
}
