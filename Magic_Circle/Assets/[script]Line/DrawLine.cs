using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawLine : MonoBehaviour
{
    public CheckCircle checkcircle;
    public int lineWidth;
    public GameObject linePrefabBold,linePrefabNormal,linePrefabThin;
    GameObject line;
    LineRenderer LR,reLR;
    private int positionCount;
    private int maxPaint = 100;//���ŏ������
    private int maxCount = 20;//1��̖��@�w�ɏ�����{��
    private int maxSaveMagicCircle = 100; //���@�w���Q�[���ɕۑ����Ă����鐔
    private int saveCount = 0;
    private Camera mainCamera;
    float[] drawX;
    float[] drawY;
    public float[,,] magicCircle;//���@�w���
    public float[,,,] SaveMagicCircle;
    int linePosX = 0, linePosY = 1, linePositionCount = 0, lineCount = 0;
    //1�s�ɂ�x�̒l�C2�s�ɂ�y�̒l������D0��ڂ͉��������Ă��炸�C1����99��܂łɒl������
    //���20�܂Ő���������
    // Start is called before the first frame update
    void Start()
    {
        lineWidth = 2;//1 -> ���� 2 -> ���� 3-> �ׂ�
        positionCount = 0;
        mainCamera = Camera.main;
        drawX = new float[maxPaint];
        drawY = new float[maxPaint];
        magicCircle = new float[2, maxPaint, maxCount];
        SaveMagicCircle = new float[2, maxPaint, maxCount, maxSaveMagicCircle];
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(lineWidth);
        // ���W�w��̐ݒ�����[�J�����W�n�ɂ������߁A�^������W�ɂ����������
        Vector3 pos = Input.mousePosition;
        pos.z = 10.0f;
        // �}�E�X�X�N���[�����W�����[���h���W�ɒ���
        pos = mainCamera.ScreenToWorldPoint(pos);

        //�����͈͊O�ɏ������Ƃ�����C����̐������������C�܂�����Ȏ��ɏ���������,���s�l�̒~�ςɂ��邩
        if (Input.GetMouseButtonDown(0)�@&& lineCount < 20)
        {
            //Debug.Log(Math.Pow(pos.x, 2) + Math.Pow(pos.y, 2));
            switch (lineWidth)
            {
                case 1:
                    line = Instantiate(linePrefabBold);
                    break;
                case 2:
                    line = Instantiate(linePrefabNormal);
                    break;
                case 3:
                    line = Instantiate(linePrefabThin);
                    break;
                default:
                    line = Instantiate(linePrefabBold);
                    break;
            }
            LR = line.GetComponent<LineRenderer>();
        }
        if (Input.GetMouseButtonUp(0) && positionCount > 0 && lineCount < 20)
        {
            if(positionCount < 2) Destroy(line); //�����̃N���b�N�Ɣ���D���̏ꍇ�͐����Ђ��Ȃ�
            else
            {
                
                for (linePositionCount = 0; linePositionCount < maxPaint; linePositionCount++)//�d�l�ɂ��C1����99�܂łɒl�������Ă���
                {
                    magicCircle[linePosX, linePositionCount, lineCount] = drawX[linePositionCount];
                    magicCircle[linePosY, linePositionCount, lineCount] = drawY[linePositionCount];
                }
                magicCircle[linePosX, 0, lineCount] = lineWidth;
                magicCircle[linePosY, 0, lineCount] = lineWidth;
                checkcircle.Check_Circle(lineCount,positionCount);
                lineCount++;
            }
            positionCount = 0;
            
        }
        if (Input.GetMouseButton(0))
        {
            // ��������[�J�����W�ɒ����B
            pos = transform.InverseTransformPoint(pos);
            // ����ꂽ���[�J�����W�����C�������_���[�ɒǉ�����
            if (positionCount < maxPaint - 1)
            {
                if (drawX[positionCount] != pos.x || drawY[positionCount] != pos.y)
                {
                    positionCount++;
                    drawX[positionCount] = pos.x;
                    drawY[positionCount] = pos.y;

                    LR.positionCount = positionCount;
                    LR.SetPosition(positionCount - 1, pos);
                }
            }
            
        }
        //Debug.Log(pos.x);
        //if (Math.Pow(pos.x, 2) + Math.Pow(pos.y, 2) > 50) Debug.Log("1");
        if (Input.GetKeyDown(KeyCode.D)) LineDelete();
        if (Input.GetKeyDown(KeyCode.S)) LineSave();
        if (Input.GetKeyDown(KeyCode.R)) LineReDraw();

    }
    public void LineDelete()
    {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag("Line");
        foreach(GameObject obj in objects)
        {
            Destroy(obj);
        }
        Array.Clear(magicCircle, 0, magicCircle.Length);
        lineCount = 0;
    }
    public void LineSave()
    {
        int a, b;
        if(saveCount < maxSaveMagicCircle)
        {
            for (a = 0; a < maxPaint; a++)
            {
                for (b = 0; b < maxCount; b++)
                {
                    SaveMagicCircle[linePosX, a, b, saveCount] = magicCircle[linePosX, a, b];
                    SaveMagicCircle[linePosY, a, b, saveCount] = magicCircle[linePosY, a, b];
                }
            }
            saveCount++;
        }
        
    }
    GameObject reLine;
    int a =0, b=0;

    public void LineReDraw()
    {
        
        if(SaveMagicCircle[0,0,0,0] != 0)
        {
            for (b = 0; b < maxCount; b++) 
            {
                switch (SaveMagicCircle[linePosX, 0, b, 0])
                {
                    case 1:
                        reLine = Instantiate(linePrefabBold);
                        break;
                    case 2:
                        reLine = Instantiate(linePrefabNormal);
                        break;
                    case 3:
                        reLine = Instantiate(linePrefabThin);
                        break;
                }
                reLR = reLine.GetComponent<LineRenderer>();
                while (SaveMagicCircle[linePosX,a,b,0] != 0 && a < maxPaint-1)
                {
                    a++;
                    Vector2 drawPos = new Vector2(SaveMagicCircle[linePosX, a, b, 0], SaveMagicCircle[linePosY, a, b, 0]);
                    Debug.Log(drawPos);
                    reLR.positionCount = a;
                    reLR.SetPosition(a - 1, drawPos);
                }
                a = 0;
                /*
                for (a = 0; a < maxPaint; a++)
                {
                    Vector2 drawPos = new Vector2(SaveMagicCircle[linePosX, a, b, 0], SaveMagicCircle[linePosY, a, b, 0]);
                    
                    reLR = reLine.GetComponent<LineRenderer>();
                    reLR.positionCount = a;
                    reLR.SetPosition(positionCount - 1, drawPos);
                }
                */
            }
            
        }
        
        /* if (positionCount < maxPaint - 1)
         {
             if (positionCount < maxPaint && (drawX[positionCount] != pos.x || drawY[positionCount] != pos.y))
             {
                 positionCount++;
                 drawX[positionCount] = pos.x;
                 drawY[positionCount] = pos.y;

                 LR.positionCount = positionCount;
                 LR.SetPosition(positionCount - 1, pos);
             }
         }*/
    }
}
