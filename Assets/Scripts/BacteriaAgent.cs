using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BacteriaAgent : MonoBehaviour
{

    public GameObject bacteriumPrefab;

    // Характеристики агента
    private int[] allSkills;
    public int foodSkill = 0;
    public int attackSkill = 0;
    public int defSkill = 0;
    public float energy = 10;
    public float age = 0;
    public System.Guid id;

    public string str_id;

    private int inputsCount = 4;
    private Genome genome;
    private NN nn;

    private Rigidbody2D rb;
    
    public static List<BacteriaAgent> listBacteria = new List<BacteriaAgent>();

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        id = System.Guid.NewGuid();

        str_id = id.ToString();

        listBacteria.Add(this);

        allSkills = new int[]{
            foodSkill,
            attackSkill,
            defSkill
        };
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90);
        age += Time.deltaTime;
    }

    void FixedUpdate()
    {
        float vision = 5f + attackSkill;
        float[] inputs = new float[inputsCount];
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, vision);

        // количество соседних объектов
        float[] neighboursCount = new float[4];

        // Вектора к центрам масс еды, красного, зеленого и синего
        Vector3[] vectors = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            neighboursCount[i] = 0;
            vectors[i] = new Vector3(0f, 0f, 0f);
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == gameObject) continue;
            if (colliders[i].gameObject.name == "food")
            {
                neighboursCount[0]++;
                vectors[0] += colliders[i].gameObject.transform.position - transform.position;
            }
            else if (colliders[i].gameObject.name == "bacterium")
            {
                BacteriaAgent ai = colliders[i].gameObject.GetComponent<BacteriaAgent>();

                neighboursCount[1] += ai.attackSkill / 3f;
                vectors[1] += (colliders[i].gameObject.transform.position - transform.position) * ai.attackSkill;
                neighboursCount[2] += ai.foodSkill / 3f;
                vectors[2] += (colliders[i].gameObject.transform.position - transform.position) * ai.foodSkill;
                neighboursCount[3] += ai.defSkill / 3f;
                vectors[3] += (colliders[i].gameObject.transform.position - transform.position) * ai.defSkill;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (neighboursCount[i] > 0)
            {
                vectors[i] /= neighboursCount[i] * vision;
                inputs[i] = vectors[i].magnitude;
            }
            else
            {
                inputs[i] = 0f;
            }
        }

        float[] outputs = nn.FeedForward(inputs);
        Vector2 target = new Vector2(0, 0);
        for (int i = 0; i < 4; i++)
        {
            if (neighboursCount[i] > 0)
            {
                Vector2 dir = new Vector2(vectors[i].x, vectors[i].y);
                dir.Normalize();
                target += dir * outputs[i];
            }
        }
        if (target.magnitude > 1f) target.Normalize();
        Vector2 velocity = rb.velocity;
        velocity += target * (0.25f + attackSkill * 0.05f);
        velocity *= 0.98f;
        rb.velocity = velocity;
        energy -= (Time.deltaTime * 0.5f);

        allSkills = new int[]{
            foodSkill,
            attackSkill,
            defSkill
        };

        if (energy < 0f)
        {
            Kill();
        }
        
    }

    public void Kill()
    {
        Destroy(gameObject);

        listBacteria.RemoveAt(listBacteria.FindIndex(
                (e) => e.id == id
            ));
    }

    private void Eat(float food)
    {
        energy += food;
        if (energy > 16)
        {
            energy *= 0.5f;
            var bact = Resources.Load("bacteria", typeof(GameObject));
            GameObject b = (GameObject)Object.Instantiate(bact, new Vector3(0, 0, 0), Quaternion.identity);
            b.transform.position = transform.position;
            b.name = "bacterium";
            Genome g = new Genome(genome);
            g.MutateWeights(0.5f);
            g.MutateSkills(allSkills);
            BacteriaAgent ai = b.GetComponent<BacteriaAgent>();
            ai.Init(g);
            ai.energy = energy;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (foodSkill == 0) return;
        if (col.gameObject.name == "food")
        {
            Eat(foodSkill);
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (age < 1f) return; // Если агент "младше" 1, он не может атаковать
        if (attackSkill == 0) return; // Если атака 0, так же не может атаковать
        if (col.gameObject.name == "bacterium")
        {
            BacteriaAgent ai = col.gameObject.GetComponent<BacteriaAgent>();
            if (ai.age < 1f) return; 

            if (attackSkill == ai.attackSkill) // Если встретил себе подобного - ничего не делать
                return;

            // Рассчёт урона (разница между силой атакующего и защитой обороняющегося)
            float damage = Mathf.Max(0f, attackSkill - ai.defSkill);

            // Если атака оказалась больше
            if (damage > 0)
            {
                Eat(ai.energy); // Хищник забирает энергию жертвы себе
                ai.energy = 0; // Энергия жертвы устанавливается в 0, т.е. смерть агента
            }
        }
    }

    public void Init(Genome g)
    {
        genome = g;
        Color col = new Color(0.1f, 0.1f, 0.25f, 1f);
        float size = 0.75f;

        // Новый алгоритм распределения скиллов. 
        if (g.skills[0] != 0)
        {
            foodSkill = g.skills[0];
            col.g = 0.2f * g.skills[0];
        }
        if (g.skills[1] != 0)
        {
            attackSkill = g.skills[1];
            col.r = 0.25f * g.skills[1];
        }
        if (g.skills[2] != 0)
        {
            defSkill = g.skills[2];
            col.b = 0.25f * g.skills[2];
        }
        if (Random.Range(0, 100) < 2)
        {
            size += 0.5f;
        }

        transform.localScale = new Vector3(size, size, size);
        gameObject.GetComponent<SpriteRenderer>().color = col;
        nn = new NN(inputsCount, 8, 4);
        for (int i = 0; i < inputsCount; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                nn.layers[0].weights[i, j] = genome.weights[i + j * inputsCount];
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                nn.layers[1].weights[i, j] = genome.weights[i + j * 8 + inputsCount * 8];
            }
        }
    }
}
