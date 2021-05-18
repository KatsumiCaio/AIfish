using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockingManager myManager; //puxando as informações do FlockingManager
    float speed;// variavel para o speed
    bool turning = false;
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);// atribuindo o posicionamento minimo e máximo de forma randomica
        
    }

   
    void Update()
    {
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);// Para definir o espaço maximo que cada peixe pode nadar longe dos outros
        RaycastHit hit = new RaycastHit();//para identificar o objeto que tem que colidir
        Vector3 direction = myManager.transform.position - transform.position;

        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
            else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
            
        else
            turning = false;

        if (turning)
        {
            Vector3 direcion = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direcion),
                myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules();// chamando o metodo criado para as novas regras adicionadas
        }

            transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules() //criando o metodo para as novas regras
    {
        GameObject[] gos; // criando o Array
        gos = myManager.allFish; // atribuindo todo o conteudo do allfish

        Vector3 vcentre = Vector3.zero;//para fazer o calculo do ponto médio dos peixes
        Vector3 vavoid = Vector3.zero;// para evitar a colisão dos peixes
        float gSpeed = 0.01f;// para acionar a movimentação
        float nDistance;//para calcular cada um dos objetos e seu grupo central
        float groupSize = 0;// para calcular cada grupo de peixes

        foreach(GameObject go in gos)// para fazer a ação dos calculos do vcentre e a quantidade dos peixes e os cardumes.
        {
            if(go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance <= myManager.neibhrourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(nDistance < 1.0)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if(groupSize > 0)// para fazer quando o grupo for maior que zero vai tomar mais corpo do cardume e calculando a quantidade de itens e tambem o ponto médio dentro da movimentação
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;// para fazer a movimentação mais parecida com de um peixe.
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);

        }

    }
}
