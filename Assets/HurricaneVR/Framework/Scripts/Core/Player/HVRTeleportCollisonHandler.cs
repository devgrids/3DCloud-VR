using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HurricaneVR.Framework.Shared.Utilities;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Shared;
using HurricaneVR.Framework.Shared.Utilities;
using UnityEngine;

namespace HurricaneVR.Framework.Core.Player
{
    [RequireComponent(typeof(HVRTeleporter))]
    public class HVRTeleportCollisonHandler : MonoBehaviour
    {
        public AfterTeleportOptions AfterTeleportOption = AfterTeleportOptions.DisableCollision;
        public LayerMask LayerMask;

        [Header("Required Objects")]

        [Tooltip("After teleporting, the hand will start at this position and sweep towards the final hand destination")]
        public Transform ResetTarget;

        public HVRHandGrabber LeftHand;
        public HVRJointHand LeftJointHand;

        public HVRHandGrabber RightHand;
        public HVRJointHand RightJointHand;


        public List<RBCollisionTracker> LeftTrackers = new List<RBCollisionTracker>();
        public List<RBCollisionTracker> RightTrackers = new List<RBCollisionTracker>();

        private readonly List<RBCollisionTracker> _cleanup = new List<RBCollisionTracker>();
        private readonly Collider[] _colliders = new Collider[100];

        private HVRGrabbable leftGrabbable;
        private HVRGrabbable rightGrabbable;

        private Vector3 _teleportStart;
        private Vector3 _teleportEnd;

        protected virtual void Start()
        {
            if (!LeftHand)
            {
                LeftHand = transform.root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Left);
            }

            if (LeftHand && !LeftJointHand)
            {
                LeftJointHand = LeftHand.gameObject.GetComponent<HVRJointHand>();
            }

            if (!RightHand)
            {
                RightHand = transform.root.GetComponentsInChildren<HVRHandGrabber>().FirstOrDefault(e => e.HandSide == HVRHandSide.Right);
            }

            if (RightHand && !RightJointHand)
            {
                RightJointHand = RightHand.gameObject.GetComponent<HVRJointHand>();
            }

            if (!ResetTarget)
            {
                var o = new GameObject("TeleportHandReset");
                o.transform.parent = transform;
                ResetTarget = o.transform;
                ResetTarget.ResetLocalProps();
                ResetTarget.localPosition = new Vector3(0f, 1.5f, 0f);
            }

            var teleporter = GetComponent<HVRTeleporter>();
            teleporter.AfterTeleport.AddListener(AfterTeleport);
            teleporter.PositionUpdate.AddListener(TeleportUpdate);
            teleporter.BeforeTeleport.AddListener(BeforeTeleport);
        }

        protected virtual void FixedUpdate()
        {
            //Debug.DrawLine(s, e, Color.red);
            //Debug.DrawLine(s, s + Vector3.up * .3f, Color.blue);

            CheckTrackers(RightTrackers, RightHand);
            CheckTrackers(LeftTrackers, LeftHand);
        }

        private void CheckTrackers(List<RBCollisionTracker> trackers, HVRHandGrabber hand)
        {
            var count = trackers.Count;

            for (var i = 0; i < trackers.Count; i++)
            {
                var tracker = trackers[i];
                tracker.Frame++;
                if (tracker.Frame == 3)
                {
                    Debug.Log($"{tracker.Rb.gameObject.name} is stuck.");
                    tracker.Rb.detectCollisions = false;
                }

                if (tracker.Frame < 3)
                    continue;


                var invalid = CheckOverlap(tracker);

                if (invalid)
                    continue;

                Debug.Log($"{tracker.Rb.gameObject.name} is free.");
                _cleanup.Add(tracker);
                tracker.Rb.detectCollisions = true;
                tracker.Rb = null;


            }

            for (var i = 0; i < _cleanup.Count; i++)
            {
                var tracker = _cleanup[i];
                trackers.Remove(tracker);
            }

            if (count > 0 && trackers.Count == 0 && hand)
            {
                hand.CanRelease = true;
            }

            _cleanup.Clear();
        }

        private bool CheckOverlap(RBCollisionTracker tracker)
        {
            var overlaps = Physics.OverlapBoxNonAlloc(tracker.Rb.transform.TransformPoint(tracker.Center), tracker.Bounds.extents, _colliders, Quaternion.identity, LayerMask);

            var invalid = false;

            for (var j = 0; j < overlaps; j++)
            {
                var c = _colliders[j];

                if (tracker.Colliders.Contains(c))
                    continue;
                invalid = true;
                break;
            }

            return invalid;
        }


        public virtual void BeforeTeleport(Vector3 position)
        {
            _teleportStart = position;

            leftGrabbable = null;
            rightGrabbable = null;

            if (LeftHand && LeftHand.GrabbedTarget)
            {
                leftGrabbable = LeftHand.GrabbedTarget;
            }

            if (RightHand && RightHand.GrabbedTarget)
            {
                rightGrabbable = RightHand.GrabbedTarget;
            }

            if (leftGrabbable && leftGrabbable.TryGetComponent<HVRTeleportOptions>(out var o) && o.BeforeTeleportOption == BeforeTeleportOptions.DropsGrabbable)
            {
                leftGrabbable.ForceRelease();
                leftGrabbable = null;
            }

            if (rightGrabbable && rightGrabbable != leftGrabbable && rightGrabbable.TryGetComponent<HVRTeleportOptions>(out o) && o.BeforeTeleportOption == BeforeTeleportOptions.DropsGrabbable)
            {
                rightGrabbable.ForceRelease();
                rightGrabbable = null;
            }

            if (leftGrabbable && leftGrabbable.Rigidbody)
            {
                leftGrabbable.Rigidbody.detectCollisions = false;
            }

            if (rightGrabbable && rightGrabbable.Rigidbody)
            {
                rightGrabbable.Rigidbody.detectCollisions = false;
            }

            if (LeftHand)
            {
                LeftHand.CanRelease = false;
            }

            if (RightHand)
            {
                RightHand.CanRelease = false;
            }

            if (LeftJointHand)
            {
                LeftJointHand.Disable();
                LeftJointHand.RigidBody.detectCollisions = false;
            }

            if (RightJointHand)
            {
                RightJointHand.Disable();
                RightJointHand.RigidBody.detectCollisions = false;
            }

            LeftHand.Rigidbody.velocity = Vector3.zero;
            RightHand.Rigidbody.velocity = Vector3.zero;

            _previousPosition = position;
        }

        private Vector3 _previousPosition;

        public virtual void TeleportUpdate(Vector3 position)
        {
            _teleportEnd = position;
            var delta = position - _previousPosition;

            if (leftGrabbable)
            {
                leftGrabbable.transform.position += delta;
                leftGrabbable.Rigidbody.position = leftGrabbable.transform.position;
            }

            if (rightGrabbable)
            {
                rightGrabbable.transform.position += delta;
                rightGrabbable.Rigidbody.position = rightGrabbable.transform.position;
            }

            if (LeftJointHand)
            {
                LeftJointHand.transform.position += delta;
                LeftJointHand.RigidBody.position += LeftJointHand.RigidBody.position;
            }

            if (RightJointHand)
            {
                RightJointHand.transform.position += delta;
                RightJointHand.RigidBody.position = RightJointHand.RigidBody.position;
            }

            //SweepHand(LeftHand, leftGrabbable);
            //SweepHand(RightHand, rightGrabbable);

            _previousPosition = position;
        }

        public virtual void AfterTeleport()
        {
            if (LeftHand)
            {
                LeftHand.CanRelease = true;
            }

            if (RightHand)
            {
                RightHand.CanRelease = true;
            }

            if (leftGrabbable && leftGrabbable.Rigidbody)
            {
                leftGrabbable.Rigidbody.detectCollisions = true;
            }

            if (LeftHand && LeftHand.GrabbedTarget && LeftHand.GrabbedTarget.Rigidbody)
            {
                var option = GetAfterOption(leftGrabbable, out var o);

                if (option == AfterTeleportOptions.DisableCollision)
                {
                    var tracker = new RBCollisionTracker(LeftHand.GrabbedTarget.Rigidbody);
                    LeftTrackers.Add(tracker);
                    LeftHand.CanRelease = false;
                }
            }

            if (rightGrabbable && rightGrabbable.Rigidbody)
            {
                rightGrabbable.Rigidbody.detectCollisions = true;
            }

            if (RightHand && RightHand.GrabbedTarget && RightHand.GrabbedTarget.Rigidbody)
            {
                var option = GetAfterOption(rightGrabbable, out var o);

                if (option == AfterTeleportOptions.DisableCollision)
                {
                    var tracker = new RBCollisionTracker(RightHand.GrabbedTarget.Rigidbody);
                    RightTrackers.Add(tracker);
                    RightHand.CanRelease = false;
                }
            }

            if (LeftHand)
            {
                LeftJointHand.RigidBody.detectCollisions = true;
            }

            if (RightJointHand)
            {
                RightJointHand.RigidBody.detectCollisions = true;
            }



            StartCoroutine(AfterFixedUpdate());

        }

        protected virtual IEnumerator AfterFixedUpdate()
        {
            yield return new WaitForFixedUpdate();

            LeftJointHand.RigidBody.position = LeftJointHand.Target.position;
            RightJointHand.RigidBody.position = RightJointHand.Target.position;

            LeftHand.Rigidbody.velocity = Vector3.zero;
            RightHand.Rigidbody.velocity = Vector3.zero;

            SweepHand(LeftHand, leftGrabbable);
            SweepHand(RightHand, rightGrabbable);

            LeftJointHand.Enable();
            RightJointHand.Enable();

            leftGrabbable = null;
            rightGrabbable = null;

        }

        protected virtual void SweepHand(HVRHandGrabber hand, HVRGrabbable g)
        {
            if (hand)
            {
                var sweepHand = true;

                if (g && g.Rigidbody)
                {
                    var option = GetAfterOption(g, out var options);

                    if (option == AfterTeleportOptions.BoundingBoxSweep)
                    {
                        SweepHandAndGrabbable(hand, g, options);
                        sweepHand = false;
                    }
                }

                if (sweepHand)
                {
                    SweepHand(hand);
                }
            }
        }

        private AfterTeleportOptions GetAfterOption(HVRGrabbable g, out HVRTeleportOptions options)
        {
            var option = AfterTeleportOption;

            if (g.gameObject.TryGetComponent<HVRTeleportOptions>(out options))
            {
                if (options.AfterTeleportOption != AfterTeleportOptions.TeleporterDefault)
                {
                    option = options.AfterTeleportOption;
                }
            }

            return option;
        }


        protected virtual void SweepHandAndGrabbable(HVRHandGrabber hand, HVRGrabbable grabbable, HVRTeleportOptions options)
        {
            var grabbableOffset = grabbable.Rigidbody.position - hand.Rigidbody.position;

            SweepHand(hand);

            grabbable.Rigidbody.position = hand.Rigidbody.position + grabbableOffset;

            var bounds = hand.Rigidbody.GetColliders().ToArray().GetColliderBounds();

            if (options && options.CustomBoundingBox)
            {
                bounds.Encapsulate(options.CustomBoundingBox.bounds);
            }
            else
            {
                bounds.Encapsulate(grabbable.Colliders.GetColliderBounds());
            }

            var direction = (_teleportStart - _teleportEnd).normalized;
            direction.y = 0f;

            var maxSide = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            var colliders = grabbable.Colliders;

            var offset = Vector3.zero;

            for (var d = 0f; d < maxSide * 1.5f; d += .01f)
            {
                offset = direction * d;
                var overlaps = Physics.OverlapBoxNonAlloc(bounds.center + offset, bounds.extents, _colliders, Quaternion.identity, LayerMask);
                if (overlaps == 0)
                    break;

                var invalid = false;

                for (var i = 0; i < overlaps; i++)
                {
                    var overlappedCollider = _colliders[i];
                    var contains = false;

                    foreach (var grabbableCollider in colliders)
                    {
                        if (overlappedCollider == grabbableCollider)
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (contains)
                    {
                        continue;
                    }

                    invalid = true;

                    break;
                }

                if (!invalid)
                    break;
            }

            offset *= 1.2f;

            hand.transform.position = hand.Rigidbody.position = hand.Rigidbody.position + offset;
            grabbable.transform.position = grabbable.Rigidbody.position = grabbable.Rigidbody.position + offset;
        }

        private Vector3 s;
        private Vector3 e;

        protected virtual void SweepHand(HVRHandGrabber hand)
        {
            var target = hand.Rigidbody.position;
            var origin = ResetTarget;
            if (!origin)
            {
                origin = this.transform;
            }
            var direction = (target - origin.position).normalized;
            var length = Vector3.Distance(target, origin.position);

            hand.Rigidbody.position = origin.position;

            var bounds = hand.Rigidbody.GetColliders().ToArray().GetColliderBounds();
            var maxSide = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            var start = bounds.center - direction * maxSide * 1.05f;
            var rbDelta = bounds.center - hand.Rigidbody.position;

            if (hand.HandSide == HVRHandSide.Left)
            {
                s = hand.Rigidbody.position;
                e = target;
            }

            //sweep test seems to collide with concave environment colliders where box cast doesn't?
            if (Physics.BoxCast(start, bounds.extents, direction, out var hit, Quaternion.identity, length, LayerMask, QueryTriggerInteraction.Ignore))
            //if (hand.Rigidbody.SweepTest(direction, out var hit, length, QueryTriggerInteraction.Ignore))
            {
               // Debug.Log($"hit {hit.distance} {hit.collider.name}");
                hand.Rigidbody.position += direction * (hit.distance - maxSide) + rbDelta;
                return;
            }

          //  Debug.Log($"no hit {length}");
            hand.Rigidbody.position = target;


        }
    }

    public class RBCollisionTracker
    {
        public Rigidbody Rb;
        public int Frame;
        public Bounds Bounds;
        public Collider[] Colliders;
        public Vector3 Center;

        public RBCollisionTracker(Rigidbody rb)
        {
            Rb = rb;
            Colliders = rb.GetColliders().ToArray();
            Bounds = Colliders.GetColliderBounds();
            Center = rb.transform.InverseTransformPoint(Bounds.center);
        }
    }
}