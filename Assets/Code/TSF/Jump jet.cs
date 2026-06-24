using UnityEngine;

public class Jumpjet : MonoBehaviour
{
  [Header("コンポーネントの割り当て")]
    // [] を付けることで「配列」になり、複数のパーティクルを登録できるようになります
    [SerializeField] private ParticleSystem[] jetParticles;
    [SerializeField] private InPutManager inputManager;

    // 現在エフェクトが再生中かどうかを追跡するフラグ
    private bool isPlaying = false;

    void Start()
    {
        if (inputManager == null)
        {
            inputManager = GetComponent<InPutManager>();
        }

        // ゲーム開始時、登録されたすべてのパーティクルを停止状態にする
        // foreach文を使って、配列の中身を一つずつ順番に処理します
        foreach (ParticleSystem ps in jetParticles)
        {
            if (ps != null)
            {
                ps.Stop();
            }
        }
    }

    void Update()
    {
        if (inputManager == null) return;

        // 入力値の取得
        float x = inputManager.x;
        int z = inputManager.z;
        float y = inputManager.gry;

        // すべての入力が「ほぼ0」であるかを判定
        bool isInputZero = (Mathf.Abs(x) < 0.1f && Mathf.Abs(y) < 0.1f && z == 0);

        // 入力があり、かつエフェクトが停止中の場合 -> すべてON
        if (!isInputZero && !isPlaying)
        {
            foreach (ParticleSystem ps in jetParticles)
            {
                if (ps != null) ps.Play();
            }
            isPlaying = true;
        }
        // 入力がなくなり、かつエフェクトが再生中の場合 -> すべてOFF
        else if (isInputZero && isPlaying)
        {
            foreach (ParticleSystem ps in jetParticles)
            {
                if (ps != null) ps.Stop();
            }
            isPlaying = false;
        }
    }
}