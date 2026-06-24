using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public float x = 0f;
    public float y = 0f;
    public int z = 0; // 1: 単押し, -1: 連続(ダブル)押し, 0: それ以外

    public float doubleTapThreshold = 0.3f; // 連続判定の閾値（秒）
    float lastSpaceTime = -1f;
    bool waitingForSecond = false;

    void Update()
    {
        x = 0f;
        y = 0f;
        z = 0;

        if (Input.GetKey(KeyCode.A))
        {
            x = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            x = 1f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            y = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            y = -1f;
        }

        // スペースキーの単押し／連続（ダブル）判定
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (waitingForSecond && (Time.time - lastSpaceTime) <= doubleTapThreshold)
            {
                // 二回連続と判定
                z = -1;
                waitingForSecond = false;
                lastSpaceTime = -1f;
            }
            else
            {
                // 一回目の押下を待つ
                waitingForSecond = true;
                lastSpaceTime = Time.time;
            }
        }

        // 連続判定のタイムアウト（第二押下が来なければ単押しとみなす）
        if (waitingForSecond && (Time.time - lastSpaceTime) > doubleTapThreshold)
        {
            z = 1;
            waitingForSecond = false;
            lastSpaceTime = -1f;
        }

        // デバッグ用：インスペクタやコンソールに値を確認したい場合
        Debug.Log($"x={x}, y={y}, z={z}");
    }
}
