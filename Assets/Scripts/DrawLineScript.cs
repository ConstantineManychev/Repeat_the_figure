using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLineScript : MonoBehaviour
{
    public string thisLevel;

    public static int points=0;
    public GUIText scoreText;

    public Transform sled;

    public GameObject three;
    public GameObject threeR;
    public GameObject four;
    public GameObject fourT;
    public GameObject fourP;
    private GameObject figure;
    private GameObject figureObj;


    public static int timeToLoose=30;
    private LineRenderer line;
    private bool mouseMoved;
    private int angleNum;//номер угла
    private int maxAngle = 7;//максимальное кол-во углов
    public float timeLeft=60;//таймер
    public float timeToCheck;//таймер на время "простоя"
    private Vector3[] mousePos = new Vector3[7];//координаты позиций мышки для углов
    private Vector3 mouseP;

    //Для переходов в меню
    public static bool mainMenu=true;
    public static bool looseMenu=false;
    public static bool winMenu=false;
    public static bool pause=true;

    //
    private float a0;
    private float b0;
    private float c0;
    private float d0;
    private int sootn;

    private int a = 0, b = 0, c = 0, d = 0;
    private int ax = 0, bx = 0, cx = 0, dx = 0;
    private int ay = 0, by = 0, cy = 0, dy = 0;


    void Start ()
    {
        line = transform.GetComponent<LineRenderer> ();
        line.SetWidth(1,1);//установка ширины линии в начале и конце
        angleNum = 0;
        mouseMoved = false;
        timeLeft = timeToLoose;//устанавливаем таймер
        scoreText.text = "Счет: " + points;//Обновить текст очков

        figureChoose();

    }

    void Update ()
    {
        timeLeft -= Time.deltaTime;//отсчет таймера
        timeToCheck += Time.deltaTime;//прибавление времени

        //когда вышло время
        if (timeLeft <= 0.0f)
        {
            Time.timeScale = 0;//остановить течение игры
            pause = true;
            looseMenu = true;
        }

        //при нажатии левой клавиши мыши
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(sled, Camera.main.ScreenToWorldPoint(Input.mousePosition), this.transform.rotation);
            SetMousePos(angleNum);
            timeToCheck = 0;
            angleNum++;

        }

        //при удержании левой клавиши мыши
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //если текущее положение курсора отличается от сохраненного
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition) != mouseP)
            {
                mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                timeToCheck = 0;//обнуляем таймер простоя
                mouseMoved = true;
                Instantiate(sled, Camera.main.ScreenToWorldPoint(Input.mousePosition), this.transform.rotation);
            }

            //если курсор был смещен от установленного угла и не двигается 10 миллисекунд
            else if (angleNum < maxAngle && timeToCheck >= 0.5f && mouseMoved == true)
            {
                timeToCheck = 0;//обнуляем таймер простоя
                SetMousePos(angleNum);
                angleNum++;//аереходим на след. угол
            }
        }

        //при отпуске левой клавиши мыши
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            angleNum = 0;//скидываем счетчик углов
            if (Mathf.RoundToInt(mousePos[0].x) >= Mathf.RoundToInt(mouseP.x - 0.5f) && Mathf.RoundToInt(mousePos[0].x) <= Mathf.RoundToInt(mouseP.x + 0.5f) && Mathf.RoundToInt(mousePos[0].y) >= Mathf.RoundToInt(mouseP.y - 0.5f) && Mathf.RoundToInt(mousePos[0].y) <= Mathf.RoundToInt(mouseP.y + 0.5f))
            {
                if (mousePos[0].x != 0 && mousePos[1].x != 0 && mousePos[2].x != 0)
                {
                    if (mousePos[4].x != 0)
                    {
                        sootnoshenie(4);
                    }
                    else { sootnoshenie(3); }

                }
            }
            for (int i = 0; i < mousePos.Length; i++)
            {
                mousePos[i].x = 0;
                mousePos[i].y = 0;
            }

        }

    }

    //Выбор случайной фигуры
    void figureChoose()
    {
        int randFigure;
        randFigure = Random.Range(0, 4);
        //в зависимости от выпавшего случайного числа ставим фигуру
        switch (randFigure)
        {
            case 0:
                figure = three;
                a0 = 5.83f;
                b0 = 6;
                c0 = a0;
                break;
            case 1:
                figure = threeR;
                a0 = 7.81f;
                b0 = 6;
                c0 = 5;
                break;
            case 2:
                figure = four;
                a0 = 6;
                b0 = 5;
                c0 = 6;
                d0 = 5;
                break;
            case 3:
                figure = fourT;
                a0 = 3;
                b0 = 5;
                c0 = 5;
                d0 = 5;
                break;

        }
        Instantiate(figure, new Vector3(0, 4, 0), this.transform.rotation);
    }

    //Установка точки ломаной линии по координатам мышки
    void SetMousePos(int i)
    {
        mousePos[i] = Input.mousePosition;//смотрим координаты курсора
        mousePos[i] = Camera.main.ScreenToWorldPoint(mousePos[i]);//относительно центра камеры
        mouseP = mousePos[i];
        line.SetVertexCount(i + 1);//Добавляем угол
        line.SetPosition(i, new Vector3(mousePos[i].x, mousePos[i].y, 0));//устанавливаем угол по координатам
        mouseMoved = false;//
    }

    //находим углы a,b,c,d и расчитываем отношение между сторонами нарисованной фигуры и требуемой
    void sootnoshenie(int ugli)
    {
        //сперва присваиваем углы как хотим
        ax = Mathf.RoundToInt(mousePos[0].x);
        ay = Mathf.RoundToInt(mousePos[0].y);
        bx = Mathf.RoundToInt(mousePos[1].x);
        by = Mathf.RoundToInt(mousePos[1].y);
        cx = Mathf.RoundToInt(mousePos[2].x);
        cy = Mathf.RoundToInt(mousePos[2].y);
        dx = 0;
        dy = 0;
        if (ugli == 4)//В зависимости от требуемой фигуры будем искать 4й угол или нет
        {
            dx = Mathf.RoundToInt(mousePos[3].x);
            dy = Mathf.RoundToInt(mousePos[3].y);
        }

        //расставляем углы по своим местами
        for (int i=0;i < ugli;i++)
            {
                if (ugli == 4)
                {
                    if (Mathf.RoundToInt(mousePos[i].y) >= ay && Mathf.RoundToInt(mousePos[i].x) <= ax)
                    {
                        ax = Mathf.RoundToInt(mousePos[i].x);
                        ay = Mathf.RoundToInt(mousePos[i].y);
                    }
                    else if (Mathf.RoundToInt(mousePos[i].y) >= by && Mathf.RoundToInt(mousePos[i].x) >= bx)
                    {
                        bx = Mathf.RoundToInt(mousePos[i].x);
                        by = Mathf.RoundToInt(mousePos[i].y);
                    }
                    else if (Mathf.RoundToInt(mousePos[i].y) <= cy && Mathf.RoundToInt(mousePos[i].x) >= cx)
                    {
                        cx = Mathf.RoundToInt(mousePos[i].x);
                        cy = Mathf.RoundToInt(mousePos[i].y);
                    }
                    else if (Mathf.RoundToInt(mousePos[i].y) <= dy && Mathf.RoundToInt(mousePos[i].x) <= dx)
                    {
                        dx = Mathf.RoundToInt(mousePos[i].x);
                        dy = Mathf.RoundToInt(mousePos[i].y);
                    }
                }
                else
                {
                    if (Mathf.RoundToInt(mousePos[i].y) > ay)
                    {
                        ax = Mathf.RoundToInt(mousePos[i].x);
                        ay = Mathf.RoundToInt(mousePos[i].y);
                    }
                    else if (Mathf.RoundToInt(mousePos[i].y) < by && Mathf.RoundToInt(mousePos[i].x) > bx)
                    {
                        bx = Mathf.RoundToInt(mousePos[i].x);
                        by = Mathf.RoundToInt(mousePos[i].y);
                    }
                    else if (Mathf.RoundToInt(mousePos[i].y) < cy && Mathf.RoundToInt(mousePos[i].x) < cx)
                    {
                        cx = Mathf.RoundToInt(mousePos[i].x);
                        cy = Mathf.RoundToInt(mousePos[i].y);
                    }
                }
            }

        a = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow((bx - ax), 2) + Mathf.Pow((by - ay),2)));
        b = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow((cx - bx), 2) + Mathf.Pow((cy - by), 2)));

        if (ugli == 4)
        {
            c = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow((dx - cx), 2) + Mathf.Pow((dy - cy),2)));
            d = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow((ax - dx), 2) + Mathf.Pow((ay - dy),2)));
        }
        else
        {
            c = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow((ax - cx), 2) + Mathf.Pow((ay - cy),2)));
        }
        sootn =Mathf.RoundToInt( a / a0);//устанавливаем отношение между размерами фигур(нарисоаной и требуемой)
        if (d == 0) win3Check(a, b, c);
        else win4Check(a, b, c, d);
    }

    //Проверка победы для 4х углов
    void win4Check(int a, int b, int c, int d)
    {
        if (((b / sootn) >= Mathf.RoundToInt(b0 - 0.5f) && ((b / sootn) <= Mathf.RoundToInt(b0 + 0.5f)) && ((c / sootn) >= Mathf.RoundToInt(c0 - 0.5f) && (c / sootn) <= Mathf.RoundToInt(c0 + 0.5f)) && ((d / sootn) >= Mathf.RoundToInt(d0 - 0.5f) && (d / sootn) <= Mathf.RoundToInt(d0 + 0.5f))))
        {
            points += 1;//Добавить очко за победу
            scoreText.text = "Счет: " + points;//Обновить текст очков
            Time.timeScale = 0;//остановить течение игры
            pause = true;
            winMenu = true;
        }
    }

    //Проверка победы для 3х углов
    void win3Check(int a, int b, int c)
    {
        if (((b / sootn) >= Mathf.RoundToInt(b0 - 0.5f) && (b / sootn) <= Mathf.RoundToInt(b0 + 0.5f)) && ((c / sootn) >= Mathf.RoundToInt(c0 - 0.5f) && (c / sootn) <= Mathf.RoundToInt(c0 + 0.5f)))
        {
            points += 1;//Добавить очко за победу
            scoreText.text = "Счет: " + points;//Обновить текст очков
            Time.timeScale = 0;//остановить течение игры
            pause = true;
            winMenu = true;
        }
    }

    void OnGUI()
    {
        if (mainMenu == true)
        {
            Time.timeScale = 0;
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2), 100, 30), "Start game"))
            {
                points = 0;//Ставим очки в 0
                pause=false;
                winMenu = false;
                looseMenu = false;
                mainMenu = false;//выйти из меню
                Time.timeScale = 1;//запустить течение времени игры
            }
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2) + 30, 100, 30), "Exit game")) Application.Quit();
        }

        else if (winMenu == true)
        {
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2), 100, 30), "Next level"))
            {
                timeToLoose -= 5;//Уменьшаем время необходимое на задание на 5 миллисекунд
                figureChoose();//выбрать случайную фигуру
                pause = false;
                mainMenu = false;
                looseMenu = false;
                winMenu = false;//выйти из меню
                Time.timeScale = 1;//запустить течение времени игры
                Application.LoadLevel(thisLevel);
            }
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2) + 30, 100, 30), "Exit game")) Application.Quit();
        }

        else if (looseMenu == true)
        {
            GUI.Box(new Rect((Screen.width / 2) - 25, (Screen.height / 2), 100, 30), "Your score is "+points);
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2)+30, 100, 30), "Restart"))
            {
                pause = false;
                mainMenu = false;
                winMenu = false;
                looseMenu = false;//выйти из меню
                Time.timeScale = 1;//запустить течение времени игры
                Application.LoadLevel(thisLevel);
            }
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2) + 60, 100, 30), "Exit game")) Application.Quit();
        }
    }


}