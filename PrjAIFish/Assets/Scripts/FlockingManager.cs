using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public GameObject fishPrefab; //variavel para o Prefab
    public int numFish = 20;// variavel para a quantidade de peixes 
    public GameObject[] allFish; //array para agrupar todos os peixes 
    public Vector3 swinLimits = new Vector3(5, 5, 5);// Para reprocriar nesse espaço informado
    public Vector3 goalPos;// variavel para fazer a identificação do eixo, para que conforme mude o posicionamento vai seguindo o gerenciador flock

    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]// controlador de velociodade
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neibhrourDistance;// a distancia entre os outros peixes
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;// velocidade de rotação

    void Start()
    {
        allFish = new GameObject[numFish];// o allFish é composto pelo nº de peixes colocados no Array
        for(int i = 0; i< numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);// procriando o item que é o objeto nas posicões acima
            allFish[i].GetComponent<Flock>().myManager = this;// para se multiplicar e se movimentar
        }
        goalPos = this.transform.position;
    }

  void Update()
    {
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z)); //para fazer a movimentação do peixe
    }
}
