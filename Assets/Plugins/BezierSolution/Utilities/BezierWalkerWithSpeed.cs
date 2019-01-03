﻿using UnityEngine;
using UnityEngine.Events;

namespace BezierSolution
{
	public class BezierWalkerWithSpeed : MonoBehaviour
	{
		public enum TravelMode { Once, Loop, PingPong };

		private Transform cachedTransform;

        // private GameObject bezierSplineParent;
		public BezierSpline spline;
		public TravelMode travelMode;

		public float speed = 5f;
        public bool useAcceleration = true;
        public float accelerationFactor = 1.1f;
        public float maxSpeed = 30f;

        [HideInInspector]
        public float acceleratedSpeed;
        [HideInInspector]
        public float progress = 0f;

        public float NormalizedT
		{
			get { return progress; }
			set { progress = value; }
		}

		[Range( 0f, 0.06f )]
		public float relaxationAtEndPoints = 0.01f;

		//public float movementLerpModifier = 10f;
		public float rotationLerpModifier = 10f;

		public bool lookForward = true;

		private bool isGoingForward = true;

		public UnityEvent onPathCompleted = new UnityEvent();
		private bool onPathCompletedCalledAt1 = false;
		private bool onPathCompletedCalledAt0 = false;

		void Awake()
		{
			cachedTransform = transform;
            acceleratedSpeed = speed;
            // bezierSplineParent = GameObject.FindGameObjectWithTag("BezierCurves");
            // spline = GetRandomChildSpline(bezierSplineParent);
            // FindObjectOfType<AudioManager>().Play("Pop");
        }

        void Update()
		{
            acceleratedSpeed *= accelerationFactor;

			float targetSpeed = ( isGoingForward ) ? speed : -speed;

            if (useAcceleration)
            {
                acceleratedSpeed = (acceleratedSpeed >= maxSpeed) ? maxSpeed : acceleratedSpeed;
                targetSpeed = acceleratedSpeed;
            }

			Vector3 targetPos;
			// Code below uses the obsolete MoveAlongSpline function
			//float absSpeed = Mathf.Abs( speed );
			//if( absSpeed <= 2f )
			//	targetPos = spline.MoveAlongSpline( ref progress, targetSpeed * Time.deltaTime, maximumError: 0f );
			//else if( absSpeed >= 40f )
			//	targetPos = spline.MoveAlongSpline( ref progress, targetSpeed * Time.deltaTime, increasedAccuracy: true );
			//else
			//	targetPos = spline.MoveAlongSpline( ref progress, targetSpeed * Time.deltaTime );

			targetPos = spline.MoveAlongSpline( ref progress, targetSpeed * Time.deltaTime );

			cachedTransform.position = targetPos;
			//cachedTransform.position = Vector3.Lerp( cachedTransform.position, targetPos, movementLerpModifier * Time.deltaTime );

			bool movingForward = ( speed > 0f ) == isGoingForward;

			if( lookForward )
			{
				Quaternion targetRotation;
				if( movingForward )
					targetRotation = Quaternion.LookRotation( spline.GetTangent( progress ) );
				else
					targetRotation = Quaternion.LookRotation( -spline.GetTangent( progress ) );

				cachedTransform.rotation = Quaternion.Lerp( cachedTransform.rotation, targetRotation, rotationLerpModifier * Time.deltaTime );
			}

			if( movingForward )
			{
				if( progress >= 1f - relaxationAtEndPoints )
				{
					if( !onPathCompletedCalledAt1 )
					{
						onPathCompleted.Invoke();
						onPathCompletedCalledAt1 = true;
					}

					if( travelMode == TravelMode.Once )
						progress = 1f;
					else if( travelMode == TravelMode.Loop )
						progress -= 1f;
					else
					{
						progress = 2f - progress;
						isGoingForward = !isGoingForward;
					}
				}
				else
				{
					onPathCompletedCalledAt1 = false;
				}
			}
			else
			{
				if( progress <= relaxationAtEndPoints )
				{
					if( !onPathCompletedCalledAt0 )
					{
						onPathCompleted.Invoke();
						onPathCompletedCalledAt0 = true;
					}

					if( travelMode == TravelMode.Once )
						progress = 0f;
					else if( travelMode == TravelMode.Loop )
						progress += 1f;
					else
					{
						progress = -progress;
						isGoingForward = !isGoingForward;
					}
				}
				else
				{
					onPathCompletedCalledAt0 = false;
				}
			}
		}

        // private BezierSpline GetRandomChildSpline(GameObject splineParent)
        // {
        //     BezierSpline tempSpline;
        //     int randomIndex = Random.Range(0, splineParent.transform.childCount);
        //     tempSpline = splineParent.transform.GetChild(randomIndex).GetComponent<BezierSpline>();
        //     tempSpline.gameObject.SetActive(true);
        //     return tempSpline;
        // }
	}
}