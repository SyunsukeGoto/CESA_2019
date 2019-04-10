using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    Renderer m_material;

    // 発光値  
    private float emissionValue = 0.0f;

    // 次の発光値  
    private float nextEmissionValue = 0.0f;

    // 最低発光値  
    [SerializeField]
    private float minEmissionValue = 0.0f;

    // 更新係数  
    [SerializeField]
    private float updateTimeFactor = 1.0f;

    // 更新時間  
    private float updateTime;

    // Start is called before the first frame update
    void Start()
    {
        m_material = GetComponent<Renderer>();
        // 次の発光値を計算  
        CalcNextEmissionValue();
        // シェーダーに発光値をセット  
        m_material.material.SetFloat("_EmissionOOFN", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // 更新時間を加算  
        updateTime += Time.fixedDeltaTime;

        // 時間係数を計算  
        float factor = Mathf.Min((updateTime / updateTimeFactor), 1.0f);

        // 補間した発光値を計算  
        float v = Mathf.Lerp(emissionValue, nextEmissionValue, factor);

        if (m_material != null)
        {
            m_material.material.EnableKeyword("_EMISSION");
            // シェーダーに発光値をセット  
            m_material.material.SetFloat("_Emission00FN", v);

            DynamicGI.SetEmissive(m_material, new Color(1.0f, 1.0f, 1.0f));
        }

        if (factor >= 1.0f)
        {
            updateTime = 0.0f;
            emissionValue = nextEmissionValue;
            CalcNextEmissionValue();
        }
    }

    void CalcNextEmissionValue()
    {
        // ランダム値を取得  
        float r = Random.Range(0.0f, 1.0f);

        // 次の発光値を間欠カオス法で計算  
        if (r <= 0.01f)
        {
            nextEmissionValue = r + 0.02f;
        }
        else if (r < 0.5f)
        {
            nextEmissionValue = r + 2.0f * r * r;
        }
        else if (r >= 0.99f)
        {
            nextEmissionValue = r - 0.01f;
        }
        else
        {
            nextEmissionValue = r - 2.0f * (1.0f - r) * (1.0f - r);
        }

        nextEmissionValue = Mathf.Max(nextEmissionValue, minEmissionValue);
    }
}
