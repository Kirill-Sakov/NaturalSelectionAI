using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Genome
{
    public static int skillCount = 5;

    public float[] weights;
    public int[] skills;

    public Genome(int size)
    {
        weights = new float[size];
        skills = new int[skillCount];
        ///
        skills[0] = 7;
        ///
        for (int i = 0; i < size; i++)
        {
            weights[i] = Random.Range(-1f, 1f);
        }
    }

    public Genome(Genome a)
    {
        weights = new float[a.weights.Length];
        Array.Copy(a.weights, 0, weights, 0, a.weights.Length);
        skills = new int[skillCount];
        Array.Copy(a.skills, 0, skills, 0, skillCount);
    }

    public void MutateWeights(float value)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            // С шансом 0.1, что некоторые веса сети изменятся
            if (Random.value < 0.1) weights[i] += Random.Range(-value, value);
        }

        /// Старый алгоритм изменения скиллов
        //for (int i = 0; i < skillCount; i++)
        //{
        //    if (Random.value < 0.05)
        //    {
        //        skills[i] = Random.Range(0, 4);
        //    }
        //}

        
    }

    public void MutateSkills(int[] allSkills)
    {
        // 6 % вероятности
        if (Random.Range(0, 100) < 7)
        {
            for (int i = 0; i < 3; i++)
            {
                skills[i] = allSkills[i];
            }

            while (true)
            {
                int[] changedSkills = new int[2];

                // Случайный скиллы
                int firstChangedSkill = Random.Range(0, 3);

                int secondChangedSkill = Random.Range(0, 3);

                while (secondChangedSkill == firstChangedSkill)
                    secondChangedSkill = Random.Range(0, 3); // Исключаем повторение

                changedSkills[0] = firstChangedSkill;
                changedSkills[1] = secondChangedSkill;

                // Случайно выбираем скилл из случайно выбранных
                int increased = Random.Range(0, 2); // Увеличиваемый 
                int diminished = new int[] { 0, 1 }.Where(val => val != increased).First(); // Уменьшаемый

                if (skills[changedSkills[diminished]] > 0)
                {
                    if (skills[changedSkills[increased]] < 4)
                    {
                        skills[changedSkills[diminished]]--;
                        skills[changedSkills[increased]]++;

                        break;
                    }
                }
                else
                    continue;
            }
        }
    }
}
