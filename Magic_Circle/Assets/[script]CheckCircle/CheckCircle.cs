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

    [Header("goodCount��badCount�̊���")]
    [SerializeField] int requiredGoodPercentage;
    [Header("臒l: 0�ɋ߂��قǐ^�~    1�͂��ׂ�OK")]
    [SerializeField] float requiredAccuracy;//�`�F�b�N���
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
    public void Check_Circle(int num,int count)//num�͉��Ԗڂ̐����Ccount�͐��̒����ipositionCount�̒l�j
    {
        if (count < 10)
        {
            Debug.Log("�Z�����邽�߁C�~�Ƃ��ăJ�E���g���Ȃ�");
            return;
        }
        int badCount = 0, goodCount = 0;
        List<Vector2> target = new List<Vector2>();
        for (int i = 1; i <= count; i++)
        {
            float x = drawline.magicCircle[0, i, num];
            float y = drawline.magicCircle[1, i, num];
            if (x == 0 && y == 0) // �z��̏I�[�ɓ��B������A���[�v���I������
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
        float radius = ((maxX - center.x) + (maxY - center.y)) / 2;//���a
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
        }//�~�̔���
        float checknum = 100.0f * goodCount / (badCount + goodCount);
        Debug.Log(checknum + "%");
        CreateCircle(center.x, center.y, radius);
        if (checknum > requiredGoodPercentage)
        {
            //CreateCircle(center.x, center.y, radius);
            //drawline.LineDelete();
            //�_���ǉ�
        }
        
        //����F�w�S�̂��������ɕ������Ĕ��f�D�~�������ƍł��߂��ʒu�Ŕ���
    }//�`�������@�w�͂��̂܂܎c�邪�C�����I�ɉ��_���]�����s�������D


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
