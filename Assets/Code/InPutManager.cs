using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPutManager : MonoBehaviour
{
    public float speed = 15f;
    public KeyboardInput keyboardInput;
    public float lockY = 30f; // z==1 のとき固定する Y 座標
    
    private float originalY = 0f;
    private bool lockedY = false;
    private Rigidbody rb;
    private float moveX = 0f;
    private float moveZ = 0f;

    void Awake()
    {
        if (keyboardInput == null)
        {
            keyboardInput = GetComponent<KeyboardInput>();
        }
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Application.targetFrameRate = 30; // フレームレートを30に設定
    }

    void Update()
    {
        if (keyboardInput == null)
        {
            return;
        }

        float x = keyboardInput.x;
        float y = keyboardInput.y;
        int zState = keyboardInput.z;

        // zState の判定: 1->固定, -1->元に戻す, 0->維持
        if (zState == 1)
        {
            if (!lockedY)
            {
                originalY = transform.position.y;
                lockedY = true;
            }
        }
        else if (zState == -1)
        {
            if (lockedY)
            {
                lockedY = false;
            }
        }

        // 移動入力を保存（FixedUpdate で使用）
        moveX = x * speed;
        moveZ = y * speed;  // Z軸は常に入力可能
    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }

        // 横方向と奥行方向の移動
        Vector3 newPosition = rb.position;
        newPosition.x += moveX * Time.fixedDeltaTime;
        newPosition.z += moveZ * Time.fixedDeltaTime;

        // Y 軸固定時の処理
        if (lockedY)
        {
            // Y 座標を固定
            newPosition.y = lockY;
            // Y 速度をゼロに（重力による落下を防ぐ）
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;
        }

        // 位置を更新
        rb.position = newPosition;
    }
}

