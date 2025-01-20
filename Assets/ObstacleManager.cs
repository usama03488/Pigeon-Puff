using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    public float spawnDistance;
    public float DestoryDistance;

    public Vector2 CurrentPosition;
    public GameObject ObstacleLine;
    public GameObject MovingObstacleLine;
    public float CurrentPositionY;
    private int HighestPoint;
    public int _stage;
    public int ScoreStage;
    public int AbilityDistance;
    public bool AbilityDrop;
    public TMP_Text StageText;

    //Ability
    public GameObject[] Abilities;
    public GameObject LineOneJump;
    public GameObject LineNoJump;



    public Material ENV1;
    public Material ENV2;
    public Material ENV3;
    public Material ENV4;
   // public bool ReadyToDropAbility;

    // Start is called before the first frame update
    void Start()
    {
        CurrentPosition = transform.position;
       /* if (!ReadyToDropAbility)
        {
            StartCoroutine(DropAbilityActivate());
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > HighestPoint)
        {
            HighestPoint = (int)transform.position.y;

            if (Mathf.Abs(transform.position.y - CurrentPosition.y) > 1.5)
            {

                CurrentPosition = transform.position;
                SpawnObstacle();


            }
        }
        if (_stage <10)
        {
            if (transform.position.y > ScoreStage)
            {
                ScoreStage = ScoreStage + 30;
                _stage++;
                StageText.text = _stage + "";
            }
        }
        if (transform.position.y > AbilityDistance)
        {
            AbilityDistance = AbilityDistance + 30;
            AbilityDrop = true;
        }

    }

/*    IEnumerator DropAbilityActivate()
    {
        yield return new WaitForSeconds(1f);
        ReadyToDropAbility = true;
        yield return new WaitForSeconds(10f);
        ReadyToDropAbility = false;

        StartCoroutine(DropAbilityActivate());


    }*/

    void SpawnObstacle()
    {
        if (_stage >= 1 && _stage <= 4)
        {
            if (_stage == 1)
            {


                RenderSettings.skybox = ENV1;
            }else if(_stage ==2)
            {
                RenderSettings.skybox = ENV2;
            }
            else if (_stage == 3)
            {
                RenderSettings.skybox = ENV3;
            }
            else if (_stage == 4)
            {
                RenderSettings.skybox = ENV4;
            }

            int RandomeValue = Random.Range(0, 5);
            if (RandomeValue == 0)
            {
                GameObject Obj1 = Instantiate(ObstacleLine, new Vector3(-2.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                
                if (AbilityDrop)
                {
                    Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    AbilityDrop = false;
                }
                else
                {
                    GameObject Obj2 = Instantiate(ObstacleLine, new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                }
              
            }
           else if (RandomeValue == 1)
            {
                GameObject Obj1 = Instantiate(ObstacleLine, new Vector3(Random.Range(-3.2f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                GameObject Obj2 = Instantiate(ObstacleLine, new Vector3(Random.Range(-1.5f, -3.2f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
            }
            else if (RandomeValue == 2)
            {
                GameObject Obj1 = Instantiate(ObstacleLine, new Vector3(-2.91f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
              
                if (AbilityDrop)
                {
                    Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    AbilityDrop = false;
                }
                else
                {
                    GameObject Obj2 = Instantiate(ObstacleLine, new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                }
                // Instantiate(HelicopterAbility, Obj1.transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
            }
            else if (RandomeValue == 3)
            {
                GameObject Obj2 = Instantiate(ObstacleLine, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
            }
            else
            {
                GameObject Obj2 = Instantiate(LineOneJump, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
            }

        }
      else if (_stage >= 5 && _stage <= 10)
        {
            if(_stage==10)
            {
               // Debug.Log("After 11 level");
                int RandomeValue = Random.Range(0, 5);
                if (RandomeValue == 0)
                {
                    GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(-2.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    Obj1.GetComponent<MovingLine>().MovingSpeed = 0.6f;
                    if (AbilityDrop)
                    {
                        Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                        AbilityDrop = false;
                    }
                    else
                    {
                        GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                        Obj2.GetComponent<MovingLine>().MovingSpeed = 0.6f;
                    }
                }
                else if (RandomeValue == 1)
                {
                    GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-3.2f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    Obj1.GetComponent<MovingLine>().MovingSpeed = 0.6f;
                    GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-1.5f, -3.2f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    Obj2.GetComponent<MovingLine>().MovingSpeed = 0.6f;
                }
                else if (RandomeValue == 2)
                {
                    GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(-2.91f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    Obj1.GetComponent<MovingLine>().MovingSpeed = 0.6f;
                    if (AbilityDrop)
                    {
                        Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                        AbilityDrop = false;
                    }
                    else
                    {
                        GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                        Obj2.GetComponent<MovingLine>().MovingSpeed = 0.6f;
                    }
                }
                else if (RandomeValue == 3)
                {
                    GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    Obj2.GetComponent<MovingLine>().MovingSpeed = 0.6f;
                }
                else
                {
                    GameObject Obj2 = Instantiate(LineOneJump, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                 //   Obj2.GetComponent<MovingLine>().MovingSpeed = 0.4f;
                }
            }
            else
            {
               // Debug.Log("After 11 level");
                int RandomeValue = Random.Range(0, 5);
                if (RandomeValue == 0)
                {
                    GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(-2.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);

                    if (AbilityDrop)
                    {
                        Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                        AbilityDrop = false;
                    }
                    else
                    {
                        GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    }
                }
                else if (RandomeValue == 1)
                {
                    GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-3.2f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-1.5f, -3.2f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                }
                else if (RandomeValue == 2)
                {
                    GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(-2.91f, transform.position.y + spawnDistance, 1f), Quaternion.identity);

                    if (AbilityDrop)
                    {
                        Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                        AbilityDrop = false;
                    }
                    else
                    {
                        GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    }
                }
                else if (RandomeValue == 3)
                {
                    GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                }
                else
                {
                    GameObject Obj2 = Instantiate(LineOneJump, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                }
            }
           
        }
        else
        {
          //  Debug.Log("After 10 level");
            int RandomeValue = Random.Range(0, 5);
            if (RandomeValue == 0)
            {
                GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(-2.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                Obj1.GetComponent<MovingLine>().MovingSpeed = 0.4f;
                if (AbilityDrop)
                {
                    Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    AbilityDrop = false;
                }
                else
                {
                    GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(-5.5f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    Obj2.GetComponent<MovingLine>().MovingSpeed = 0.4f;
                }
            }
            else if (RandomeValue == 1)
            {
                GameObject Obj1 = Instantiate(LineOneJump, new Vector3(Random.Range(-3.2f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
               // Obj1.GetComponent<MovingLine>().MovingSpeed = 0.4f;
                GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-1.5f, -3.2f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
              //  Obj2.GetComponent<MovingLine>().MovingSpeed = 0.4f;
            }
            else if (RandomeValue == 2)
            {
                GameObject Obj1 = Instantiate(MovingObstacleLine, new Vector3(-2.91f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                Obj1.GetComponent<MovingLine>().MovingSpeed = 0.4f;
                if (AbilityDrop)
                {
                    Instantiate(Abilities[Random.Range(0, Abilities.Length)], new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    AbilityDrop = false;
                }
                else
                {
                    GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(-5.1f, transform.position.y + spawnDistance, 1f), Quaternion.identity);
                    Obj2.GetComponent<MovingLine>().MovingSpeed = 0.4f;
                }
            }
            else if (RandomeValue == 3)
            {
                GameObject Obj2 = Instantiate(MovingObstacleLine, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
                Obj2.GetComponent<MovingLine>().MovingSpeed = 0.4f;
            }
            else
            {
                GameObject Obj2 = Instantiate(LineOneJump, new Vector3(Random.Range(-1.5f, -5.5f), transform.position.y + spawnDistance, 1f), Quaternion.identity);
            }
        }

    }
}
public enum Stages
{
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Stage6,
    Stage7,
    Stage8,
    Stage9,
    Stage10
}
