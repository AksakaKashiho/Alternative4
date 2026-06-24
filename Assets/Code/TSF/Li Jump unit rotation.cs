using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LiJumpunitrotation : MonoBehaviour
{
    [SerializeField] InPutManager inputManager;
    [SerializeField] float rotateSpeed = 720f; // degrees per second

    private Quaternion targetRotation;

    void Awake()
    {
        if (inputManager == null)
        {
            inputManager = GetComponent<InPutManager>();
        }
        // 初期状態のローカル角（親から見た相対的な回転）を保持
        targetRotation = transform.localRotation;
    }

    void Update()
    {
        if (inputManager == null) return;

        float x = inputManager.x;
        int z = inputManager.z;
        float y = inputManager.gry;

        // 入力状態に応じて、ローカル座標系の目標回転を直接指定
        
        // 初期状態: 入力がほぼ無いとき（遊びを考慮して Mathf.Abs を使用）
        if (Mathf.Abs(x) < 0.1f && Mathf.Abs(y) < 0.1f &&z == 0)
        {
            targetRotation = Quaternion.Euler(-90f, 90f, -90f);
            Debug.Log("初期位置");
            Debug.Log($"{targetRotation}");
        }
        else if (x > 0.1f) // x == 1f の厳密な判定から、安全な閾値判定に変更
        {
             targetRotation = Quaternion.Euler(-135f, 175f, -100f);
            Debug.Log("右");
        }
        else if (x < -0.1f) // x == -1f の厳密な判定から、安全な閾値判定に変更
        {
           
            targetRotation = Quaternion.Euler(-135f, 0f, -90f);
            Debug.Log("左");
        }

        else if (y > 0.1f)
        {
            targetRotation = Quaternion.Euler(-180f, 180f, -180f);
            Debug.Log("前方");
        }
        else if (y < -0.1f)
        {
            targetRotation = Quaternion.Euler(-155f, 270f, 0f);
            Debug.Log("後方"); // ログを修正
        }

        // どの条件にも当てはまらない場合は targetRotation を更新しない（現在の目標ローカル角を維持）

        // 指定したローカル角（targetRotation）に向けて、現在地からスムーズに回転させる
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}