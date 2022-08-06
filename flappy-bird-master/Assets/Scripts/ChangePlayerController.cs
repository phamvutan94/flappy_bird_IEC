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

    [SerializeField]
    float duration_time = 2f;

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
    //hàm chuyển màu player sang grayscale;
    public void ChangeColorPlayerDead(){
        StartCoroutine(GrayscaleRoutine());
    }
    //chuyển dần từ từ màu sắc sang màu đen trắng
    public IEnumerator GrayscaleRoutine(){
        float time = 0;
        while(duration_time > time){
            float duration_frame = Time.deltaTime;
            float ratio = time/duration_time;
            SetGrayScale(ratio);
            time += duration_frame;
            yield return null;
        }
    }
    //đặt giá trị của grayscale
    public void SetGrayScale(float amount){
        flapp_bird.material.SetFloat("_GrayscaleAmount", amount);
    }
    
}
