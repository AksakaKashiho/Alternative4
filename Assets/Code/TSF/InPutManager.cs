using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPutManager : MonoBehaviour
{
     [SerializeField] float speed = 15f;
     [SerializeField] KeyboardInput keyboardInput;
     [SerializeField] float lockY = 30f; // z==1 のとき固定する Y 座標
     [SerializeField] float riseSpeed = 10f; // Y座標上昇の速度
    
    // 公開入力フィールド（他クラスから参照するため）
    public float x = 0f;
    public int z = 0;
    public float gry = 0f;
    
    private float originalY = 0f;
    private bool lockedY = false;
    private bool holdY = false; // スペースキーで高度を固定するフラグ
    private float holdYValue = 0f; // 固定された高度の値
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

        // 公開フィールドに最新値を反映
        this.x = x;
        this.z = zState;
        this.gry=y;

        // スペースキーで高度を固定／再開
        if (Input.GetKeyDown(KeyCode.Space) && lockedY)
        {
            if (holdY)
            {
                // 目標高度に達していなければ再び上昇
                if (transform.position.y < lockY)
                {
                    holdY = false;
                }
            }
            else
            {
                // 現在の高度を保持
                holdY = true;
                holdYValue = transform.position.y;
            }
        }

        // zState の判定: 1->固定, -1->元に戻す, 0->維持
        if (zState == 1)
        {
            if (!lockedY)
            {
                originalY = transform.position.y;
                lockedY = true;
                holdY = false; // 新たに上昇開始時は高度固定を解除
            }
        }
        else if (zState == -1)
        {
            if (lockedY)
            {
                lockedY = false;
                holdY = false; // 下降時は高度固定を解除
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

        // Y 軸の処理
        if (lockedY)
        {
            if (holdY)
            {
                // スペースキーで固定された高度を維持
                newPosition.y = holdYValue;
            }
            else
            {
                // Y座標を徐々に目標値に近づける
                if (newPosition.y < lockY)
                {
                    newPosition.y = Mathf.Min(newPosition.y + riseSpeed * Time.fixedDeltaTime, lockY);
                }
                else
                {
                    // 目標高度に到達したらそれ以上上がらないようにする
                    newPosition.y = lockY;
                    holdY = true;
                    holdYValue = lockY;
                }
            }

            // Y 速度をゼロに（重力による落下を防ぐ）
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;
        }

        // 位置を更新
        rb.position = newPosition;
    }
}

