using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Serializable]  // インスペクターに表示できるようにするアトリビュート
    public class Parameter
    {   // カメラ操作で扱うメンバー変数
        public Transform target;    // 追いかけるターゲット
        public Vector3 position;    // カメラの座標
        public Vector3 angles;      // カメラの角度
        public float tiltMax;       // チルトの最大値
        public float tiltMin;       // チルトの最小値
        public float distance;      // ターゲットとの距離
        public float distanceMax;   // ターゲットとの距離の最大値
        public float distanceMin;   // ターゲットとの距離の最小値
        public float fieldOfView;   // カメラの視野角
        public Vector3 offsetPosition;  // カメラの座標（ずらし）
        public Vector3 offsetAngles;    // カメラの角度（ずらし）
    }

    public Transform parent;        // カメラの親オブジェクト
    public Transform child;         // カメラの子オブジェクト
    public Camera camera;           // カメラ本体のCameraコンポーネント
    public Parameter parameter;     // パラメーター
    public int targetIndex;         // 追いかけるターゲットの番号
    public TargetManager targetManager; // TargetManagerコンポーネント

    void Start( )
    {
        
    }

    void Update()
    {
        // === スクロールホイールでターゲットとの距離を調整する
        float scroll = Input.GetAxis("Mouse ScrollWheel");  // ホイールの回転値を取得
        parameter.distance += scroll * 3f * -1;             // 回転値で距離を変える
        parameter.distance = 
            Mathf.Clamp(parameter.distance, parameter.distanceMin, parameter.distanceMax);

        // === マウス移動でカメラを回転させる
        Vector3 mouseAngles = new Vector3(0, 0, 0);
        mouseAngles.x = -Input.GetAxisRaw("Mouse Y");   // マウスY軸の移動量を取得
        mouseAngles.y = Input.GetAxisRaw("Mouse X");    // マウスX軸の移動量を取得
        parameter.angles += mouseAngles * 5f;           // カメラの角度を変える
        parameter.angles.x =
            Mathf.Clamp(parameter.angles.x, parameter.tiltMin, parameter.tiltMax);

        // === カメラの平行移動
        Vector3 inputAxis = new Vector3(0, 0, 0);
        inputAxis.x = Input.GetAxis("Horizontal");      // Input設定の横軸(水平)を取得
        inputAxis.y = Input.GetAxis("Vertical");        // Input設定の縦軸(垂直)を取得
        parameter.offsetPosition += inputAxis * Time.deltaTime;

        // === ターゲットを切り替える
        if(Input.GetKeyDown(KeyCode.Tab) == true)
        {   // [Tabキー]で切りかえる
            targetIndex++;      // 番号を切り替え
            parameter.target = targetManager.targetObjectArray[targetIndex % targetManager.targetObjectArray.Length].transform;
        }
    }

    // Unityのライフサイクルの最後に実行されるUpdate
    void LateUpdate()
    {
        if(parent == null || child == null || camera == null)
        {
            Debug.LogWarning("parent か child か camera のいずれかが見つかりません。");
            return;     // オブジェクトがどれか一つでも"空"だったら強制終了する
        }

        // === ターゲットを追いかける
        if(parameter.target != null)
        {   // ターゲットが"空じゃない"場合に追いかける
            parameter.position = Vector3.Lerp(parameter.position, parameter.target.position, Time.deltaTime * 10);
        }

        parent.position = parameter.position;   // 実際にオブジェクトの座標を書き換える
        parent.eulerAngles = parameter.angles;  // 実際にオブジェクトの角度を書き換える

        // === ターゲットと距離を取る
        Vector3 childPos = child.localPosition; // 「子」のローカル座標を取得する
        childPos.z = -parameter.distance;       // ターゲットとの距離分離れる
        child.localPosition = childPos;         // 「子」のローカル座標を書き換える

        // === カメラ本体の設定
        camera.fieldOfView = parameter.fieldOfView;     // 視野角を書き換える
        camera.transform.localPosition = parameter.offsetPosition;      // 座標に"ずらし"を適用する
        camera.transform.localEulerAngles = parameter.offsetAngles;     // 角度に"ずらし"を適用する
    }
}
