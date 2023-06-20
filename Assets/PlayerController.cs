using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// === ����L�����N�^�̋����N���X >>>
public class PlayerController : MonoBehaviour
{
    // === �t�B�[���h
    public Vector3 inputAxis = new Vector3(0, 0, 0);


    void Start()
    {
        
    }

    void Update()
    {
        this.Movement();    // �ړ����������s
    }

    // === �ړ����\�b�h 
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

        // �J�����̃I�u�W�F�N�g���擾
        Transform cameraObject = GameObject.Find("CameraParent").transform;
        Vector3 moveSpeed = new Vector3(0, 0, 0);
        if(cameraObject != null)
        {   // �J�������������ꍇ�̂ݎ��s
            Vector3 moveVelocity = Vector3.Scale(
                                                 cameraObject.forward,
                                                 new Vector3(1, 0, 1)
                                                 ).normalized;
            moveSpeed = inputAxis.x * cameraObject.right +
                        inputAxis.z * moveVelocity;
        }

        // === �ړ�����
        this.transform.Translate(moveSpeed * Time.deltaTime * 5, Space.World);
    }
}
