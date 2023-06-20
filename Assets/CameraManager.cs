using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Serializable]  // �C���X�y�N�^�[�ɕ\���ł���悤�ɂ���A�g���r���[�g
    public class Parameter
    {   // �J��������ň��������o�[�ϐ�
        public Transform target;    // �ǂ�������^�[�Q�b�g
        public Vector3 position;    // �J�����̍��W
        public Vector3 angles;      // �J�����̊p�x
        public float tiltMax;       // �`���g�̍ő�l
        public float tiltMin;       // �`���g�̍ŏ��l
        public float distance;      // �^�[�Q�b�g�Ƃ̋���
        public float distanceMax;   // �^�[�Q�b�g�Ƃ̋����̍ő�l
        public float distanceMin;   // �^�[�Q�b�g�Ƃ̋����̍ŏ��l
        public float fieldOfView;   // �J�����̎���p
        public Vector3 offsetPosition;  // �J�����̍��W�i���炵�j
        public Vector3 offsetAngles;    // �J�����̊p�x�i���炵�j
    }

    public Transform parent;        // �J�����̐e�I�u�W�F�N�g
    public Transform child;         // �J�����̎q�I�u�W�F�N�g
    public Camera camera;           // �J�����{�̂�Camera�R���|�[�l���g
    public Parameter parameter;     // �p�����[�^�[
    public int targetIndex;         // �ǂ�������^�[�Q�b�g�̔ԍ�
    public TargetManager targetManager; // TargetManager�R���|�[�l���g

    void Start( )
    {
        
    }

    void Update()
    {
        // === �X�N���[���z�C�[���Ń^�[�Q�b�g�Ƃ̋����𒲐�����
        float scroll = Input.GetAxis("Mouse ScrollWheel");  // �z�C�[���̉�]�l���擾
        parameter.distance += scroll * 3f * -1;             // ��]�l�ŋ�����ς���
        parameter.distance = 
            Mathf.Clamp(parameter.distance, parameter.distanceMin, parameter.distanceMax);

        // === �}�E�X�ړ��ŃJ��������]������
        Vector3 mouseAngles = new Vector3(0, 0, 0);
        mouseAngles.x = -Input.GetAxisRaw("Mouse Y");   // �}�E�XY���̈ړ��ʂ��擾
        mouseAngles.y = Input.GetAxisRaw("Mouse X");    // �}�E�XX���̈ړ��ʂ��擾
        parameter.angles += mouseAngles * 5f;           // �J�����̊p�x��ς���
        parameter.angles.x =
            Mathf.Clamp(parameter.angles.x, parameter.tiltMin, parameter.tiltMax);

        // === �J�����̕��s�ړ�
        Vector3 inputAxis = new Vector3(0, 0, 0);
        inputAxis.x = Input.GetAxis("Horizontal");      // Input�ݒ�̉���(����)���擾
        inputAxis.y = Input.GetAxis("Vertical");        // Input�ݒ�̏c��(����)���擾
        parameter.offsetPosition += inputAxis * Time.deltaTime;

        // === �^�[�Q�b�g��؂�ւ���
        if(Input.GetKeyDown(KeyCode.Tab) == true)
        {   // [Tab�L�[]�Ő؂肩����
            targetIndex++;      // �ԍ���؂�ւ�
            parameter.target = targetManager.targetObjectArray[targetIndex % targetManager.targetObjectArray.Length].transform;
        }
    }

    // Unity�̃��C�t�T�C�N���̍Ō�Ɏ��s�����Update
    void LateUpdate()
    {
        if(parent == null || child == null || camera == null)
        {
            Debug.LogWarning("parent �� child �� camera �̂����ꂩ��������܂���B");
            return;     // �I�u�W�F�N�g���ǂꂩ��ł�"��"�������狭���I������
        }

        // === �^�[�Q�b�g��ǂ�������
        if(parameter.target != null)
        {   // �^�[�Q�b�g��"�󂶂�Ȃ�"�ꍇ�ɒǂ�������
            parameter.position = Vector3.Lerp(parameter.position, parameter.target.position, Time.deltaTime * 10);
        }

        parent.position = parameter.position;   // ���ۂɃI�u�W�F�N�g�̍��W������������
        parent.eulerAngles = parameter.angles;  // ���ۂɃI�u�W�F�N�g�̊p�x������������

        // === �^�[�Q�b�g�Ƌ��������
        Vector3 childPos = child.localPosition; // �u�q�v�̃��[�J�����W���擾����
        childPos.z = -parameter.distance;       // �^�[�Q�b�g�Ƃ̋����������
        child.localPosition = childPos;         // �u�q�v�̃��[�J�����W������������

        // === �J�����{�̂̐ݒ�
        camera.fieldOfView = parameter.fieldOfView;     // ����p������������
        camera.transform.localPosition = parameter.offsetPosition;      // ���W��"���炵"��K�p����
        camera.transform.localEulerAngles = parameter.offsetAngles;     // �p�x��"���炵"��K�p����
    }
}
