using UnityEngine;

namespace Assets.Platforming_script_one
{
	public class PlayerController : Controller2D
	{
		private const float MaxClimbAngle = 80;
		private readonly float _maxDescendAngle = 80;

		public CollisionInfo Collisions;

		public override void Start()
		{
			base.Start();
		}

		public void Move(Vector3 velocity, bool standingOnPlatform = false)
		{
			UpdateRaycastOrigins();
			Collisions.Reset();
			Collisions.VelocityOld = velocity;

			if (velocity.y < 0)
			{
				DescendSlope(ref velocity);
			}
			if (velocity.x != 0)
			{
				HorizontalCollisions(ref velocity);
			}
			if (velocity.y != 0)
			{
				VerticalCollisions(ref velocity);
			}

			transform.Translate(velocity);

			if (standingOnPlatform)
			{
				Collisions.Below = true;
			}
		}

		private void HorizontalCollisions(ref Vector3 velocity)
		{
			float directionX = Mathf.Sign(velocity.x);
			float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

			for (int i = 0; i < horizontalRayCount; i++)
			{
				Vector2 rayOrigin = (directionX == -1) ? RaycastOrigin.BottomLeft : RaycastOrigin.BottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

				if (hit)
				{

					if (hit.distance == 0)
					{
						continue;
					}

					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

					if (i == 0 && slopeAngle <= MaxClimbAngle)
					{
						if (Collisions.DescendingSlope)
						{
							Collisions.DescendingSlope = false;
							velocity = Collisions.VelocityOld;
						}
						float distanceToSlopeStart = 0;
						if (slopeAngle != Collisions.SlopeAngleOld)
						{
							distanceToSlopeStart = hit.distance - SkinWidth;
							velocity.x -= distanceToSlopeStart * directionX;
						}
						ClimbSlope(ref velocity, slopeAngle);
						velocity.x += distanceToSlopeStart * directionX;
					}

					if (!Collisions.ClimbingSlope || slopeAngle > MaxClimbAngle)
					{
						velocity.x = (hit.distance - SkinWidth) * directionX;
						rayLength = hit.distance;

						if (Collisions.ClimbingSlope)
						{
							velocity.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
						}

						Collisions.Left = directionX == -1;
						Collisions.Right = directionX == 1;
					}
				}
			}
		}

		private void VerticalCollisions(ref Vector3 velocity)
		{
			float directionY = Mathf.Sign(velocity.y);
			float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

			for (int i = 0; i < verticalRayCount; i++)
			{

				Vector2 rayOrigin = (directionY == -1) ? RaycastOrigin.BottomLeft : RaycastOrigin.TopLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

				if (hit)
				{

					velocity.y = (hit.distance - SkinWidth) * directionY;
					rayLength = hit.distance;

					if (Collisions.ClimbingSlope)
					{
						velocity.x = velocity.y / Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
					}

					Collisions.Below = directionY == -1;
					Collisions.Above = directionY == 1;
				}
			}

			if (Collisions.ClimbingSlope)
			{
				float directionX = Mathf.Sign(velocity.x);
				rayLength = Mathf.Abs(velocity.x) + SkinWidth;
				Vector2 rayOrigin = ((directionX == -1) ? RaycastOrigin.BottomLeft : RaycastOrigin.BottomRight) + Vector2.up * velocity.y;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

				if (hit)
				{
					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
					if (slopeAngle != Collisions.SlopeAngle)
					{
						velocity.x = (hit.distance - SkinWidth) * directionX;
						Collisions.SlopeAngle = slopeAngle;
					}
				}
			}
		}

		private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
		{
			float moveDistance = Mathf.Abs(velocity.x);
			float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

			if (velocity.y <= climbVelocityY)
			{
				velocity.y = climbVelocityY;
				velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
				Collisions.Below = true;
				Collisions.ClimbingSlope = true;
				Collisions.SlopeAngle = slopeAngle;
			}
		}

		private void DescendSlope(ref Vector3 velocity)
		{
			float directionX = Mathf.Sign(velocity.x);
			Vector2 rayOrigin = (directionX == -1) ? RaycastOrigin.BottomRight : RaycastOrigin.BottomLeft;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != 0 && slopeAngle <= _maxDescendAngle)
				{
					if (Mathf.Sign(hit.normal.x) == directionX)
					{
						if (hit.distance - SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
						{
							float moveDistance = Mathf.Abs(velocity.x);
							float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
							velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
							velocity.y -= descendVelocityY;

							Collisions.SlopeAngle = slopeAngle;
							Collisions.DescendingSlope = true;
							Collisions.Below = true;
						}
					}
				}
			}
		}



		public struct CollisionInfo
		{
			public bool Above, Below;
			public bool Left, Right;

			public bool ClimbingSlope;
			public bool DescendingSlope;
			public float SlopeAngle, SlopeAngleOld;
			public Vector3 VelocityOld;

			public void Reset()
			{
				Above = Below = false;
				Left = Right = false;
				ClimbingSlope = false;
				DescendingSlope = false;

				SlopeAngleOld = SlopeAngle;
				SlopeAngle = 0;
			}
		}
	}
}
