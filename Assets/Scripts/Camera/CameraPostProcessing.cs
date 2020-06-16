using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// 使用方法，
/// 预先在 PostProcessing 下，在 ColorGrading -> Lift 圆环 设置好 目标枯萎程度的值
/// 游戏启动则会记录该值 到 targetLift，并重置 Lift 为 0
/// 之后设置 SetDroopEffect 来得到默认Lift 到 目标 Lift 的插值程度
/// </summary>
public class CameraPostProcessing : DCLSingletonBase<CameraPostProcessing>
{
    [SerializeField] PostProcessVolume volume;
    ColorGrading grading;
    Vector4 targetLift;

    private float curDegree = 0f;

    public void Start()
    {
        volume.profile.TryGetSettings(out grading);
        grading.enabled = new BoolParameter { value = true };
        GetCurLiftAsTarget();
        grading.lift.value = new Vector4(1, 1, 1, 0);
        // SetDroopEffect(0f);
        SetSaturation(0f);
    }

    public void GetCurLiftAsTarget()
    {
        targetLift = grading.lift.value;
    }

    [ContextMenu("Print Cur Lift Value")]
    private void PrintCurLift()
    {
        Debug.Assert(volume.profile.TryGetSettings(out grading), "目前 Processing Volume 中未包含 ColorGrading");
        Debug.Log($"Cur Lift {grading.lift.value}");
    }

    /// <summary>
    /// 后处理设置 枯萎颜色的程度
    /// </summary>
    /// <param name="degree"> 0 - 1f 的枯萎程度 </param>
    public void SetDroopEffect(float degree)
    {
        // degree = Mathf.Clamp01(degree);
        // grading.lift.Interp(new Vector4(0.1f, 0.1f, .5f, 0), targetLift, degree);
        grading.lift.value = new Vector4(0.1f, .1f, .1f, 0);
    }

    /// <summary>
    /// 后处理设置 画面色调
    /// </summary>
    /// <param name="degree"> 0f - 100f 的掉色程度，100f 为全黑白 </param>
    public void SetSaturation(float degree)
    {
        grading.saturation.value = -degree;
    }
}
