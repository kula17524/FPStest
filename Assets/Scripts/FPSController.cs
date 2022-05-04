using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    // 移動用の座標
    float x, z;
    // スピード調整
    float speed = 0.1f;

    // カメラ
    public GameObject cam;
    Quaternion cameraRot, characterRot;
    // 視点の角度制限
    float minX = -90f, maxX = 90f;

    // マウス感度の調整
    float Xsensityvity = 3f, Ysensityvity = 3f;
    // カーソルの非表示(true=非表示)
    bool cursorLock = true;

    // アニメーション
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        // 最初にカメラとキャラクターの向きを取得
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // マウス感度の調整
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);

        // 関数を使って視点制限
        cameraRot = ClampRotation(cameraRot);
        
        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        // カーソルの表示・非表示の関数呼び出し
        UpdateCursorLock();

        // アニメーション
        // 射撃
        if (Input.GetMouseButton(0))
        {
            animator.SetTrigger("Fire");
        }
        // リロード
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reload");
        }
        // 歩く
        // 前後移動に対応するため絶対値で判定(Mathf.Abs)
        if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
        {
            if (!animator.GetBool("Walk"))
            {
                animator.SetBool("Walk", true);
            }
        }
        else if (animator.GetBool("Walk"))
        {
            animator.SetBool("Walk", false);
        }
        // 走る
        // 後ろ向き移動の時は走らせない
        if (z > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            if (!animator.GetBool("Run"))
            {
                animator.SetBool("Run", true);
                speed = 0.25f;
            }
        }
        else if (animator.GetBool("Run"))
        {
            animator.SetBool("Run", false);
            speed = 0.1f;
        }
    }

    private void FixedUpdate()
    {
        // 座標を0とする
        x = 0;
        z = 0;

        // マウスの入力に応じて移動
        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;
        // カメラの向きを正面として移動
        transform.position += cam.transform.forward * z + cam.transform.right * x;
    }

    // カーソルの表示設定用関数
    public void UpdateCursorLock()
    {
        //カーソルの表示・非表示
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if(Input.GetMouseButton(0))
        {
            cursorLock = true;
        }

        if(cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked; 
        }
        else if(!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // 視点の角度制限用関数
    // void(返り値なし)でなく、Quaternion型の返り値を返す
    public Quaternion ClampRotation(Quaternion q)
    {
        //qのx, y, zは量と向きを持つ(座標)
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        //qのwは量しか持たない(回転量)
        q.w = 1f;

        // オイラー角に変換
        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;
        // 視点制限
        angleX = Mathf.Clamp(angleX, minX, maxX);
        // 角度を反映
        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}
