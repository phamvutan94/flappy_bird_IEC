using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer flapp_bird;

    [SerializeField]
    private Sprite[] flapp_bird_Image;

    [SerializeField]
    private Animator Anim;

    private void Awake() {
        Random_bird();
     }

    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void Random_bird()
    {
        // tạo số ngẫu nhiên trong khoảng 0 - số lượng Sprite của player
        int a = Random.Range(0, flapp_bird_Image.Length);
       //set Sprite cho player
        flapp_bird.sprite = flapp_bird_Image[a];
        // set animation cho player;
        Anim.SetInteger("Player", a);
    }
}
