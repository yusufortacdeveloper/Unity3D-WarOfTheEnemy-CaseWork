using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Klareh
{
    public class ZeroController : MonoBehaviour 
    {
        public bool Engine;
        public bool Wheels;
        private Animator _animator;

        void Start()
        {
            Engine = false;
            Wheels = true;
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Engine)
            _animator.SetBool("EngineState", true);
            else
            _animator.SetBool("EngineState", false);

            if (Wheels)
            _animator.SetBool("WheelsState", true);
            else
            _animator.SetBool("WheelsState", false);
        }
    }
}
