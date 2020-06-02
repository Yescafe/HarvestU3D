using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRectDraw: DCLSingletonBase<CameraRectDraw>
{
    public Material rectMaterial = null;//这里使用Sprite下的defaultshader的材质即可
    public Color rectColor;

    public bool drawRectangle = false;
    public Vector3 rectStart, rectEnd;
    // OnPostRender 只会在当脚本放在相机上才会调用
    // OnPostObject 只会在物体被渲染后才会调用
    void OnPostRender()
    {
        //画线这种操作推荐在OnPostRender（）里进行 而不是直接放在Update，所以需要标志来开启
        // Debug.Log("Post Render");
        if (drawRectangle)
        {
            rectEnd = Input.mousePosition;//鼠标当前位置

            GL.PushMatrix();//保存摄像机变换矩阵,把投影视图矩阵和模型视图矩阵压入堆栈保存
            if (!rectMaterial)
                return;
            rectMaterial.SetPass(0);//为渲染激活给定的pass。
            GL.LoadPixelMatrix();//设置用屏幕坐标绘图
            GL.Begin(GL.QUADS);//开始绘制矩形
            GL.Color(new Color(rectColor.r, rectColor.g, rectColor.b, 0.1f));//设置颜色和透明度，方框内部透明
            //绘制顶点
            GL.Vertex3(rectStart.x, rectStart.y, 0);
            GL.Vertex3(rectEnd.x, rectStart.y, 0);
            GL.Vertex3(rectEnd.x, rectEnd.y, 0);
            GL.Vertex3(rectStart.x, rectEnd.y, 0);

            GL.End();


            GL.Begin(GL.LINES);//开始绘制线
            GL.Color(rectColor);//设置方框的边框颜色 边框不透明

            GL.Vertex3(rectStart.x, rectStart.y, 0);
            GL.Vertex3(rectEnd.x, rectStart.y, 0);
            GL.Vertex3(rectEnd.x, rectStart.y, 0);
            GL.Vertex3(rectEnd.x, rectEnd.y, 0);
            GL.Vertex3(rectEnd.x, rectEnd.y, 0);
            GL.Vertex3(rectStart.x, rectEnd.y, 0);
            GL.Vertex3(rectStart.x, rectEnd.y, 0);
            GL.Vertex3(rectStart.x, rectStart.y, 0);

            GL.End();

            GL.PopMatrix();//恢复摄像机投影矩阵
        }
    }

    void EnableDrawRect()
    {
        drawRectangle = true;
    }
}