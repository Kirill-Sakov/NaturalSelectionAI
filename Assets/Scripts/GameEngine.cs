using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public Vector2 area = new Vector2(10f, 10f);

    public GameObject bacteriumPrefab;
    public GameObject boidPrefab;
    public GameObject foodPrefab;

    private int frame = 0;

    // Start is called before the first frame update
    void Start()
    {
        Evolution();
    }

    private void Boids()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject b = Instantiate(boidPrefab, new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), 0), Quaternion.identity);
        }
    }

    private void Evolution()
    {
        for (int i = 0; i < 100; i++)
        {
            Genome genome = new Genome(64);
            GameObject b = Instantiate(bacteriumPrefab, new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), 0), Quaternion.identity);
            b.name = "bacterium";
            b.GetComponent<BacteriaAgent>().Init(genome);
        }
        for (int i = 0; i < 1000; i++)
        {
            GameObject food = Instantiate(foodPrefab, new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), 0), Quaternion.identity);
            food.name = "food";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (frame % 1 == 0)
        {
            GameObject food = Instantiate(foodPrefab, new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), 0), Quaternion.identity);
            food.name = "food";
        }
        frame++;

        if ((frame != 0) && (frame % 500 == 0))
        {

            int countGreenFoodSkill = (from i in BacteriaAgent.listBacteria
                                  where
                                    i.attackSkill == 0 // Травоядный тот, кто не атакует других
                                  select i
                                  ).Count();

            int countAttackSkill = (from i in BacteriaAgent.listBacteria
                                     where
                                       i.attackSkill > i.defSkill // Хищник тот, у кого атака развита больше, чем защита
                                    select i
                                  ).Count();
            
            int countDefSkill = (from i in BacteriaAgent.listBacteria
                                 where
                                   i.defSkill >= i.attackSkill &&
                                   i.defSkill != 0
                                 select i
                                  ).Count();

            System.IO.File.AppendAllText(@"E:\Programming\source\repos\My\NaturalSelectionAI\stats\statistic.txt",
                $"{frame}\t{BacteriaAgent.listBacteria.Count}\t{countGreenFoodSkill}\t{countAttackSkill}\t{countDefSkill}" + System.Environment.NewLine);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
