using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class MovimentPlayer : MonoBehaviour
{
    public float speed = 6.0f;

    GameObject cameraFPS;
    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    float rotacaoX = 0.0f, rotacaoY = 0.0f;

    void Start()
    {
        transform.tag = "Player";
        cameraFPS = GetComponentInChildren(typeof(Camera)).transform.gameObject;
        cameraFPS.transform.localPosition = new Vector3(0, 1, 0);
        cameraFPS.transform.localRotation = Quaternion.identity;
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        Vector3 direcaoFrente = new Vector3(cameraFPS.transform.forward.x, 0, cameraFPS.transform.forward.z);
        Vector3 direcaoLado = new Vector3(cameraFPS.transform.right.x, 0, cameraFPS.transform.right.z);
        direcaoFrente.Normalize();
        direcaoLado.Normalize();
        direcaoFrente = direcaoFrente * Input.GetAxis("Vertical");
        direcaoLado = direcaoLado * Input.GetAxis("Horizontal");
        Vector3 direcFinal = direcaoFrente + direcaoLado;
        if (direcFinal.sqrMagnitude > 1)
        {
            direcFinal.Normalize();
        }

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(direcFinal.x, 0, direcFinal.z);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = 10.0f;
            }
        }

        moveDirection.y -= 20.0f * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        CameraPrimeiraPessoa();
    }

    void CameraPrimeiraPessoa()
    {
        rotacaoX += Input.GetAxis("Mouse X") * 10.0f;
        rotacaoY += Input.GetAxis("Mouse Y") * 10.0f;
        rotacaoX = ClampAngleFps(rotacaoX, -360, 360);
        rotacaoY = ClampAngleFps(rotacaoY, -80, 80);
        Quaternion xQuartenion = Quaternion.AngleAxis(rotacaoX, Vector3.up);
        Quaternion yQuartenion = Quaternion.AngleAxis(rotacaoY, Vector3.up);
        Quaternion rotacFinal = Quaternion.identity * xQuartenion * yQuartenion;
        cameraFPS.transform.localRotation = Quaternion.Lerp(cameraFPS.transform.localRotation, rotacFinal, Time.deltaTime * 10.0f);
    }
    float ClampAngleFps(float angulo, float min, float max)
    {
        if (angulo < -360)
        {
            angulo += 360;
        }
        if (angulo > 360)
        {
            angulo -= 360;
        }
        return Mathf.Clamp(angulo, min, max);
    }
}
