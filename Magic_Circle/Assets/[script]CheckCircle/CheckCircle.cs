using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CheckCircle : MonoBehaviour
{
    float[,] target;
    public DrawLine drawline;
    public GameObject lineMissCheck,lineAnswer;
    GameObject line;
    LineRenderer LR;

    [Header("goodCountとbadCountの割合")]
    [SerializeField] int requiredGoodPercentage;
    [Header("閾値: 0に近いほど真円    1はすべてOK")]
    [SerializeField] float requiredAccuracy;//チェックが基準
    // Start is called before the first frame update
    void Start()
    {
        target = new float[2, 100];
        requiredGoodPercentage = 65;
        requiredAccuracy = 0.3f;

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Check_Circle(int num,int count)//numは何番目の線か，countは線の長さ（positionCountの値）
    {
        if (count < 10)
        {
            Debug.Log("短すぎるため，円としてカウントしない");
            return;
        }
        int badCount = 0, goodCount = 0;
        List<Vector2> target = new List<Vector2>();
        for (int i = 1; i <= count; i++)
        {
            float x = drawline.magicCircle[0, i, num];
            float y = drawline.magicCircle[1, i, num];
            if (x == 0 && y == 0) // 配列の終端に到達したら、ループを終了する
            {
                break;
            }
            target.Add(new Vector2(x, y));
        }
        float maxX = target.Max(v => v.x);
        float maxY = target.Max(v => v.y);
        float minX = target.Min(v => v.x);
        float minY = target.Min(v => v.y);
        Vector2 center = new Vector2((maxX + minX) / 2, (maxY + minY) / 2);
        float radius = ((maxX - center.x) + (maxY - center.y)) / 2;//半径
        float radius2 = radius * radius;

        for (int i = 0; i < target.Count; i++)
        {
            Vector2 targetVec = new Vector2(target[i].x, target[i].y);
            Vector2 CirCenter = new Vector2(center.x, center.y);
            if (Vector2.Distance(targetVec, CirCenter) >= radius * 1 - requiredAccuracy &&
                Vector2.Distance(targetVec, CirCenter) <= radius * 1 + requiredAccuracy) goodCount++;
            else {
                badCount++;
                Vector3 point = new Vector3(target[i].x, target[i].y, 0.0f);
                Instantiate(lineMissCheck, point, Quaternion.identity);
                Debug.Log(i);
            }
        }//円の判定
        float checknum = 100.0f * goodCount / (badCount + goodCount);
        Debug.Log(checknum + "%");
        CreateCircle(center.x, center.y, radius);
        if (checknum > requiredGoodPercentage)
        {
            //CreateCircle(center.x, center.y, radius);
            //drawline.LineDelete();
            //点数追加
        }
        
        //今後：陣全体をいくつかに分解して判断．円を書くと最も近い位置で判定
    }//描いた魔法陣はそのまま残るが，内部的に何点か評価を行いたい．


    void CreateCircle(float cirx, float ciry,float R)
    {
        line = Instantiate(lineAnswer);
        LR = line.GetComponent<LineRenderer>();
        Vector3 center = new Vector3(cirx, ciry, 10.0f);
        float radius = R;
        int numpoints = 30;
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numpoints; i++)
        {
            float angle = i * Mathf.PI * 2f / numpoints;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            points.Add(center + new Vector3(x, y, 0f));
        }
        points.Add(points[0]);
        LR.positionCount = numpoints + 1;
        for(int i = 0; i < numpoints + 1; i++)
        {
            LR.SetPosition(i,points[i]);
        }

    }
}
