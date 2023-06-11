using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TPCamera
{
    [ExecuteAlways]
    public class Rail : MonoBehaviour
    {
        public bool isLoop;

        public GameObject Bunny;

        public List<GameObject> nodes;
        [SerializeField]
        private float m_length = 0;

        [SerializeField]
        private float currentDistance = 100;

        private void Start()
        {
            CalculateLength();
        }

        private void OnValidate()
        {
            CalculateLength();
        }

        public void CalculateLength()
        {
            m_length = 0;
            if (nodes.Count > 1)
            {
                GameObject preceding = nodes[0];
                for (int i = 1; i < nodes.Count; i++)
                {
                    GameObject g = nodes[i];
                    m_length += Vector3.Distance(preceding.transform.position, g.transform.position);
                    preceding = nodes[i];
                    if (i == nodes.Count - 1 && isLoop)
                    {
                        m_length += Vector3.Distance(preceding.transform.position, nodes[0].transform.position);
                    }
                }
            }
        }

        public void UpdatePosition(float axis)
        {
            currentDistance += axis;
            Bunny.transform.SetPositionAndRotation(GetPosition(currentDistance), Bunny.transform.rotation);
        }

        public float GetLength()
        {
            return m_length;
        }
        public Vector3 GetPosition(float distance)
        {
            if (!isLoop)
            {
                Mathf.Clamp(distance, 0, m_length);
            }
            else
            {
                distance = Mathf.Repeat(distance, m_length);
            }
            float remainingDistance = distance;
            if (distance <= 0)
            {
                return nodes[0].transform.position;
            }
            if (distance >= m_length)
            {
                return nodes[nodes.Count - 1].transform.position;
            }
            int i = 1;
            while (remainingDistance - Vector3.Distance(nodes[i - 1].transform.position, nodes[i % nodes.Count].transform.position) > 0)
            {
                remainingDistance -= Vector3.Distance(nodes[i - 1].transform.position, nodes[i % nodes.Count].transform.position);
                i++;
            }
            Vector3 posPrec = nodes[i - 1].transform.position;
            Vector3 posNext = nodes[i % nodes.Count].transform.position;
            Vector3 direction = posNext - posPrec;
            return posPrec + direction.normalized * remainingDistance;

        }

        private void OnDrawGizmos()
        {
            if (nodes.Count > 1)
            {
                GameObject preceding = nodes[0];
                for (int i = 1; i < nodes.Count; i++)
                {
                    GameObject g = nodes[i];
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(preceding.transform.position, g.transform.position);
                    preceding = nodes[i];
                    if (i == nodes.Count - 1 && isLoop)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(preceding.transform.position, nodes[0].transform.position);
                    }
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawCube(GetPosition(currentDistance), new Vector3(5.0f, 5.0f, 5.0f));
        }
    }
}