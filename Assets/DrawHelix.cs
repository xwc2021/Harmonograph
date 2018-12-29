using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pattern
{
    Const,
    Increase,
    Decrease,
    Cos,
    Sin,
}

public class DrawHelix : MonoBehaviour
{
    void getVertexArray(float round,float R,float spin, Pattern X, Pattern Y,Vector2[] vertexArray) {

        int count = vertexArray.Length;
        var totalRound = round * 2.0f * Mathf.PI;
        var diff = totalRound / count;
        for (var i = 0; i < count; ++i)
        {
            var rad = diff * i;
            var ratio = rad / totalRound;

            float rX = R;
            switch (X) {
                case Pattern.Increase:
                    rX = R * ratio;
                    break;

                case Pattern.Decrease:
                    rX = R * (1.0f-ratio);
                    break;

                case Pattern.Cos:
                    rX = R * Mathf.Cos(spin + rad);
                    break;

                case Pattern.Sin:
                    rX = R * Mathf.Sin(spin + rad);
                    break;
            }

            float rY = R;
            switch (Y)
            {
                case Pattern.Increase:
                    rY = R * ratio;
                    break;

                case Pattern.Decrease:
                    rY = R * (1.0f - ratio);
                    break;

                case Pattern.Cos:
                    rY = R * Mathf.Cos(spin + rad);
                    break;

                case Pattern.Sin:
                    rY = R * Mathf.Sin(spin + rad);
                    break;
            }

            var x = Mathf.Cos(spin+rad) * rX;
            var y = Mathf.Sin(spin+rad) * rY;
            vertexArray[i] += new Vector2(x, y);
        }
    }

    public Pattern[] X;
    public Pattern[] Y;
    public float[] spin;
    public float[] F;
    public float[] R;
    public int Count=1000;

    public float[] spinDiff;
    public bool[] animateSpin;

    public Material mat;
    public void OnPostRender(){
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix(); // Save the current state
        mat.SetPass(0);
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINES);

        var vertexArray=new Vector2[Count];

        for (var i = 0; i < F.Length; ++i) {
            if(animateSpin[i])
                spin[i] += spinDiff[i];
        }

        for (var i = 0; i < F.Length; ++i) 
            getVertexArray(F[i],R[i], spin[i],X[i],Y[i],vertexArray);

        var offset = 0.5f*new Vector2(Screen.width,Screen.height);
        var count = vertexArray.Length;

        GL.Color(Color.yellow);
        for (var i = 0; i < count-1; ++i)
        {
            var now = vertexArray[i];
            var next = vertexArray[(i + 1)];
            GL.Vertex3(offset.x + now.x, offset.y + now.y, 0);
            GL.Vertex3(offset.x + next.x, offset.y + next.y, 0);

            GL.TexCoord2((float)i/count,0.0f);
        }


        GL.End();

        GL.PopMatrix(); // Pop changes.
    }
}
