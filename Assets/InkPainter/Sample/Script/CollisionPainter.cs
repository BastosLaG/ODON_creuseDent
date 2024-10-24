using UnityEditor.PackageManager;
using UnityEngine;

namespace Es.InkPainter.Sample
{
	[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
	public class CollisionPainter : MonoBehaviour
	{
		[SerializeField]
		private Brush brush = null;

        [SerializeField]
        bool erase = false;

		public void Awake()
		{
			GetComponent<MeshRenderer>().material.color = brush.Color;
		}


		public void OnCollisionStay(Collision collision)
		{
            bool success = true;

			foreach(var p in collision.contacts)
			{
				var canvas = p.otherCollider.GetComponent<InkCanvas>();
				if(canvas != null)
                    success = erase? canvas.Erase(brush, p.point) : canvas.Paint(brush, p.point);
			}
		}
	}
}