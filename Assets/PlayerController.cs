using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// === 操作キャラクタの挙動クラス >>>
public class PlayerController : MonoBehaviour
{
    // === フィールド
    public Vector3 inputAxis = new Vector3(0, 0, 0);


    void Start()
    {
        
    }

    void Update()
    {
        this.Movement();    // 移動処理を実行
    }

    // === 移動メソッド 
    public void Movement()
    {
        inputAxis = new Vector3(0, 0, 0);
        if(Input.GetKey(KeyCode.W) == true)
        {
            inputAxis = new Vector3(0, 0, 1);
        }
        if(Input.GetKey(KeyCode.A) == true)
        {
            inputAxis = new Vector3(-1, 0, 0);
        }
        if(Input.GetKey(KeyCode.S) == true)
        {
            inputAxis = new Vector3(0, 0, -1);
        }
        if(Input.GetKey(KeyCode.D) == true)
        {
            inputAxis = new Vector3(1, 0, 0);
        }

        // カメラのオブジェクトを取得
        Transform cameraObject = GameObject.Find("CameraParent").transform;
        Vector3 moveSpeed = new Vector3(0, 0, 0);
        if(cameraObject != null)
        {   // カメラを見つけた場合のみ実行
            Vector3 moveVelocity = Vector3.Scale(
                                                 cameraObject.forward,
                                                 new Vector3(1, 0, 1)
                                                 ).normalized;
            moveSpeed = inputAxis.x * cameraObject.right +
                        inputAxis.z * moveVelocity;
        }

        // === 移動処理
        this.transform.Translate(moveSpeed * Time.deltaTime * 5, Space.World);
    }
}
